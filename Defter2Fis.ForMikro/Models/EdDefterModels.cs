using System;
using System.Collections.Generic;

namespace Defter2Fis.ForMikro.Models
{
    /// <summary>
    /// Bir Yevmiye defteri XML dosyasının tamamını temsil eder.
    /// accountingEntries > documentInfo seviyesindeki bilgiler.
    /// </summary>
    public class YevmiyeDefteri
    {
        /// <summary>Defterin benzersiz kimliği (uniqueID). Ör: YEV202504000004</summary>
        public string BenzersizId { get; set; }

        /// <summary>Dönem başlangıç tarihi (periodCoveredStart)</summary>
        public DateTime DonemBaslangic { get; set; }

        /// <summary>Dönem bitiş tarihi (periodCoveredEnd)</summary>
        public DateTime DonemBitis { get; set; }

        /// <summary>Defter oluşturulma tarihi (creationDate)</summary>
        public DateTime OlusturmaTarihi { get; set; }

        /// <summary>Defter açıklaması (entriesComment)</summary>
        public string Aciklama { get; set; }

        /// <summary>Kaynak uygulama bilgisi (sourceApplication)</summary>
        public string KaynakUygulama { get; set; }

        /// <summary>Firma sicil numarası (identifier)</summary>
        public string SicilNo { get; set; }

        /// <summary>Firma unvanı (organizationIdentifier)</summary>
        public string FirmaUnvani { get; set; }

        /// <summary>Mali yıl başlangıç (fiscalYearStart)</summary>
        public DateTime MaliYilBaslangic { get; set; }

        /// <summary>Mali yıl bitiş (fiscalYearEnd)</summary>
        public DateTime MaliYilBitis { get; set; }

        /// <summary>Kaynak XML dosya adı</summary>
        public string DosyaAdi { get; set; }

        /// <summary>Bu defterdeki yevmiye fişleri (entryHeader listesi)</summary>
        public List<YevmiyeFisi> Fisler { get; } = new List<YevmiyeFisi>();
    }

    /// <summary>
    /// Tek bir yevmiye fişini (entryHeader) temsil eder.
    /// </summary>
    public class YevmiyeFisi
    {
        /// <summary>Yevmiye numarası (entryNumber). Ör: 0000000001</summary>
        public string YevmiyeNo { get; set; }

        /// <summary>Yevmiye sayaç değeri (entryNumberCounter). DB'ye yevmiye_no olarak yazılır.</summary>
        public int YevmiyeNoSayac { get; set; }

        /// <summary>Fişin girildiği tarih (enteredDate)</summary>
        public DateTime GirisTarihi { get; set; }

        /// <summary>Fişi giren kullanıcı (enteredBy)</summary>
        public string GirenKullanici { get; set; }

        /// <summary>Fiş açıklaması (entryComment)</summary>
        public string Aciklama { get; set; }

        /// <summary>Toplam borç tutarı (totalDebit)</summary>
        public decimal ToplamBorc { get; set; }

        /// <summary>Toplam alacak tutarı (totalCredit)</summary>
        public decimal ToplamAlacak { get; set; }

        /// <summary>Fiş satırları (entryDetail listesi)</summary>
        public List<FisDetaySatiri> Satirlar { get; } = new List<FisDetaySatiri>();
    }

    /// <summary>
    /// Yevmiye fişinin tek bir satırını (entryDetail) temsil eder.
    /// </summary>
    public class FisDetaySatiri
    {
        /// <summary>Satır numarası (lineNumber)</summary>
        public int SatirNo { get; set; }

        /// <summary>Satır sayaç değeri (lineNumberCounter)</summary>
        public int SatirNoSayac { get; set; }

        /// <summary>Ana hesap kodu (accountMainID). Ör: 770</summary>
        public string AnaHesapKod { get; set; }

        /// <summary>Ana hesap açıklaması (accountMainDescription). Ör: GENEL YÖNETİM GİDERLERİ</summary>
        public string AnaHesapAd { get; set; }

        /// <summary>Alt hesap kodu (accountSubID). Ör: 770.50.0018</summary>
        public string AltHesapKod { get; set; }

        /// <summary>Alt hesap açıklaması (accountSubDescription). Ör: SERTİFİKA GİDERİ</summary>
        public string AltHesapAd { get; set; }

        /// <summary>Tutar (amount)</summary>
        public decimal Tutar { get; set; }

        /// <summary>Borç/Alacak kodu (debitCreditCode). D=Borç, C=Alacak</summary>
        public string BorcAlacakKodu { get; set; }

        /// <summary>Kayıt tarihi (postingDate)</summary>
        public DateTime KayitTarihi { get; set; }

        /// <summary>Belge türü (documentType). Ör: invoice, other</summary>
        public string BelgeTuru { get; set; }

        /// <summary>Belge numarası (documentNumber)</summary>
        public string BelgeNo { get; set; }

        /// <summary>Belge referansı (documentReference)</summary>
        public string BelgeReferansi { get; set; }

        /// <summary>Belge tarihi (documentDate)</summary>
        public DateTime? BelgeTarihi { get; set; }

        /// <summary>Satır açıklaması (detailComment)</summary>
        public string DetayAciklama { get; set; }

        /// <summary>Hesaplanan meblağ: D ise +Tutar, C ise -Tutar</summary>
        public decimal Meblag
        {
            get { return BorcAlacakKodu == "D" ? Tutar : -Tutar; }
        }

        /// <summary>Kullanılacak hesap kodu: AltHesapKod varsa o, yoksa AnaHesapKod</summary>
        public string EtkinHesapKod
        {
            get { return string.IsNullOrWhiteSpace(AltHesapKod) ? AnaHesapKod : AltHesapKod; }
        }

        /// <summary>Kullanılacak hesap adı: AltHesapAd varsa o, yoksa AnaHesapAd</summary>
        public string EtkinHesapAd
        {
            get { return string.IsNullOrWhiteSpace(AltHesapAd) ? AnaHesapAd : AltHesapAd; }
        }
    }
}
