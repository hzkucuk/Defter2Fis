using System;
using System.Collections.Generic;

namespace Defter2Fis.ForMikro.Models
{
    /// <summary>
    /// MUHASEBE_FISLERI tablosunun entity modeli.
    /// Her satır bir fiş satırını temsil eder; fişler fis_sira_no + fis_tarih ile gruplanır.
    /// </summary>
    public class MuhasebeFisi
    {
        public Guid FisGuid { get; set; }
        public short FisDBCno { get; set; }
        public int FisSpecRECno { get; set; }
        public bool FisIptal { get; set; }
        public short FisFileId { get; set; }
        public bool FisHidden { get; set; }
        public bool FisKilitli { get; set; }
        public bool FisDegisti { get; set; }
        public int FisChecksum { get; set; }
        public short FisCreateUser { get; set; }
        public DateTime FisCreateDate { get; set; }
        public short FisLastupUser { get; set; }
        public DateTime FisLastupDate { get; set; }
        public string FisSpecial1 { get; set; }
        public string FisSpecial2 { get; set; }
        public string FisSpecial3 { get; set; }
        public int FisFirmaNo { get; set; }
        public int FisSubeNo { get; set; }
        public int FisMaliYil { get; set; }
        public DateTime FisTarih { get; set; }
        public int FisSiraNo { get; set; }
        public byte FisTur { get; set; }
        public string FisHesapKod { get; set; }
        public int FisSatirNo { get; set; }
        public string FisAciklama1 { get; set; }
        public double FisMeblag0 { get; set; }
        public double FisMeblag1 { get; set; }
        public double FisMeblag2 { get; set; }
        public double FisMeblag3 { get; set; }
        public double FisMeblag4 { get; set; }
        public double FisMeblag5 { get; set; }
        public double FisMeblag6 { get; set; }
        public string FisSorumlulukKodu { get; set; }
        public byte FisTicariTip { get; set; }
        public Guid FisTicariUid { get; set; }
        public bool FisKurFarkiFl { get; set; }
        public byte FisTicariEvrakTip { get; set; }
        public string FisTicEvrakSeri { get; set; }
        public int FisTicEvrakSira { get; set; }
        public string FisTicBelgeNo { get; set; }
        public DateTime FisTicBelgeTarihi { get; set; }
        public int FisYevmiyeNo { get; set; }
        public short FisKatagori { get; set; }
        public short FisEvrakDBCno { get; set; }
        public byte FisFMahsupTipi { get; set; }
        public string FisFOzelMahKod { get; set; }
        public string FisGrupKodu { get; set; }
        public byte FisAktifPasif { get; set; }
        public string FisProjeKodu { get; set; }
        public string FisHareketGrupKodu1 { get; set; }
        public string FisHareketGrupKodu2 { get; set; }
        public string FisHareketGrupKodu3 { get; set; }

        /// <summary>
        /// E-Defter verilerinden yeni bir Mikro fiş satırı oluşturur.
        /// </summary>
        public static MuhasebeFisi FromEdDefter(
            FisDetaySatiri satir,
            int firmaNo,
            int subeNo,
            short dbcNo,
            int maliYil,
            int siraNo,
            int satirNo,
            int yevmiyeNo)
        {
            return new MuhasebeFisi
            {
                FisGuid = Guid.NewGuid(),
                FisDBCno = dbcNo,
                FisSpecRECno = 0,
                FisIptal = false,
                FisFileId = 0,
                FisHidden = false,
                FisKilitli = false,
                FisDegisti = false,
                FisChecksum = 0,
                FisCreateUser = 1,
                FisCreateDate = DateTime.Now,
                FisLastupUser = 1,
                FisLastupDate = DateTime.Now,
                FisSpecial1 = string.Empty,
                FisSpecial2 = string.Empty,
                FisSpecial3 = string.Empty,
                FisFirmaNo = firmaNo,
                FisSubeNo = subeNo,
                FisMaliYil = maliYil,
                FisTarih = satir.KayitTarihi,
                FisSiraNo = siraNo,
                FisTur = 0, // Mahsup fişi
                FisHesapKod = satir.EtkinHesapKod ?? string.Empty,
                FisSatirNo = satirNo,
                FisAciklama1 = TruncateString(satir.DetayAciklama, 127),
                FisMeblag0 = (double)satir.Meblag,
                FisMeblag1 = 0,
                FisMeblag2 = 0,
                FisMeblag3 = 0,
                FisMeblag4 = 0,
                FisMeblag5 = 0,
                FisMeblag6 = 0,
                FisSorumlulukKodu = string.Empty,
                FisTicariTip = 0,
                FisTicariUid = Guid.Empty,
                FisKurFarkiFl = false,
                FisTicariEvrakTip = 0,
                FisTicEvrakSeri = string.Empty,
                FisTicEvrakSira = 0,
                FisTicBelgeNo = satir.BelgeNo ?? string.Empty,
                FisTicBelgeTarihi = satir.BelgeTarihi ?? satir.KayitTarihi,
                FisYevmiyeNo = yevmiyeNo,
                FisKatagori = 0,
                FisEvrakDBCno = 0,
                FisFMahsupTipi = 0,
                FisFOzelMahKod = string.Empty,
                FisGrupKodu = string.Empty,
                FisAktifPasif = 0,
                FisProjeKodu = string.Empty,
                FisHareketGrupKodu1 = string.Empty,
                FisHareketGrupKodu2 = string.Empty,
                FisHareketGrupKodu3 = string.Empty
            };
        }

