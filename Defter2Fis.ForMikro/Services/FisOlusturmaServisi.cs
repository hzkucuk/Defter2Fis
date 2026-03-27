using System;
using System.Collections.Generic;
using System.Linq;
using Defter2Fis.ForMikro.Models;

namespace Defter2Fis.ForMikro.Services
{
    /// <summary>
    /// E-Defter verilerinden Mikro ERP muhasebe fişleri oluşturur.
    /// Hesap planı kontrolü, fiş yazımı ve cari/stok hareket senkronizasyonunu orkestre eder.
    /// </summary>
    public class FisOlusturmaServisi
    {
        private readonly MikroDbService _dbService;
        private readonly DefterAnalyzer _analyzer;
        private readonly EvrakBilgisiParser _evrakParser;
        private readonly LogService _log;

        public FisOlusturmaServisi(MikroDbService dbService, LogService log)
        {
            if (dbService == null) throw new ArgumentNullException(nameof(dbService));
            if (log == null) throw new ArgumentNullException(nameof(log));

            _dbService = dbService;
            _analyzer = new DefterAnalyzer();
            _evrakParser = new EvrakBilgisiParser();
            _log = log;
        }

        /// <summary>
        /// Fiş oluşturma sonuç bilgisi.
        /// </summary>
        public class OlusturmaSonucu
        {
            public int OlusturulanFisSayisi { get; set; }
            public int OlusturulanSatirSayisi { get; set; }
            public int EklenenHesapSayisi { get; set; }
            public int EslestirilenCariSayisi { get; set; }
            public int EslestirilenStokSayisi { get; set; }
            public int AtlananFisSayisi { get; set; }
            public List<string> Hatalar { get; } = new List<string>();
            public bool Basarili { get; set; }
        }

