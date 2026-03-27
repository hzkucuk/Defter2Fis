using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Defter2Fis.ForMikro.Models;

namespace Defter2Fis.ForMikro.Services
{
    /// <summary>
    /// Mikro ERP V16 Jump MSSQL veritabanı servis katmanı.
    /// MUHASEBE_FISLERI, MUHASEBE_HESAP_PLANI tabloları üzerinde okuma/yazma işlemleri.
    /// </summary>
    public class MikroDbService : IMikroDbService
    {
        private readonly string _connectionString;

        public MikroDbService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MikroDB"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new ConfigurationErrorsException("MikroDB connection string bulunamadı.");
        }

        /// <summary>
        /// Veritabanı bağlantısını test eder.
        /// </summary>
        public bool BaglantıTest(out string hataMesaji)
        {
            hataMesaji = null;
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return false;
            }
        }

        #region Hesap Planı İşlemleri

        /// <summary>
        /// Hesap planında belirtilen kodun mevcut olup olmadığını kontrol eder.
        /// </summary>
        public bool HesapMevcutMu(string hesapKod)
        {
            if (string.IsNullOrWhiteSpace(hesapKod)) return false;

            const string sql = @"SELECT COUNT(1) FROM MUHASEBE_HESAP_PLANI 
                                 WHERE muh_hesap_kod = @hesapKod AND muh_iptal = 0 AND muh_DBCno = 0";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@hesapKod", SqlDbType.NVarChar, 25).Value = hesapKod;
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        /// <summary>
        /// Mevcut tüm hesap kodlarını HashSet olarak döner (performanslı toplu kontrol için).
        /// </summary>
        public HashSet<string> TumHesapKodlariniGetir()
        {
            var kodlar = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            const string sql = @"SELECT muh_hesap_kod FROM MUHASEBE_HESAP_PLANI 
                                 WHERE muh_iptal = 0 AND muh_DBCno = 0";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string kod = reader.GetString(0)?.Trim();
                        if (!string.IsNullOrEmpty(kod))
                            kodlar.Add(kod);
                    }
                }
            }

            return kodlar;
        }

        /// <summary>
        /// Eksik hesap planı kaydını veritabanına ekler.
        /// </summary>
        public void HesapPlanıEkle(MuhasebeHesapPlani hesap, SqlConnection conn, SqlTransaction tran)
        {
            const string sql = @"INSERT INTO MUHASEBE_HESAP_PLANI (
                muh_Guid, muh_DBCno, muh_SpecRECno, muh_iptal, muh_fileid,
                muh_hidden, muh_kilitli, muh_degisti, muh_checksum,
                muh_create_user, muh_create_date, muh_lastup_user, muh_lastup_date,
                muh_special1, muh_special2, muh_special3,
                muh_hesap_kod, muh_hesap_isim1, muh_hesap_isim2, muh_hesap_tip,
                muh_doviz_cinsi, muh_kurfarki_fl, muh_sorum_merk, muh_kilittarihi,
                muh_hes_dav_bicimi, muh_kdv_tipi, muh_calisma_sekli,
                muh_maliyet_dagitim_sekli, muh_grupkodu, muh_enf_fark_maliyet_fl,
                muh_kdv_dagitim_sekli, muh_miktar_oto_fl, muh_ticariden_bilgi_girisi_fl,
                muh_proje_detayi, muh_kesin_mizan_hesap_kodu, muh_enf_calisma_sekli
            ) VALUES (
                @guid, @dbcno, 0, 0, 0,
                0, 0, 0, 0,
                1, @createDate, 1, @createDate,
                '', '', '',
                @hesapKod, @hesapIsim1, '', @hesapTip,
                0, 0, 0, @kilitTarihi,
                0, 0, 0,
                0, '', 0,
                0, 0, 0,
                0, '', 0
            )";

            using (var cmd = new SqlCommand(sql, conn, tran))
            {
                cmd.Parameters.Add("@guid", SqlDbType.UniqueIdentifier).Value = hesap.MuhGuid;
                cmd.Parameters.Add("@dbcno", SqlDbType.SmallInt).Value = hesap.MuhDBCno;
                cmd.Parameters.Add("@createDate", SqlDbType.DateTime).Value = hesap.MuhCreateDate;
                cmd.Parameters.Add("@hesapKod", SqlDbType.NVarChar, 25).Value = hesap.MuhHesapKod;
                cmd.Parameters.Add("@hesapIsim1", SqlDbType.NVarChar, 90).Value = hesap.MuhHesapIsim1;
                cmd.Parameters.Add("@hesapTip", SqlDbType.TinyInt).Value = hesap.MuhHesapTip;
                cmd.Parameters.Add("@kilitTarihi", SqlDbType.DateTime).Value = hesap.MuhKilitTarihi;

                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Fiş İşlemleri

        /// <summary>
        /// Belirtilen tarih ve mali yıl için mevcut en büyük fiş sıra numarasını döner.
        /// </summary>
        public int MaxSiraNoGetir(DateTime tarih, int maliYil, int firmaNo, int subeNo)
        {
            const string sql = @"SELECT ISNULL(MAX(fis_sira_no), 0) 
                                 FROM MUHASEBE_FISLERI 
                                 WHERE fis_tarih = @tarih 
                                   AND fis_maliyil = @maliYil 
                                   AND fis_firmano = @firmaNo 
                                   AND fis_subeno = @subeNo 
                                   AND fis_tur = 0 
                                   AND fis_iptal = 0 
                                   AND fis_DBCno = 0";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@tarih", SqlDbType.DateTime).Value = tarih.Date;
                cmd.Parameters.Add("@maliYil", SqlDbType.Int).Value = maliYil;
                cmd.Parameters.Add("@firmaNo", SqlDbType.Int).Value = firmaNo;
                cmd.Parameters.Add("@subeNo", SqlDbType.Int).Value = subeNo;
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Belirtilen yevmiye numarasının zaten DB'de olup olmadığını kontrol eder (mükerrer koruma).
        /// </summary>
        public bool YevmiyeNoMevcutMu(int yevmiyeNo, int maliYil, int firmaNo, int subeNo)
        {
            const string sql = @"SELECT COUNT(1) FROM MUHASEBE_FISLERI 
                                 WHERE fis_yevmiye_no = @yevNo
                                   AND fis_maliyil = @maliYil
                                   AND fis_firmano = @firmaNo
                                   AND fis_subeno = @subeNo
                                   AND fis_tur = 0
                                   AND fis_iptal = 0
                                   AND fis_DBCno = 0";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@yevNo", SqlDbType.Int).Value = yevmiyeNo;
                cmd.Parameters.Add("@maliYil", SqlDbType.Int).Value = maliYil;
                cmd.Parameters.Add("@firmaNo", SqlDbType.Int).Value = firmaNo;
                cmd.Parameters.Add("@subeNo", SqlDbType.Int).Value = subeNo;
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        /// <summary>
        /// Tek bir fiş satırını veritabanına ekler.
        /// </summary>
        public void FisSatiriEkle(MuhasebeFisi fis, SqlConnection conn, SqlTransaction tran)
        {
            const string sql = @"INSERT INTO MUHASEBE_FISLERI (
                fis_Guid, fis_DBCno, fis_SpecRECno, fis_iptal, fis_fileid,
                fis_hidden, fis_kilitli, fis_degisti, fis_checksum,
                fis_create_user, fis_create_date, fis_lastup_user, fis_lastup_date,
                fis_special1, fis_special2, fis_special3,
                fis_firmano, fis_subeno, fis_maliyil, fis_tarih,
                fis_sira_no, fis_tur, fis_hesap_kod, fis_satir_no,
                fis_aciklama1, fis_meblag0, fis_meblag1, fis_meblag2,
                fis_meblag3, fis_meblag4, fis_meblag5, fis_meblag6,
                fis_sorumluluk_kodu, fis_ticari_tip, fis_ticari_uid,
                fis_kurfarkifl, fis_ticari_evraktip,
                fis_tic_evrak_seri, fis_tic_evrak_sira,
                fis_tic_belgeno, fis_tic_belgetarihi,
                fis_yevmiye_no, fis_katagori, fis_evrak_DBCno,
                fis_fmahsup_tipi, fis_fozelmahkod, fis_grupkodu,
                fis_aktif_pasif, fis_proje_kodu,
                fis_HareketGrupKodu1, fis_HareketGrupKodu2, fis_HareketGrupKodu3
            ) VALUES (
                @guid, @dbcno, 0, 0, 0,
                0, 0, 0, 0,
                1, @createDate, 1, @createDate,
                '', '', '',
                @firmaNo, @subeNo, @maliYil, @tarih,
                @siraNo, @tur, @hesapKod, @satirNo,
                @aciklama1, @meblag0, 0, 0,
                0, 0, 0, 0,
                '', 0, @ticariUid,
                0, 0,
                '', 0,
                @ticBelgeNo, @ticBelgeTarihi,
                @yevmiyeNo, 0, 0,
                0, '', '',
                0, '',
                '', '', ''
            )";

            using (var cmd = new SqlCommand(sql, conn, tran))
            {
                cmd.Parameters.Add("@guid", SqlDbType.UniqueIdentifier).Value = fis.FisGuid;
                cmd.Parameters.Add("@dbcno", SqlDbType.SmallInt).Value = fis.FisDBCno;
                cmd.Parameters.Add("@createDate", SqlDbType.DateTime).Value = fis.FisCreateDate;
                cmd.Parameters.Add("@firmaNo", SqlDbType.Int).Value = fis.FisFirmaNo;
                cmd.Parameters.Add("@subeNo", SqlDbType.Int).Value = fis.FisSubeNo;
                cmd.Parameters.Add("@maliYil", SqlDbType.Int).Value = fis.FisMaliYil;
                cmd.Parameters.Add("@tarih", SqlDbType.DateTime).Value = fis.FisTarih;
                cmd.Parameters.Add("@siraNo", SqlDbType.Int).Value = fis.FisSiraNo;
                cmd.Parameters.Add("@tur", SqlDbType.TinyInt).Value = fis.FisTur;
                cmd.Parameters.Add("@hesapKod", SqlDbType.NVarChar, 25).Value = fis.FisHesapKod;
                cmd.Parameters.Add("@satirNo", SqlDbType.Int).Value = fis.FisSatirNo;
                cmd.Parameters.Add("@aciklama1", SqlDbType.NVarChar, 127).Value = fis.FisAciklama1;
                cmd.Parameters.Add("@meblag0", SqlDbType.Float).Value = fis.FisMeblag0;
                cmd.Parameters.Add("@ticariUid", SqlDbType.UniqueIdentifier).Value = fis.FisTicariUid;
                cmd.Parameters.Add("@ticBelgeNo", SqlDbType.NVarChar, 50).Value = fis.FisTicBelgeNo;
                cmd.Parameters.Add("@ticBelgeTarihi", SqlDbType.DateTime).Value = fis.FisTicBelgeTarihi;
                cmd.Parameters.Add("@yevmiyeNo", SqlDbType.Int).Value = fis.FisYevmiyeNo;

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Yeni bir bağlantı ve transaction açar (fiş grubu yazımı için).
        /// </summary>
        public SqlConnection YeniBaglanti()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

        #endregion

        #region İstatistik

        /// <summary>
        /// Belirtilen mali yıl ve tarih aralığında mevcut fiş sayısını döner.
        /// </summary>
        public int MevcutFisSayisi(int maliYil, DateTime baslangic, DateTime bitis, int firmaNo, int subeNo)
        {
            const string sql = @"SELECT COUNT(DISTINCT fis_yevmiye_no) 
                                 FROM MUHASEBE_FISLERI 
                                 WHERE fis_maliyil = @maliYil 
                                   AND fis_tarih >= @baslangic 
                                   AND fis_tarih <= @bitis
                                   AND fis_firmano = @firmaNo
                                   AND fis_subeno = @subeNo
                                   AND fis_tur = 0
                                   AND fis_iptal = 0
                                   AND fis_DBCno = 0";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@maliYil", SqlDbType.Int).Value = maliYil;
                cmd.Parameters.Add("@baslangic", SqlDbType.DateTime).Value = baslangic.Date;
                cmd.Parameters.Add("@bitis", SqlDbType.DateTime).Value = bitis.Date;
                cmd.Parameters.Add("@firmaNo", SqlDbType.Int).Value = firmaNo;
                cmd.Parameters.Add("@subeNo", SqlDbType.Int).Value = subeNo;
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Hesap planındaki toplam hesap sayısını döner.
        /// </summary>
        public int HesapPlaniSayisi()
        {
            const string sql = "SELECT COUNT(1) FROM MUHASEBE_HESAP_PLANI WHERE muh_iptal = 0 AND muh_DBCno = 0";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        #endregion

        #region Dönem Veri Kontrolü (Pre-check)

        /// <summary>
        /// Belirtilen dönem ve mali yıl için mevcut fiş özetlerini döner.
        /// İşlem öncesi veri kontrolü (pre-check) için kullanılır.
        /// </summary>
        public List<DonemFisOzeti> DonemVerileriGetir(int maliYil, DateTime baslangic, DateTime bitis, int firmaNo, int subeNo)
        {
            var sonuc = new List<DonemFisOzeti>();

            const string sql = @"SELECT 
                fis_yevmiye_no,
                fis_tarih,
                COUNT(*) AS SatirSayisi,
                SUM(CASE WHEN fis_meblag0 > 0 THEN fis_meblag0 ELSE 0 END) AS ToplamBorc,
                SUM(CASE WHEN fis_meblag0 < 0 THEN ABS(fis_meblag0) ELSE 0 END) AS ToplamAlacak,
                MIN(fis_aciklama1) AS Aciklama
            FROM MUHASEBE_FISLERI 
            WHERE fis_maliyil = @maliYil 
                AND fis_tarih >= @baslangic 
                AND fis_tarih <= @bitis
                AND fis_firmano = @firmaNo
                AND fis_subeno = @subeNo
                AND fis_tur = 0
                AND fis_iptal = 0
                AND fis_DBCno = 0
            GROUP BY fis_yevmiye_no, fis_tarih
            ORDER BY fis_yevmiye_no";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@maliYil", SqlDbType.Int).Value = maliYil;
                cmd.Parameters.Add("@baslangic", SqlDbType.DateTime).Value = baslangic.Date;
                cmd.Parameters.Add("@bitis", SqlDbType.DateTime).Value = bitis.Date;
                cmd.Parameters.Add("@firmaNo", SqlDbType.Int).Value = firmaNo;
                cmd.Parameters.Add("@subeNo", SqlDbType.Int).Value = subeNo;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sonuc.Add(new DonemFisOzeti
                        {
                            YevmiyeNo = reader.GetInt32(0),
                            Tarih = reader.GetDateTime(1),
                            SatirSayisi = reader.GetInt32(2),
                            ToplamBorc = (decimal)reader.GetDouble(3),
                            ToplamAlacak = (decimal)reader.GetDouble(4),
                            Aciklama = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                        });
                    }
                }
            }

            return sonuc;
        }

        /// <summary>
        /// Belirtilen dönemdeki tüm fiş satırlarını siler.
        /// KRİTİK: Transaction içinde çalışır, hata durumunda rollback yapılır.
        /// </summary>
        /// <returns>Silinen satır sayısı</returns>
        public int DonemVerileriSil(int maliYil, DateTime baslangic, DateTime bitis, int firmaNo, int subeNo)
        {
            const string sql = @"DELETE FROM MUHASEBE_FISLERI 
            WHERE fis_maliyil = @maliYil 
                AND fis_tarih >= @baslangic 
                AND fis_tarih <= @bitis
                AND fis_firmano = @firmaNo
                AND fis_subeno = @subeNo
                AND fis_tur = 0
                AND fis_iptal = 0
                AND fis_DBCno = 0";

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new SqlCommand(sql, conn, tran))
                        {
                            cmd.Parameters.Add("@maliYil", SqlDbType.Int).Value = maliYil;
                            cmd.Parameters.Add("@baslangic", SqlDbType.DateTime).Value = baslangic.Date;
                            cmd.Parameters.Add("@bitis", SqlDbType.DateTime).Value = bitis.Date;
                            cmd.Parameters.Add("@firmaNo", SqlDbType.Int).Value = firmaNo;
                            cmd.Parameters.Add("@subeNo", SqlDbType.Int).Value = subeNo;

                            int silinenSatir = cmd.ExecuteNonQuery();
                            tran.Commit();
                            return silinenSatir;
                        }
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Cari Hesap Hareketleri

        /// <summary>
        /// Belirtilen dönemdeki cari hesap hareketlerini evrak seri/sıra bilgileri ile getirir.
        /// Muhasebe fiş referansı senkronizasyonu için kullanılır.
        /// </summary>
        public List<CariHesapHareketi> DonemCariHareketleriGetir(
            DateTime baslangic, DateTime bitis, int firmaNo, int subeNo)
        {
            var sonuc = new List<CariHesapHareketi>();

            const string sql = @"SELECT 
                cha_Guid, cha_evrak_tip, cha_evrakno_seri, cha_evrakno_sira,
                cha_tarihi, cha_kod, cha_muh_fis_no, cha_muh_fis_tarihi
            FROM CARI_HESAP_HAREKETLERI
            WHERE cha_tarihi >= @baslangic 
                AND cha_tarihi <= @bitis
                AND cha_firmano = @firmaNo
                AND cha_subession = @subeNo
                AND cha_iptal = 0
                AND cha_DBCno = 0
            ORDER BY cha_evrakno_seri, cha_evrakno_sira";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@baslangic", SqlDbType.DateTime).Value = baslangic.Date;
                cmd.Parameters.Add("@bitis", SqlDbType.DateTime).Value = bitis.Date;
                cmd.Parameters.Add("@firmaNo", SqlDbType.Int).Value = firmaNo;
                cmd.Parameters.Add("@subeNo", SqlDbType.Int).Value = subeNo;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sonuc.Add(new CariHesapHareketi
                        {
                            ChaGuid = reader.GetGuid(0),
                            ChaEvrakTip = reader.GetByte(1),
                            ChaEvraknoSeri = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            ChaEvraknoSira = reader.GetInt32(3),
                            ChaTarihi = reader.GetDateTime(4),
                            ChaKod = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            ChaMuhFisNo = reader.GetInt32(6),
                            ChaMuhFisTarihi = reader.GetDateTime(7)
                        });
                    }
                }
            }

            return sonuc;
        }

        /// <summary>
        /// Cari hesap hareketinin muhasebe fiş referansını günceller.
        /// </summary>
        public void CariHareketMuhFisGuncelle(
            Guid chaGuid, int muhFisNo, DateTime muhFisTarihi,
            SqlConnection conn, SqlTransaction tran)
        {
            const string sql = @"UPDATE CARI_HESAP_HAREKETLERI 
                SET cha_muh_fis_no = @muhFisNo,
                    cha_muh_fis_tarihi = @muhFisTarihi
                WHERE cha_Guid = @chaGuid AND cha_DBCno = 0";

            using (var cmd = new SqlCommand(sql, conn, tran))
            {
                cmd.Parameters.Add("@muhFisNo", SqlDbType.Int).Value = muhFisNo;
                cmd.Parameters.Add("@muhFisTarihi", SqlDbType.DateTime).Value = muhFisTarihi;
                cmd.Parameters.Add("@chaGuid", SqlDbType.UniqueIdentifier).Value = chaGuid;

                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Stok Hareketleri

        /// <summary>
        /// Belirtilen dönemdeki stok hareketlerini evrak seri/sıra bilgileri ile getirir.
        /// Muhasebe fiş referansı senkronizasyonu için kullanılır.
        /// </summary>
        public List<StokHareketi> DonemStokHareketleriGetir(
            DateTime baslangic, DateTime bitis, int firmaNo, int subeNo)
        {
            var sonuc = new List<StokHareketi>();

            const string sql = @"SELECT 
                sth_Guid, sth_evrak_tip, sth_evrakno_seri, sth_evrakno_sira,
                sth_tarihi, sth_muh_fis_no, sth_muh_fis_tarihi
            FROM STOK_HAREKETLERI
            WHERE sth_tarihi >= @baslangic 
                AND sth_tarihi <= @bitis
                AND sth_firmano = @firmaNo
                AND sth_subeno = @subeNo
                AND sth_iptal = 0
                AND sth_DBCno = 0
            ORDER BY sth_evrakno_seri, sth_evrakno_sira";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@baslangic", SqlDbType.DateTime).Value = baslangic.Date;
                cmd.Parameters.Add("@bitis", SqlDbType.DateTime).Value = bitis.Date;
                cmd.Parameters.Add("@firmaNo", SqlDbType.Int).Value = firmaNo;
                cmd.Parameters.Add("@subeNo", SqlDbType.Int).Value = subeNo;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sonuc.Add(new StokHareketi
                        {
                            SthGuid = reader.GetGuid(0),
                            SthEvrakTip = reader.GetByte(1),
                            SthEvraknoSeri = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            SthEvraknoSira = reader.GetInt32(3),
                            SthTarihi = reader.GetDateTime(4),
                            SthMuhFisNo = reader.GetInt32(5),
                            SthMuhFisTarihi = reader.GetDateTime(6)
                        });
                    }
                }
            }

            return sonuc;
        }

        /// <summary>
        /// Stok hareketinin muhasebe fiş referansını günceller.
        /// </summary>
        public void StokHareketMuhFisGuncelle(
            Guid sthGuid, int muhFisNo, DateTime muhFisTarihi,
            SqlConnection conn, SqlTransaction tran)
        {
            const string sql = @"UPDATE STOK_HAREKETLERI 
                SET sth_muh_fis_no = @muhFisNo,
                    sth_muh_fis_tarihi = @muhFisTarihi
                WHERE sth_Guid = @sthGuid AND sth_DBCno = 0";

            using (var cmd = new SqlCommand(sql, conn, tran))
            {
                cmd.Parameters.Add("@muhFisNo", SqlDbType.Int).Value = muhFisNo;
                cmd.Parameters.Add("@muhFisTarihi", SqlDbType.DateTime).Value = muhFisTarihi;
                cmd.Parameters.Add("@sthGuid", SqlDbType.UniqueIdentifier).Value = sthGuid;

                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Güvenli Sorgulama Yardımcıları

        /// <summary>
        /// Cari hareketleri güvenli şekilde getirir. Tablo yoksa (SqlException 208) boş liste döner.
        /// </summary>
        public List<CariHesapHareketi> DonemCariHareketleriGetirGuvenli(
            DateTime baslangic, DateTime bitis, int firmaNo, int subeNo)
        {
            try
            {
                return DonemCariHareketleriGetir(baslangic, bitis, firmaNo, subeNo);
            }
            catch (SqlException ex) when (ex.Number == 208)
            {
                return new List<CariHesapHareketi>();
            }
        }

        /// <summary>
        /// Stok hareketleri güvenli şekilde getirir. Tablo yoksa (SqlException 208) boş liste döner.
        /// </summary>
        public List<StokHareketi> DonemStokHareketleriGetirGuvenli(
            DateTime baslangic, DateTime bitis, int firmaNo, int subeNo)
        {
            try
            {
                return DonemStokHareketleriGetir(baslangic, bitis, firmaNo, subeNo);
            }
            catch (SqlException ex) when (ex.Number == 208)
            {
                return new List<StokHareketi>();
            }
        }

        /// <summary>
        /// Cari hareketlerden evrak anahtarı bazlı index oluşturur (SERİ-SIRA → kayıt listesi).
        /// </summary>
        public static Dictionary<string, List<CariHesapHareketi>> CariIndexOlustur(
            List<CariHesapHareketi> hareketler)
        {
            if (hareketler == null) throw new ArgumentNullException(nameof(hareketler));

            var index = new Dictionary<string, List<CariHesapHareketi>>(StringComparer.OrdinalIgnoreCase);
            foreach (var hareket in hareketler)
            {
                string anahtar = hareket.EvrakAnahtar;
                if (!index.ContainsKey(anahtar))
                    index[anahtar] = new List<CariHesapHareketi>();
                index[anahtar].Add(hareket);
            }
            return index;
        }

        /// <summary>
        /// Stok hareketlerden evrak anahtarı bazlı index oluşturur (SERİ-SIRA → kayıt listesi).
        /// </summary>
        public static Dictionary<string, List<StokHareketi>> StokIndexOlustur(
            List<StokHareketi> hareketler)
        {
            if (hareketler == null) throw new ArgumentNullException(nameof(hareketler));

            var index = new Dictionary<string, List<StokHareketi>>(StringComparer.OrdinalIgnoreCase);
            foreach (var hareket in hareketler)
            {
                string anahtar = hareket.EvrakAnahtar;
                if (!index.ContainsKey(anahtar))
                    index[anahtar] = new List<StokHareketi>();
                index[anahtar].Add(hareket);
            }
            return index;
        }

        #endregion

        #region Veritabanı Yedekleme

        /// <summary>
        /// SQL Server'ın varsayılan yedek dizinini registry'den sorgular.
        /// </summary>
        private string SqlServerVarsayilanYedekDiziniGetir()
        {
            const string sql = @"EXEC master.dbo.xp_instance_regread 
                N'HKEY_LOCAL_MACHINE', 
                N'SOFTWARE\Microsoft\MSSQLServer\MSSQLServer', 
                N'BackupDirectory'";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return reader.GetString(1);
                }
            }

            return null;
        }

        /// <summary>
        /// Veritabanının tam yedeğini (FULL BACKUP) alır.
        /// DB motoru açıkken çalışır (ONLINE backup).
        /// </summary>
        /// <param name="yedekDizini">Yedek dosyasının yazılacağı dizin. null ise SQL Server default backup dizini kullanılır.</param>
        /// <param name="ilerlemeCallback">Yedekleme ilerleme bildirimi (yüzde 0-100, durum mesajı). null olabilir.</param>
        /// <returns>Oluşturulan yedek dosyasının tam yolu.</returns>
        /// <exception cref="InvalidOperationException">Bağlantı dizesi geçersizse veya DB adı alınamıyorsa.</exception>
        /// <exception cref="SqlException">SQL Server yedekleme hatası.</exception>
        public YedeklemeSonucu VeritabaniYedekle(string yedekDizini = null, Action<int, string> ilerlemeCallback = null)
        {
            var builder = new SqlConnectionStringBuilder(_connectionString);
            string dbAdi = builder.InitialCatalog;

            if (string.IsNullOrWhiteSpace(dbAdi))
                throw new InvalidOperationException(
                    "Connection string'den veritabanı adı alınamadı. 'Initial Catalog' veya 'Database' tanımlı olmalı.");

            string zaman = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string dosyaAdi = $"{dbAdi}_{zaman}.bak";

            // Yedek yolu: SQL Server'ın yazabildiği bir dizin bul (fallback zinciri)
            string yedekYolu;
            if (!string.IsNullOrWhiteSpace(yedekDizini))
            {
                YedekDiziniHazirla(yedekDizini);
                yedekYolu = Path.Combine(yedekDizini, dosyaAdi);
            }
            else
            {
                string guvenliDizin = YedekIcinGuvenliDizinBul();
                yedekYolu = Path.Combine(guvenliDizin, dosyaAdi);
            }

            // BACKUP DATABASE komutu — INIT: üzerine yaz, COMPRESSION: sıkıştır, STATS: ilerleme
            // Parameterized SQL backup komutunda parametre kullanılamaz, DB adı ve yol doğrudan yazılmalı.
            // SQL injection riski yok çünkü değerler connection string ve dosya sistemi kaynaklı.
            string backupSql = string.Format(
                "BACKUP DATABASE [{0}] TO DISK = N'{1}' WITH INIT, COMPRESSION, NAME = N'{0} Full Backup {2}', STATS = 5",
                dbAdi.Replace("]", "]]"),
                yedekYolu.Replace("'", "''"),
                zaman);

            var sonuc = new YedeklemeSonucu
            {
                VeritabaniAdi = dbAdi,
                BaslangicZamani = DateTime.Now
            };

            ilerlemeCallback?.Invoke(5, "BACKUP DATABASE baslatiliyor...");

            // Yedekleme uzun sürebilir, timeout'u artır
            using (var conn = new SqlConnection(_connectionString))
            {
                // SqlInfoMessage ile STATS ilerleme bilgisi yakala
                if (ilerlemeCallback != null)
                {
                    conn.InfoMessage += (s, e) =>
                    {
                        // STATS çıktısı: "5 percent processed." formatında gelir
                        string mesaj = e.Message;
                        int percentIdx = mesaj.IndexOf("percent", StringComparison.OrdinalIgnoreCase);
                        if (percentIdx > 0)
                        {
                            string yuzdeStr = mesaj.Substring(0, percentIdx).Trim();
                            if (int.TryParse(yuzdeStr, out int yuzde))
                            {
                                // 5-95 aralığında ölçekle (0-5 bağlantı, 95-100 bitiş için)
                                int olcekliYuzde = 5 + (int)(yuzde * 0.90);
                                ilerlemeCallback(Math.Min(olcekliYuzde, 95), $"Yedekleniyor... %{yuzde}");
                            }
                        }
                    };
                }

                conn.Open();
                using (var cmd = new SqlCommand(backupSql, conn))
                {
                    cmd.CommandTimeout = 600; // 10 dakika
                    cmd.ExecuteNonQuery();
                }
            }

            sonuc.BitisZamani = DateTime.Now;
            sonuc.DosyaYolu = yedekYolu;
            sonuc.Basarili = true;

            // Dosya boyutu — SQL Server kendi dizinine yazdığında uygulama erişemeyebilir
            try
            {
                if (File.Exists(yedekYolu))
                    sonuc.DosyaBoyutu = new FileInfo(yedekYolu).Length;
            }
            catch
            {
                // SQL Server dizinine erişim yoksa boyut bilgisi alınamaz — sorun değil
            }

            ilerlemeCallback?.Invoke(100, "Yedek tamamlandi.");

            return sonuc;
        }

        /// <summary>
        /// Yedek dizinini hazırlar — önce xp_create_subdir, başarısızsa Directory.CreateDirectory dener.
        /// </summary>
        private void YedekDiziniHazirla(string dizinYolu)
        {
            if (string.IsNullOrWhiteSpace(dizinYolu)) return;

            // Dizin zaten varsa dokunma
            if (Directory.Exists(dizinYolu)) return;

            // SQL Server servis hesabıyla oluşturmayı dene
            try
            {
                string sql = string.Format(
                    "EXEC master.dbo.xp_create_subdir N'{0}'",
                    dizinYolu.Replace("'", "''"));

                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return;
            }
            catch
            {
                // xp_create_subdir başarısız — .NET ile dene
            }

            Directory.CreateDirectory(dizinYolu);
        }

        /// <summary>
        /// SQL Server'ın yazabildiği güvenli bir yedek dizini bulur.
        /// Hedef veritabanının .mdf dosyasının bulunduğu klasörü kullanır —
        /// SQL Server bu dizine kesinlikle yazabilir çünkü aktif veri dosyası orada.
        /// </summary>
        private string YedekIcinGuvenliDizinBul()
        {
            // Hedef DB'nin .mdf fiziksel yolunu sorgula
            string mdfDizini = HedefVeritabaniDiziniGetir();
            if (!string.IsNullOrWhiteSpace(mdfDizini))
                return mdfDizini;

            // Son çare: Kullanıcı belgeler dizini (SQL Server yazamayabilir ama denenecek)
            string belgeler = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Defter2Fis", "Yedekler");
            Directory.CreateDirectory(belgeler);
            return belgeler;
        }

        /// <summary>
        /// Hedef veritabanının .mdf dosyasının bulunduğu dizini döner.
        /// Bu dizine SQL Server servis hesabı garantili yazma iznine sahiptir.
        /// </summary>
        private string HedefVeritabaniDiziniGetir()
        {
            const string sql = @"SELECT physical_name FROM sys.database_files WHERE type = 0 AND file_id = 1";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    string mdfYolu = cmd.ExecuteScalar() as string;
                    if (!string.IsNullOrWhiteSpace(mdfYolu))
                        return Path.GetDirectoryName(mdfYolu);
                }
            }
            catch
            {
                // Sorgu başarısızsa null dön — caller fallback'e geçer
            }

            return null;
        }

        #endregion
    }

    /// <summary>
    /// Yedekleme sonuç bilgisi.
    /// </summary>
    public class YedeklemeSonucu
    {
        public string VeritabaniAdi { get; set; }
        public string DosyaYolu { get; set; }
        public long DosyaBoyutu { get; set; }
        public DateTime BaslangicZamani { get; set; }
        public DateTime BitisZamani { get; set; }
        public bool Basarili { get; set; }

        /// <summary>
        /// İnsan okunabilir dosya boyutu.
        /// </summary>
        public string DosyaBoyutuFormatli
        {
            get
            {
                if (DosyaBoyutu <= 0) return "?";
                string[] birimler = { "B", "KB", "MB", "GB" };
                double boyut = DosyaBoyutu;
                int birimIdx = 0;
                while (boyut >= 1024 && birimIdx < birimler.Length - 1)
                {
                    boyut /= 1024;
                    birimIdx++;
                }
                return $"{boyut:N1} {birimler[birimIdx]}";
            }
        }

        /// <summary>
        /// Yedekleme süresi.
        /// </summary>
        public TimeSpan Sure => BitisZamani - BaslangicZamani;
    }
}
