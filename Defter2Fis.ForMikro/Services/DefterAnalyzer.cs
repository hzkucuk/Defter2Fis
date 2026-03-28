using System;
using System.Collections.Generic;
using System.Linq;
using Defter2Fis.ForMikro.Models;

namespace Defter2Fis.ForMikro.Services
{
    /// <summary>
    /// E-Defter verileri üzerinde analiz, doğrulama ve raporlama.
    /// Fiş oluşturma öncesi ön kontrol amaçlı kullanılır.
    /// </summary>
    public class DefterAnalyzer
    {
        /// <summary>
        /// Defterlerin genel özetini hesaplar.
        /// </summary>
        public AnalizOzeti OzetHesapla(List<YevmiyeDefteri> defterler)
        {
            var ozet = new AnalizOzeti();

            foreach (var defter in defterler)
            {
                int satirSayisi = defter.Fisler.Sum(f => f.Satirlar.Count);
                ozet.DosyaSayisi++;
                ozet.ToplamFis += defter.Fisler.Count;
                ozet.ToplamSatir += satirSayisi;

                ozet.DosyaOzetleri.Add(new DosyaOzeti
                {
                    DosyaAdi = defter.DosyaAdi,
                    BenzersizId = defter.BenzersizId,
                    FirmaUnvani = defter.FirmaUnvani,
                    SicilNo = defter.SicilNo,
                    DonemBaslangic = defter.DonemBaslangic,
                    DonemBitis = defter.DonemBitis,
                    FisSayisi = defter.Fisler.Count,
                    SatirSayisi = satirSayisi
                });
            }

            return ozet;
        }

        /// <summary>
        /// Tüm fişlerdeki benzersiz hesap kodlarını (Ana + Alt) toplar.
        /// </summary>
        public HashSet<string> BenzersizHesapKodlari(List<YevmiyeDefteri> defterler)
        {
            var kodlar = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var defter in defterler)
            {
                foreach (var fis in defter.Fisler)
                {
                    foreach (var satir in fis.Satirlar)
                    {
                        // Ana hesap kodu ekle (ör: 770)
                        if (!string.IsNullOrWhiteSpace(satir.AnaHesapKod))
                            kodlar.Add(satir.AnaHesapKod);

                        // Alt hesap kodu ekle (ör: 770.50.0018)
                        if (!string.IsNullOrWhiteSpace(satir.AltHesapKod))
                            kodlar.Add(satir.AltHesapKod);

                        // Ara seviye kodları da ekle (ör: 770.50)
                        if (!string.IsNullOrWhiteSpace(satir.AltHesapKod))
                        {
                            var araKodlar = AraSeviyeleriUret(satir.AltHesapKod);
                            foreach (string araKod in araKodlar)
                                kodlar.Add(araKod);
                        }
                    }
                }
            }

            return kodlar;
        }

        /// <summary>
        /// Hesap kodunun ara seviyelerini üretir.
        /// Ör: "770.50.0018" → ["770", "770.50"]
        /// </summary>
        public List<string> AraSeviyeleriUret(string hesapKod)
        {
            var sonuc = new List<string>();
            if (string.IsNullOrWhiteSpace(hesapKod)) return sonuc;

            string[] parcalar = hesapKod.Split('.');
            string current = string.Empty;

            // Son parça hariç tüm ara seviyeleri ekle
            for (int i = 0; i < parcalar.Length - 1; i++)
            {
                current = i == 0 ? parcalar[i] : current + "." + parcalar[i];
                sonuc.Add(current);
            }

            return sonuc;
        }