        /// <summary>
        /// Tüm fiş oluşturma sürecini yönetir.
        /// 1. Eksik hesap planı kontrolü ve ekleme
        /// 2. Cari/Stok hareketleri sorgulama
        /// 3. Evrak bilgisi parse ve eşleştirme
        /// 4. Muhasebe fişleri oluşturma (ticari referanslarla)
        /// 5. Cari/Stok hareket muhasebe fiş referansları güncelleme
        /// </summary>
        public OlusturmaSonucu FisleriOlustur(
            List<YevmiyeDefteri> defterler,
            int firmaNo,
            int subeNo,
            short dbcNo,
            Action<int, string> ilerlemeRaporla = null)
        {
            if (defterler == null || defterler.Count == 0)
                throw new ArgumentException("Defter listesi boş.", nameof(defterler));

            var sonuc = new OlusturmaSonucu();

            try
            {
                var ilkDefter = defterler[0];
                int maliYil = ilkDefter.MaliYilBaslangic.Year;
                DateTime donemBas = ilkDefter.DonemBaslangic;
                DateTime donemBit = ilkDefter.DonemBitis;

                // Adım 1: Eksik hesap planı kontrolü ve ekleme
                ilerlemeRaporla?.Invoke(5, "Hesap planı kontrol ediliyor...");
                sonuc.EklenenHesapSayisi = EksikHesaplariEkle(defterler, dbcNo);

                // Adım 2: Dönem cari/stok hareketlerini getir
                ilerlemeRaporla?.Invoke(15, "Cari hesap hareketleri sorgulanıyor...");
                var cariHareketler = _dbService.DonemCariHareketleriGetirGuvenli(donemBas, donemBit, firmaNo, subeNo);
                _log.Bilgi($"Dönem cari hareketleri: {cariHareketler.Count} kayıt");

                ilerlemeRaporla?.Invoke(20, "Stok hareketleri sorgulanıyor...");
                var stokHareketler = _dbService.DonemStokHareketleriGetirGuvenli(donemBas, donemBit, firmaNo, subeNo);
                _log.Bilgi($"Dönem stok hareketleri: {stokHareketler.Count} kayıt");

                // Cari/Stok eşleştirme index'leri: EvrakAnahtar → liste
                var cariIndex = MikroDbService.CariIndexOlustur(cariHareketler);
                var stokIndex = MikroDbService.StokIndexOlustur(stokHareketler);

                // Adım 3: Tüm fişleri tarihe göre grupla ve sıra no ata
                ilerlemeRaporla?.Invoke(30, "Fişler sıralanıyor ve evrak bilgileri parse ediliyor...");
                var tumFisler = defterler.SelectMany(d => d.Fisler).ToList();
                var tarihGruplari = tumFisler
                    .OrderBy(f => f.Satirlar.Count > 0 ? f.Satirlar[0].KayitTarihi : f.GirisTarihi)
                    .ThenBy(f => f.YevmiyeNoSayac)
                    .GroupBy(f => f.Satirlar.Count > 0 ? f.Satirlar[0].KayitTarihi.Date : f.GirisTarihi.Date)
                    .OrderBy(g => g.Key)
                    .ToList();

                // Adım 4: Fiş oluşturma
                ilerlemeRaporla?.Invoke(40, "Muhasebe fişleri oluşturuluyor...");
                int toplamFis = tumFisler.Count;
                int islenenFis = 0;

                foreach (var tarihGrubu in tarihGruplari)
                {
                    DateTime fisTarihi = tarihGrubu.Key;
                    int mevcutMaxSira = _dbService.MaxSiraNoGetir(fisTarihi, maliYil, firmaNo, subeNo);
                    int siradakiSiraNo = mevcutMaxSira + 1;

                    foreach (var yevmiyeFisi in tarihGrubu)
                    {
                        islenenFis++;
                        int yuzde = 40 + (islenenFis * 50 / toplamFis);
                        ilerlemeRaporla?.Invoke(yuzde,
                            $"Fiş yazılıyor: Yevmiye #{yevmiyeFisi.YevmiyeNoSayac} ({islenenFis}/{toplamFis})");

                        // Mükerrer kontrolü
                        if (_dbService.YevmiyeNoMevcutMu(yevmiyeFisi.YevmiyeNoSayac, maliYil, firmaNo, subeNo))
                        {
                            _log.Uyari($"Yevmiye #{yevmiyeFisi.YevmiyeNoSayac} zaten mevcut — atlanıyor.");
                            sonuc.AtlananFisSayisi++;
                            continue;
                        }

                        // Fiş satırlarını oluştur ve yaz
                        int siraNo = siradakiSiraNo++;
                        bool fisBasarili = TekFisYaz(
                            yevmiyeFisi, firmaNo, subeNo, dbcNo, maliYil, siraNo,
                            cariIndex, stokIndex, sonuc);

                        if (fisBasarili)
                        {
                            sonuc.OlusturulanFisSayisi++;
                        }
                    }
                }

                sonuc.Basarili = sonuc.Hatalar.Count == 0;

                // Adım 5: Özet rapor
                ilerlemeRaporla?.Invoke(95, "Sonuçlar raporlanıyor...");
                SonucRaporla(sonuc);
            }
            catch (Exception ex)
            {
                string detay = ex.InnerException != null
                    ? $"{ex.Message} -> {ex.InnerException.Message}"
                    : ex.Message;

                sonuc.Hatalar.Add($"Kritik hata: {detay}");
                sonuc.Basarili = false;
                _log.Hata($"Fiş oluşturma kritik hata: {detay}");
            }

            return sonuc;
        }

