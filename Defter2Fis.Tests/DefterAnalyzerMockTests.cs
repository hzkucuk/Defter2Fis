using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Defter2Fis.ForMikro.Models;
using Defter2Fis.ForMikro.Services;

namespace Defter2Fis.Tests
{
    [TestFixture]
    public class DefterAnalyzerMockTests
    {
        private DefterAnalyzer _analyzer;
        private Mock<IMikroDbService> _mockDbService;

        [SetUp]
        public void SetUp()
        {
            _analyzer = new DefterAnalyzer();
            _mockDbService = new Mock<IMikroDbService>(MockBehavior.Strict);
        }

        #region OncekiAyDogrula — İlk Ay (yevmiye 1'den başlıyor)

        [Test]
        public void WhenIlkAyVeArdisikYevmiyelerThenAktarimIzinli()
        {
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 1, yevmiyeSayisi: 5);

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.IlkAy, Is.True);
            Assert.That(sonuc.OncekiAyMevcut, Is.True);
            Assert.That(sonuc.Surekli, Is.True);
            Assert.That(sonuc.AktarimIzinli, Is.True);
            Assert.That(sonuc.DbMaxYevmiyeNo, Is.EqualTo(0));
            Assert.That(sonuc.DbYevmiyeSayisi, Is.EqualTo(0));
        }

        [Test]
        public void WhenIlkAyAmaYevmiyeBooslukVarsaThenAktarimEngellenir()
        {
            // Yevmiye 1, 2, 4 (3 eksik)
            var defterler = DefterlerOlusturOzel(new[] { 1, 2, 4 });

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.IlkAy, Is.True);
            Assert.That(sonuc.AktarimIzinli, Is.False);
            Assert.That(sonuc.Mesajlar.Any(m => m.Contains("boşluk")), Is.True);
        }

        [Test]
        public void WhenIlkAyTekFisThenAktarimIzinli()
        {
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 1, yevmiyeSayisi: 1);

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.IlkAy, Is.True);
            Assert.That(sonuc.AktarimIzinli, Is.True);
        }

        #endregion

        #region OncekiAyDogrula — Normal Süreklilik (ilk ay değil)

        [Test]
        public void WhenDbSureklilkTamamThenAktarimIzinli()
        {
            // DB'de yevmiye 1-10, XML'de 11-15
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 11, yevmiyeSayisi: 5);

            MockDbSureklilk(
                calislanMinYevmiye: 11,
                dbYevmiyeSayisi: 10,
                dbMaxYevmiyeNo: 10);
            MockAyFisBilgisi();

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.IlkAy, Is.False);
            Assert.That(sonuc.OncekiAyMevcut, Is.True);
            Assert.That(sonuc.Surekli, Is.True);
            Assert.That(sonuc.AktarimIzinli, Is.True);
            Assert.That(sonuc.DbMaxYevmiyeNo, Is.EqualTo(10));
        }

        [Test]
        public void WhenDbMaxPlusOneCalislanMinEsitThenSurekli()
        {
            // DB max = 50, XML min = 51 → 50 + 1 = 51 ✓
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 51, yevmiyeSayisi: 3);

            MockDbSureklilk(calislanMinYevmiye: 51, dbYevmiyeSayisi: 50, dbMaxYevmiyeNo: 50);
            MockAyFisBilgisi();

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.Surekli, Is.True);
            Assert.That(sonuc.AktarimIzinli, Is.True);
        }

        #endregion

        #region OncekiAyDogrula — Boşluk ve Hata Durumları

        [Test]
        public void WhenDbdeHicYevmiyeYokThenAktarimEngellenir()
        {
            // XML yevmiye 11'den başlıyor ama DB'de hiç veri yok
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 11, yevmiyeSayisi: 3);

            MockDbSureklilk(calislanMinYevmiye: 11, dbYevmiyeSayisi: 0, dbMaxYevmiyeNo: 0);
            MockAyFisBilgisi();

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.OncekiAyMevcut, Is.False);
            Assert.That(sonuc.AktarimIzinli, Is.False);
            Assert.That(sonuc.Mesajlar.Any(m => m.Contains("HATA")), Is.True);
        }

        [Test]
        public void WhenYevmiyeNumarasindaBooslukThenSurekliDegil()
        {
            // DB max = 8, XML min = 11 → 8 + 1 = 9 ≠ 11 → boşluk
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 11, yevmiyeSayisi: 3);

            MockDbSureklilk(calislanMinYevmiye: 11, dbYevmiyeSayisi: 8, dbMaxYevmiyeNo: 8);
            MockAyFisBilgisi();

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.Surekli, Is.False);
            Assert.That(sonuc.AktarimIzinli, Is.False);
            Assert.That(sonuc.Mesajlar.Any(m => m.Contains("BOZUK")), Is.True);
        }

        [Test]
        public void WhenDbMaxBuyukCalislanMinThenSurekliDegil()
        {
            // DB max = 15, XML min = 11 → çakışma/geri atlama
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 11, yevmiyeSayisi: 3);

            MockDbSureklilk(calislanMinYevmiye: 11, dbYevmiyeSayisi: 15, dbMaxYevmiyeNo: 15);
            MockAyFisBilgisi();

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.Surekli, Is.False);
            Assert.That(sonuc.AktarimIzinli, Is.False);
        }

        [Test]
        public void WhenAyIciYevmiyeBooslukVarsaThenAktarimEngellenir()
        {
            // DB sürekli ama XML içinde boşluk: 11, 12, 14 (13 eksik)
            // Her fis 1 satır, satirBaslangic=11 → satirlar: 11,12,13 (iç süreklilik OK)
            var defterler = DefterlerOlusturOzel(new[] { 11, 12, 14 }, satirBaslangic: 11);

            MockDbSureklilk(calislanMinYevmiye: 11, dbYevmiyeSayisi: 10, dbMaxYevmiyeNo: 10, dbToplamSatir: 10);
            MockAyFisBilgisi();

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.OncekiAyMevcut, Is.True);
            Assert.That(sonuc.Surekli, Is.True); // DB sürekliliği tamam
            Assert.That(sonuc.AktarimIzinli, Is.False); // Ama iç boşluk var
        }

        #endregion

        #region OncekiAyDogrula — Hata Parametreleri

        [Test]
        public void WhenDbServiceNullThenArgumentNullException()
        {
            var defterler = DefterlerOlustur(1, 1);

            Assert.Throws<ArgumentNullException>(
                () => _analyzer.OncekiAyDogrula(null, defterler, 0, 0));
        }

        [Test]
        public void WhenDefterlerNullThenArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => _analyzer.OncekiAyDogrula(_mockDbService.Object, null, 0, 0));
        }

        [Test]
        public void WhenDefterlerBosThenArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => _analyzer.OncekiAyDogrula(_mockDbService.Object, new List<YevmiyeDefteri>(), 0, 0));
        }

        #endregion

        #region OncekiAyDogrula — Satır Numarası (lineNumber) Sürekliliği

        [Test]
        public void WhenSatirDbUyumluThenSatirSurekli()
        {
            // DB'de 20 satır (yevmiye 1-10 arası, her fis 2 satır), XML satırlar 21'den başlıyor
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 11, yevmiyeSayisi: 3);

            MockDbSureklilk(calislanMinYevmiye: 11, dbYevmiyeSayisi: 10, dbMaxYevmiyeNo: 10, dbToplamSatir: 20);
            MockAyFisBilgisi();

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.SatirSurekli, Is.True);
            Assert.That(sonuc.AktarimIzinli, Is.True);
            Assert.That(sonuc.Mesajlar.Any(m => m.Contains("Satır numarası sürekliliği OK")), Is.True);
        }

        [Test]
        public void WhenSatirDbUyumsuzThenSatirSurekliDegil()
        {
            // DB'de 18 satır var ama XML 21'den başlıyor → 18+1=19 ≠ 21 → boşluk
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 11, yevmiyeSayisi: 3);

            MockDbSureklilk(calislanMinYevmiye: 11, dbYevmiyeSayisi: 10, dbMaxYevmiyeNo: 10, dbToplamSatir: 18);
            MockAyFisBilgisi();

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.SatirSurekli, Is.False);
            Assert.That(sonuc.AktarimIzinli, Is.False);
            Assert.That(sonuc.Mesajlar.Any(m => m.Contains("Satır numarası sürekliliği BOZUK")), Is.True);
        }

        [Test]
        public void WhenIlkAySatir1denBasliyorsaThenSatirSurekli()
        {
            // İlk ay, satırlar 1'den başlıyor → OK
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 1, yevmiyeSayisi: 3);

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.IlkAy, Is.True);
            Assert.That(sonuc.SatirSurekli, Is.True);
            Assert.That(sonuc.AktarimIzinli, Is.True);
        }

        [Test]
        public void WhenIlkAySatir1denBaslamiyorsaThenAktarimEngellenir()
        {
            // İlk ay, ama satırlar 5'ten başlıyor → lineNumber sürekliliği bozuk
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 1, yevmiyeSayisi: 2, satirBaslangic: 5);

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.IlkAy, Is.True);
            Assert.That(sonuc.SatirSurekli, Is.False);
            Assert.That(sonuc.AktarimIzinli, Is.False);
            Assert.That(sonuc.Mesajlar.Any(m => m.Contains("lineNumber 1'den başlamıyor")), Is.True);
        }

        [Test]
        public void WhenSatirIcBoslukVarsaThenAktarimEngellenir()
        {
            // Satır numaralarında iç boşluk: 21,22,24 (23 eksik)
            var defterler = DefterlerOlusturOzelSatirlar(
                yevmiyeBaslangic: 11, yevmiyeSayisi: 3,
                satirNoSayaclar: new[] { 21, 22, 24 });

            MockDbSureklilk(calislanMinYevmiye: 11, dbYevmiyeSayisi: 10, dbMaxYevmiyeNo: 10, dbToplamSatir: 20);
            MockAyFisBilgisi();

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.SatirSurekli, Is.True); // DB sürekliliği OK (21 = 20+1)
            Assert.That(sonuc.AktarimIzinli, Is.False); // Ama iç satır boşluğu var
            Assert.That(sonuc.Mesajlar.Any(m => m.Contains("Satır numarası boşluğu")), Is.True);
        }

        [Test]
        public void WhenSatirDbFazlaThenSatirSurekliDegil()
        {
            // DB'de 25 satır var ama XML 21'den başlıyor → 25+1=26 ≠ 21 → çakışma
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 11, yevmiyeSayisi: 3);

            MockDbSureklilk(calislanMinYevmiye: 11, dbYevmiyeSayisi: 10, dbMaxYevmiyeNo: 10, dbToplamSatir: 25);
            MockAyFisBilgisi();

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.SatirSurekli, Is.False);
            Assert.That(sonuc.AktarimIzinli, Is.False);
        }

        #endregion

        #region OncekiAyDogrula — CalislanAyBilgisi Kontrolü

        [Test]
        public void WhenBasariliAnalizThenCalislanAyBilgisiDoldurulur()
        {
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 1, yevmiyeSayisi: 3);

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.CalislanAyBilgisi, Is.Not.Null);
            Assert.That(sonuc.CalislanAyBilgisi.FisSayisi, Is.EqualTo(3));
            Assert.That(sonuc.CalislanAyBilgisi.MinYevmiyeNo, Is.EqualTo(1));
            Assert.That(sonuc.CalislanAyBilgisi.MaxYevmiyeNo, Is.EqualTo(3));
        }

        [Test]
        public void WhenNormalAyThenOncekiAyBilgisiDbdenGetirilir()
        {
            var defterler = DefterlerOlustur(yevmiyeBaslangic: 11, yevmiyeSayisi: 2);

            MockDbSureklilk(calislanMinYevmiye: 11, dbYevmiyeSayisi: 10, dbMaxYevmiyeNo: 10);
            MockAyFisBilgisi(fisSayisi: 25, minYevmiye: 1, maxYevmiye: 10);

            var sonuc = _analyzer.OncekiAyDogrula(_mockDbService.Object, defterler, 0, 0);

            Assert.That(sonuc.OncekiAyBilgisi, Is.Not.Null);
            Assert.That(sonuc.OncekiAyBilgisi.FisSayisi, Is.EqualTo(25));
        }

        #endregion

        #region DbDurumGetir

        [Test]
        public void WhenDbDurumGetirThenServisdekiDegerlerDondurulur()
        {
            var mockDb = new Mock<IMikroDbService>();
            mockDb.Setup(s => s.HesapPlaniSayisi()).Returns(150);
            mockDb.Setup(s => s.MevcutFisSayisi(2025,
                new DateTime(2025, 4, 1), new DateTime(2025, 4, 30), 0, 0)).Returns(320);

            var bilgi = _analyzer.DbDurumGetir(
                mockDb.Object,
                new DateTime(2025, 4, 1),
                new DateTime(2025, 4, 30),
                2025);

            Assert.That(bilgi.HesapSayisi, Is.EqualTo(150));
            Assert.That(bilgi.FisSayisi, Is.EqualTo(320));
            Assert.That(bilgi.DonemBaslangic, Is.EqualTo(new DateTime(2025, 4, 1)));
            Assert.That(bilgi.DonemBitis, Is.EqualTo(new DateTime(2025, 4, 30)));
        }

        #endregion

        #region Yardımcı Metodlar

        private static List<YevmiyeDefteri> DefterlerOlustur(int yevmiyeBaslangic, int yevmiyeSayisi, int satirBaslangic = -1)
        {
            int satirPerFis = 2;
            if (satirBaslangic < 0)
                satirBaslangic = (yevmiyeBaslangic - 1) * satirPerFis + 1;

            var defter = new YevmiyeDefteri
            {
                DosyaAdi = "test.xml",
                BenzersizId = "BID001",
                DonemBaslangic = new DateTime(2025, 4, 1),
                DonemBitis = new DateTime(2025, 4, 30),
                MaliYilBaslangic = new DateTime(2025, 1, 1),
                MaliYilBitis = new DateTime(2025, 12, 31)
            };

            int satirSayac = satirBaslangic;

            for (int i = 0; i < yevmiyeSayisi; i++)
            {
                int yevNo = yevmiyeBaslangic + i;
                var fis = new YevmiyeFisi
                {
                    YevmiyeNo = yevNo.ToString("D10"),
                    YevmiyeNoSayac = yevNo,
                    GirisTarihi = new DateTime(2025, 4, 1),
                    ToplamBorc = 1000m,
                    ToplamAlacak = 1000m
                };

                fis.Satirlar.Add(new FisDetaySatiri
                {
                    SatirNo = satirSayac,
                    SatirNoSayac = satirSayac,
                    AnaHesapKod = "770",
                    AltHesapKod = "770.50",
                    Tutar = 1000m,
                    BorcAlacakKodu = "D",
                    KayitTarihi = new DateTime(2025, 4, 1)
                });
                satirSayac++;

                fis.Satirlar.Add(new FisDetaySatiri
                {
                    SatirNo = satirSayac,
                    SatirNoSayac = satirSayac,
                    AnaHesapKod = "100",
                    AltHesapKod = "100.01",
                    Tutar = 1000m,
                    BorcAlacakKodu = "C",
                    KayitTarihi = new DateTime(2025, 4, 1)
                });
                satirSayac++;

                defter.Fisler.Add(fis);
            }

            return new List<YevmiyeDefteri> { defter };
        }

        private static List<YevmiyeDefteri> DefterlerOlusturOzel(int[] yevmiyeNolar, int satirBaslangic = -1)
        {
            if (satirBaslangic < 0)
            {
                int minYev = yevmiyeNolar.Min();
                satirBaslangic = minYev == 1 ? 1 : (minYev - 1) + 1;
            }

            var defter = new YevmiyeDefteri
            {
                DosyaAdi = "test.xml",
                BenzersizId = "BID001",
                DonemBaslangic = new DateTime(2025, 4, 1),
                DonemBitis = new DateTime(2025, 4, 30),
                MaliYilBaslangic = new DateTime(2025, 1, 1),
                MaliYilBitis = new DateTime(2025, 12, 31)
            };

            int satirSayac = satirBaslangic;

            foreach (int yevNo in yevmiyeNolar)
            {
                var fis = new YevmiyeFisi
                {
                    YevmiyeNo = yevNo.ToString("D10"),
                    YevmiyeNoSayac = yevNo,
                    GirisTarihi = new DateTime(2025, 4, 1),
                    ToplamBorc = 500m,
                    ToplamAlacak = 500m
                };

                fis.Satirlar.Add(new FisDetaySatiri
                {
                    SatirNo = satirSayac,
                    SatirNoSayac = satirSayac,
                    AnaHesapKod = "100",
                    AltHesapKod = "100.01",
                    Tutar = 500m,
                    BorcAlacakKodu = "D",
                    KayitTarihi = new DateTime(2025, 4, 1)
                });
                satirSayac++;

                defter.Fisler.Add(fis);
            }

            return new List<YevmiyeDefteri> { defter };
        }

        private void MockDbSureklilk(
            int calislanMinYevmiye, int dbYevmiyeSayisi, int dbMaxYevmiyeNo, int dbToplamSatir = -1)
        {
            if (dbToplamSatir < 0)
                dbToplamSatir = (calislanMinYevmiye - 1) * 2;

            _mockDbService
                .Setup(s => s.YevmiyeSureklilkBilgisiGetir(2025, calislanMinYevmiye, 0, 0))
                .Returns(new YevmiyeSureklilkBilgisi
                {
                    YevmiyeSayisi = dbYevmiyeSayisi,
                    MaxYevmiyeNo = dbMaxYevmiyeNo
                });

            _mockDbService
                .Setup(s => s.SatirSureklilkBilgisiGetir(2025, calislanMinYevmiye, 0, 0))
                .Returns(new SatirSureklilkBilgisi
                {
                    ToplamSatirSayisi = dbToplamSatir
                });
        }

        private void MockAyFisBilgisi(int fisSayisi = 10, int minYevmiye = 1, int maxYevmiye = 10)
        {
            _mockDbService
                .Setup(s => s.AyFisBilgisiGetir(
                    2025,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    0, 0))
                .Returns(new AyFisBilgisi
                {
                    FisSayisi = fisSayisi,
                    MinYevmiyeNo = minYevmiye,
                    MaxYevmiyeNo = maxYevmiye,
                    DonemBaslangic = new DateTime(2025, 3, 1),
                    DonemBitis = new DateTime(2025, 3, 31)
                });
        }

        /// <summary>
        /// Her fişe tam 1 satır ekler; satır numaraları dışarıdan explicit verilir (boşluk testi için).
        /// </summary>
        private static List<YevmiyeDefteri> DefterlerOlusturOzelSatirlar(
            int yevmiyeBaslangic, int yevmiyeSayisi, int[] satirNoSayaclar)
        {
            if (satirNoSayaclar.Length != yevmiyeSayisi)
                throw new ArgumentException("satirNoSayaclar sayısı yevmiyeSayisi ile eşit olmalı.");

            var defter = new YevmiyeDefteri
            {
                DosyaAdi = "test.xml",
                BenzersizId = "BID001",
                DonemBaslangic = new DateTime(2025, 4, 1),
                DonemBitis = new DateTime(2025, 4, 30),
                MaliYilBaslangic = new DateTime(2025, 1, 1),
                MaliYilBitis = new DateTime(2025, 12, 31)
            };

            for (int i = 0; i < yevmiyeSayisi; i++)
            {
                int yevNo = yevmiyeBaslangic + i;
                var fis = new YevmiyeFisi
                {
                    YevmiyeNo = yevNo.ToString("D10"),
                    YevmiyeNoSayac = yevNo,
                    GirisTarihi = new DateTime(2025, 4, 1),
                    ToplamBorc = 500m,
                    ToplamAlacak = 500m
                };

                fis.Satirlar.Add(new FisDetaySatiri
                {
                    SatirNo = satirNoSayaclar[i],
                    SatirNoSayac = satirNoSayaclar[i],
                    AnaHesapKod = "100",
                    AltHesapKod = "100.01",
                    Tutar = 500m,
                    BorcAlacakKodu = "D",
                    KayitTarihi = new DateTime(2025, 4, 1)
                });

                defter.Fisler.Add(fis);
            }

            return new List<YevmiyeDefteri> { defter };
        }

        #endregion
    }
}
