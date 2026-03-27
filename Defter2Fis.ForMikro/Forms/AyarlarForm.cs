using System;
using System.Configuration;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace Defter2Fis.ForMikro.Forms
{
    /// <summary>
    /// Uygulama ayarları düzenleme formu.
    /// App.config appSettings ve connectionStrings değerlerini düzenler.
    /// </summary>
    public partial class AyarlarForm : KryptonForm
    {
        public AyarlarForm()
        {
            InitializeComponent();
            AyarlariYukle();
        }

        private void AyarlariYukle()
        {
            _txtConnectionString.Text = ConfigurationManager.ConnectionStrings["MikroDB"]?.ConnectionString ?? string.Empty;
            _txtEdDefterKok.Text = ConfigurationManager.AppSettings["EdDefterRootPath"] ?? string.Empty;
            _txtSicilNo.Text = ConfigurationManager.AppSettings["SicilNo"] ?? string.Empty;
            _txtMaliYil.Text = ConfigurationManager.AppSettings["MaliYilAraligi"] ?? string.Empty;
            _txtAyKlasoru.Text = ConfigurationManager.AppSettings["AyKlasoru"] ?? string.Empty;
            _txtFirmaNo.Text = ConfigurationManager.AppSettings["FirmaNo"] ?? "0";
            _txtSubeNo.Text = ConfigurationManager.AppSettings["SubeNo"] ?? "0";
            _txtDBCNo.Text = ConfigurationManager.AppSettings["DBCNo"] ?? "0";
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.ConnectionStrings.ConnectionStrings["MikroDB"].ConnectionString = _txtConnectionString.Text.Trim();

                config.AppSettings.Settings["EdDefterRootPath"].Value = _txtEdDefterKok.Text.Trim();
                config.AppSettings.Settings["SicilNo"].Value = _txtSicilNo.Text.Trim();
                config.AppSettings.Settings["MaliYilAraligi"].Value = _txtMaliYil.Text.Trim();
                config.AppSettings.Settings["AyKlasoru"].Value = _txtAyKlasoru.Text.Trim();
                config.AppSettings.Settings["FirmaNo"].Value = _txtFirmaNo.Text.Trim();
                config.AppSettings.Settings["SubeNo"].Value = _txtSubeNo.Text.Trim();
                config.AppSettings.Settings["DBCNo"].Value = _txtDBCNo.Text.Trim();

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                ConfigurationManager.RefreshSection("connectionStrings");

                MessageBox.Show(
                    "Ayarlar kaydedildi. Değişikliklerin etkili olması için uygulamayı yeniden başlatın.",
                    "Ayarlar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ayarlar kaydedilemedi: {ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnIptal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnGozat_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "E-Defter kök dizinini seçin";
                dlg.SelectedPath = _txtEdDefterKok.Text;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _txtEdDefterKok.Text = dlg.SelectedPath;
                }
            }
        }
    }
}

