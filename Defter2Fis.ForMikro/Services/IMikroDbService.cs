using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Defter2Fis.ForMikro.Models;

namespace Defter2Fis.ForMikro.Services
{
    /// <summary>
    /// Mikro ERP V16 Jump MSSQL veritabanı servis sözleşmesi.
    /// Test edilebilirlik ve bağımlılık enjeksiyonu için soyutlama katmanı.
    /// </summary>
    public interface IMikroDbService
    {
        #region Bağlantı

        /// <summary>
        /// Veritabanı bağlantısını test eder.
        /// </summary>
        bool BaglantıTest(out string hataMesaji);

        #endregion

        #region Hesap Planı İşlemleri

        /// <summary>
        /// Hesap planında belirtilen kodun mevcut olup olmadığını kontrol eder.
        /// </summary>
        bool HesapMevcutMu(string hesapKod);

        /// <summary>
        /// Mevcut tüm hesap kodlarını HashSet olarak döner (performanslı toplu kontrol için).
        /// </summary>
        HashSet<string> TumHesapKodlariniGetir();

        /// <summary>
        /// Eksik hesap planı kaydını veritabanına ekler.
        /// </summary>
        void HesapPlanıEkle(MuhasebeHesapPlani hesap, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Fiş İşlemleri

        /// <summary>
        /// Belirtilen tarih ve mali yıl için mevcut en büyük fiş sıra numarasını döner.
        /// </summary>
        int MaxSiraNoGetir(DateTime tarih, int maliYil, int firmaNo, int subeNo);

        /// <summary>
        /// Belirtilen yevmiye numarasının DB'de olup olmadığını kontrol eder (mükerrer koruma).
        /// NDX_MUHASEBE_FISLERI_02 unique index ile uyumlu: fis_firmano + fis_maliyil + fis_yevmiye_no
        /// </summary>
        bool YevmiyeNoMevcutMu(int yevmiyeNo, int maliYil, int firmaNo, int subeNo);

        /// <summary>
        /// Belirtilen yevmiye numarasına ait mevcut fiş satırlarını siler.
        /// Üzerine yazma senaryosunda INSERT öncesi çağrılır.
        /// </summary>
        int YevmiyeFisleriniSil(int yevmiyeNo, int maliYil, int firmaNo, int subeNo, SqlConnection conn, SqlTransaction tran);

        /// <summary>
        /// Tek bir fiş satırını veritabanına ekler.
        /// </summary>
        void FisSatiriEkle(MuhasebeFisi fis, SqlConnection conn, SqlTransaction tran);

        /// <summary>
        /// Yeni bir bağlantı ve transaction açar (fiş grubu yazımı için).
        /// </summary>
        SqlConnection YeniBaglanti();

        #endregion

        #region İstatistik

        /// <summary>
        /// Belirtilen mali yıl ve tarih aralığında mevcut fiş sayısını döner.
        /// </summary>
        int MevcutFisSayisi(int maliYil, DateTime baslangic, DateTime bitis, int firmaNo, int subeNo);

        /// <summary>
        /// Hesap planındaki toplam hesap sayısını döner.
        /// </summary>
        int HesapPlaniSayisi();

        /// <summary>
        /// Belirtilen tarih aralığındaki muhasebe fişlerinin özet bilgisini döner.
        /// Bilgilendirme amaçlı kullanılır (tarih bazlı).
        /// </summary>
        AyFisBilgisi AyFisBilgisiGetir(int maliYil, DateTime donemBas, DateTime donemBit, int firmaNo, int subeNo);

        /// <summary>
        /// Belirtilen yevmiye numarasından önceki tüm yevmiyelerin süreklilik bilgisini döner.
        /// Tarihten bağımsız, yevmiye numarası bazlı kontrol için kullanılır.
        /// </summary>
        YevmiyeSureklilkBilgisi YevmiyeSureklilkBilgisiGetir(int maliYil, int calislanMinYevmiye, int firmaNo, int subeNo);

        #endregion

        #region Dönem Veri Kontrolü

        /// <summary>
        /// Belirtilen dönem ve mali yıl için mevcut fiş özetlerini döner.
        /// </summary>
        List<DonemFisOzeti> DonemVerileriGetir(int maliYil, DateTime baslangic, DateTime bitis, int firmaNo, int subeNo);

        /// <summary>
        /// Belirtilen dönemdeki tüm fiş satırlarını siler.
        /// </summary>
        int DonemVerileriSil(int maliYil, DateTime baslangic, DateTime bitis, int firmaNo, int subeNo);

        #endregion

        #region Cari Hesap Hareketleri

        /// <summary>
        /// Belirtilen dönemdeki cari hesap hareketlerini fiş tarih/sıra bilgileri ile getirir.
        /// </summary>
        List<CariHesapHareketi> DonemCariHareketleriGetir(DateTime baslangic, DateTime bitis, int firmaNo, int subeNo);

        #endregion

        #region Stok Hareketleri

        /// <summary>
        /// Belirtilen dönemdeki stok hareketlerini fiş tarih/sıra bilgileri ile getirir.
        /// </summary>
        List<StokHareketi> DonemStokHareketleriGetir(DateTime baslangic, DateTime bitis, int firmaNo, int subeNo);

        #endregion

        #region Güvenli Sorgulama Yardımcıları

        /// <summary>
        /// Cari hareketleri güvenli şekilde getirir. Tablo yoksa boş liste döner.
        /// </summary>
        List<CariHesapHareketi> DonemCariHareketleriGetirGuvenli(DateTime baslangic, DateTime bitis, int firmaNo, int subeNo);

        /// <summary>
        /// Stok hareketleri güvenli şekilde getirir. Tablo yoksa boş liste döner.
        /// </summary>
        List<StokHareketi> DonemStokHareketleriGetirGuvenli(DateTime baslangic, DateTime bitis, int firmaNo, int subeNo);

        #endregion

        #region Veritabanı Yedekleme

        /// <summary>
        /// Veritabanının tam yedeğini (FULL BACKUP) alır.
        /// </summary>
        /// <param name="yedekDizini">Yedek dizini. null ise SQL Server varsayılan dizini kullanılır.</param>
        /// <param name="ilerlemeCallback">İlerleme bildirimi (yüzde 0-100, durum mesajı). null olabilir.</param>
        YedeklemeSonucu VeritabaniYedekle(string yedekDizini = null, Action<int, string> ilerlemeCallback = null);

        #endregion
    }
}
