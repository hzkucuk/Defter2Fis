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