        /// <summary>
        /// Tek bir yevmiye fişini transaction içinde yazar.
        /// Ticari referansları ayarlar ve cari/stok referanslarını günceller.
        /// </summary>
        private bool TekFisYaz(
            YevmiyeFisi yevmiyeFisi,
            int firmaNo, int subeNo, short dbcNo, int maliYil, int siraNo,
            Dictionary<string, List<CariHesapHareketi>> cariIndex,
            Dictionary<string, List<StokHareketi>> stokIndex,
            OlusturmaSonucu sonuc)
        {
            using (var conn = _dbService.YeniBaglanti())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    int satirNo = 0;
                    DateTime fisTarihi = DateTime.MinValue;

                    // Her fiş satırı için evrak bilgisi parse et ve ticari referansları ayarla
                    var eslesmeCariGuidler = new List<Guid>();
                    var eslesmeStokGuidler = new List<Guid>();

                    foreach (var satir in yevmiyeFisi.Satirlar)
                    {
                        // Evrak bilgisi parse
                        var evrak = _evrakParser.Parse(
                            satir.DetayAciklama, satir.BelgeNo, satir.BelgeReferansi);

                        // Muhasebe fişi oluştur
                        var fis = MuhasebeFisi.FromEdDefter(
                            satir, firmaNo, subeNo, dbcNo, maliYil, siraNo, satirNo,
                            yevmiyeFisi.YevmiyeNoSayac);

                        // Ticari referansları set et (evrak eşleşmesi varsa)
                        if (evrak != null)
                        {
                            fis.FisTicEvrakSeri = evrak.Seri;
                            fis.FisTicEvrakSira = evrak.Sira;

                            // Cari eşleştirme
                            if (cariIndex.ContainsKey(evrak.Anahtar))
                            {
                                var eslesenCariListe = cariIndex[evrak.Anahtar];
                                if (eslesenCariListe.Count > 0)
                                {
                                    fis.FisTicariTip = 1; // Cari hareket
                                    fis.FisTicariUid = eslesenCariListe[0].ChaGuid;
                                    fis.FisTicariEvrakTip = eslesenCariListe[0].ChaEvrakTip;

                                    foreach (var cha in eslesenCariListe)
                                    {
                                        if (!eslesmeCariGuidler.Contains(cha.ChaGuid))
                                            eslesmeCariGuidler.Add(cha.ChaGuid);
                                    }
                                }
                            }

                            // Stok eşleştirme
                            if (stokIndex.ContainsKey(evrak.Anahtar))
                            {
                                var eslesenStokListe = stokIndex[evrak.Anahtar];
                                foreach (var sth in eslesenStokListe)
                                {
                                    if (!eslesmeStokGuidler.Contains(sth.SthGuid))
                                        eslesmeStokGuidler.Add(sth.SthGuid);
                                }

                                // Stok eşleşmesi varsa ve cari yoksa, ticari tip stok olarak ayarla
                                if (fis.FisTicariTip == 0 && eslesenStokListe.Count > 0)
                                {
                                    fis.FisTicariTip = 2; // Stok hareket
                                    fis.FisTicariUid = eslesenStokListe[0].SthGuid;
                                    fis.FisTicariEvrakTip = eslesenStokListe[0].SthEvrakTip;
                                }
                            }
                        }

                        // Fiş satırını yaz
                        _dbService.FisSatiriEkle(fis, conn, tran);
                        satirNo++;
                        fisTarihi = satir.KayitTarihi;
                    }

                    // Cari hareket muhasebe fiş referanslarını güncelle
                    foreach (var chaGuid in eslesmeCariGuidler)
                    {
                        _dbService.CariHareketMuhFisGuncelle(chaGuid, siraNo, fisTarihi, conn, tran);
                        sonuc.EslestirilenCariSayisi++;
                    }

                    // Stok hareket muhasebe fiş referanslarını güncelle
                    foreach (var sthGuid in eslesmeStokGuidler)
                    {
                        _dbService.StokHareketMuhFisGuncelle(sthGuid, siraNo, fisTarihi, conn, tran);
                        sonuc.EslestirilenStokSayisi++;
                    }

                    tran.Commit();
                    sonuc.OlusturulanSatirSayisi += satirNo;

                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    string detay = ex.InnerException != null
                        ? $"{ex.Message} -> {ex.InnerException.Message}"
                        : ex.Message;

                    string hata = $"Yevmiye #{yevmiyeFisi.YevmiyeNoSayac} yazma hatası: {detay}";
                    sonuc.Hatalar.Add(hata);
                    _log.Hata(hata);

                    return false;
                }
            }
        }

        /// <summary>
        /// Eksik hesapları tespit edip veritabanına ekler.
        /// </summary>
        private int EksikHesaplariEkle(List<YevmiyeDefteri> defterler, short dbcNo)
        {
            var mevcutKodlar = _dbService.TumHesapKodlariniGetir();
            var eksikHesaplar = _analyzer.EksikHesaplariTespit(defterler, mevcutKodlar);

            if (eksikHesaplar.Count == 0)
            {
                _log.Basari("Hesap planı tam — eksik hesap yok.");
                return 0;
            }

            _log.Bilgi($"{eksikHesaplar.Count} eksik hesap tespit edildi, ekleniyor...");

            // Hesap adlarını topla
            var hesapIsimMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var defter in defterler)
            {
                foreach (var fis in defter.Fisler)
                {
                    foreach (var satir in fis.Satirlar)
                    {
                        if (!string.IsNullOrWhiteSpace(satir.AnaHesapKod))
                            hesapIsimMap[satir.AnaHesapKod] = satir.AnaHesapAd ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(satir.AltHesapKod))
                            hesapIsimMap[satir.AltHesapKod] = satir.AltHesapAd ?? string.Empty;
                    }
                }
            }

            int eklenen = 0;
            using (var conn = _dbService.YeniBaglanti())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    foreach (var eksik in eksikHesaplar)
                    {
                        string isim = hesapIsimMap.ContainsKey(eksik.HesapKod)
                            ? hesapIsimMap[eksik.HesapKod]
                            : eksik.HesapIsim;

                        var hesap = MuhasebeHesapPlani.FromEdDefter(eksik.HesapKod, isim, dbcNo);
                        _dbService.HesapPlanıEkle(hesap, conn, tran);
                        eklenen++;
                    }

                    tran.Commit();
                    _log.Basari($"{eklenen} eksik hesap başarıyla eklendi.");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _log.Hata($"Hesap planı ekleme hatası (rollback): {ex.Message}");
                    throw;
                }
            }

            return eklenen;
        }

        /// <summary>
        /// Oluşturma sonucunu loglara yazar.
        /// </summary>
        private void SonucRaporla(OlusturmaSonucu sonuc)
        {
            _log.Bilgi("══════════════════════════════════════════");
            _log.Bilgi("      FİŞ OLUŞTURMA SONUÇ RAPORU         ");
            _log.Bilgi("══════════════════════════════════════════");
            _log.Bilgi($"  Oluşturulan fiş     : {sonuc.OlusturulanFisSayisi:N0}");
            _log.Bilgi($"  Toplam satır        : {sonuc.OlusturulanSatirSayisi:N0}");
            _log.Bilgi($"  Eklenen hesap       : {sonuc.EklenenHesapSayisi:N0}");
            _log.Bilgi($"  Eşleşen cari        : {sonuc.EslestirilenCariSayisi:N0}");
            _log.Bilgi($"  Eşleşen stok        : {sonuc.EslestirilenStokSayisi:N0}");
            _log.Bilgi($"  Atlanan (mükerrer)   : {sonuc.AtlananFisSayisi:N0}");

            if (sonuc.Hatalar.Count > 0)
            {
                _log.Uyari($"  Hata sayısı         : {sonuc.Hatalar.Count}");
                foreach (string hata in sonuc.Hatalar)
                {
                    _log.Hata($"    → {hata}");
                }
            }

            _log.Bilgi("══════════════════════════════════════════");

            if (sonuc.Basarili)
                _log.Basari("FİŞ OLUŞTURMA BAŞARIYLA TAMAMLANDI.");
            else
                _log.Uyari("FİŞ OLUŞTURMA HATALARLA TAMAMLANDI — detayları kontrol edin.");
        }
    }
}
