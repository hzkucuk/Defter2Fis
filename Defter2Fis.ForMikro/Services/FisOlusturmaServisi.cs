using System;
using System.Collections.Generic;
using System.Linq;
using Defter2Fis.ForMikro.Models;

namespace Defter2Fis.ForMikro.Services
{
    /// <summary>
    /// E-Defter verilerinden Mikro ERP muhasebe fişleri oluşturur.
    /// Hesap planı kontrolü ve fiş yazımını orkestre eder.
    /// </summary>
    public class FisOlusturmaServisi
    {
        private readonly IMikroDbService _dbService;
        private readonly DefterAnalyzer _analyzer;
        private readonly EvrakBilgisiParser _evrakParser;
        private readonly LogService _log;

        public FisOlusturmaServisi(IMikroDbService dbService, LogService log)
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
            public int AtlananFisSayisi { get; set; }
            public bool SimulasyonBasarili { get; set; }
            public List<string> Hatalar { get; } = new List<string>();
            public bool Basarili { get; set; }
        }

        /// <summary>
        /// Simülasyonda oluşturulan tek bir fişin tüm verilerini taşır.
        /// Bellekte saklanır, doğrulama sonrası toplu yazıma gönderilir.
        /// </summary>
        private class SimulasyonFis
        {
            public int YevmiyeNoSayac;
            public int SiraNo;
            public DateTime FisTarihi;
            public List<MuhasebeFisi> FisSatirlari = new List<MuhasebeFisi>();
        }

        /// <summary>
        /// Tüm fiş oluşturma sürecini yönetir (simülasyon-önce yaklaşım).
        /// 1. Eksik hesap planı kontrolü ve ekleme
        /// 2. Cari/Stok hareketleri sorgulama (ticari eşleştirme bilgisi için)
        /// 3. SİMÜLASYON: Tüm fişleri bellekte oluştur ve doğrula
        /// 4. YAZIM: Tüm fişleri tek transaction içinde atomik yaz
        /// Hata durumunda tüm ay için rollback yapılır.
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

                var cariIndex = MikroDbService.CariIndexOlustur(cariHareketler);
                var stokIndex = MikroDbService.StokIndexOlustur(stokHareketler);

                // Adım 3: SİMÜLASYON — tüm fişleri bellekte oluştur
                ilerlemeRaporla?.Invoke(30, "Simülasyon başlatılıyor...");
                _log.Bilgi("══════════════════════════════════════════");
                _log.Bilgi("  SİMÜLASYON AŞAMASI (DB'ye yazılmıyor)  ");
                _log.Bilgi("══════════════════════════════════════════");

                var simulasyonFisler = SimulasyonCalistir(
                    defterler, firmaNo, subeNo, dbcNo, maliYil,
                    cariIndex, stokIndex, sonuc, ilerlemeRaporla,
                    donemBas, donemBit);

                if (sonuc.Hatalar.Count > 0)
                {
                    sonuc.SimulasyonBasarili = false;
                    sonuc.Basarili = false;
                    _log.Hata($"Simülasyon {sonuc.Hatalar.Count} hata ile başarısız — DB'ye yazım iptal edildi.");
                    SonucRaporla(sonuc);
                    return sonuc;
                }

                sonuc.SimulasyonBasarili = true;
                _log.Basari($"Simülasyon başarılı: {simulasyonFisler.Count} fiş, " +
                            $"{simulasyonFisler.Sum(f => f.FisSatirlari.Count)} satır hazırlandı.");

                // Adım 4: ATOMIK YAZIM — tek transaction içinde tüm ayı yaz
                ilerlemeRaporla?.Invoke(70, "Atomik yazım başlatılıyor...");
                _log.Bilgi("══════════════════════════════════════════");
                _log.Bilgi("  ATOMIK YAZIM (tek transaction)          ");
                _log.Bilgi("══════════════════════════════════════════");

