namespace Defter2Fis.ForMikro.Forms
{
    partial class ConnectionBuilderForm
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
            _tblMain = new System.Windows.Forms.TableLayoutPanel();
            _grpSunucu = new System.Windows.Forms.GroupBox();
            _tblSunucu = new System.Windows.Forms.TableLayoutPanel();
            _lblSunucu = new System.Windows.Forms.Label();
            _txtSunucu = new System.Windows.Forms.TextBox();
            _lblSunucuIpucu = new System.Windows.Forms.Label();
            _lblVeritabani = new System.Windows.Forms.Label();
            _txtVeritabani = new System.Windows.Forms.TextBox();
            _lblVeritabaniIpucu = new System.Windows.Forms.Label();
            _grpKimlik = new System.Windows.Forms.GroupBox();
            _tblKimlik = new System.Windows.Forms.TableLayoutPanel();
            _rbWindowsAuth = new System.Windows.Forms.RadioButton();
            _rbSqlAuth = new System.Windows.Forms.RadioButton();
            _lblKullaniciAdi = new System.Windows.Forms.Label();
            _txtKullaniciAdi = new System.Windows.Forms.TextBox();
            _lblSifre = new System.Windows.Forms.Label();
            _txtSifre = new System.Windows.Forms.TextBox();
            _flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            _btnIptal = new System.Windows.Forms.Button();
            _btnTamam = new System.Windows.Forms.Button();
            _btnTestEt = new System.Windows.Forms.Button();

            components = new System.ComponentModel.Container();

            _tblMain.SuspendLayout();
            _grpSunucu.SuspendLayout();
            _tblSunucu.SuspendLayout();
            _grpKimlik.SuspendLayout();
            _tblKimlik.SuspendLayout();
            _flpButtons.SuspendLayout();
            SuspendLayout();

            // _tblMain
            _tblMain.ColumnCount = 1;
            _tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblMain.Controls.Add(_grpSunucu, 0, 0);
            _tblMain.Controls.Add(_grpKimlik, 0, 1);
            _tblMain.Controls.Add(_flpButtons, 0, 2);
            _tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblMain.Location = new System.Drawing.Point(10, 10);
            _tblMain.Name = "_tblMain";
            _tblMain.RowCount = 3;
            _tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblMain.Size = new System.Drawing.Size(440, 340);

            // _grpSunucu
            _grpSunucu.AutoSize = true;
            _grpSunucu.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            _grpSunucu.Controls.Add(_tblSunucu);
            _grpSunucu.Dock = System.Windows.Forms.DockStyle.Fill;
            _grpSunucu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _grpSunucu.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            _grpSunucu.Name = "_grpSunucu";
            _grpSunucu.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            _grpSunucu.Text = "SQL Server Ba\u011flant\u0131 Bilgileri";

            // _tblSunucu
            _tblSunucu.AutoSize = true;
            _tblSunucu.ColumnCount = 2;
            _tblSunucu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblSunucu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblSunucu.Controls.Add(_lblSunucu, 0, 0);
            _tblSunucu.Controls.Add(_txtSunucu, 1, 0);
            _tblSunucu.Controls.Add(_lblSunucuIpucu, 1, 1);
            _tblSunucu.Controls.Add(_lblVeritabani, 0, 2);
            _tblSunucu.Controls.Add(_txtVeritabani, 1, 2);
            _tblSunucu.Controls.Add(_lblVeritabaniIpucu, 1, 3);
            _tblSunucu.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblSunucu.Name = "_tblSunucu";
            _tblSunucu.RowCount = 4;
            _tblSunucu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblSunucu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblSunucu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblSunucu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // Sunucu
            _lblSunucu.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblSunucu.AutoSize = true;
            _lblSunucu.Name = "_lblSunucu";
            _lblSunucu.Text = "Sunucu:";

            _txtSunucu.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtSunucu.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtSunucu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtSunucu.Margin = new System.Windows.Forms.Padding(3);
            _txtSunucu.Name = "_txtSunucu";