        private static string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

    /// <summary>
    /// MUHASEBE_HESAP_PLANI tablosunun entity modeli.
    /// </summary>
    public class MuhasebeHesapPlani
    {
        public Guid MuhGuid { get; set; }
        public short MuhDBCno { get; set; }
        public int MuhSpecRECno { get; set; }
        public bool MuhIptal { get; set; }
        public short MuhFileId { get; set; }
        public bool MuhHidden { get; set; }
        public bool MuhKilitli { get; set; }
        public bool MuhDegisti { get; set; }
        public int MuhChecksum { get; set; }
        public short MuhCreateUser { get; set; }
        public DateTime MuhCreateDate { get; set; }
        public short MuhLastupUser { get; set; }
        public DateTime MuhLastupDate { get; set; }
        public string MuhSpecial1 { get; set; }
        public string MuhSpecial2 { get; set; }
        public string MuhSpecial3 { get; set; }
        public string MuhHesapKod { get; set; }
        public string MuhHesapIsim1 { get; set; }
        public string MuhHesapIsim2 { get; set; }
        public byte MuhHesapTip { get; set; }
        public byte MuhDovizCinsi { get; set; }
        public bool MuhKurFarkiFl { get; set; }
        public byte MuhSorumMerk { get; set; }
        public DateTime MuhKilitTarihi { get; set; }

        /// <summary>
        /// Hesap kodu derinliğine göre hesap tipini belirler.
        /// Mikro'da: 0=Ana grup, 1=Alt grup, 2-4=Detay seviye
        /// Nokta sayısına göre: "770"→0 nokta→tip 0, "770.50"→1 nokta→tip 1, "770.50.0018"→2 nokta→tip 2, vb.
        /// </summary>
        public static byte HesapTipBelirle(string hesapKod)
        {
            if (string.IsNullOrWhiteSpace(hesapKod)) return 0;

            int noktaSayisi = 0;
            foreach (char c in hesapKod)
            {
                if (c == '.') noktaSayisi++;
            }

            if (noktaSayisi >= 4) return 4;
            return (byte)noktaSayisi;
        }

        /// <summary>
        /// E-Defter hesap bilgilerinden yeni bir hesap planı kaydı oluşturur.
        /// </summary>
        public static MuhasebeHesapPlani FromEdDefter(
            string hesapKod,
            string hesapIsim,
            short dbcNo)
        {
            return new MuhasebeHesapPlani
            {
                MuhGuid = Guid.NewGuid(),
                MuhDBCno = dbcNo,
                MuhSpecRECno = 0,
                MuhIptal = false,
                MuhFileId = 0,
                MuhHidden = false,
                MuhKilitli = false,
                MuhDegisti = false,
                MuhChecksum = 0,
                MuhCreateUser = 1,
                MuhCreateDate = DateTime.Now,
                MuhLastupUser = 1,
                MuhLastupDate = DateTime.Now,
                MuhSpecial1 = string.Empty,
                MuhSpecial2 = string.Empty,
                MuhSpecial3 = string.Empty,
                MuhHesapKod = hesapKod ?? string.Empty,
                MuhHesapIsim1 = TruncateString(hesapIsim, 90),
                MuhHesapIsim2 = string.Empty,
                MuhHesapTip = HesapTipBelirle(hesapKod),
                MuhDovizCinsi = 0,
                MuhKurFarkiFl = false,
                MuhSorumMerk = 0,
                MuhKilitTarihi = new DateTime(1899, 12, 30) // Mikro default
            };
        }

