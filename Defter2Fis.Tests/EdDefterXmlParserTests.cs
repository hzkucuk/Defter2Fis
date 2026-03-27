using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Defter2Fis.ForMikro.Services;

namespace Defter2Fis.Tests
{
    [TestFixture]
    public class EdDefterXmlParserTests
    {
        private EdDefterXmlParser _parser;
        private string _tempDir;

        [SetUp]
        public void SetUp()
        {
            _parser = new EdDefterXmlParser();
            _tempDir = Path.Combine(Path.GetTempPath(), "Defter2FisTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        [TearDown]
        public void TearDown()
        {
            try { Directory.Delete(_tempDir, true); }
            catch { /* test ortamı temizliği — hata yutulabilir */ }
        }

        #region DosyadanOku — Temel Parse

        [Test]
        public void WhenGecerliXmlThenDefterBilgileriDogruParsEdilir()
        {
            string dosya = XmlDosyaOlustur(
                uniqueId: "YEV202504000001",
                periodStart: "2025-04-01",
                periodEnd: "2025-04-30",
                creationDate: "2025-05-15",
                orgIdentifier: "TEST FİRMA A.Ş.",
                sicilNo: "7330029626",
                fiscalStart: "2025-01-01",
                fiscalEnd: "2025-12-31",
                entriesXml: "");

            var defter = _parser.DosyadanOku(dosya);

            Assert.That(defter.BenzersizId, Is.EqualTo("YEV202504000001"));
            Assert.That(defter.DonemBaslangic, Is.EqualTo(new DateTime(2025, 4, 1)));
            Assert.That(defter.DonemBitis, Is.EqualTo(new DateTime(2025, 4, 30)));
            Assert.That(defter.FirmaUnvani, Is.EqualTo("TEST FİRMA A.Ş."));
            Assert.That(defter.SicilNo, Is.EqualTo("7330029626"));
            Assert.That(defter.MaliYilBaslangic, Is.EqualTo(new DateTime(2025, 1, 1)));
            Assert.That(defter.MaliYilBitis, Is.EqualTo(new DateTime(2025, 12, 31)));
        }

        [Test]
        public void WhenTekFisThenEntryHeaderDogruParsEdilir()
        {
            string entryXml = EntryHeaderXml(
                entryNumber: "0000000001",
                entryNumberCounter: "1",
                enteredDate: "2025-04-01",
                entryComment: "Test fiş açıklaması",
                totalDebit: "1000.00",
                totalCredit: "1000.00",
                detailsXml: "");

            string dosya = XmlDosyaOlustur(entriesXml: entryXml);
            var defter = _parser.DosyadanOku(dosya);

            Assert.That(defter.Fisler.Count, Is.EqualTo(1));
            Assert.That(defter.Fisler[0].YevmiyeNo, Is.EqualTo("0000000001"));
            Assert.That(defter.Fisler[0].YevmiyeNoSayac, Is.EqualTo(1));
            Assert.That(defter.Fisler[0].GirisTarihi, Is.EqualTo(new DateTime(2025, 4, 1)));
            Assert.That(defter.Fisler[0].Aciklama, Is.EqualTo("Test fiş açıklaması"));
            Assert.That(defter.Fisler[0].ToplamBorc, Is.EqualTo(1000.00m));
            Assert.That(defter.Fisler[0].ToplamAlacak, Is.EqualTo(1000.00m));
        }

        [Test]
        public void WhenEntryDetailThenSatirBilgileriDogruParsEdilir()
        {
            string detailXml = EntryDetailXml(
                lineNumber: "1",
                amount: "1500.50",
                debitCreditCode: "D",
                postingDate: "2025-04-15",
                documentType: "invoice",
                documentNumber: "A-000123",
                detailComment: "Fatura açıklaması",
                mainAccountId: "770",
                mainAccountDesc: "GENEL YÖNETİM GİDERLERİ",
                subAccountId: "770.50.0018",
                subAccountDesc: "SERTİFİKA GİDERİ");

            string entryXml = EntryHeaderXml(detailsXml: detailXml);
            string dosya = XmlDosyaOlustur(entriesXml: entryXml);
            var defter = _parser.DosyadanOku(dosya);

            var satir = defter.Fisler[0].Satirlar[0];
            Assert.That(satir.SatirNo, Is.EqualTo(1));
            Assert.That(satir.Tutar, Is.EqualTo(1500.50m));
            Assert.That(satir.BorcAlacakKodu, Is.EqualTo("D"));
            Assert.That(satir.KayitTarihi, Is.EqualTo(new DateTime(2025, 4, 15)));
            Assert.That(satir.BelgeTuru, Is.EqualTo("invoice"));
            Assert.That(satir.BelgeNo, Is.EqualTo("A-000123"));
            Assert.That(satir.DetayAciklama, Is.EqualTo("Fatura açıklaması"));
            Assert.That(satir.AnaHesapKod, Is.EqualTo("770"));
            Assert.That(satir.AnaHesapAd, Is.EqualTo("GENEL YÖNETİM GİDERLERİ"));
            Assert.That(satir.AltHesapKod, Is.EqualTo("770.50.0018"));
            Assert.That(satir.AltHesapAd, Is.EqualTo("SERTİFİKA GİDERİ"));
        }

        #endregion

        #region DosyadanOku — Çoklu Fiş ve Satır

        [Test]
        public void WhenCokluFisThenHepsiParsEdilir()
        {
            string detail1 = EntryDetailXml(lineNumber: "1", amount: "500", debitCreditCode: "D");
            string detail2 = EntryDetailXml(lineNumber: "2", amount: "500", debitCreditCode: "C");

            string entry1 = EntryHeaderXml(entryNumber: "0000000001", entryNumberCounter: "1",
                totalDebit: "500", totalCredit: "500", detailsXml: detail1 + detail2);
            string entry2 = EntryHeaderXml(entryNumber: "0000000002", entryNumberCounter: "2",
                totalDebit: "500", totalCredit: "500", detailsXml: detail1);

            string dosya = XmlDosyaOlustur(entriesXml: entry1 + entry2);
            var defter = _parser.DosyadanOku(dosya);

            Assert.That(defter.Fisler.Count, Is.EqualTo(2));
            Assert.That(defter.Fisler[0].Satirlar.Count, Is.EqualTo(2));
            Assert.That(defter.Fisler[1].Satirlar.Count, Is.EqualTo(1));
        }

        #endregion

        #region DosyadanOku — Hata Durumları

        [Test]
        public void WhenDosyaYoluNullThenArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _parser.DosyadanOku(null));
        }

