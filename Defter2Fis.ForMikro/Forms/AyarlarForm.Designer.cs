namespace Defter2Fis.ForMikro.Forms
{
    partial class AyarlarForm
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
            _grpVeritabani = new System.Windows.Forms.GroupBox();
            _tblDb = new System.Windows.Forms.TableLayoutPanel();
            _lblConnectionString = new System.Windows.Forms.Label();
            _txtConnectionString = new System.Windows.Forms.TextBox();
            _btnBaglantiOlustur = new System.Windows.Forms.Button();
            _btnBaglantiTest = new System.Windows.Forms.Button();
            _grpEdDefter = new System.Windows.Forms.GroupBox();
            _tblEdDefter = new System.Windows.Forms.TableLayoutPanel();
            _lblEdDefterKok = new System.Windows.Forms.Label();
            _txtEdDefterKok = new System.Windows.Forms.TextBox();
            _btnGozat = new System.Windows.Forms.Button();
            _lblSicilNo = new System.Windows.Forms.Label();
            _txtSicilNo = new System.Windows.Forms.TextBox();
            _lblMaliYil = new System.Windows.Forms.Label();
            _txtMaliYil = new System.Windows.Forms.TextBox();
            _lblAyKlasoru = new System.Windows.Forms.Label();
            _txtAyKlasoru = new System.Windows.Forms.TextBox();
            _grpMikro = new System.Windows.Forms.GroupBox();
            _tblMikro = new System.Windows.Forms.TableLayoutPanel();
            _lblFirmaNo = new System.Windows.Forms.Label();
            _txtFirmaNo = new System.Windows.Forms.TextBox();
            _lblSubeNo = new System.Windows.Forms.Label();
            _txtSubeNo = new System.Windows.Forms.TextBox();
            _lblDBCNo = new System.Windows.Forms.Label();
            _txtDBCNo = new System.Windows.Forms.TextBox();
            _flpButonlar = new System.Windows.Forms.FlowLayoutPanel();
            _btnIptal = new System.Windows.Forms.Button();
            _btnKaydet = new System.Windows.Forms.Button();

            components = new System.ComponentModel.Container();

            _tblAna.SuspendLayout();
            _grpVeritabani.SuspendLayout();
            _tblDb.SuspendLayout();
            _grpEdDefter.SuspendLayout();
            _tblEdDefter.SuspendLayout();
            _grpMikro.SuspendLayout();
            _tblMikro.SuspendLayout();
            _flpButonlar.SuspendLayout();
            SuspendLayout();

            // _tblAna
            _tblAna.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            _tblAna.ColumnCount = 1;
            _tblAna.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblAna.Controls.Add(_grpVeritabani, 0, 0);
            _tblAna.Controls.Add(_grpEdDefter, 0, 1);
            _tblAna.Controls.Add(_grpMikro, 0, 2);
            _tblAna.Controls.Add(_flpButonlar, 0, 3);
            _tblAna.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblAna.Location = new System.Drawing.Point(8, 8);
            _tblAna.Name = "_tblAna";
            _tblAna.RowCount = 4;
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.Size = new System.Drawing.Size(484, 454);

            // _grpVeritabani
            _grpVeritabani.AutoSize = true;
            _grpVeritabani.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            _grpVeritabani.Controls.Add(_tblDb);
            _grpVeritabani.Dock = System.Windows.Forms.DockStyle.Fill;
            _grpVeritabani.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _grpVeritabani.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            _grpVeritabani.Name = "_grpVeritabani";
            _grpVeritabani.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            _grpVeritabani.Size = new System.Drawing.Size(478, 60);
            _grpVeritabani.Text = "Veritaban\u0131";

            // _tblDb
            _tblDb.AutoSize = true;
            _tblDb.ColumnCount = 2;
            _tblDb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblDb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblDb.Controls.Add(_lblConnectionString, 0, 0);
            _tblDb.Controls.Add(_txtConnectionString, 1, 0);
            _tblDb.Controls.Add(_btnBaglantiOlustur, 0, 1);
            _tblDb.Controls.Add(_btnBaglantiTest, 1, 1);
            _tblDb.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblDb.Name = "_tblDb";
            _tblDb.RowCount = 2;
            _tblDb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblDb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // _lblConnectionString
            _lblConnectionString.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblConnectionString.AutoSize = true;
            _lblConnectionString.Name = "_lblConnectionString";
            _lblConnectionString.Text = "Ba\u011flant\u0131:";

            // _txtConnectionString
            _txtConnectionString.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtConnectionString.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtConnectionString.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtConnectionString.Margin = new System.Windows.Forms.Padding(3);
            _txtConnectionString.Name = "_txtConnectionString";

            // _btnBaglantiOlustur
            _btnBaglantiOlustur.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            _btnBaglantiOlustur.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 100, 180);
            _btnBaglantiOlustur.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnBaglantiOlustur.ForeColor = System.Drawing.Color.White;
            _btnBaglantiOlustur.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            _btnBaglantiOlustur.Name = "_btnBaglantiOlustur";
            _btnBaglantiOlustur.Size = new System.Drawing.Size(130, 28);
            _btnBaglantiOlustur.Text = "\u2699 Ba\u011flant\u0131 Olu\u015ftur...";
            _btnBaglantiOlustur.Click += BtnBaglantiOlustur_Click;

            // _btnBaglantiTest
            _btnBaglantiTest.Anchor = System.Windows.Forms.AnchorStyles.Left;
            _btnBaglantiTest.BackColor = System.Drawing.Color.FromArgb(62, 62, 64);
            _btnBaglantiTest.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(63, 63, 70);
            _btnBaglantiTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnBaglantiTest.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _btnBaglantiTest.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            _btnBaglantiTest.Name = "_btnBaglantiTest";
            _btnBaglantiTest.Size = new System.Drawing.Size(120, 28);
            _btnBaglantiTest.Text = "\u26A1 Ba\u011flant\u0131y\u0131 Test Et";
            _btnBaglantiTest.Click += BtnBaglantiTest_Click;

            // _grpEdDefter
            _grpEdDefter.AutoSize = true;
            _grpEdDefter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            _grpEdDefter.Controls.Add(_tblEdDefter);
            _grpEdDefter.Dock = System.Windows.Forms.DockStyle.Fill;
            _grpEdDefter.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _grpEdDefter.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            _grpEdDefter.Name = "_grpEdDefter";
            _grpEdDefter.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            _grpEdDefter.Size = new System.Drawing.Size(478, 160);
            _grpEdDefter.Text = "E-Defter";

            // _tblEdDefter
            _tblEdDefter.AutoSize = true;
            _tblEdDefter.ColumnCount = 3;
            _tblEdDefter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblEdDefter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblEdDefter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblEdDefter.Controls.Add(_lblEdDefterKok, 0, 0);
            _tblEdDefter.Controls.Add(_txtEdDefterKok, 1, 0);
            _tblEdDefter.Controls.Add(_btnGozat, 2, 0);
            _tblEdDefter.Controls.Add(_lblSicilNo, 0, 1);
            _tblEdDefter.Controls.Add(_txtSicilNo, 1, 1);
            _tblEdDefter.Controls.Add(_lblMaliYil, 0, 2);
            _tblEdDefter.Controls.Add(_txtMaliYil, 1, 2);
            _tblEdDefter.Controls.Add(_lblAyKlasoru, 0, 3);
            _tblEdDefter.Controls.Add(_txtAyKlasoru, 1, 3);
            _tblEdDefter.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblEdDefter.Name = "_tblEdDefter";
            _tblEdDefter.RowCount = 4;
            _tblEdDefter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblEdDefter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblEdDefter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblEdDefter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // E-Defter labels and textboxes
            _lblEdDefterKok.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblEdDefterKok.AutoSize = true;
            _lblEdDefterKok.Name = "_lblEdDefterKok";
            _lblEdDefterKok.Text = "K\u00f6k Dizin:";

            _txtEdDefterKok.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtEdDefterKok.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtEdDefterKok.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtEdDefterKok.Margin = new System.Windows.Forms.Padding(3);
            _txtEdDefterKok.Name = "_txtEdDefterKok";

            _btnGozat.BackColor = System.Drawing.Color.FromArgb(62, 62, 64);
            _btnGozat.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(63, 63, 70);
            _btnGozat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnGozat.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _btnGozat.Name = "_btnGozat";
            _btnGozat.Size = new System.Drawing.Size(30, 23);
            _btnGozat.Text = "...";
            _btnGozat.Click += BtnGozat_Click;

            _lblSicilNo.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblSicilNo.AutoSize = true;
            _lblSicilNo.Name = "_lblSicilNo";
            _lblSicilNo.Text = "Sicil No:";

            _txtSicilNo.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtSicilNo.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtSicilNo.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtSicilNo.Margin = new System.Windows.Forms.Padding(3);
            _txtSicilNo.Name = "_txtSicilNo";

            _lblMaliYil.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblMaliYil.AutoSize = true;
            _lblMaliYil.Name = "_lblMaliYil";
            _lblMaliYil.Text = "Mali Y\u0131l:";

            _txtMaliYil.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtMaliYil.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtMaliYil.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtMaliYil.Margin = new System.Windows.Forms.Padding(3);
            _txtMaliYil.Name = "_txtMaliYil";

            _lblAyKlasoru.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblAyKlasoru.AutoSize = true;
            _lblAyKlasoru.Name = "_lblAyKlasoru";
            _lblAyKlasoru.Text = "Ay:";

            _txtAyKlasoru.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtAyKlasoru.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtAyKlasoru.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtAyKlasoru.Margin = new System.Windows.Forms.Padding(3);
            _txtAyKlasoru.Name = "_txtAyKlasoru";
            _txtAyKlasoru.Size = new System.Drawing.Size(80, 23);

            // _grpMikro
            _grpMikro.AutoSize = true;
            _grpMikro.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            _grpMikro.Controls.Add(_tblMikro);
            _grpMikro.Dock = System.Windows.Forms.DockStyle.Fill;
            _grpMikro.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _grpMikro.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            _grpMikro.Name = "_grpMikro";
            _grpMikro.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            _grpMikro.Size = new System.Drawing.Size(478, 100);
            _grpMikro.Text = "Mikro ERP Parametreleri";

            // _tblMikro
            _tblMikro.AutoSize = true;
            _tblMikro.ColumnCount = 6;
            _tblMikro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblMikro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            _tblMikro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblMikro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            _tblMikro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblMikro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            _tblMikro.Controls.Add(_lblFirmaNo, 0, 0);
            _tblMikro.Controls.Add(_txtFirmaNo, 1, 0);
            _tblMikro.Controls.Add(_lblSubeNo, 2, 0);
            _tblMikro.Controls.Add(_txtSubeNo, 3, 0);
            _tblMikro.Controls.Add(_lblDBCNo, 4, 0);
            _tblMikro.Controls.Add(_txtDBCNo, 5, 0);
            _tblMikro.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblMikro.Name = "_tblMikro";
            _tblMikro.RowCount = 1;
            _tblMikro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            _lblFirmaNo.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblFirmaNo.AutoSize = true;
            _lblFirmaNo.Name = "_lblFirmaNo";
            _lblFirmaNo.Text = "Firma No:";

            _txtFirmaNo.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtFirmaNo.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtFirmaNo.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtFirmaNo.Margin = new System.Windows.Forms.Padding(3);
            _txtFirmaNo.Name = "_txtFirmaNo";

            _lblSubeNo.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblSubeNo.AutoSize = true;
            _lblSubeNo.Name = "_lblSubeNo";
            _lblSubeNo.Text = "\u015Eube No:";

            _txtSubeNo.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtSubeNo.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtSubeNo.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtSubeNo.Margin = new System.Windows.Forms.Padding(3);
            _txtSubeNo.Name = "_txtSubeNo";

            _lblDBCNo.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblDBCNo.AutoSize = true;
            _lblDBCNo.Name = "_lblDBCNo";
            _lblDBCNo.Text = "DBC No:";

            _txtDBCNo.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _txtDBCNo.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtDBCNo.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _txtDBCNo.Margin = new System.Windows.Forms.Padding(3);
            _txtDBCNo.Name = "_txtDBCNo";

            // _flpButonlar
            _flpButonlar.AutoSize = true;
            _flpButonlar.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            _flpButonlar.Dock = System.Windows.Forms.DockStyle.Fill;
            _flpButonlar.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            _flpButonlar.Controls.Add(_btnIptal);
            _flpButonlar.Controls.Add(_btnKaydet);
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

            // _btnKaydet
            _btnKaydet.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            _btnKaydet.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 100, 180);
            _btnKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnKaydet.ForeColor = System.Drawing.Color.White;
            _btnKaydet.Name = "_btnKaydet";
            _btnKaydet.Size = new System.Drawing.Size(85, 30);
            _btnKaydet.Text = "Kaydet";
            _btnKaydet.Click += BtnKaydet_Click;

            // AyarlarForm
            AcceptButton = _btnKaydet;
            CancelButton = _btnIptal;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            ClientSize = new System.Drawing.Size(500, 470);
            Controls.Add(_tblAna);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AyarlarForm";
            Padding = new System.Windows.Forms.Padding(8);
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Ayarlar — Defter2Fi\u015F";

            _tblAna.ResumeLayout(false);
            _tblAna.PerformLayout();
            _grpVeritabani.ResumeLayout(false);
            _grpVeritabani.PerformLayout();
            _tblDb.ResumeLayout(false);
            _tblDb.PerformLayout();
            _grpEdDefter.ResumeLayout(false);
            _grpEdDefter.PerformLayout();
            _tblEdDefter.ResumeLayout(false);
            _tblEdDefter.PerformLayout();
            _grpMikro.ResumeLayout(false);
            _grpMikro.PerformLayout();
            _tblMikro.ResumeLayout(false);
            _tblMikro.PerformLayout();
            _flpButonlar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tblAna;
        private System.Windows.Forms.GroupBox _grpVeritabani;
        private System.Windows.Forms.TableLayoutPanel _tblDb;
        private System.Windows.Forms.Label _lblConnectionString;
        private System.Windows.Forms.TextBox _txtConnectionString;
        private System.Windows.Forms.Button _btnBaglantiOlustur;
        private System.Windows.Forms.Button _btnBaglantiTest;
        private System.Windows.Forms.GroupBox _grpEdDefter;
        private System.Windows.Forms.TableLayoutPanel _tblEdDefter;
        private System.Windows.Forms.Label _lblEdDefterKok;
        private System.Windows.Forms.TextBox _txtEdDefterKok;
        private System.Windows.Forms.Button _btnGozat;
        private System.Windows.Forms.Label _lblSicilNo;
        private System.Windows.Forms.TextBox _txtSicilNo;
        private System.Windows.Forms.Label _lblMaliYil;
        private System.Windows.Forms.TextBox _txtMaliYil;
        private System.Windows.Forms.Label _lblAyKlasoru;
        private System.Windows.Forms.TextBox _txtAyKlasoru;
        private System.Windows.Forms.GroupBox _grpMikro;
        private System.Windows.Forms.TableLayoutPanel _tblMikro;
        private System.Windows.Forms.Label _lblFirmaNo;
        private System.Windows.Forms.TextBox _txtFirmaNo;
        private System.Windows.Forms.Label _lblSubeNo;
        private System.Windows.Forms.TextBox _txtSubeNo;
        private System.Windows.Forms.Label _lblDBCNo;
        private System.Windows.Forms.TextBox _txtDBCNo;
        private System.Windows.Forms.FlowLayoutPanel _flpButonlar;
        private System.Windows.Forms.Button _btnKaydet;
        private System.Windows.Forms.Button _btnIptal;
    }
}
