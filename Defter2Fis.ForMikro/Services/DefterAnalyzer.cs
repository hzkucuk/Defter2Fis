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
        /// Defterlerin genel özetini konsola yazar.
        /// </summary>
        public void OzetYazdir(List<YevmiyeDefteri> defterler)
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║               E-DEFTER ANALİZ RAPORU                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");

            int toplamFis = 0;
            int toplamSatir = 0;

            foreach (var defter in defterler)
            {
                Console.WriteLine();
                Console.WriteLine($"  Dosya        : {defter.DosyaAdi}");
                Console.WriteLine($"  Defter ID    : {defter.BenzersizId}");
                Console.WriteLine($"  Firma        : {defter.FirmaUnvani}");
                Console.WriteLine($"  Sicil No     : {defter.SicilNo}");
                Console.WriteLine($"  Dönem        : {defter.DonemBaslangic:dd.MM.yyyy} - {defter.DonemBitis:dd.MM.yyyy}");
                Console.WriteLine($"  Mali Yıl     : {defter.MaliYilBaslangic:yyyy} - {defter.MaliYilBitis:yyyy}");
                Console.WriteLine($"  Fiş Sayısı   : {defter.Fisler.Count:N0}");

                int satirSayisi = defter.Fisler.Sum(f => f.Satirlar.Count);
                Console.WriteLine($"  Satır Sayısı : {satirSayisi:N0}");

                toplamFis += defter.Fisler.Count;
                toplamSatir += satirSayisi;
            }

            Console.WriteLine();
            Console.WriteLine($"  ─── TOPLAM: {defterler.Count} dosya, {toplamFis:N0} fiş, {toplamSatir:N0} satır ───");
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
        /// Eksik hesap raporunu konsola yazar.
        /// </summary>
        public void EksikHesapRaporuYazdir(List<EksikHesap> eksikler)
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║             EKSİK HESAP PLANI RAPORU                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");

            if (eksikler.Count == 0)
            {
                Console.WriteLine("  Tüm hesaplar hesap planında mevcut. Eksik hesap yok.");
                return;
            }

            Console.WriteLine($"  Toplam {eksikler.Count} eksik hesap tespit edildi:");
            Console.WriteLine();
            Console.WriteLine($"  {"Hesap Kodu",-20} {"Tip",-5} {"Hesap İsmi",-50}");
            Console.WriteLine($"  {"".PadRight(20, '─')} {"".PadRight(5, '─')} {"".PadRight(50, '─')}");

            foreach (var eksik in eksikler)
            {
                string tipAd = eksik.HesapTip == 0 ? "Ana" :
                               eksik.HesapTip == 1 ? "Alt" : $"D{eksik.HesapTip}";
                Console.WriteLine($"  {eksik.HesapKod,-20} {tipAd,-5} {eksik.HesapIsim,-50}");
            }
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
        /// Dengesiz fiş raporunu konsola yazar.
        /// </summary>
        public void DengesizFisRaporuYazdir(List<DengeSizFis> dengesizler)
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║            BORÇ-ALACAK DENGE KONTROLÜ                       ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");

            if (dengesizler.Count == 0)
            {
                Console.WriteLine("  Tüm fişler dengeli. Borç = Alacak ✓");
                return;
            }

            Console.WriteLine($"  [UYARI] {dengesizler.Count} adet dengesiz fiş tespit edildi!");
            Console.WriteLine();

            foreach (var d in dengesizler)
            {
                Console.WriteLine($"  Yevmiye: {d.YevmiyeNo} (#{d.YevmiyeNoSayac}) — " +
                                  $"Borç: {d.ToplamBorc:N2} Alacak: {d.ToplamAlacak:N2} Fark: {d.Fark:N2}");
            }
        }

        /// <summary>
        /// DB mevcut durum istatistiğini yazar.
        /// </summary>
        public void DbDurumRaporuYazdir(MikroDbService dbService, DateTime donemBas, DateTime donemBit, int maliYil)
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║               VERİTABANI MEVCUT DURUM                       ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");

            int hesapSayisi = dbService.HesapPlaniSayisi();
            int fisSayisi = dbService.MevcutFisSayisi(maliYil, donemBas, donemBit, 0, 0);

            Console.WriteLine($"  Hesap Planı  : {hesapSayisi:N0} kayıt");
            Console.WriteLine($"  Mevcut Fişler: {fisSayisi:N0} yevmiye ({donemBas:dd.MM.yyyy} - {donemBit:dd.MM.yyyy})");
        }
    }

    /// <summary>
    /// Eksik hesap bilgisi.
    /// </summary>
    public class EksikHesap
    {
        public string HesapKod { get; set; }
        public string HesapIsim { get; set; }
        public byte HesapTip { get; set; }
    }

    /// <summary>
    /// Borç-Alacak dengesiz fiş bilgisi.
    /// </summary>
    public class DengeSizFis
    {
        public string DosyaAdi { get; set; }
        public string YevmiyeNo { get; set; }
        public int YevmiyeNoSayac { get; set; }
        public decimal ToplamBorc { get; set; }
        public decimal ToplamAlacak { get; set; }
        public decimal Fark { get; set; }
    }
}
