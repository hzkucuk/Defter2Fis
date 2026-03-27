using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Defter2Fis.ForMikro.Models;

namespace Defter2Fis.ForMikro.Services
{
    /// <summary>
    /// E-Defter Yevmiye XML dosyalarını (XBRL-GL formatı) parse eder.
    /// Namespace: gl-cor (2006-10-25), gl-bus (2006-10-25)
    /// </summary>
    public class EdDefterXmlParser
    {
        private static readonly XNamespace GlCor = "http://www.xbrl.org/int/gl/cor/2006-10-25";
        private static readonly XNamespace GlBus = "http://www.xbrl.org/int/gl/bus/2006-10-25";
        private static readonly XNamespace Xbrli = "http://www.xbrl.org/2003/instance";

        /// <summary>
        /// Belirtilen klasördeki tüm Yevmiye XML dosyalarını bulur ve parse eder.
        /// Dosya pattern: {SicilNo}-{YYYYMM}-Y-{SıraNo}.xml
        /// </summary>
        public List<YevmiyeDefteri> KlasordenOku(string klasorYolu, string sicilNo, Action<string> dosyaIslendi = null)
        {
            if (string.IsNullOrWhiteSpace(klasorYolu))
                throw new ArgumentNullException(nameof(klasorYolu));
            if (!Directory.Exists(klasorYolu))
                throw new DirectoryNotFoundException($"E-Defter klasörü bulunamadı: {klasorYolu}");

            string pattern = $"{sicilNo}-*-Y-*.xml";
            var dosyalar = Directory.GetFiles(klasorYolu, pattern)
                                    .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
                                    .ToArray();

            if (dosyalar.Length == 0)
                throw new FileNotFoundException(
                    $"Yevmiye dosyası bulunamadı. Klasör: {klasorYolu}, Pattern: {pattern}");

            var defterler = new List<YevmiyeDefteri>();
            foreach (string dosya in dosyalar)
            {
                var defter = DosyadanOku(dosya);
                defterler.Add(defter);
                dosyaIslendi?.Invoke(
                    $"{Path.GetFileName(dosya)} — {defter.Fisler.Count} fiş, " +
                    $"{defter.Fisler.Sum(f => f.Satirlar.Count)} satır");
            }

            return defterler;
        }

        /// <summary>
        /// Tek bir Yevmiye XML dosyasını parse eder.
        /// </summary>
        public YevmiyeDefteri DosyadanOku(string dosyaYolu)
        {
            if (string.IsNullOrWhiteSpace(dosyaYolu))
                throw new ArgumentNullException(nameof(dosyaYolu));
            if (!File.Exists(dosyaYolu))
                throw new FileNotFoundException($"Dosya bulunamadı: {dosyaYolu}");

            XDocument xdoc = XDocument.Load(dosyaYolu);
            XElement accountingEntries = xdoc.Descendants(GlCor + "accountingEntries").FirstOrDefault();

            if (accountingEntries == null)
                throw new InvalidOperationException(
                    $"accountingEntries elementi bulunamadı: {dosyaYolu}");

            var defter = new YevmiyeDefteri
            {
                DosyaAdi = Path.GetFileName(dosyaYolu)
            };

            // documentInfo parse
            XElement docInfo = accountingEntries.Element(GlCor + "documentInfo");
            if (docInfo != null)
            {
                defter.BenzersizId = ElementDegeri(docInfo, GlCor + "uniqueID");
                defter.OlusturmaTarihi = TarihParse(ElementDegeri(docInfo, GlCor + "creationDate"));
                defter.Aciklama = ElementDegeri(docInfo, GlCor + "entriesComment");
                defter.KaynakUygulama = ElementDegeri(docInfo, GlBus + "sourceApplication");
                defter.DonemBaslangic = TarihParse(ElementDegeri(docInfo, GlCor + "periodCoveredStart"));
                defter.DonemBitis = TarihParse(ElementDegeri(docInfo, GlCor + "periodCoveredEnd"));
            }

            // entityInformation parse
            XElement entityInfo = accountingEntries.Element(GlCor + "entityInformation");
            if (entityInfo != null)
            {
                XElement orgIds = entityInfo.Element(GlBus + "organizationIdentifiers");
                if (orgIds != null)
                {
                    defter.FirmaUnvani = ElementDegeri(orgIds, GlBus + "organizationIdentifier");
                }

                defter.MaliYilBaslangic = TarihParse(ElementDegeri(entityInfo, GlBus + "fiscalYearStart"));
                defter.MaliYilBitis = TarihParse(ElementDegeri(entityInfo, GlBus + "fiscalYearEnd"));
            }

            // identifier (sicil no) - xbrli:context > xbrli:entity > xbrli:identifier
            XElement context = xdoc.Descendants(Xbrli + "context").FirstOrDefault();
            if (context != null)
            {
                XElement identifier = context.Descendants(Xbrli + "identifier").FirstOrDefault();
                if (identifier != null)
                {
                    defter.SicilNo = identifier.Value?.Trim();
                }
            }

            // entryHeader'ları parse et (her biri bir yevmiye fişi)
            var entryHeaders = accountingEntries.Elements(GlCor + "entryHeader");
            foreach (XElement header in entryHeaders)
            {
                var fis = EntryHeaderParse(header);
                if (fis != null)
                {
                    defter.Fisler.Add(fis);
                }
            }

            return defter;
        }