            _lblSunucuIpucu.AutoSize = true;
            _lblSunucuIpucu.ForeColor = System.Drawing.Color.FromArgb(140, 140, 140);
            _lblSunucuIpucu.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            _lblSunucuIpucu.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            _lblSunucuIpucu.Name = "_lblSunucuIpucu";
            _lblSunucuIpucu.Text = "\u00d6rnek: BILGISAYAR-ADI\\SQLEXPRESS  veya  192.168.1.100\\MSSQLSERVER,1433";

            // Veritabani
            _lblVeritabani.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblVeritabani.AutoSize = true;
            _lblVeritabani.Name = "_lblVeritabani";
            _lblVeritabani.Text = "Veritaban\u0131:";

            _txtVeritabani.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtVeritabani.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtVeritabani.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtVeritabani.Margin = new System.Windows.Forms.Padding(3);
            _txtVeritabani.Name = "_txtVeritabani";

            _lblVeritabaniIpucu.AutoSize = true;
            _lblVeritabaniIpucu.ForeColor = System.Drawing.Color.FromArgb(140, 140, 140);
            _lblVeritabaniIpucu.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            _lblVeritabaniIpucu.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            _lblVeritabaniIpucu.Name = "_lblVeritabaniIpucu";
            _lblVeritabaniIpucu.Text = "\u00d6rnek: MikroDB_V16_FIRMAADI2025";

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
            _tblKimlik.Controls.Add(_rbWindowsAuth, 0, 0);
            _tblKimlik.Controls.Add(_rbSqlAuth, 0, 1);
            _tblKimlik.Controls.Add(_lblKullaniciAdi, 0, 2);
            _tblKimlik.Controls.Add(_txtKullaniciAdi, 1, 2);
            _tblKimlik.Controls.Add(_lblSifre, 0, 3);
            _tblKimlik.Controls.Add(_txtSifre, 1, 3);
            _tblKimlik.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblKimlik.Name = "_tblKimlik";
            _tblKimlik.RowCount = 4;
            _tblKimlik.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblKimlik.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblKimlik.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblKimlik.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // RadioButtons
            _tblKimlik.SetColumnSpan(_rbWindowsAuth, 2);
            _rbWindowsAuth.AutoSize = true;
            _rbWindowsAuth.Checked = true;
            _rbWindowsAuth.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _rbWindowsAuth.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            _rbWindowsAuth.Name = "_rbWindowsAuth";
            _rbWindowsAuth.TabStop = true;
            _rbWindowsAuth.Text = "Windows Kimlik Do\u011frulama (Integrated Security)";
            _rbWindowsAuth.CheckedChanged += RbWindowsAuth_CheckedChanged;

            _tblKimlik.SetColumnSpan(_rbSqlAuth, 2);
            _rbSqlAuth.AutoSize = true;
            _rbSqlAuth.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _rbSqlAuth.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            _rbSqlAuth.Name = "_rbSqlAuth";
            _rbSqlAuth.Text = "SQL Server Kimlik Do\u011frulama";

            // Kullanici Adi
            _lblKullaniciAdi.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblKullaniciAdi.AutoSize = true;
            _lblKullaniciAdi.Enabled = false;
            _lblKullaniciAdi.Name = "_lblKullaniciAdi";
            _lblKullaniciAdi.Text = "Kullan\u0131c\u0131:";

            _txtKullaniciAdi.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtKullaniciAdi.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtKullaniciAdi.Enabled = false;
            _txtKullaniciAdi.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtKullaniciAdi.Margin = new System.Windows.Forms.Padding(3);
            _txtKullaniciAdi.Name = "_txtKullaniciAdi";

            // Sifre
            _lblSifre.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblSifre.AutoSize = true;
            _lblSifre.Enabled = false;
            _lblSifre.Name = "_lblSifre";
            _lblSifre.Text = "\u015Eifre:";

            _txtSifre.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtSifre.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtSifre.Enabled = false;
            _txtSifre.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtSifre.Margin = new System.Windows.Forms.Padding(3);
            _txtSifre.Name = "_txtSifre";
            _txtSifre.UseSystemPasswordChar = true;

