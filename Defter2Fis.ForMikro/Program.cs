using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Defter2Fis.ForMikro.Models;
using Defter2Fis.ForMikro.Services;

namespace Defter2Fis.ForMikro
{
    /// <summary>
    /// Defter2Fis — E-Defter XML'den Mikro ERP Muhasebe Fişi Oluşturucu
    /// Ana konsol uygulaması giriş noktası.
    /// </summary>
    internal class Program
    {
        // Sabitler: App.config'den okunacak ayarlar
        private static readonly string EdDefterRootPath = ConfigurationManager.AppSettings["EdDefterRootPath"];
        private static readonly string SicilNo = ConfigurationManager.AppSettings["SicilNo"];
        private static readonly string MaliYilAraligi = ConfigurationManager.AppSettings["MaliYilAraligi"];
        private static readonly string AyKlasoru = ConfigurationManager.AppSettings["AyKlasoru"];
        private static readonly int FirmaNo = int.Parse(ConfigurationManager.AppSettings["FirmaNo"] ?? "0");
        private static readonly int SubeNo = int.Parse(ConfigurationManager.AppSettings["SubeNo"] ?? "0");
        private static readonly short DBCNo = short.Parse(ConfigurationManager.AppSettings["DBCNo"] ?? "0");

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  DEFTER2FİS — E-Defter → Mikro ERP Muhasebe Fişi v1.0.0    ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                // 1. Ayarları göster
                AyarlariYazdir();

                // 2. Veritabanı bağlantı testi
                Console.WriteLine("[1/4] Veritabanı bağlantısı test ediliyor...");
                var dbService = new MikroDbService();
                if (!dbService.BaglantıTest())
                {
                    Console.WriteLine("  [HATA] Veritabanına bağlanılamadı! Çıkılıyor.");
                    BekleVeCik(1);
                    return;
                }
                Console.WriteLine("  [OK] Veritabanı bağlantısı başarılı.");

                // 3. E-Defter XML dosyalarını parse et
                Console.WriteLine();
                Console.WriteLine("[2/4] E-Defter Yevmiye XML dosyaları okunuyor...");
                string klasorYolu = Path.Combine(EdDefterRootPath, SicilNo, MaliYilAraligi, AyKlasoru);
                Console.WriteLine($"  Klasör: {klasorYolu}");

                var parser = new EdDefterXmlParser();
                List<YevmiyeDefteri> defterler = parser.KlasordenOku(klasorYolu, SicilNo);

                // 4. Analiz ve raporlama
                Console.WriteLine();
                Console.WriteLine("[3/4] Analiz ve doğrulama yapılıyor...");
                var analyzer = new DefterAnalyzer();

                // 4a. Genel özet
                analyzer.OzetYazdir(defterler);

                // 4b. Borç-Alacak denge kontrolü
                var dengesizler = analyzer.DengeKontrolu(defterler);
                analyzer.DengesizFisRaporuYazdir(dengesizler);

                // 4c. DB mevcut durum
                var ilkDefter = defterler[0];
                analyzer.DbDurumRaporuYazdir(dbService, ilkDefter.DonemBaslangic, ilkDefter.DonemBitis, ilkDefter.MaliYilBaslangic.Year);

                // 4d. Eksik hesap planı tespiti
                Console.WriteLine();
                Console.WriteLine("[4/4] Hesap planı kontrolü yapılıyor...");
                HashSet<string> mevcutKodlar = dbService.TumHesapKodlariniGetir();
                Console.WriteLine($"  DB'de mevcut hesap sayısı: {mevcutKodlar.Count:N0}");

                var eksikHesaplar = analyzer.EksikHesaplariTespit(defterler, mevcutKodlar);
                analyzer.EksikHesapRaporuYazdir(eksikHesaplar);

                // Sonuç özeti
                Console.WriteLine();
                Console.WriteLine("══════════════════════════════════════════════════════════════");
                Console.WriteLine("  ANALİZ TAMAMLANDI");
                Console.WriteLine($"  Dengesiz fiş   : {dengesizler.Count}");
                Console.WriteLine($"  Eksik hesap     : {eksikHesaplar.Count}");
                Console.WriteLine("══════════════════════════════════════════════════════════════");
                Console.WriteLine();
                Console.WriteLine("  Fiş oluşturma işlemi için sonraki aşamaya geçilecek.");
                Console.WriteLine("  (Bu aşamada sadece okuma ve analiz yapıldı, DB'ye yazma yok.)");

                BekleVeCik(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"  [KRİTİK HATA] {ex.GetType().Name}: {ex.Message}");
                Console.WriteLine($"  Konum: {ex.StackTrace}");
                BekleVeCik(2);
            }
        }

        /// <summary>
        /// Uygulama ayarlarını konsola yazar.
        /// </summary>
        private static void AyarlariYazdir()
        {
            Console.WriteLine("  Ayarlar:");
            Console.WriteLine($"    E-Defter Kök : {EdDefterRootPath}");
            Console.WriteLine($"    Sicil No     : {SicilNo}");
            Console.WriteLine($"    Mali Yıl     : {MaliYilAraligi}");
            Console.WriteLine($"    Ay           : {AyKlasoru}");
            Console.WriteLine($"    Firma/Şube   : {FirmaNo}/{SubeNo}  DBC: {DBCNo}");
            Console.WriteLine();
        }

        /// <summary>
        /// Çıkış öncesi kullanıcı bekletme.
        /// </summary>
        private static void BekleVeCik(int exitCode)
        {
            Console.WriteLine();
            Console.WriteLine("Çıkmak için bir tuşa basın...");
            Console.ReadKey(true);
            Environment.Exit(exitCode);
        }
    }
}
