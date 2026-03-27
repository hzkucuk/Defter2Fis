using NUnit.Framework;
using Defter2Fis.ForMikro.Services;

namespace Defter2Fis.Tests
{
    [TestFixture]
    public class EvrakBilgisiParserTests
    {
        private EvrakBilgisiParser _parser;

        [SetUp]
        public void SetUp()
        {
            _parser = new EvrakBilgisiParser();
        }

        #region Pattern 1: SERİ:X NO:NNNNNN

        [TestCase("SERİ:A NO:000123", "A", 123)]
        [TestCase("SERI:B NO:000456", "B", 456)]
        [TestCase("SERİ:B1 SIRA:789", "B1", 789)]
        [TestCase("Seri: C No: 000001", "C", 1)]
        public void WhenSeriNoPatternThenParsesCorrectly(string aciklama, string beklenenSeri, int beklenenSira)
        {
            var sonuc = _parser.Parse(aciklama, null, null);

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Seri, Is.EqualTo(beklenenSeri));
            Assert.That(sonuc.Sira, Is.EqualTo(beklenenSira));
        }

        #endregion

        #region Pattern 2: EVRAK SERİ:X EVRAK NO:NNNNNN

        [TestCase("EVRAK SERİ: A EVRAK NO: 000123", "A", 123)]
        [TestCase("EVRAK SERI:B EVRAK NO:999", "B", 999)]
        public void WhenEvrakSeriNoPatternThenParsesCorrectly(string aciklama, string beklenenSeri, int beklenenSira)
        {
            var sonuc = _parser.Parse(aciklama, null, null);

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Seri, Is.EqualTo(beklenenSeri));
            Assert.That(sonuc.Sira, Is.EqualTo(beklenenSira));
        }

        #endregion

        #region Pattern 3: A-000123 / A/000123

        [TestCase("A-000123", "A", 123)]
        [TestCase("B/000456", "B", 456)]
        [TestCase("CD-00789", "CD", 789)]
        public void WhenSeriTireSiraPatternThenParsesCorrectly(string aciklama, string beklenenSeri, int beklenenSira)
        {
            var sonuc = _parser.Parse(aciklama, null, null);

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Seri, Is.EqualTo(beklenenSeri));
            Assert.That(sonuc.Sira, Is.EqualTo(beklenenSira));
        }

        #endregion

        #region Pattern 4: A 000123 (seri + boşluk + sıra)

        [TestCase("A 000123", "A", 123)]
        [TestCase("B 9876543", "B", 9876543)]
        public void WhenSeriBoslukSiraPatternThenParsesCorrectly(string aciklama, string beklenenSeri, int beklenenSira)
        {
            var sonuc = _parser.Parse(aciklama, null, null);

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Seri, Is.EqualTo(beklenenSeri));
            Assert.That(sonuc.Sira, Is.EqualTo(beklenenSira));
        }

        #endregion

        #region BelgeNo fallback

        [Test]
        public void WhenAciklamaEmptyThenFallsToBelgeNo()
        {
            var sonuc = _parser.Parse(null, "A-000555", null);

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Seri, Is.EqualTo("A"));
            Assert.That(sonuc.Sira, Is.EqualTo(555));
        }

        [Test]
        public void WhenAciklamaAndBelgeNoEmptyThenFallsToBelgeReferansi()
        {
            var sonuc = _parser.Parse(null, null, "B-000777");

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Seri, Is.EqualTo("B"));
            Assert.That(sonuc.Sira, Is.EqualTo(777));
        }

        [Test]
        public void WhenBelgeNoSadeceRakamThenSeriBosSiraRakam()
        {
            var sonuc = _parser.Parse(null, "42", null);

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Seri, Is.EqualTo(string.Empty));
            Assert.That(sonuc.Sira, Is.EqualTo(42));
        }

        #endregion

        #region Pattern 5: e-Defter açıklama formatı

        [TestCase("Al.fat. : TA4202500000642/31.05.2025/KDVO:%20.00/TÜRK HAVAYOLLARI", "TA4", 642)]
        [TestCase("Al.fat. : GB3202500809960/31.05.2025//TÜRK TELEKOM", "GB3", 809960)]
        [TestCase("Al.fat. : A162025004088937/31.05.2025/Internet/TTNET A.Ş.", "A16", 4088937)]
        [TestCase("Al.fat. : C012025000725240/31.05.2025/AYLIK ÜCRET", "C01", 725240)]
        [TestCase("Al.fat. : P012025002634501/31.05.2025/AYLIK ÜCRET", "P01", 2634501)]
        public void WhenEdDefterEttnAciklamaThenParsesCorrectly(string aciklama, string beklenenSeri, int beklenenSira)
        {
            var sonuc = _parser.Parse(aciklama, null, null);

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Seri, Is.EqualTo(beklenenSeri));
            Assert.That(sonuc.Sira, Is.EqualTo(beklenenSira));
        }

        [TestCase("C.A.V.D : 6933/31.05.2025/GÜMRÜK 3680014533I", 6933)]
        [TestCase("Gl.Hav. : 3470/31.05.2025/34DV5817 TAŞIT TANIMA", 3470)]
        public void WhenEdDefterSayisalAciklamaThenParsesCorrectly(string aciklama, int beklenenSira)
        {
            var sonuc = _parser.Parse(aciklama, null, null);

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Seri, Is.EqualTo(string.Empty));
            Assert.That(sonuc.Sira, Is.EqualTo(beklenenSira));
        }

        #endregion

        #region ETTN BelgeNo fallback

        [TestCase("TA4202500000642", "TA4", 642)]
        [TestCase("GB3202500809960", "GB3", 809960)]
        [TestCase("A162025004088937", "A16", 4088937)]
        [TestCase("C012025000725240", "C01", 725240)]
        public void WhenBelgeNoEttnFormatThenParsesCorrectly(string belgeNo, string beklenenSeri, int beklenenSira)
        {
            var sonuc = _parser.Parse(null, belgeNo, null);

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Seri, Is.EqualTo(beklenenSeri));
            Assert.That(sonuc.Sira, Is.EqualTo(beklenenSira));
        }

        [Test]
        public void WhenEttnFormatThenAnahtarSeriTireSira()
        {
            var sonuc = _parser.Parse(null, "TA4202500000642", null);

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Anahtar, Is.EqualTo("TA4-642"));
        }

        #endregion

        #region Null / boş input

        [Test]
        public void WhenAllInputsNullThenReturnsNull()
        {
            var sonuc = _parser.Parse(null, null, null);

            Assert.That(sonuc, Is.Null);
        }

        [Test]
        public void WhenAllInputsEmptyThenReturnsNull()
        {
            var sonuc = _parser.Parse("", "", "");

            Assert.That(sonuc, Is.Null);
        }

        [Test]
        public void WhenUnrecognizedFormatThenReturnsNull()
        {
            var sonuc = _parser.Parse("random text without pattern", null, null);

            Assert.That(sonuc, Is.Null);
        }

        #endregion

        #region Anahtar property

        [Test]
        public void WhenSeriVarsaAnahtarSeriTireSiraFormati()
        {
            var sonuc = _parser.Parse("A-000100", null, null);

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Anahtar, Is.EqualTo("A-100"));
        }

        [Test]
        public void WhenSeriYoksaAnahtarSadeceSira()
        {
            var sonuc = _parser.Parse(null, "999", null);

            Assert.That(sonuc, Is.Not.Null);
            Assert.That(sonuc.Anahtar, Is.EqualTo("999"));
        }

        #endregion
    }
}
