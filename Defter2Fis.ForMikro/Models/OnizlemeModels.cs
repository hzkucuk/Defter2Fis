using System;
using System.Collections.Generic;

namespace Defter2Fis.ForMikro.Models
{
    /// <summary>
    /// Önizleme (dry-run) sonuç modeli.
    /// Fiş oluşturma işleminden önce ne olacağını gösterir.
    /// </summary>
    public class OnizlemeSonucu
    {
        /// <summary>İşlenen dönem başlangıcı</summary>
        public DateTime DonemBaslangic { get; set; }

        /// <summary>İşlenen dönem bitişi</summary>
        public DateTime DonemBitis { get; set; }

        /// <summary>Mali yıl</summary>
        public int MaliYil { get; set; }

        /// <summary>Oluşturulacak fiş kayıtları</summary>
        public List<OnizlemeFisKaydi> FisKayitlari { get; } = new List<OnizlemeFisKaydi>();

        /// <summary>Eşleşen cari hesap hareketleri</summary>
        public List<OnizlemeCariEslesmesi> CariEslesmeleri { get; } = new List<OnizlemeCariEslesmesi>();

        /// <summary>Eşleşen stok hareketleri</summary>
        public List<OnizlemeStokEslesmesi> StokEslesmeleri { get; } = new List<OnizlemeStokEslesmesi>();

        /// <summary>Eklenecek eksik hesap planı kayıtları</summary>
        public List<OnizlemeEksikHesap> EksikHesaplar { get; } = new List<OnizlemeEksikHesap>();

        /// <summary>Uyarı ve bilgi mesajları</summary>
        public List<OnizlemeUyari> Uyarilar { get; } = new List<OnizlemeUyari>();

        /// <summary>Mükerrer yevmiyeler (atlanacak)</summary>
        public List<int> MukerrerYevmiyeNolar { get; } = new List<int>();

        /// <summary>Dönemde mevcut cari hareket sayısı (toplam)</summary>
        public int ToplamCariHareket { get; set; }

        /// <summary>Dönemde mevcut stok hareket sayısı (toplam)</summary>
        public int ToplamStokHareket { get; set; }

        #region Hesaplanan Özellikler

        /// <summary>Oluşturulacak fiş sayısı (mükerrer hariç)</summary>
        public int OlusturulacakFisSayisi => FisKayitlari.Count;

        /// <summary>Toplam satır sayısı</summary>
        public int ToplamSatirSayisi
        {
            get
            {
                int toplam = 0;
                foreach (var fis in FisKayitlari)
                    toplam += fis.SatirSayisi;
                return toplam;
            }
        }

        /// <summary>Eşleşen cari hareket sayısı</summary>
        public int EslesenCariSayisi => CariEslesmeleri.Count;

        /// <summary>Eşleşen stok hareket sayısı</summary>
        public int EslesenStokSayisi => StokEslesmeleri.Count;

        /// <summary>Eşleşmeyen cari hareket sayısı</summary>
        public int EslesmeyenCariSayisi => ToplamCariHareket - EslesenCariSayisi;

        /// <summary>Eşleşmeyen stok hareket sayısı</summary>
        public int EslesmeyenStokSayisi => ToplamStokHareket - EslesenStokSayisi;

        /// <summary>Atlanacak mükerrer fiş sayısı</summary>
        public int MukerrerSayisi => MukerrerYevmiyeNolar.Count;