        private static string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

    /// <summary>
    /// Dönem içi mevcut fiş özet bilgisi (pre-check için DataGridView'de gösterilir).
    /// </summary>
    public class DonemFisOzeti
    {
        /// <summary>Yevmiye numarası</summary>
        public int YevmiyeNo { get; set; }

        /// <summary>Fiş tarihi</summary>
        public DateTime Tarih { get; set; }

        /// <summary>Fişteki satır sayısı</summary>
        public int SatirSayisi { get; set; }

        /// <summary>Toplam borç tutarı</summary>
        public decimal ToplamBorc { get; set; }

        /// <summary>Toplam alacak tutarı</summary>
        public decimal ToplamAlacak { get; set; }

        /// <summary>Fişin ilk açıklama satırı</summary>
        public string Aciklama { get; set; }
    }

    /// <summary>
    /// CARI_HESAP_HAREKETLERI tablosunun özet DTO'su.
    /// Evrak seri/sıra eşleştirme ve muhasebe fiş referans güncelleme için kullanılır.
    /// </summary>
    public class CariHesapHareketi
    {
        /// <summary>Kayıt benzersiz kimliği (cha_Guid)</summary>
        public Guid ChaGuid { get; set; }

        /// <summary>Evrak tipi (cha_evrak_tip). 0:Fatura, 1:İrsaliye, vb.</summary>
        public byte ChaEvrakTip { get; set; }

        /// <summary>Evrak seri (cha_evrakno_seri). Ör: "A"</summary>
        public string ChaEvraknoSeri { get; set; }

        /// <summary>Evrak sıra numarası (cha_evrakno_sira). Ör: 123</summary>
        public int ChaEvraknoSira { get; set; }

        /// <summary>İşlem tarihi (cha_tarihi)</summary>
        public DateTime ChaTarihi { get; set; }

        /// <summary>Hesap kodu (cha_kod)</summary>
        public string ChaKod { get; set; }

        /// <summary>Muhasebe fiş sıra numarası (cha_fis_sirano)</summary>
        public int ChaMuhFisNo { get; set; }

        /// <summary>Muhasebe fiş tarihi (cha_fis_tarih)</summary>
        public DateTime ChaMuhFisTarihi { get; set; }

        /// <summary>Eşleştirme anahtarı: SERİ-SIRA</summary>
        public string EvrakAnahtar
        {
            get
            {
                string seri = (ChaEvraknoSeri ?? string.Empty).Trim();
                return string.IsNullOrEmpty(seri) ? ChaEvraknoSira.ToString() : $"{seri}-{ChaEvraknoSira}";
            }
        }
    }

    /// <summary>
    /// Belirli bir ay dönemine ait muhasebe fişi özet bilgisi.
    /// Önceki ay doğrulama ve yevmiye sürekliliği kontrolü için kullanılır.
    /// </summary>
    public class AyFisBilgisi
    {
        /// <summary>Dönem başlangıç tarihi</summary>
        public DateTime DonemBaslangic { get; set; }

        /// <summary>Dönem bitiş tarihi</summary>
        public DateTime DonemBitis { get; set; }

        /// <summary>Benzersiz yevmiye (fiş) sayısı</summary>
        public int FisSayisi { get; set; }

        /// <summary>Toplam fiş satır sayısı</summary>
        public int SatirSayisi { get; set; }

        /// <summary>En küçük yevmiye numarası</summary>
        public int MinYevmiyeNo { get; set; }

        /// <summary>En büyük yevmiye numarası</summary>
        public int MaxYevmiyeNo { get; set; }