            // _flpButtons
            _flpButtons.AutoSize = true;
            _flpButtons.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            _flpButtons.Controls.Add(_btnIptal);
            _flpButtons.Controls.Add(_btnTamam);
            _flpButtons.Controls.Add(_btnTestEt);
            _flpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            _flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            _flpButtons.Name = "_flpButtons";
            _flpButtons.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);

            // _btnIptal
            _btnIptal.BackColor = System.Drawing.Color.FromArgb(62, 62, 64);
            _btnIptal.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            _btnIptal.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(63, 63, 70);
            _btnIptal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnIptal.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _btnIptal.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            _btnIptal.Name = "_btnIptal";
            _btnIptal.Size = new System.Drawing.Size(85, 30);
            _btnIptal.Text = "\u0130ptal";
            _btnIptal.Click += BtnIptal_Click;

            // _btnTamam
            _btnTamam.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            _btnTamam.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 100, 180);
            _btnTamam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnTamam.ForeColor = System.Drawing.Color.White;
            _btnTamam.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            _btnTamam.Name = "_btnTamam";
            _btnTamam.Size = new System.Drawing.Size(85, 30);
            _btnTamam.Text = "Tamam";
            _btnTamam.Click += BtnTamam_Click;

            // _btnTestEt
            _btnTestEt.BackColor = System.Drawing.Color.FromArgb(62, 62, 64);
            _btnTestEt.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(63, 63, 70);
            _btnTestEt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnTestEt.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _btnTestEt.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            _btnTestEt.Name = "_btnTestEt";
            _btnTestEt.Size = new System.Drawing.Size(110, 30);
            _btnTestEt.Text = "\u26A1 Ba\u011flant\u0131y\u0131 Test Et";
            _btnTestEt.Click += BtnTestEt_Click;

            // ConnectionBuilderForm
            AcceptButton = _btnTamam;
            CancelButton = _btnIptal;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            ClientSize = new System.Drawing.Size(460, 360);
            Controls.Add(_tblMain);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConnectionBuilderForm";
            Padding = new System.Windows.Forms.Padding(10);
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "SQL Server Ba\u011flant\u0131 Olu\u015fturucu";

            _tblMain.ResumeLayout(false);
            _tblMain.PerformLayout();
            _grpSunucu.ResumeLayout(false);
            _grpSunucu.PerformLayout();
            _tblSunucu.ResumeLayout(false);
            _tblSunucu.PerformLayout();
            _grpKimlik.ResumeLayout(false);
            _grpKimlik.PerformLayout();
            _tblKimlik.ResumeLayout(false);
            _tblKimlik.PerformLayout();
            _flpButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tblMain;
        private System.Windows.Forms.GroupBox _grpSunucu;
        private System.Windows.Forms.TableLayoutPanel _tblSunucu;
        private System.Windows.Forms.Label _lblSunucu;
        private System.Windows.Forms.TextBox _txtSunucu;
        private System.Windows.Forms.Label _lblSunucuIpucu;
        private System.Windows.Forms.Label _lblVeritabani;
        private System.Windows.Forms.TextBox _txtVeritabani;
        private System.Windows.Forms.Label _lblVeritabaniIpucu;
        private System.Windows.Forms.GroupBox _grpKimlik;
        private System.Windows.Forms.TableLayoutPanel _tblKimlik;
        private System.Windows.Forms.RadioButton _rbWindowsAuth;
        private System.Windows.Forms.RadioButton _rbSqlAuth;
        private System.Windows.Forms.Label _lblKullaniciAdi;
        private System.Windows.Forms.TextBox _txtKullaniciAdi;
        private System.Windows.Forms.Label _lblSifre;
        private System.Windows.Forms.TextBox _txtSifre;
        private System.Windows.Forms.FlowLayoutPanel _flpButtons;
        private System.Windows.Forms.Button _btnTestEt;
        private System.Windows.Forms.Button _btnTamam;
        private System.Windows.Forms.Button _btnIptal;
    }
}
