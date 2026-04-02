using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Defter2Fis.ForMikro.Forms
{
    /// <summary>
    /// SQL Server bağlantı dizesi oluşturucu formu.
    /// Kullanıcıdan sunucu, veritabanı ve kimlik doğrulama bilgilerini alarak
    /// connection string oluşturur.
    /// </summary>
    public partial class ConnectionBuilderForm : Form
    {
        /// <summary>
        /// Oluşturulan connection string.
        /// </summary>
        public string ConnectionString { get; private set; }

        public ConnectionBuilderForm(string mevcutConnectionString)
        {
            InitializeComponent();
            MevcutBaglantiCoz(mevcutConnectionString);
        }

        private void MevcutBaglantiCoz(string connStr)
        {
            if (string.IsNullOrWhiteSpace(connStr))
            {
                return;
            }

            try
            {
                var builder = new SqlConnectionStringBuilder(connStr);
                _txtSunucu.Text = builder.DataSource;
                _txtVeritabani.Text = builder.InitialCatalog;

                if (builder.IntegratedSecurity)
                {
                    _rbWindowsAuth.Checked = true;
                }
                else
                {
                    _rbSqlAuth.Checked = true;
                    _txtKullaniciAdi.Text = builder.UserID;
                    _txtSifre.Text = builder.Password;
                }
            }
            catch
            {
                // Parse edilemeyen connection string — alanları boş bırak
            }
        }

        private void RbWindowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            bool sqlAuth = _rbSqlAuth.Checked;
            _txtKullaniciAdi.Enabled = sqlAuth;
            _txtSifre.Enabled = sqlAuth;
            _lblKullaniciAdi.Enabled = sqlAuth;
            _lblSifre.Enabled = sqlAuth;
        }

        private void BtnTestEt_Click(object sender, EventArgs e)
        {
            string connStr = BaglantiDizesiOlustur();
            if (connStr == null)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                }
                MessageBox.Show(
                    "✅ Bağlantı başarılı!",
                    "Test Sonucu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Bağlantı başarısız:\n\n{ex.Message}\n\nLütfen bilgileri kontrol edin.",
                    "Test Sonucu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BtnTamam_Click(object sender, EventArgs e)
        {
            string connStr = BaglantiDizesiOlustur();
            if (connStr == null)
            {
                return;
            }

            ConnectionString = connStr;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnIptal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private string BaglantiDizesiOlustur()
        {
            string sunucu = _txtSunucu.Text.Trim();
            string veritabani = _txtVeritabani.Text.Trim();

            if (string.IsNullOrWhiteSpace(sunucu))
            {
                MessageBox.Show(
                    "Sunucu adı boş olamaz.\n\nÖrnek: BILGISAYAR-ADI\\SQLEXPRESS",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                _txtSunucu.Focus();
                return null;
            }

            if (string.IsNullOrWhiteSpace(veritabani))
            {
                MessageBox.Show(
                    "Veritabanı adı boş olamaz.\n\nÖrnek: MikroDB_V16_FIRMAADI",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                _txtVeritabani.Focus();
                return null;
            }

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = sunucu,
                InitialCatalog = veritabani,
                ConnectTimeout = 30
            };

            if (_rbWindowsAuth.Checked)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                string kullanici = _txtKullaniciAdi.Text.Trim();
                if (string.IsNullOrWhiteSpace(kullanici))
                {
                    MessageBox.Show(
                        "SQL Kimlik Doğrulama seçildi ancak kullanıcı adı boş.",
                        "Eksik Bilgi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    _txtKullaniciAdi.Focus();
                    return null;
                }

                builder.IntegratedSecurity = false;
                builder.UserID = kullanici;
                builder.Password = _txtSifre.Text;
            }

            return builder.ConnectionString;
        }
    }
}
