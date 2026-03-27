using System;
using System.Collections.Generic;
using NUnit.Framework;
using Defter2Fis.ForMikro.Models;
using Defter2Fis.ForMikro.Services;

namespace Defter2Fis.Tests
{
    [TestFixture]
    public class DefterAnalyzerTests
    {
        private DefterAnalyzer _analyzer;

        [SetUp]
        public void SetUp()
        {
            _analyzer = new DefterAnalyzer();
        }

        #region AraSeviyeleriUret

        [Test]
        public void WhenUcSeviyeKodThenIkiAraSeviyeUretir()
        {
            var sonuc = _analyzer.AraSeviyeleriUret("770.50.0018");

            Assert.That(sonuc.Count, Is.EqualTo(2));
            Assert.That(sonuc[0], Is.EqualTo("770"));
            Assert.That(sonuc[1], Is.EqualTo("770.50"));
        }

        [Test]
        public void WhenIkiSeviyeKodThenBirAraSeviyeUretir()
        {
            var sonuc = _analyzer.AraSeviyeleriUret("100.01");

            Assert.That(sonuc.Count, Is.EqualTo(1));
            Assert.That(sonuc[0], Is.EqualTo("100"));
        }

        [Test]
        public void WhenTekSeviyeKodThenBosSonucDoner()
        {
            var sonuc = _analyzer.AraSeviyeleriUret("770");

            Assert.That(sonuc, Is.Empty);
        }