        /// <summary>
        /// Tek bir entryHeader elementini YevmiyeFisi objesine parse eder.
        /// </summary>
        private YevmiyeFisi EntryHeaderParse(XElement header)
        {
            var fis = new YevmiyeFisi
            {
                YevmiyeNo = ElementDegeri(header, GlCor + "entryNumber"),
                YevmiyeNoSayac = IntParse(ElementDegeri(header, GlCor + "entryNumberCounter")),
                GirisTarihi = TarihParse(ElementDegeri(header, GlCor + "enteredDate")),
                GirenKullanici = ElementDegeri(header, GlCor + "enteredBy"),
                Aciklama = ElementDegeri(header, GlCor + "entryComment"),
                ToplamBorc = DecimalParse(ElementDegeri(header, GlBus + "totalDebit")),
                ToplamAlacak = DecimalParse(ElementDegeri(header, GlBus + "totalCredit"))
            };

            // entryDetail satırlarını parse et
            var entryDetails = header.Elements(GlCor + "entryDetail");
            foreach (XElement detail in entryDetails)
            {
                var satir = EntryDetailParse(detail);
                if (satir != null)
                {
                    fis.Satirlar.Add(satir);
                }
            }

            return fis;
        }

        /// <summary>
        /// Tek bir entryDetail elementini FisDetaySatiri objesine parse eder.
        /// </summary>
        private FisDetaySatiri EntryDetailParse(XElement detail)
        {
            var satir = new FisDetaySatiri
            {
                SatirNo = IntParse(ElementDegeri(detail, GlCor + "lineNumber")),
                SatirNoSayac = IntParse(ElementDegeri(detail, GlCor + "lineNumberCounter")),
                Tutar = DecimalParse(ElementDegeri(detail, GlCor + "amount")),
                BorcAlacakKodu = ElementDegeri(detail, GlCor + "debitCreditCode"),
                KayitTarihi = TarihParse(ElementDegeri(detail, GlCor + "postingDate")),
                BelgeTuru = ElementDegeri(detail, GlCor + "documentType"),
                BelgeNo = ElementDegeri(detail, GlCor + "documentNumber"),
                BelgeReferansi = ElementDegeri(detail, GlCor + "documentReference"),
                DetayAciklama = ElementDegeri(detail, GlCor + "detailComment")
            };

            // documentDate opsiyonel
            string belgeTarihStr = ElementDegeri(detail, GlCor + "documentDate");
            if (!string.IsNullOrWhiteSpace(belgeTarihStr))
            {
                satir.BelgeTarihi = TarihParse(belgeTarihStr);
            }

            // account > accountMainID, accountMainDescription
            XElement account = detail.Element(GlCor + "account");
            if (account != null)
            {
                satir.AnaHesapKod = ElementDegeri(account, GlCor + "accountMainID");
                satir.AnaHesapAd = ElementDegeri(account, GlCor + "accountMainDescription");

                // account > accountSub > accountSubID, accountSubDescription
                XElement accountSub = account.Element(GlCor + "accountSub");
                if (accountSub != null)
                {
                    satir.AltHesapKod = ElementDegeri(accountSub, GlCor + "accountSubID");
                    satir.AltHesapAd = ElementDegeri(accountSub, GlCor + "accountSubDescription");
                }
            }

            return satir;
        }

        #region Yardımcı Metodlar

        /// <summary>
        /// Element değerini güvenli şekilde okur, null/boş ise string.Empty döner.
        /// </summary>
        private static string ElementDegeri(XElement parent, XName elementName)
        {
            XElement el = parent.Element(elementName);
            return el?.Value?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Tarih string'ini DateTime'a parse eder. Beklenen format: yyyy-MM-dd
        /// </summary>
        private static DateTime TarihParse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DateTime.MinValue;

            if (DateTime.TryParseExact(value, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }

            if (DateTime.TryParse(value, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out result))
            {
                return result;
            }

            return DateTime.MinValue;
        }

        /// <summary>
        /// Decimal parse. XBRL'de nokta ondalık ayırıcı.
        /// </summary>
        private static decimal DecimalParse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0m;

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }

            return 0m;
        }

        /// <summary>
        /// Integer parse.
        /// </summary>
        private static int IntParse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }

            return 0;
        }

        #endregion
    }
}
