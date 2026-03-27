using System;
using System.Collections.Generic;
using System.Linq;
using Defter2Fis.ForMikro.Models;

namespace Defter2Fis.ForMikro.Services
{
    /// <summary>
    /// Fiş oluşturma işleminin kuru-çalıştırma (dry-run) simülasyonu.
    /// DB'ye yazma yapmadan, oluşacak kayıtları ve referansları önizleme olarak sunar.
    /// </summary>
    public class OnizlemeServisi
    {
        private readonly IMikroDbService _dbService;
        private readonly DefterAnalyzer _analyzer;
        private readonly EvrakBilgisiParser _evrakParser;
        private readonly LogService _log;

        public OnizlemeServisi(IMikroDbService dbService, LogService log)
        {
            if (dbService == null) throw new ArgumentNullException(nameof(dbService));
            if (log == null) throw new ArgumentNullException(nameof(log));

            _dbService = dbService;
            _analyzer = new DefterAnalyzer();
            _evrakParser = new EvrakBilgisiParser();
            _log = log;
        }

        /// <summary>
        /// Fiş oluşturma simülasyonu yapar. DB'ye yazma yok.
        /// </summary>
        public OnizlemeSonucu Calistir(
            List<YevmiyeDefteri> defterler,
            int firmaNo,
            int subeNo,
            short dbcNo,
            Action<int, string> ilerlemeRaporla = null)
        {
            if (defterler == null || defterler.Count == 0)
                throw new ArgumentException("Defter listesi boş.", nameof(defterler));

            var sonuc = new OnizlemeSonucu();
            var ilkDefter = defterler[0];

            sonuc.DonemBaslangic = ilkDefter.DonemBaslangic;
            sonuc.DonemBitis = ilkDefter.DonemBitis;
            sonuc.MaliYil = ilkDefter.MaliYilBaslangic.Year;

            try
            {
                // 1. Eksik hesap planı tespiti
                ilerlemeRaporla?.Invoke(5, "Hesap planı kontrol ediliyor...");
                EksikHesaplariTespit(defterler, sonuc);

                // 2. Cari/Stok hareketlerini getir
                ilerlemeRaporla?.Invoke(15, "Cari hesap hareketleri sorgulanıyor...");
                var cariHareketler = _dbService.DonemCariHareketleriGetirGuvenli(
                    sonuc.DonemBaslangic, sonuc.DonemBitis, firmaNo, subeNo);
                sonuc.ToplamCariHareket = cariHareketler.Count;

                ilerlemeRaporla?.Invoke(25, "Stok hareketleri sorgulanıyor...");
                var stokHareketler = _dbService.DonemStokHareketleriGetirGuvenli(
                    sonuc.DonemBaslangic, sonuc.DonemBitis, firmaNo, subeNo);
                sonuc.ToplamStokHareket = stokHareketler.Count;

                // Eşleştirme index'leri (fis_tarih + fis_sirano bazlı)
                var cariIndex = MikroDbService.CariIndexOlustur(cariHareketler);
                var stokIndex = MikroDbService.StokIndexOlustur(stokHareketler);

                // 3. Fişleri simüle et
                ilerlemeRaporla?.Invoke(35, "Fiş oluşturma simüle ediliyor...");
                var tumFisler = defterler.SelectMany(d => d.Fisler).ToList();
                var tarihGruplari = tumFisler
                    .OrderBy(f => f.Satirlar.Count > 0 ? f.Satirlar[0].KayitTarihi : f.GirisTarihi)
                    .ThenBy(f => f.YevmiyeNoSayac)
                    .GroupBy(f => f.Satirlar.Count > 0 ? f.Satirlar[0].KayitTarihi.Date : f.GirisTarihi.Date)
                    .OrderBy(g => g.Key)
                    .ToList();

                int toplamFis = tumFisler.Count;
                int islenenFis = 0;

                foreach (var tarihGrubu in tarihGruplari)
                {
                    DateTime fisTarihi = tarihGrubu.Key;
                    int mevcutMaxSira = _dbService.MaxSiraNoGetir(
                        fisTarihi, sonuc.MaliYil, firmaNo, subeNo);
                    int siradakiSiraNo = mevcutMaxSira + 1;

                    foreach (var yevmiyeFisi in tarihGrubu)
                    {
                        islenenFis++;
                        int yuzde = 35 + (islenenFis * 55 / toplamFis);
                        ilerlemeRaporla?.Invoke(yuzde,
                            $"Simülasyon: Yevmiye #{yevmiyeFisi.YevmiyeNoSayac} ({islenenFis}/{toplamFis})");

                        // Mükerrer kontrolü (unique index uyumlu: firmano + maliyil + yevmiye_no)
                        bool mukerrer = _dbService.YevmiyeNoMevcutMu(
                            yevmiyeFisi.YevmiyeNoSayac, sonuc.MaliYil, firmaNo, subeNo);

                        if (mukerrer)
                        {
                            sonuc.MukerrerYevmiyeNolar.Add(yevmiyeFisi.YevmiyeNoSayac);
                        }

                        // Mükerrer olsa bile sıra no ata (önizlemede kullanıcı görsün)
                        int siraNo = siradakiSiraNo++;

                        // Borç/Alacak topla
                        decimal borc = 0, alacak = 0;
                        foreach (var s in yevmiyeFisi.Satirlar)
                        {
                            if (s.BorcAlacakKodu == "D") borc += s.Tutar;
                            else if (s.BorcAlacakKodu == "C") alacak += s.Tutar;
                        }

                        // İlk satırdan evrak bilgisi parse et (bilgi amaçlı)
                        string evrakSeri = string.Empty;
                        int evrakSira = 0;

                        if (yevmiyeFisi.Satirlar.Count > 0)
                        {
                            var ilkSatir = yevmiyeFisi.Satirlar[0];
                            var evrak = _evrakParser.Parse(
                                ilkSatir.DetayAciklama, ilkSatir.BelgeNo, ilkSatir.BelgeReferansi);

                            if (evrak != null)
                            {
                                evrakSeri = evrak.Seri;
                                evrakSira = evrak.Sira;
                            }
                        }

                        // Tarih+SıraNo bazlı eşleştirme (cha_fis_tarih + cha_fis_sirano / sth_fis_tarihi + sth_fis_sirano)
                        string fisAnahtar = $"{fisTarihi:yyyy-MM-dd}|{siraNo}";
                        bool cariEslesti = cariIndex.ContainsKey(fisAnahtar);
                        bool stokEslesti = stokIndex.ContainsKey(fisAnahtar);
                        string eslesmeDurumu;

                        if (cariEslesti && stokEslesti)
                            eslesmeDurumu = "Cari+Stok";
                        else if (cariEslesti)
                            eslesmeDurumu = "Cari";
                        else if (stokEslesti)
                            eslesmeDurumu = "Stok";
                        else
                            eslesmeDurumu = "Eşleşme yok";

                        // Cari eşleşme detayları
                        if (cariEslesti && !mukerrer)
                        {
                            foreach (var cha in cariIndex[fisAnahtar])
                            {
                                sonuc.CariEslesmeleri.Add(new OnizlemeCariEslesmesi
                                {
                                    YevmiyeNo = yevmiyeFisi.YevmiyeNoSayac,
                                    CariHesapKod = cha.ChaKod,
                                    IslemTarihi = cha.ChaTarihi
                                });
                            }
                        }

                        // Stok eşleşme detayları
                        if (stokEslesti && !mukerrer)
                        {
                            foreach (var sth in stokIndex[fisAnahtar])
                            {
                                sonuc.StokEslesmeleri.Add(new OnizlemeStokEslesmesi
                                {
                                    YevmiyeNo = yevmiyeFisi.YevmiyeNoSayac,
                                    IslemTarihi = sth.SthTarihi
                                });
                            }
                        }

                        // Fiş önizleme kaydı
                        string aciklama = string.Empty;
                        if (yevmiyeFisi.Satirlar.Count > 0)
                        {
                            aciklama = yevmiyeFisi.Satirlar[0].DetayAciklama ?? string.Empty;
                        }

                        sonuc.FisKayitlari.Add(new OnizlemeFisKaydi
                        {
                            YevmiyeNo = yevmiyeFisi.YevmiyeNoSayac,
                            Tarih = yevmiyeFisi.Satirlar.Count > 0
                                ? yevmiyeFisi.Satirlar[0].KayitTarihi
                                : yevmiyeFisi.GirisTarihi,
                            SatirSayisi = yevmiyeFisi.Satirlar.Count,
                            ToplamBorc = borc,
                            ToplamAlacak = alacak,
                            Aciklama = aciklama,
                            AtanacakSiraNo = siraNo,
                            EslesmeDurumu = eslesmeDurumu,
                            EvrakSeri = evrakSeri,
                            EvrakSira = evrakSira,
                            Mukerrer = mukerrer
                        });
                    }
                }

                // 4. Uyarıları oluştur
                ilerlemeRaporla?.Invoke(92, "Uyarılar kontrol ediliyor...");
                UyarilariOlustur(sonuc);

                ilerlemeRaporla?.Invoke(100, "Önizleme tamamlandı.");
            }
            catch (Exception ex)
            {
                sonuc.Uyarilar.Add(new OnizlemeUyari
                {
                    Seviye = UyariSeviye.Kritik,
                    Mesaj = $"Önizleme hatası: {ex.Message}"
                });
                _log.Hata($"Önizleme kritik hata: {ex.Message}");
            }

            return sonuc;
        }