                TopluFisYaz(simulasyonFisler, sonuc, ilerlemeRaporla);

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
        /// Simülasyon: Tüm fişleri bellekte oluşturur, doğrular ama DB'ye yazmaz.
        /// Mükerrer kontrolü, sıra no hesabı ve ticari eşleştirme bu aşamada yapılır.
        /// </summary>
        private List<SimulasyonFis> SimulasyonCalistir(
            List<YevmiyeDefteri> defterler,
            int firmaNo, int subeNo, short dbcNo, int maliYil,
            Dictionary<string, List<CariHesapHareketi>> cariIndex,
            Dictionary<string, List<StokHareketi>> stokIndex,
            OlusturmaSonucu sonuc,
            Action<int, string> ilerlemeRaporla,
            DateTime donemBas, DateTime donemBit)
        {
            var simulasyonFisler = new List<SimulasyonFis>();

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
                int mevcutMaxSira = _dbService.MaxSiraNoGetir(fisTarihi, maliYil, firmaNo, subeNo);
                int siradakiSiraNo = mevcutMaxSira + 1;

                foreach (var yevmiyeFisi in tarihGrubu)
                {
                    islenenFis++;
                    int yuzde = 30 + (islenenFis * 35 / toplamFis);
                    ilerlemeRaporla?.Invoke(yuzde,
                        $"Simülasyon: Yevmiye #{yevmiyeFisi.YevmiyeNoSayac} ({islenenFis}/{toplamFis})");

                    // Mükerrer kontrolü (dönem bazlı)
                    if (_dbService.YevmiyeNoMevcutMu(yevmiyeFisi.YevmiyeNoSayac, maliYil, firmaNo, subeNo,
                        donemBas, donemBit))
                    {
                        _log.Uyari($"Yevmiye #{yevmiyeFisi.YevmiyeNoSayac} zaten mevcut — atlanıyor.");
                        sonuc.AtlananFisSayisi++;
                        continue;
                    }

                    int siraNo = siradakiSiraNo++;
                    var simFis = new SimulasyonFis
                    {
                        YevmiyeNoSayac = yevmiyeFisi.YevmiyeNoSayac,
                        SiraNo = siraNo,
                        FisTarihi = fisTarihi
                    };

                    // Tarih+SıraNo bazlı eşleştirme (cha_fis_tarih + cha_fis_sirano / sth_fis_tarihi + sth_fis_sirano)
                    string fisAnahtar = $"{fisTarihi:yyyy-MM-dd}|{siraNo}";

                    int satirNo = 0;
                    foreach (var satir in yevmiyeFisi.Satirlar)
                    {
                        var evrak = _evrakParser.Parse(
                            satir.DetayAciklama, satir.BelgeNo, satir.BelgeReferansi);

                        var fis = MuhasebeFisi.FromEdDefter(
                            satir, firmaNo, subeNo, dbcNo, maliYil, siraNo, satirNo,
                            yevmiyeFisi.YevmiyeNoSayac);

                        if (evrak != null)
                        {
                            fis.FisTicEvrakSeri = evrak.Seri;
                            fis.FisTicEvrakSira = evrak.Sira;
                        }

                        // Ticari eşleştirme bilgisini ilk satıra ata
                        if (satirNo == 0)
                        {
                            if (cariIndex.ContainsKey(fisAnahtar))
                            {
                                fis.FisTicariTip = 1;
                                fis.FisTicariUid = cariIndex[fisAnahtar][0].ChaGuid;
                                fis.FisTicariEvrakTip = cariIndex[fisAnahtar][0].ChaEvrakTip;
                            }
                            else if (stokIndex.ContainsKey(fisAnahtar))
                            {
                                fis.FisTicariTip = 2;
                                fis.FisTicariUid = stokIndex[fisAnahtar][0].SthGuid;
                                fis.FisTicariEvrakTip = stokIndex[fisAnahtar][0].SthEvrakTip;
                            }
                        }

                        simFis.FisSatirlari.Add(fis);
                        satirNo++;
                    }

                    simulasyonFisler.Add(simFis);
                }
            }

            return simulasyonFisler;
        }

        /// <summary>
        /// Simülasyondaki tüm fişleri tek bir transaction içinde atomik olarak yazar.
        /// Herhangi bir hata durumunda tüm ay rollback edilir.
        /// </summary>
        private void TopluFisYaz(
            List<SimulasyonFis> simulasyonFisler,
            OlusturmaSonucu sonuc,
            Action<int, string> ilerlemeRaporla)
        {
            if (simulasyonFisler.Count == 0)
            {
                _log.Bilgi("Yazılacak fiş yok.");
                return;
            }

            using (var conn = _dbService.YeniBaglanti())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    int toplamFis = simulasyonFisler.Count;
                    int yazilan = 0;

                    foreach (var simFis in simulasyonFisler)
                    {
                        yazilan++;
                        int yuzde = 70 + (yazilan * 20 / toplamFis);
                        ilerlemeRaporla?.Invoke(yuzde,
                            $"Yazılıyor: Yevmiye #{simFis.YevmiyeNoSayac} ({yazilan}/{toplamFis})");

                        // Fiş satırlarını yaz
                        foreach (var fis in simFis.FisSatirlari)
                        {
                            _dbService.FisSatiriEkle(fis, conn, tran);
                        }

                        sonuc.OlusturulanFisSayisi++;
                        sonuc.OlusturulanSatirSayisi += simFis.FisSatirlari.Count;
                    }

                    // Tüm ay başarılı — commit
                    tran.Commit();
                    _log.Basari($"Atomik yazım başarılı: {yazilan} fiş, {sonuc.OlusturulanSatirSayisi} satır DB'ye yazıldı.");
                }
                catch (Exception ex)
                {
                    // Hata — tüm ay rollback
                    tran.Rollback();

                    string detay = ex.InnerException != null
                        ? $"{ex.Message} -> {ex.InnerException.Message}"
                        : ex.Message;

                    string hata = $"Atomik yazım BAŞARISIZ — tüm ay ROLLBACK yapıldı: {detay}";
                    sonuc.Hatalar.Add(hata);
                    sonuc.OlusturulanFisSayisi = 0;
                    sonuc.OlusturulanSatirSayisi = 0;

                    _log.Hata(hata);
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
            _log.Bilgi($"  Simülasyon          : {(sonuc.SimulasyonBasarili ? "BAŞARILI" : "BAŞARISIZ")}");
            _log.Bilgi($"  Oluşturulan fiş     : {sonuc.OlusturulanFisSayisi:N0}");
            _log.Bilgi($"  Toplam satır        : {sonuc.OlusturulanSatirSayisi:N0}");
            _log.Bilgi($"  Eklenen hesap       : {sonuc.EklenenHesapSayisi:N0}");
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