        [Test]
        public void WhenDortSeviyeKodThenUcAraSeviyeUretir()
        {
            var sonuc = _analyzer.AraSeviyeleriUret("770.50.001.0018");

            Assert.That(sonuc.Count, Is.EqualTo(3));
            Assert.That(sonuc[0], Is.EqualTo("770"));
            Assert.That(sonuc[1], Is.EqualTo("770.50"));
            Assert.That(sonuc[2], Is.EqualTo("770.50.001"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void WhenBosVeyaNullKodThenBosSonucDoner(string kod)
        {
            var sonuc = _analyzer.AraSeviyeleriUret(kod);

            Assert.That(sonuc, Is.Empty);
        }

        #endregion

        #region OzetHesapla

        [Test]
        public void WhenTekDefterThenOzetDogruHesaplanir()
        {
            var defter = DefterOlustur("test.xml", "BID001", fisSayisi: 2, satirPerFis: 3);
            var defterler = new List<YevmiyeDefteri> { defter };

            var ozet = _analyzer.OzetHesapla(defterler);

            Assert.That(ozet.DosyaSayisi, Is.EqualTo(1));
            Assert.That(ozet.ToplamFis, Is.EqualTo(2));
            Assert.That(ozet.ToplamSatir, Is.EqualTo(6));
            Assert.That(ozet.DosyaOzetleri.Count, Is.EqualTo(1));
            Assert.That(ozet.DosyaOzetleri[0].DosyaAdi, Is.EqualTo("test.xml"));
        }

        [Test]
        public void WhenCokluDefterThenToplamlarDogruToplanir()
        {
            var defter1 = DefterOlustur("d1.xml", "B1", fisSayisi: 3, satirPerFis: 2);
            var defter2 = DefterOlustur("d2.xml", "B2", fisSayisi: 1, satirPerFis: 5);
            var defterler = new List<YevmiyeDefteri> { defter1, defter2 };

            var ozet = _analyzer.OzetHesapla(defterler);

            Assert.That(ozet.DosyaSayisi, Is.EqualTo(2));
            Assert.That(ozet.ToplamFis, Is.EqualTo(4));
            Assert.That(ozet.ToplamSatir, Is.EqualTo(11));
        }

        [Test]
        public void WhenBosDefterListesiThenSifirOzet()
        {
            var ozet = _analyzer.OzetHesapla(new List<YevmiyeDefteri>());

            Assert.That(ozet.DosyaSayisi, Is.EqualTo(0));
            Assert.That(ozet.ToplamFis, Is.EqualTo(0));
            Assert.That(ozet.ToplamSatir, Is.EqualTo(0));
        }

        #endregion

        #region BenzersizHesapKodlari

        [Test]
        public void WhenFarkliHesapKodlariThenTumuToplanir()
        {
            var defter = YeniDefter();
            var fis = YeniFis(1);
            fis.Satirlar.Add(YeniSatir("100", "100.01", "D", 1000m));
            fis.Satirlar.Add(YeniSatir("320", "320.01.001", "C", 1000m));
            defter.Fisler.Add(fis);

            var kodlar = _analyzer.BenzersizHesapKodlari(new List<YevmiyeDefteri> { defter });

            // 100, 100.01, 320, 320.01, 320.01.001
            Assert.That(kodlar, Does.Contain("100"));
            Assert.That(kodlar, Does.Contain("100.01"));
            Assert.That(kodlar, Does.Contain("320"));
            Assert.That(kodlar, Does.Contain("320.01"));
            Assert.That(kodlar, Does.Contain("320.01.001"));
        }

        [Test]
        public void WhenAyniKodBirdenFazlaThenTekSeferEklenir()
        {
            var defter = YeniDefter();
            var fis = YeniFis(1);
            fis.Satirlar.Add(YeniSatir("770", "770.50.0018", "D", 500m));
            fis.Satirlar.Add(YeniSatir("770", "770.50.0018", "C", 500m));
            defter.Fisler.Add(fis);

            var kodlar = _analyzer.BenzersizHesapKodlari(new List<YevmiyeDefteri> { defter });

            // 770, 770.50, 770.50.0018 — her biri bir kere
            Assert.That(kodlar.Count, Is.EqualTo(3));
        }

        [Test]
        public void WhenBosDefterThenBosSetDoner()
        {
            var kodlar = _analyzer.BenzersizHesapKodlari(new List<YevmiyeDefteri>());

            Assert.That(kodlar, Is.Empty);
        }

        #endregion

        #region EksikHesaplariTespit

        [Test]
        public void WhenDbdeOlmayanKodVarsaThenEksikListesindeGosterir()
        {
            var defter = YeniDefter();
            var fis = YeniFis(1);
            fis.Satirlar.Add(YeniSatir("770", "770.50.0018", "D", 1000m, altHesapAd: "SERTİFİKA GİDERİ"));
            defter.Fisler.Add(fis);

            var mevcutKodlar = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "770" };

            var eksikler = _analyzer.EksikHesaplariTespit(
                new List<YevmiyeDefteri> { defter }, mevcutKodlar);

            Assert.That(eksikler.Count, Is.EqualTo(2)); // 770.50 ve 770.50.0018
            Assert.That(eksikler.Exists(e => e.HesapKod == "770.50"), Is.True);
            Assert.That(eksikler.Exists(e => e.HesapKod == "770.50.0018"), Is.True);
        }

        [Test]
        public void WhenTumKodlarMevcutThenBosListeDoner()
        {
            var defter = YeniDefter();
            var fis = YeniFis(1);
            fis.Satirlar.Add(YeniSatir("100", "100.01", "D", 500m));
            defter.Fisler.Add(fis);

            var mevcutKodlar = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "100", "100.01" };

            var eksikler = _analyzer.EksikHesaplariTespit(
                new List<YevmiyeDefteri> { defter }, mevcutKodlar);

            Assert.That(eksikler, Is.Empty);
        }

        [Test]
        public void WhenAraSeviyeEksikThenAraSeviyeIcinIsimParantezli()
        {
            var defter = YeniDefter();
            var fis = YeniFis(1);
            fis.Satirlar.Add(YeniSatir("770", "770.50.0018", "D", 100m, altHesapAd: "TEST"));
            defter.Fisler.Add(fis);

            // 770 ve 770.50.0018 mevcut, ama 770.50 yok
            var mevcutKodlar = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "770", "770.50.0018" };

            var eksikler = _analyzer.EksikHesaplariTespit(
                new List<YevmiyeDefteri> { defter }, mevcutKodlar);

            Assert.That(eksikler.Count, Is.EqualTo(1));
            Assert.That(eksikler[0].HesapKod, Is.EqualTo("770.50"));
            Assert.That(eksikler[0].HesapIsim, Is.EqualTo("(Ara seviye)"));
        }