        /// <summary>
        /// Eksik hesapları tespit eder (DB'ye eklemez).
        /// </summary>
        private void EksikHesaplariTespit(List<YevmiyeDefteri> defterler, OnizlemeSonucu sonuc)
        {
            var mevcutKodlar = _dbService.TumHesapKodlariniGetir();
            var eksikler = _analyzer.EksikHesaplariTespit(defterler, mevcutKodlar);

            foreach (var eksik in eksikler)
            {
                sonuc.EksikHesaplar.Add(new OnizlemeEksikHesap
                {
                    HesapKod = eksik.HesapKod,
                    HesapIsim = eksik.HesapIsim,
                    HesapTip = eksik.HesapTip
                });
            }
        }

        /// <summary>
        /// Sonuçlara göre uyarı mesajları oluşturur.
        /// </summary>
        private void UyarilariOlustur(OnizlemeSonucu sonuc)
        {
            // Mükerrer bilgisi (sadece bilgilendirme — yazımda üzerine yazılacak)
            if (sonuc.MukerrerSayisi > 0)
            {
                sonuc.Uyarilar.Add(new OnizlemeUyari
                {
                    Seviye = UyariSeviye.Bilgi,
                    Mesaj = $"{sonuc.MukerrerSayisi} yevmiye DB'de zaten mevcut — üzerine yazılacak."
                });
            }

            // Eksik hesap bilgisi
            if (sonuc.EksikHesaplar.Count > 0)
            {
                sonuc.Uyarilar.Add(new OnizlemeUyari
                {
                    Seviye = UyariSeviye.Bilgi,
                    Mesaj = $"{sonuc.EksikHesaplar.Count} eksik hesap otomatik eklenecek."
                });
            }

            // Eşleşme oranı uyarısı
            int eslesmeyenFis = 0;
            foreach (var fis in sonuc.FisKayitlari)
            {
                if (!fis.Mukerrer && fis.EslesmeDurumu != "Cari" &&
                    fis.EslesmeDurumu != "Stok" && fis.EslesmeDurumu != "Cari+Stok")
                {
                    eslesmeyenFis++;
                }
            }

            if (eslesmeyenFis > 0)
            {
                sonuc.Uyarilar.Add(new OnizlemeUyari
                {
                    Seviye = UyariSeviye.Uyari,
                    Mesaj = $"{eslesmeyenFis} fişte cari/stok eşleşmesi bulunamadı."
                });
            }

            // Cari/Stok tablo yok bilgisi
            if (sonuc.ToplamCariHareket == 0 && sonuc.ToplamStokHareket == 0)
            {
                sonuc.Uyarilar.Add(new OnizlemeUyari
                {
                    Seviye = UyariSeviye.Bilgi,
                    Mesaj = "Dönemde cari/stok hareketi bulunamadı — sadece muhasebe fişleri oluşturulacak."
                });
            }

            // Borç-Alacak dengesi
            foreach (var fis in sonuc.FisKayitlari)
            {
                if (!fis.Mukerrer && Math.Abs(fis.ToplamBorc - fis.ToplamAlacak) > 0.01m)
                {
                    sonuc.Uyarilar.Add(new OnizlemeUyari
                    {
                        Seviye = UyariSeviye.Kritik,
                        Mesaj = $"Yevmiye #{fis.YevmiyeNo}: Borç-Alacak dengesiz! " +
                                $"Borç={fis.ToplamBorc:N2}, Alacak={fis.ToplamAlacak:N2}",
                        YevmiyeNo = fis.YevmiyeNo
                    });
                }
            }
        }
    }
}
