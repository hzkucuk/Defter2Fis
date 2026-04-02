namespace Defter2Fis.ForMikro.Forms
{
    partial class SqlBaglantiForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            _tblAna = new System.Windows.Forms.TableLayoutPanel();
            _grpSunucu = new System.Windows.Forms.GroupBox();
            _tblSunucu = new System.Windows.Forms.TableLayoutPanel();
            _lblSunucu = new System.Windows.Forms.Label();
            _txtSunucu = new System.Windows.Forms.TextBox();
            _grpKimlik = new System.Windows.Forms.GroupBox();
            _tblKimlik = new System.Windows.Forms.TableLayoutPanel();
            _chkWindowsAuth = new System.Windows.Forms.CheckBox();
            _lblKullaniciAdi = new System.Windows.Forms.Label();
            _txtKullaniciAdi = new System.Windows.Forms.TextBox();
            _lblSifre = new System.Windows.Forms.Label();
            _txtSifre = new System.Windows.Forms.TextBox();
            _grpVeritabani = new System.Windows.Forms.GroupBox();
            _tblVeritabani = new System.Windows.Forms.TableLayoutPanel();
            _lblVeritabani = new System.Windows.Forms.Label();
            _cmbVeritabani = new System.Windows.Forms.ComboBox();
            _btnVeritabaniListele = new System.Windows.Forms.Button();
            _btnBaglantiTest = new System.Windows.Forms.Button();
            _flpButonlar = new System.Windows.Forms.FlowLayoutPanel();
            _btnIptal = new System.Windows.Forms.Button();
            _btnTamam = new System.Windows.Forms.Button();

            components = new System.ComponentModel.Container();

            _tblAna.SuspendLayout();
            _grpSunucu.SuspendLayout();
            _tblSunucu.SuspendLayout();
            _grpKimlik.SuspendLayout();
            _tblKimlik.SuspendLayout();
            _grpVeritabani.SuspendLayout();
            _tblVeritabani.SuspendLayout();
            _flpButonlar.SuspendLayout();
            SuspendLayout();

            // _tblAna
            _tblAna.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            _tblAna.ColumnCount = 1;
            _tblAna.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblAna.Controls.Add(_grpSunucu, 0, 0);
            _tblAna.Controls.Add(_grpKimlik, 0, 1);
            _tblAna.Controls.Add(_grpVeritabani, 0, 2);
            _tblAna.Controls.Add(_flpButonlar, 0, 3);
            _tblAna.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblAna.Location = new System.Drawing.Point(8, 8);
            _tblAna.Name = "_tblAna";
            _tblAna.RowCount = 4;
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.Size = new System.Drawing.Size(384, 340);

            // _grpSunucu
            _grpSunucu.AutoSize = true;
            _grpSunucu.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            _grpSunucu.Controls.Add(_tblSunucu);
            _grpSunucu.Dock = System.Windows.Forms.DockStyle.Fill;
            _grpSunucu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _grpSunucu.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            _grpSunucu.Name = "_grpSunucu";
            _grpSunucu.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            _grpSunucu.Text = "Sunucu";

            // _tblSunucu
            _tblSunucu.AutoSize = true;
            _tblSunucu.ColumnCount = 2;
            _tblSunucu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblSunucu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblSunucu.Controls.Add(_lblSunucu, 0, 0);
            _tblSunucu.Controls.Add(_txtSunucu, 1, 0);
            _tblSunucu.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblSunucu.Name = "_tblSunucu";
            _tblSunucu.RowCount = 1;
            _tblSunucu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // _lblSunucu
            _lblSunucu.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblSunucu.AutoSize = true;
            _lblSunucu.Name = "_lblSunucu";
            _lblSunucu.Text = "Sunucu Ad\u0131:";

            // _txtSunucu
            _txtSunucu.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtSunucu.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtSunucu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtSunucu.Margin = new System.Windows.Forms.Padding(3);
            _txtSunucu.Name = "_txtSunucu";

            // _grpKimlik
            _grpKimlik.AutoSize = true;
            _grpKimlik.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            _grpKimlik.Controls.Add(_tblKimlik);
            _grpKimlik.Dock = System.Windows.Forms.DockStyle.Fill;
            _grpKimlik.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _grpKimlik.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            _grpKimlik.Name = "_grpKimlik";
            _grpKimlik.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            _grpKimlik.Text = "Kimlik Do\u011frulama";

            // _tblKimlik
            _tblKimlik.AutoSize = true;
            _tblKimlik.ColumnCount = 2;
            _tblKimlik.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblKimlik.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblKimlik.Controls.Add(_chkWindowsAuth, 0, 0);
            _tblKimlik.Controls.Add(_lblKullaniciAdi, 0, 1);
            _tblKimlik.Controls.Add(_txtKullaniciAdi, 1, 1);
            _tblKimlik.Controls.Add(_lblSifre, 0, 2);
            _tblKimlik.Controls.Add(_txtSifre, 1, 2);
            _tblKimlik.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblKimlik.Name = "_tblKimlik";
            _tblKimlik.RowCount = 3;
            _tblKimlik.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblKimlik.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblKimlik.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblKimlik.SetColumnSpan(_chkWindowsAuth, 2);

            // _chkWindowsAuth
            _chkWindowsAuth.AutoSize = true;
            _chkWindowsAuth.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _chkWindowsAuth.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            _chkWindowsAuth.Name = "_chkWindowsAuth";
            _chkWindowsAuth.Text = "Windows Kimlik Do\u011frulamas\u0131 kullan";
            _chkWindowsAuth.CheckedChanged += ChkWindowsAuth_CheckedChanged;

            // _lblKullaniciAdi
            _lblKullaniciAdi.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblKullaniciAdi.AutoSize = true;
            _lblKullaniciAdi.Name = "_lblKullaniciAdi";
            _lblKullaniciAdi.Text = "Kullan\u0131c\u0131:";

            // _txtKullaniciAdi
            _txtKullaniciAdi.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtKullaniciAdi.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtKullaniciAdi.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtKullaniciAdi.Margin = new System.Windows.Forms.Padding(3);
            _txtKullaniciAdi.Name = "_txtKullaniciAdi";

            // _lblSifre
            _lblSifre.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblSifre.AutoSize = true;
            _lblSifre.Name = "_lblSifre";
            _lblSifre.Text = "\u015Eifre:";

            // _txtSifre
            _txtSifre.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtSifre.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtSifre.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtSifre.Margin = new System.Windows.Forms.Padding(3);
            _txtSifre.Name = "_txtSifre";
            _txtSifre.UseSystemPasswordChar = true;

            // _grpVeritabani
            _grpVeritabani.AutoSize = true;
            _grpVeritabani.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            _grpVeritabani.Controls.Add(_tblVeritabani);
            _grpVeritabani.Dock = System.Windows.Forms.DockStyle.Fill;
            _grpVeritabani.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _grpVeritabani.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            _grpVeritabani.Name = "_grpVeritabani";
            _grpVeritabani.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            _grpVeritabani.Text = "Veritaban\u0131";

            // _tblVeritabani
            _tblVeritabani.AutoSize = true;
            _tblVeritabani.ColumnCount = 3;
            _tblVeritabani.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblVeritabani.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblVeritabani.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblVeritabani.Controls.Add(_lblVeritabani, 0, 0);
            _tblVeritabani.Controls.Add(_cmbVeritabani, 1, 0);
            _tblVeritabani.Controls.Add(_btnVeritabaniListele, 2, 0);
            _tblVeritabani.Controls.Add(_btnBaglantiTest, 1, 1);
            _tblVeritabani.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblVeritabani.Name = "_tblVeritabani";
            _tblVeritabani.RowCount = 2;
            _tblVeritabani.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblVeritabani.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // _lblVeritabani
            _lblVeritabani.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblVeritabani.AutoSize = true;
            _lblVeritabani.Name = "_lblVeritabani";
            _lblVeritabani.Text = "Veritaban\u0131:";

            // _cmbVeritabani
            _cmbVeritabani.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _cmbVeritabani.Dock = System.Windows.Forms.DockStyle.Fill;
            _cmbVeritabani.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _cmbVeritabani.FormattingEnabled = true;
            _cmbVeritabani.Margin = new System.Windows.Forms.Padding(3);
            _cmbVeritabani.Name = "_cmbVeritabani";

            // _btnVeritabaniListele
            _btnVeritabaniListele.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            _btnVeritabaniListele.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 100, 180);
            _btnVeritabaniListele.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnVeritabaniListele.ForeColor = System.Drawing.Color.White;
            _btnVeritabaniListele.Name = "_btnVeritabaniListele";
            _btnVeritabaniListele.Size = new System.Drawing.Size(85, 25);
            _btnVeritabaniListele.Text = "Listele";
            _btnVeritabaniListele.Click += BtnVeritabaniListele_Click;

            // _btnBaglantiTest
            _btnBaglantiTest.Anchor = System.Windows.Forms.AnchorStyles.Left;
            _btnBaglantiTest.BackColor = System.Drawing.Color.FromArgb(62, 62, 64);
            _btnBaglantiTest.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(63, 63, 70);
            _btnBaglantiTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnBaglantiTest.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _btnBaglantiTest.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            _btnBaglantiTest.Name = "_btnBaglantiTest";
            _btnBaglantiTest.Size = new System.Drawing.Size(130, 28);
            _btnBaglantiTest.Text = "\u26A1 Ba\u011flant\u0131y\u0131 Test Et";
            _btnBaglantiTest.Click += BtnBaglantiTest_Click;

            // _flpButonlar
            _flpButonlar.AutoSize = true;
            _flpButonlar.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            _flpButonlar.Dock = System.Windows.Forms.DockStyle.Fill;
            _flpButonlar.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            _flpButonlar.Controls.Add(_btnIptal);
            _flpButonlar.Controls.Add(_btnTamam);
            _flpButonlar.Name = "_flpButonlar";
            _flpButonlar.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);

            // _btnIptal
            _btnIptal.BackColor = System.Drawing.Color.FromArgb(62, 62, 64);
            _btnIptal.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            _btnIptal.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(63, 63, 70);
            _btnIptal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnIptal.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _btnIptal.Name = "_btnIptal";
            _btnIptal.Size = new System.Drawing.Size(85, 30);
            _btnIptal.Text = "\u0130ptal";
            _btnIptal.Click += BtnIptal_Click;

            // _btnTamam
            _btnTamam.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            _btnTamam.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 100, 180);
            _btnTamam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnTamam.ForeColor = System.Drawing.Color.White;
            _btnTamam.Name = "_btnTamam";
            _btnTamam.Size = new System.Drawing.Size(85, 30);
            _btnTamam.Text = "Tamam";
            _btnTamam.Click += BtnTamam_Click;

            // SqlBaglantiForm
            AcceptButton = _btnTamam;
            CancelButton = _btnIptal;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            ClientSize = new System.Drawing.Size(400, 360);
            Controls.Add(_tblAna);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SqlBaglantiForm";
            Padding = new System.Windows.Forms.Padding(8);
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "SQL Server Ba\u011flant\u0131s\u0131";

            _tblAna.ResumeLayout(false);
            _tblAna.PerformLayout();
            _grpSunucu.ResumeLayout(false);
            _grpSunucu.PerformLayout();
            _tblSunucu.ResumeLayout(false);
            _tblSunucu.PerformLayout();
            _grpKimlik.ResumeLayout(false);
            _grpKimlik.PerformLayout();
            _tblKimlik.ResumeLayout(false);
            _tblKimlik.PerformLayout();
            _grpVeritabani.ResumeLayout(false);
            _grpVeritabani.PerformLayout();
            _tblVeritabani.ResumeLayout(false);
            _tblVeritabani.PerformLayout();
            _flpButonlar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tblAna;
        private System.Windows.Forms.GroupBox _grpSunucu;
        private System.Windows.Forms.TableLayoutPanel _tblSunucu;
        private System.Windows.Forms.Label _lblSunucu;
        private System.Windows.Forms.TextBox _txtSunucu;
        private System.Windows.Forms.GroupBox _grpKimlik;
        private System.Windows.Forms.TableLayoutPanel _tblKimlik;
        private System.Windows.Forms.CheckBox _chkWindowsAuth;
        private System.Windows.Forms.Label _lblKullaniciAdi;
        private System.Windows.Forms.TextBox _txtKullaniciAdi;
        private System.Windows.Forms.Label _lblSifre;
        private System.Windows.Forms.TextBox _txtSifre;
        private System.Windows.Forms.GroupBox _grpVeritabani;
        private System.Windows.Forms.TableLayoutPanel _tblVeritabani;
        private System.Windows.Forms.Label _lblVeritabani;
        private System.Windows.Forms.ComboBox _cmbVeritabani;
        private System.Windows.Forms.Button _btnVeritabaniListele;
        private System.Windows.Forms.Button _btnBaglantiTest;
        private System.Windows.Forms.FlowLayoutPanel _flpButonlar;
        private System.Windows.Forms.Button _btnTamam;
        private System.Windows.Forms.Button _btnIptal;
    }
}