        [Test]
        public void WhenDosyaYoluBosThenArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _parser.DosyadanOku(""));
        }

        [Test]
        public void WhenDosyaBulunamazThenFileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(
                () => _parser.DosyadanOku(Path.Combine(_tempDir, "olmayan.xml")));
        }

        [Test]
        public void WhenAccountingEntriesYokThenInvalidOperationException()
        {
            string dosya = Path.Combine(_tempDir, "bozuk.xml");
            File.WriteAllText(dosya, "<?xml version=\"1.0\"?><root></root>");

            Assert.Throws<InvalidOperationException>(() => _parser.DosyadanOku(dosya));
        }

        #endregion

        #region KlasordenOku

        [Test]
        public void WhenGecerliKlasorThenDosyalarParsEdilir()
        {
            string sicilNo = "7330029626";
            string dosya1 = Path.Combine(_tempDir, $"{sicilNo}-202504-Y-0001.xml");
            string dosya2 = Path.Combine(_tempDir, $"{sicilNo}-202504-Y-0002.xml");

            File.WriteAllText(dosya1, TamXmlOlustur());
            File.WriteAllText(dosya2, TamXmlOlustur());

            var defterler = _parser.KlasordenOku(_tempDir, sicilNo);

            Assert.That(defterler.Count, Is.EqualTo(2));
        }

        [Test]
        public void WhenKlasorBulunamazThenDirectoryNotFoundException()
        {
            Assert.Throws<DirectoryNotFoundException>(
                () => _parser.KlasordenOku(Path.Combine(_tempDir, "olmayan"), "1234567890"));
        }

        [Test]
        public void WhenKlasordeEslesenDosyaYokThenFileNotFoundException()
        {
            // Klasör var ama eşleşen dosya yok
            Assert.Throws<FileNotFoundException>(
                () => _parser.KlasordenOku(_tempDir, "0000000000"));
        }

        [Test]
        public void WhenKlasorYoluNullThenArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _parser.KlasordenOku(null, "1234567890"));
        }

        [Test]
        public void WhenCallbackVerilmisseThenHerDosyaIcinCagrilir()
        {
            string sicilNo = "7330029626";
            string dosya = Path.Combine(_tempDir, $"{sicilNo}-202504-Y-0001.xml");
            File.WriteAllText(dosya, TamXmlOlustur());

            int callbackSayisi = 0;
            _parser.KlasordenOku(_tempDir, sicilNo, msg => callbackSayisi++);

            Assert.That(callbackSayisi, Is.EqualTo(1));
        }

        #endregion

        #region DosyadanOku — Eksik Opsiyonel Alanlar

        [Test]
        public void WhenAccountSubYokThenAltHesapBosKalir()
        {
            string detailXml = EntryDetailXml(
                lineNumber: "1", amount: "100", debitCreditCode: "D",
                mainAccountId: "770", mainAccountDesc: "GYG",
                subAccountId: null, subAccountDesc: null);

            string entryXml = EntryHeaderXml(detailsXml: detailXml);
            string dosya = XmlDosyaOlustur(entriesXml: entryXml);
            var defter = _parser.DosyadanOku(dosya);

            var satir = defter.Fisler[0].Satirlar[0];
            Assert.That(satir.AnaHesapKod, Is.EqualTo("770"));
            Assert.That(satir.AltHesapKod, Is.Null.Or.Empty);
        }

        [Test]
        public void WhenDocumentDateYokThenBelgeTarihiNull()
        {
            // documentDate opsiyoneldir
            string detailXml = EntryDetailXml(
                lineNumber: "1", amount: "200", debitCreditCode: "C",
                includeDocumentDate: false);

            string entryXml = EntryHeaderXml(detailsXml: detailXml);
            string dosya = XmlDosyaOlustur(entriesXml: entryXml);
            var defter = _parser.DosyadanOku(dosya);

            var satir = defter.Fisler[0].Satirlar[0];
            Assert.That(satir.BelgeTarihi, Is.Null);
        }

        #endregion

        #region Yardımcı Metodlar

        private string XmlDosyaOlustur(
            string uniqueId = "YEV202504000001",
            string periodStart = "2025-04-01",
            string periodEnd = "2025-04-30",
            string creationDate = "2025-05-15",
            string orgIdentifier = "TEST FİRMA",
            string sicilNo = "7330029626",
            string fiscalStart = "2025-01-01",
            string fiscalEnd = "2025-12-31",
            string entriesXml = "")
        {
            string xml = TamXmlIcerikOlustur(uniqueId, periodStart, periodEnd, creationDate,
                orgIdentifier, sicilNo, fiscalStart, fiscalEnd, entriesXml);

            string dosyaAdi = $"test_{Guid.NewGuid():N}.xml";
            string dosyaYolu = Path.Combine(_tempDir, dosyaAdi);
            File.WriteAllText(dosyaYolu, xml);
            return dosyaYolu;
        }

        private static string TamXmlOlustur()
        {
            string detail = EntryDetailXml(lineNumber: "1", amount: "100", debitCreditCode: "D");
            string entry = EntryHeaderXml(detailsXml: detail);
            return TamXmlIcerikOlustur(entriesXml: entry);
        }

        private static string TamXmlIcerikOlustur(
            string uniqueId = "YEV202504000001",
            string periodStart = "2025-04-01",
            string periodEnd = "2025-04-30",
            string creationDate = "2025-05-15",
            string orgIdentifier = "TEST FİRMA",
            string sicilNo = "7330029626",
            string fiscalStart = "2025-01-01",
            string fiscalEnd = "2025-12-31",
            string entriesXml = "")
        {
            return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xbrli:xbrl xmlns:xbrli=""http://www.xbrl.org/2003/instance""
            xmlns:gl-cor=""http://www.xbrl.org/int/gl/cor/2006-10-25""
            xmlns:gl-bus=""http://www.xbrl.org/int/gl/bus/2006-10-25"">
  <xbrli:context id=""ctx"">
    <xbrli:entity>
      <xbrli:identifier scheme=""http://www.vergikimlikno.gov.tr"">{sicilNo}</xbrli:identifier>
    </xbrli:entity>
    <xbrli:period>
      <xbrli:startDate>{periodStart}</xbrli:startDate>
      <xbrli:endDate>{periodEnd}</xbrli:endDate>
    </xbrli:period>
  </xbrli:context>
  <gl-cor:accountingEntries>
    <gl-cor:documentInfo>
      <gl-cor:uniqueID>{uniqueId}</gl-cor:uniqueID>
      <gl-cor:creationDate>{creationDate}</gl-cor:creationDate>
      <gl-cor:periodCoveredStart>{periodStart}</gl-cor:periodCoveredStart>
      <gl-cor:periodCoveredEnd>{periodEnd}</gl-cor:periodCoveredEnd>
      <gl-cor:entriesComment>Test defter</gl-cor:entriesComment>
      <gl-bus:sourceApplication>TestApp</gl-bus:sourceApplication>
    </gl-cor:documentInfo>
    <gl-cor:entityInformation>
      <gl-bus:organizationIdentifiers>
        <gl-bus:organizationIdentifier>{orgIdentifier}</gl-bus:organizationIdentifier>
      </gl-bus:organizationIdentifiers>
      <gl-bus:fiscalYearStart>{fiscalStart}</gl-bus:fiscalYearStart>
      <gl-bus:fiscalYearEnd>{fiscalEnd}</gl-bus:fiscalYearEnd>
    </gl-cor:entityInformation>
    {entriesXml}
  </gl-cor:accountingEntries>
</xbrli:xbrl>";
        }

        private static string EntryHeaderXml(
            string entryNumber = "0000000001",
            string entryNumberCounter = "1",
            string enteredDate = "2025-04-01",
            string entryComment = "Test fiş",
            string totalDebit = "1000.00",
            string totalCredit = "1000.00",
            string detailsXml = "")
        {
            return $@"
    <gl-cor:entryHeader>
      <gl-cor:entryNumber>{entryNumber}</gl-cor:entryNumber>
      <gl-cor:entryNumberCounter>{entryNumberCounter}</gl-cor:entryNumberCounter>
      <gl-cor:enteredDate>{enteredDate}</gl-cor:enteredDate>
      <gl-cor:entryComment>{entryComment}</gl-cor:entryComment>
      <gl-bus:totalDebit>{totalDebit}</gl-bus:totalDebit>
      <gl-bus:totalCredit>{totalCredit}</gl-bus:totalCredit>
      {detailsXml}
    </gl-cor:entryHeader>";
        }

        private static string EntryDetailXml(
            string lineNumber = "1",
            string amount = "100.00",
            string debitCreditCode = "D",
            string postingDate = "2025-04-15",
            string documentType = "other",
            string documentNumber = "TEST-001",
            string detailComment = "Test satır",
            string mainAccountId = "100",
            string mainAccountDesc = "KASA",
            string subAccountId = "100.01",
            string subAccountDesc = "MERKEZ KASA",
            bool includeDocumentDate = true)
        {
            string accountSubXml = subAccountId != null
                ? $@"
          <gl-cor:accountSub>
            <gl-cor:accountSubID>{subAccountId}</gl-cor:accountSubID>
            <gl-cor:accountSubDescription>{subAccountDesc ?? ""}</gl-cor:accountSubDescription>
          </gl-cor:accountSub>"
                : "";

            string docDateXml = includeDocumentDate
                ? $"<gl-cor:documentDate>{postingDate}</gl-cor:documentDate>"
                : "";

            return $@"
      <gl-cor:entryDetail>
        <gl-cor:lineNumber>{lineNumber}</gl-cor:lineNumber>
        <gl-cor:amount>{amount}</gl-cor:amount>
        <gl-cor:debitCreditCode>{debitCreditCode}</gl-cor:debitCreditCode>
        <gl-cor:postingDate>{postingDate}</gl-cor:postingDate>
        <gl-cor:documentType>{documentType}</gl-cor:documentType>
        <gl-cor:documentNumber>{documentNumber}</gl-cor:documentNumber>
        <gl-cor:detailComment>{detailComment}</gl-cor:detailComment>
        {docDateXml}
        <gl-cor:account>
          <gl-cor:accountMainID>{mainAccountId}</gl-cor:accountMainID>
          <gl-cor:accountMainDescription>{mainAccountDesc}</gl-cor:accountMainDescription>
          {accountSubXml}
        </gl-cor:account>
      </gl-cor:entryDetail>";
        }

        #endregion
    }
}