        /// <summary>
        /// E-Defter'de olan ama DB'de olmayan hesap kodlarını tespit eder.
        /// Hesap adlarını da eşleştirir.
        /// </summary>
        public List<EksikHesap> EksikHesaplariTespit(
            List<YevmiyeDefteri> defterler,
            HashSet<string> mevcutHesapKodlari)
        {
            // Önce tüm hesap kod-isim eşleştirmesini topla
            var hesapIsimMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var defter in defterler)
            {
                foreach (var fis in defter.Fisler)
                {
                    foreach (var satir in fis.Satirlar)
                    {
                        if (!string.IsNullOrWhiteSpace(satir.AnaHesapKod) &&
                            !hesapIsimMap.ContainsKey(satir.AnaHesapKod))
                        {
                            hesapIsimMap[satir.AnaHesapKod] = satir.AnaHesapAd ?? string.Empty;
                        }

                        if (!string.IsNullOrWhiteSpace(satir.AltHesapKod) &&
                            !hesapIsimMap.ContainsKey(satir.AltHesapKod))
                        {
                            hesapIsimMap[satir.AltHesapKod] = satir.AltHesapAd ?? string.Empty;
                        }
                    }
                }
            }

            // E-Defter'deki tüm benzersiz kodlar
            var edDefterKodlar = BenzersizHesapKodlari(defterler);

            var eksikler = new List<EksikHesap>();

            foreach (string kod in edDefterKodlar.OrderBy(k => k))
            {
                if (!mevcutHesapKodlari.Contains(kod))
                {
                    string isim = hesapIsimMap.ContainsKey(kod) ? hesapIsimMap[kod] : string.Empty;

                    // Ara seviye ise ve isim yoksa, üst hesap adından türet
                    if (string.IsNullOrEmpty(isim))
                    {
                        isim = "(Ara seviye)";
                    }

                    eksikler.Add(new EksikHesap
                    {
                        HesapKod = kod,
                        HesapIsim = isim,
                        HesapTip = MuhasebeHesapPlani.HesapTipBelirle(kod)
                    });
                }
            }