        /// <summary>En erken fiş tarihi</summary>
        public DateTime MinTarih { get; set; }

        /// <summary>En geç fiş tarihi</summary>
        public DateTime MaxTarih { get; set; }

        /// <summary>DB'de veri var mı?</summary>
        public bool VeriMevcut => FisSayisi > 0;
    }

    /// <summary>
    /// Ay sürekliliği kontrol sonucu.
    /// Önceki ay ile çalışılan ay arasındaki yevmiye numarası sürekliliğini doğrular.
    /// </summary>
    public class SureklilkKontrolSonucu
    {
        /// <summary>Önceki yevmiyeler DB'de mevcut mu?</summary>
        public bool OncekiAyMevcut { get; set; }

        /// <summary>Yevmiye numaraları sürekli mi (boşluk yok)?</summary>
        public bool Surekli { get; set; }

        /// <summary>Aktarıma izin verilir mi?</summary>
        public bool AktarimIzinli { get; set; }

        /// <summary>İlk ay mı (yevmiye 1'den başlıyor)?</summary>
        public bool IlkAy { get; set; }

        /// <summary>DB'deki mevcut max yevmiye numarası (mali yıl genelinde)</summary>
        public int DbMaxYevmiyeNo { get; set; }

        /// <summary>DB'deki toplam benzersiz yevmiye sayısı (çalışılan aydan önceki)</summary>
        public int DbYevmiyeSayisi { get; set; }

        /// <summary>Önceki aya ait DB bilgisi (tarih bazlı, bilgilendirme amaçlı)</summary>
        public AyFisBilgisi OncekiAyBilgisi { get; set; }

        /// <summary>Çalışılan aya ait E-Defter bilgisi</summary>
        public AyFisBilgisi CalislanAyBilgisi { get; set; }

        /// <summary>Kontrol mesajları (log için)</summary>
        public List<string> Mesajlar { get; } = new List<string>();
    }

    /// <summary>
    /// Mali yıl genelinde yevmiye süreklilik bilgisi (tarihten bağımsız).
    /// </summary>
    public class YevmiyeSureklilkBilgisi
    {
        /// <summary>Belirtilen yevmiye numarasından önceki benzersiz yevmiye sayısı</summary>
        public int YevmiyeSayisi { get; set; }

        /// <summary>Belirtilen yevmiye numarasından önceki en büyük yevmiye numarası</summary>
        public int MaxYevmiyeNo { get; set; }

        /// <summary>DB'de hiç yevmiye var mı?</summary>
        public bool VeriMevcut => YevmiyeSayisi > 0;
    }

    /// <summary>
    /// STOK_HAREKETLERI tablosunun özet DTO'su.
    /// Evrak seri/sıra eşleştirme ve muhasebe fiş referans güncelleme için kullanılır.
    /// </summary>
    public class StokHareketi
    {
        /// <summary>Kayıt benzersiz kimliği (sth_Guid)</summary>
        public Guid SthGuid { get; set; }

        /// <summary>Evrak tipi (sth_evrak_tip)</summary>
        public byte SthEvrakTip { get; set; }

        /// <summary>Evrak seri (sth_evrakno_seri). Ör: "A"</summary>
        public string SthEvraknoSeri { get; set; }

        /// <summary>Evrak sıra numarası (sth_evrakno_sira). Ör: 123</summary>
        public int SthEvraknoSira { get; set; }

        /// <summary>İşlem tarihi (sth_tarihi)</summary>
        public DateTime SthTarihi { get; set; }

        /// <summary>Muhasebe fiş sıra numarası (sth_fis_sirano)</summary>
        public int SthMuhFisNo { get; set; }

        /// <summary>Muhasebe fiş tarihi (sth_fis_tarihi)</summary>
        public DateTime SthMuhFisTarihi { get; set; }

        /// <summary>Eşleştirme anahtarı: SERİ-SIRA</summary>
        public string EvrakAnahtar
        {
            get
            {
                string seri = (SthEvraknoSeri ?? string.Empty).Trim();
                return string.IsNullOrEmpty(seri) ? SthEvraknoSira.ToString() : $"{seri}-{SthEvraknoSira}";
            }
        }
    }
}
