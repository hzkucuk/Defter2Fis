using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Defter2Fis.ForMikro.Forms
{
    /// <summary>
    /// SQL Server bağlantı dizesi oluşturma formu.
    /// Sunucu adı, kimlik doğrulama ve veritabanı seçimi sağlar.
    /// </summary>
    public partial class SqlBaglantiForm : Form
    {
        /// <summary>
        /// Oluşturulan bağlantı dizesini döndürür.
        /// </summary>
        public string ConnectionString { get; private set; } = string.Empty;

        public SqlBaglantiForm()
        {
            InitializeComponent();
            _chkWindowsAuth.Checked = true;
            KimlikDogrulamaGuncelle();
        }

        /// <summary>
        /// Mevcut bağlantı dizesini parse ederek form alanlarını doldurur.
        /// </summary>
        public void MevcutBaglantiDizesiniYukle(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                return;

            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                _txtSunucu.Text = builder.DataSource;
                _chkWindowsAuth.Checked = builder.IntegratedSecurity;

                if (!builder.IntegratedSecurity)
                {
                    _txtKullaniciAdi.Text = builder.UserID;
                    _txtSifre.Text = builder.Password;
                }

                if (!string.IsNullOrWhiteSpace(builder.InitialCatalog))
                {
                    _cmbVeritabani.Items.Add(builder.InitialCatalog);
                    _cmbVeritabani.SelectedItem = builder.InitialCatalog;
                }

                KimlikDogrulamaGuncelle();
            }
            catch
            {
                // Geçersiz connection string — sessizce atla
            }
        }

        private void ChkWindowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            KimlikDogrulamaGuncelle();
        }

        private void KimlikDogrulamaGuncelle()
        {
            bool sqlAuth = !_chkWindowsAuth.Checked;
            _lblKullaniciAdi.Enabled = sqlAuth;
            _txtKullaniciAdi.Enabled = sqlAuth;
            _lblSifre.Enabled = sqlAuth;
            _txtSifre.Enabled = sqlAuth;

            if (!sqlAuth)
            {
                _txtKullaniciAdi.Text = string.Empty;
                _txtSifre.Text = string.Empty;
            }
        }

        private void BtnVeritabaniListele_Click(object sender, EventArgs e)
        {
            string sunucu = _txtSunucu.Text.Trim();
            if (string.IsNullOrWhiteSpace(sunucu))
            {
                MessageBox.Show(
                    "Sunucu adı boş olamaz.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Cursor = Cursors.WaitCursor;
            _cmbVeritabani.Items.Clear();

            try
            {
                string tempConnStr = BaglantıDizesiOlustur(masterDb: true);

                using (var conn = new SqlConnection(tempConnStr))
                {
                    conn.Open();

                    using (var cmd = new SqlCommand(
                        "SELECT name FROM sys.databases WHERE state = 0 ORDER BY name", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _cmbVeritabani.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }

                if (_cmbVeritabani.Items.Count > 0)
                {
                    _cmbVeritabani.DroppedDown = true;
                }
                else
                {
                    MessageBox.Show(
                        "Sunucuda veritabanı bulunamadı.",
                        "Bilgi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Veritabanı listesi alınamadı:\n\n{ex.Message}",
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BtnBaglantiTest_Click(object sender, EventArgs e)
        {
            if (!GirdiDogrula())
                return;

            Cursor = Cursors.WaitCursor;
            try
            {
                string connStr = BaglantıDizesiOlustur(masterDb: false);
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                }

                MessageBox.Show(
                    "✅ Bağlantı başarılı!",
                    "Bağlantı Testi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Bağlantı başarısız:\n\n{ex.Message}",
                    "Bağlantı Testi",
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
            if (!GirdiDogrula())
                return;

            ConnectionString = BaglantıDizesiOlustur(masterDb: false);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnIptal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool GirdiDogrula()
        {
            if (string.IsNullOrWhiteSpace(_txtSunucu.Text))
            {
                MessageBox.Show("Sunucu adı girilmelidir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtSunucu.Focus();
                return false;
            }

            if (!_chkWindowsAuth.Checked && string.IsNullOrWhiteSpace(_txtKullaniciAdi.Text))
            {
                MessageBox.Show("SQL kimlik doğrulaması için kullanıcı adı gereklidir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtKullaniciAdi.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(_cmbVeritabani.Text))
            {
                MessageBox.Show("Veritabanı seçilmelidir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _cmbVeritabani.Focus();
                return false;
            }

            return true;
        }

        private string BaglantıDizesiOlustur(bool masterDb)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = _txtSunucu.Text.Trim(),
                InitialCatalog = masterDb ? "master" : _cmbVeritabani.Text.Trim(),
                ConnectTimeout = 10
            };

            if (_chkWindowsAuth.Checked)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.IntegratedSecurity = false;
                builder.UserID = _txtKullaniciAdi.Text.Trim();
                builder.Password = _txtSifre.Text;
            }

            return builder.ConnectionString;
        }
    }
}
