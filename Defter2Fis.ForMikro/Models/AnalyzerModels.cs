using System;
using System.Collections.Generic;

namespace Defter2Fis.ForMikro.Models
{
    /// <summary>
    /// Analiz özet bilgisi.
    /// </summary>
    public class AnalizOzeti
    {
        public int DosyaSayisi { get; set; }
        public int ToplamFis { get; set; }
        public int ToplamSatir { get; set; }
        public List<DosyaOzeti> DosyaOzetleri { get; } = new List<DosyaOzeti>();
    }

    /// <summary>
    /// Tek dosya özet bilgisi.
    /// </summary>
    public class DosyaOzeti
    {
        public string DosyaAdi { get; set; }
        public string BenzersizId { get; set; }
        public string FirmaUnvani { get; set; }
        public string SicilNo { get; set; }
        public DateTime DonemBaslangic { get; set; }
        public DateTime DonemBitis { get; set; }
        public int FisSayisi { get; set; }
        public int SatirSayisi { get; set; }
    }

    /// <summary>
    /// DB mevcut durum bilgisi.
    /// </summary>
    public class DbDurumBilgisi
    {
        public int HesapSayisi { get; set; }
        public int FisSayisi { get; set; }
        public DateTime DonemBaslangic { get; set; }
        public DateTime DonemBitis { get; set; }
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