        [Test]
        public void WhenEksikHesapThenHesapTipDogruBelirlenir()
        {
            var defter = YeniDefter();
            var fis = YeniFis(1);
            fis.Satirlar.Add(YeniSatir("770", "770.50.0018", "D", 100m));
            defter.Fisler.Add(fis);

            var mevcutKodlar = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var eksikler = _analyzer.EksikHesaplariTespit(
                new List<YevmiyeDefteri> { defter }, mevcutKodlar);

            var anaGrup = eksikler.Find(e => e.HesapKod == "770");
            var altGrup = eksikler.Find(e => e.HesapKod == "770.50");
            var detay = eksikler.Find(e => e.HesapKod == "770.50.0018");

            Assert.That(anaGrup, Is.Not.Null);
            Assert.That(anaGrup.HesapTip, Is.EqualTo(0)); // 0 nokta = Ana grup
            Assert.That(altGrup.HesapTip, Is.EqualTo(1)); // 1 nokta = Alt grup
            Assert.That(detay.HesapTip, Is.EqualTo(2));   // 2 nokta = Detay
        }

        #endregion

        #region DengeKontrolu

        [Test]
        public void WhenBorcAlacakEsitThenDengesizListeBos()
        {
            var defter = YeniDefter();
            var fis = YeniFis(1);
            fis.Satirlar.Add(YeniSatir("770", "770.50", "D", 1000m));
            fis.Satirlar.Add(YeniSatir("100", "100.01", "C", 1000m));
            defter.Fisler.Add(fis);

            var dengesizler = _analyzer.DengeKontrolu(new List<YevmiyeDefteri> { defter });

            Assert.That(dengesizler, Is.Empty);
        }

        [Test]
        public void WhenBorcAlacakFarkliThenDengesizFisListelenir()
        {
            var defter = YeniDefter();
            var fis = YeniFis(1);
            fis.Satirlar.Add(YeniSatir("770", "770.50", "D", 1000m));
            fis.Satirlar.Add(YeniSatir("100", "100.01", "C", 900m));
            defter.Fisler.Add(fis);

            var dengesizler = _analyzer.DengeKontrolu(new List<YevmiyeDefteri> { defter });

            Assert.That(dengesizler.Count, Is.EqualTo(1));
            Assert.That(dengesizler[0].ToplamBorc, Is.EqualTo(1000m));
            Assert.That(dengesizler[0].ToplamAlacak, Is.EqualTo(900m));
            Assert.That(dengesizler[0].Fark, Is.EqualTo(100m));
        }

        [Test]
        public void WhenFarkKurusToleransIcindeThenDengeliSayilir()
        {
            var defter = YeniDefter();
            var fis = YeniFis(1);
            fis.Satirlar.Add(YeniSatir("770", "770.50", "D", 1000.005m));
            fis.Satirlar.Add(YeniSatir("100", "100.01", "C", 1000.00m));
            defter.Fisler.Add(fis);

            var dengesizler = _analyzer.DengeKontrolu(new List<YevmiyeDefteri> { defter });

            // Fark = 0.005 < 0.01 tolerans → dengeli sayılır
            Assert.That(dengesizler, Is.Empty);
        }

        [Test]
        public void WhenFarkToleransSinirindaThenDengeliSayilir()
        {
            var defter = YeniDefter();
            var fis = YeniFis(1);
            fis.Satirlar.Add(YeniSatir("770", "770.50", "D", 1000.01m));
            fis.Satirlar.Add(YeniSatir("100", "100.01", "C", 1000.00m));
            defter.Fisler.Add(fis);

            var dengesizler = _analyzer.DengeKontrolu(new List<YevmiyeDefteri> { defter });

            // Fark = 0.01 = 0.01 tolerans → dengeli (> 0.01 değil)
            Assert.That(dengesizler, Is.Empty);
        }