            return eksikler;
        }

        /// <summary>
        /// Eksik hesap raporunu döner (UI tarafında gösterilir).
        /// </summary>
        public List<EksikHesap> EksikHesaplariGetir(
            List<YevmiyeDefteri> defterler,
            HashSet<string> mevcutHesapKodlari)
        {
            return EksikHesaplariTespit(defterler, mevcutHesapKodlari);
        }

        /// <summary>
        /// Borç-Alacak dengesini doğrular (her fiş için Borç = Alacak olmalı).
        /// </summary>
        public List<DengeSizFis> DengeKontrolu(List<YevmiyeDefteri> defterler)
        {
            var dengesizler = new List<DengeSizFis>();

            foreach (var defter in defterler)
            {
                foreach (var fis in defter.Fisler)
                {
                    decimal toplamBorc = 0;
                    decimal toplamAlacak = 0;

                    foreach (var satir in fis.Satirlar)
                    {
                        if (satir.BorcAlacakKodu == "D")
                            toplamBorc += satir.Tutar;
                        else if (satir.BorcAlacakKodu == "C")
                            toplamAlacak += satir.Tutar;
                    }

                    // 0.01 tolerans (kuruş farkları)
                    if (Math.Abs(toplamBorc - toplamAlacak) > 0.01m)
                    {
                        dengesizler.Add(new DengeSizFis
                        {
                            DosyaAdi = defter.DosyaAdi,
                            YevmiyeNo = fis.YevmiyeNo,
                            YevmiyeNoSayac = fis.YevmiyeNoSayac,
                            ToplamBorc = toplamBorc,
                            ToplamAlacak = toplamAlacak,
                            Fark = toplamBorc - toplamAlacak
                        });
                    }
                }
            }

            return dengesizler;
        }

        /// <summary>
        /// DB mevcut durum istatistiğini döner.
        /// </summary>
        public DbDurumBilgisi DbDurumGetir(IMikroDbService dbService, DateTime donemBas, DateTime donemBit, int maliYil)
        {
            return new DbDurumBilgisi
            {
                HesapSayisi = dbService.HesapPlaniSayisi(),
                FisSayisi = dbService.MevcutFisSayisi(maliYil, donemBas, donemBit, 0, 0),
                DonemBaslangic = donemBas,
                DonemBitis = donemBit
            };
        }

        /// <summary>
        /// Yevmiye numarası bazlı süreklilik kontrolü yapar.
        /// DB'deki mevcut yevmiye numaralarının, çalışılan ayın ilk yevmiyesiyle
        /// boşluksuz devam ettiğini doğrular (tarihten bağımsız).
        /// </summary>
        /// <param name="dbService">DB servis arayüzü</param>
        /// <param name="defterler">Çalışılan ayın E-Defter verileri</param>
        /// <param name="firmaNo">Firma numarası</param>
        /// <param name="subeNo">Şube numarası</param>
        public SureklilkKontrolSonucu OncekiAyDogrula(
            IMikroDbService dbService,
            List<YevmiyeDefteri> defterler,
            int firmaNo,
            int subeNo)
        {
            if (dbService == null) throw new ArgumentNullException(nameof(dbService));
            if (defterler == null || defterler.Count == 0)
                throw new ArgumentException("Defter listesi boş.", nameof(defterler));

            var sonuc = new SureklilkKontrolSonucu();
            var ilkDefter = defterler[0];
            int maliYil = ilkDefter.MaliYilBaslangic.Year;
            DateTime donemBas = ilkDefter.DonemBaslangic;
            DateTime donemBit = ilkDefter.DonemBitis;

            // Çalışılan ayın E-Defter yevmiye bilgisi
            var tumFisler = defterler.SelectMany(d => d.Fisler).ToList();
            int calislanMinYevmiye = tumFisler.Min(f => f.YevmiyeNoSayac);
            int calislanMaxYevmiye = tumFisler.Max(f => f.YevmiyeNoSayac);

            sonuc.CalislanAyBilgisi = new AyFisBilgisi
            {
                DonemBaslangic = donemBas,
                DonemBitis = donemBit,
                FisSayisi = tumFisler.Count,
                SatirSayisi = tumFisler.Sum(f => f.Satirlar.Count),
                MinYevmiyeNo = calislanMinYevmiye,
                MaxYevmiyeNo = calislanMaxYevmiye,
                MinTarih = donemBas,
                MaxTarih = donemBit
            };

            sonuc.Mesajlar.Add($"Çalışılan ay yevmiye aralığı: {calislanMinYevmiye} - {calislanMaxYevmiye} ({tumFisler.Count} fiş)");

            // İlk ay kontrolü: yevmiye 1'den başlıyorsa ilk aydır
            sonuc.IlkAy = calislanMinYevmiye == 1;

            if (sonuc.IlkAy)
            {
                sonuc.Mesajlar.Add("Mali yıl ilk dönemi — yevmiye 1'den başlıyor, önceki dönem kontrolü gerekmiyor.");
                sonuc.OncekiAyMevcut = true;
                sonuc.Surekli = true;
                sonuc.DbMaxYevmiyeNo = 0;
                sonuc.DbYevmiyeSayisi = 0;

                // Çalışılan ay içi yevmiye süreklilik kontrolü
                var yevmiyeNolar = tumFisler.Select(f => f.YevmiyeNoSayac).OrderBy(n => n).ToList();
                bool icSurekli = YevmiyeIcSureklilkKontrol(yevmiyeNolar, sonuc.Mesajlar);

                sonuc.AktarimIzinli = icSurekli;
                if (sonuc.AktarimIzinli)
                    sonuc.Mesajlar.Add("Tüm kontroller başarılı — aktarıma izin verildi.");
                else
                    sonuc.Mesajlar.Add("Aktarım ENGELLENDI — yevmiye numaralarında boşluk tespit edildi.");

                return sonuc;
            }

            // Yevmiye numarası bazlı süreklilik sorgusu (tarihten bağımsız)
            var dbBilgi = dbService.YevmiyeSureklilkBilgisiGetir(maliYil, calislanMinYevmiye, firmaNo, subeNo);
            sonuc.DbMaxYevmiyeNo = dbBilgi.MaxYevmiyeNo;
            sonuc.DbYevmiyeSayisi = dbBilgi.YevmiyeSayisi;
            sonuc.OncekiAyMevcut = dbBilgi.VeriMevcut;

            // Bilgilendirme amaçlı önceki ay tarih bazlı bilgisi
            DateTime oncekiAyBas = donemBas.AddMonths(-1);
            DateTime oncekiAyBit = donemBas.AddDays(-1);
            sonuc.OncekiAyBilgisi = dbService.AyFisBilgisiGetir(maliYil, oncekiAyBas, oncekiAyBit, firmaNo, subeNo);

            if (!sonuc.OncekiAyMevcut)
            {
                sonuc.Surekli = false;
                sonuc.AktarimIzinli = false;
                sonuc.Mesajlar.Add($"HATA: Yevmiye {calislanMinYevmiye} öncesinde DB'de hiç yevmiye bulunamadı!");
                sonuc.Mesajlar.Add("Aktarım yapılamaz — önce önceki dönemlerin fişleri oluşturulmalıdır.");
                return sonuc;
            }

            // DB bilgilerini logla
            sonuc.Mesajlar.Add($"DB'deki yevmiye sayısı (yevmiye < {calislanMinYevmiye}): {dbBilgi.YevmiyeSayisi:N0}");
            sonuc.Mesajlar.Add($"DB'deki son yevmiye no: {dbBilgi.MaxYevmiyeNo}");

            // Yevmiye sürekliliği:
            int beklenenBaslangic = dbBilgi.MaxYevmiyeNo + 1;
            sonuc.Surekli = (calislanMinYevmiye == beklenenBaslangic);

            if (sonuc.Surekli)
            {
                sonuc.Mesajlar.Add($"Yevmiye sürekliliği OK: DB son={dbBilgi.MaxYevmiyeNo}, " +
                                   $"çalışılan ay ilk={calislanMinYevmiye}");
            }
            else
            {
                sonuc.Mesajlar.Add($"UYARI: Yevmiye sürekliliği BOZUK! " +
                                   $"Beklenen başlangıç={beklenenBaslangic}, gerçek={calislanMinYevmiye}");

                int beklenenSayisi = calislanMinYevmiye - 1;
                if (dbBilgi.YevmiyeSayisi < beklenenSayisi)
                {
                    sonuc.Mesajlar.Add($"UYARI: DB'de {beklenenSayisi - dbBilgi.YevmiyeSayisi} adet eksik yevmiye var " +
                                       $"(beklenen: {beklenenSayisi}, mevcut: {dbBilgi.YevmiyeSayisi})");
                }
            }

            // Çalışılan ay içinde yevmiye numaralarının sıralı ve boşluksuz olduğunu kontrol et
            var yevmiyeNolarList = tumFisler.Select(f => f.YevmiyeNoSayac).OrderBy(n => n).ToList();
            bool icSurekliSonuc = YevmiyeIcSureklilkKontrol(yevmiyeNolarList, sonuc.Mesajlar);

            sonuc.AktarimIzinli = sonuc.OncekiAyMevcut && sonuc.Surekli && icSurekliSonuc;

            if (sonuc.AktarimIzinli)
            {
                sonuc.Mesajlar.Add("Tüm kontroller başarılı — aktarıma izin verildi.");
            }
            else if (!icSurekliSonuc)
            {
                sonuc.Mesajlar.Add("Aktarım ENGELLENDI — yevmiye numaralarında boşluk tespit edildi.");
            }
            else
            {
                sonuc.Mesajlar.Add("Aktarım ENGELLENDI — yevmiye sürekliliği sağlanamadı.");
            }

            return sonuc;
        }

        /// <summary>
        /// Ay içindeki yevmiye numaralarının boşluksuz ve ardışık olduğunu kontrol eder.
        /// </summary>
        private bool YevmiyeIcSureklilkKontrol(List<int> siraliYevmiyeNolar, List<string> mesajlar)
        {
            if (siraliYevmiyeNolar.Count <= 1) return true;

            bool surekli = true;
            for (int i = 1; i < siraliYevmiyeNolar.Count; i++)
            {
                int fark = siraliYevmiyeNolar[i] - siraliYevmiyeNolar[i - 1];
                if (fark != 1)
                {
                    surekli = false;
                    mesajlar.Add($"UYARI: Yevmiye boşluğu: {siraliYevmiyeNolar[i - 1]} → {siraliYevmiyeNolar[i]} (fark: {fark})");
                }
            }

            if (surekli)
            {
                mesajlar.Add($"Ay içi yevmiye sürekliliği OK: {siraliYevmiyeNolar.First()} - {siraliYevmiyeNolar.Last()} (boşluk yok)");
            }

            return surekli;
        }
    }
}