        /// <summary>Kritik uyarı var mı</summary>
        public bool KritikUyariVar
        {
            get
            {
                foreach (var u in Uyarilar)
                    if (u.Seviye == UyariSeviye.Kritik) return true;
                return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// Oluşturulacak tek bir fiş kaydının önizlemesi.
    /// </summary>
    public class OnizlemeFisKaydi
    {
        /// <summary>Yevmiye numarası</summary>
        public int YevmiyeNo { get; set; }

        /// <summary>Fiş tarihi</summary>
        public DateTime Tarih { get; set; }

        /// <summary>Fiş satır sayısı</summary>
        public int SatirSayisi { get; set; }

        /// <summary>Toplam borç</summary>
        public decimal ToplamBorc { get; set; }

        /// <summary>Toplam alacak</summary>
        public decimal ToplamAlacak { get; set; }

        /// <summary>Fiş açıklaması (ilk satırdan)</summary>
        public string Aciklama { get; set; }

        /// <summary>Atanacak sıra no</summary>
        public int AtanacakSiraNo { get; set; }

        /// <summary>Evrak eşleşme durumu</summary>
        public string EslesmeDurumu { get; set; }

        /// <summary>Parse edilen evrak seri bilgisi (ilk satırdan)</summary>
        public string EvrakSeri { get; set; }

        /// <summary>Parse edilen evrak sıra bilgisi (ilk satırdan)</summary>
        public int EvrakSira { get; set; }

        /// <summary>Mükerrer mi (DB'de zaten var)</summary>
        public bool Mukerrer { get; set; }
    }

    /// <summary>
    /// Cari hesap hareketi eşleşme detayı.
    /// </summary>
    public class OnizlemeCariEslesmesi
    {
        /// <summary>E-Defter yevmiye no</summary>
        public int YevmiyeNo { get; set; }

        /// <summary>Cari hesap kodu</summary>
        public string CariHesapKod { get; set; }

        /// <summary>İşlem tarihi</summary>
        public DateTime IslemTarihi { get; set; }

        /// <summary>Mevcut muhasebe fiş no (0 ise atanmamış)</summary>
        public int MevcutMuhFisNo { get; set; }

        /// <summary>Atanacak yeni muhasebe fiş sıra no</summary>
        public int AtanacakMuhFisNo { get; set; }

        /// <summary>Üzerine yazılacak mı (mevcut fiş no varsa)</summary>
        public bool UzerineYazilacak => MevcutMuhFisNo > 0;
    }

    /// <summary>
    /// Stok hareketi eşleşme detayı.
    /// </summary>
    public class OnizlemeStokEslesmesi
    {
        /// <summary>E-Defter yevmiye no</summary>
        public int YevmiyeNo { get; set; }

        /// <summary>İşlem tarihi</summary>
        public DateTime IslemTarihi { get; set; }

        /// <summary>Mevcut muhasebe fiş no (0 ise atanmamış)</summary>
        public int MevcutMuhFisNo { get; set; }

        /// <summary>Atanacak yeni muhasebe fiş sıra no</summary>
        public int AtanacakMuhFisNo { get; set; }

        /// <summary>Üzerine yazılacak mı</summary>
        public bool UzerineYazilacak => MevcutMuhFisNo > 0;
    }

    /// <summary>
    /// Eklenecek eksik hesap planı kaydı.
    /// </summary>
    public class OnizlemeEksikHesap
    {
        /// <summary>Hesap kodu</summary>
        public string HesapKod { get; set; }

        /// <summary>Hesap adı</summary>
        public string HesapIsim { get; set; }

        /// <summary>Hesap tipi (0:Ana, 1:Alt, 2+:Detay)</summary>
        public byte HesapTip { get; set; }

        /// <summary>Tip açıklaması</summary>
        public string TipAciklama
        {
            get
            {
                switch (HesapTip)
                {
                    case 0: return "Ana";
                    case 1: return "Alt";
                    default: return $"D{HesapTip}";
                }
            }
        }
    }

    /// <summary>
    /// Uyarı seviyesi.
    /// </summary>
    public enum UyariSeviye
    {
        Bilgi,
        Uyari,
        Kritik
    }

    /// <summary>
    /// Önizleme uyarı/bilgi mesajı.
    /// </summary>
    public class OnizlemeUyari
    {
        /// <summary>Uyarı seviyesi</summary>
        public UyariSeviye Seviye { get; set; }

        /// <summary>Uyarı mesajı</summary>
        public string Mesaj { get; set; }

        /// <summary>İlgili yevmiye no (varsa)</summary>
        public int? YevmiyeNo { get; set; }

        /// <summary>Seviye ikonu</summary>
        public string SeviyeIkon
        {
            get
            {
                switch (Seviye)
                {
                    case UyariSeviye.Bilgi: return "ℹ";
                    case UyariSeviye.Uyari: return "⚠";
                    case UyariSeviye.Kritik: return "⛔";
                    default: return "?";
                }
            }
        }
    }
}