        [Test]
        public void WhenFarkToleransUzerindeThenDengesiz()
        {
            var defter = YeniDefter();
            var fis = YeniFis(1);
            fis.Satirlar.Add(YeniSatir("770", "770.50", "D", 1000.02m));
            fis.Satirlar.Add(YeniSatir("100", "100.01", "C", 1000.00m));
            defter.Fisler.Add(fis);

            var dengesizler = _analyzer.DengeKontrolu(new List<YevmiyeDefteri> { defter });

            // Fark = 0.02 > 0.01 tolerans → dengesiz
            Assert.That(dengesizler.Count, Is.EqualTo(1));
        }

        [Test]
        public void WhenBirdenFazlaFisThenHerBiriAyriKontrolEdilir()
        {
            var defter = YeniDefter();

            var fisDengeli = YeniFis(1);
            fisDengeli.Satirlar.Add(YeniSatir("770", "770.50", "D", 500m));
            fisDengeli.Satirlar.Add(YeniSatir("100", "100.01", "C", 500m));
            defter.Fisler.Add(fisDengeli);

            var fisDengesiz = YeniFis(2);
            fisDengesiz.Satirlar.Add(YeniSatir("770", "770.50", "D", 1000m));
            fisDengesiz.Satirlar.Add(YeniSatir("100", "100.01", "C", 800m));
            defter.Fisler.Add(fisDengesiz);

            var dengesizler = _analyzer.DengeKontrolu(new List<YevmiyeDefteri> { defter });

            Assert.That(dengesizler.Count, Is.EqualTo(1));
            Assert.That(dengesizler[0].YevmiyeNoSayac, Is.EqualTo(2));
        }

        #endregion

        #region Yardımcı Metodlar

        private static YevmiyeDefteri YeniDefter(string dosyaAdi = "test.xml")
        {
            return new YevmiyeDefteri
            {
                DosyaAdi = dosyaAdi,
                BenzersizId = "BID001",
                DonemBaslangic = new DateTime(2025, 4, 1),
                DonemBitis = new DateTime(2025, 4, 30),
                MaliYilBaslangic = new DateTime(2025, 1, 1),
                MaliYilBitis = new DateTime(2025, 12, 31)
            };
        }

        private static YevmiyeFisi YeniFis(int yevmiyeNoSayac)
        {
            return new YevmiyeFisi
            {
                YevmiyeNo = yevmiyeNoSayac.ToString("D10"),
                YevmiyeNoSayac = yevmiyeNoSayac,
                GirisTarihi = new DateTime(2025, 4, 1)
            };
        }

        private static FisDetaySatiri YeniSatir(
            string anaHesapKod,
            string altHesapKod,
            string borcAlacak,
            decimal tutar,
            string altHesapAd = null)
        {
            return new FisDetaySatiri
            {
                AnaHesapKod = anaHesapKod,
                AltHesapKod = altHesapKod,
                AltHesapAd = altHesapAd,
                BorcAlacakKodu = borcAlacak,
                Tutar = tutar,
                KayitTarihi = new DateTime(2025, 4, 1)
            };
        }

        private static YevmiyeDefteri DefterOlustur(
            string dosyaAdi, string benzersizId,
            int fisSayisi, int satirPerFis)
        {
            var defter = new YevmiyeDefteri
            {
                DosyaAdi = dosyaAdi,
                BenzersizId = benzersizId,
                DonemBaslangic = new DateTime(2025, 4, 1),
                DonemBitis = new DateTime(2025, 4, 30)
            };

            for (int f = 0; f < fisSayisi; f++)
            {
                var fis = new YevmiyeFisi
                {
                    YevmiyeNo = (f + 1).ToString("D10"),
                    YevmiyeNoSayac = f + 1,
                    GirisTarihi = new DateTime(2025, 4, 1)
                };

                for (int s = 0; s < satirPerFis; s++)
                {
                    fis.Satirlar.Add(new FisDetaySatiri
                    {
                        SatirNo = s,
                        AnaHesapKod = "100",
                        AltHesapKod = $"100.0{s}",
                        Tutar = 100m,
                        BorcAlacakKodu = s % 2 == 0 ? "D" : "C",
                        KayitTarihi = new DateTime(2025, 4, 1)
                    });
                }

                defter.Fisler.Add(fis);
            }

            return defter;
        }

        #endregion
    }
}
