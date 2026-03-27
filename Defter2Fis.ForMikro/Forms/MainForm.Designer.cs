namespace Defter2Fis.ForMikro.Forms
{
    partial class MainForm
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
            _menuStrip = new System.Windows.Forms.MenuStrip();
            _tsmDosya = new System.Windows.Forms.ToolStripMenuItem();
            _tsmCikis = new System.Windows.Forms.ToolStripMenuItem();
            _tsmAraclar = new System.Windows.Forms.ToolStripMenuItem();
            _tsmAyarlar = new System.Windows.Forms.ToolStripMenuItem();
            _tsmLoglariTemizle = new System.Windows.Forms.ToolStripMenuItem();
            _tsmYardim = new System.Windows.Forms.ToolStripMenuItem();
            _tsmHakkinda = new System.Windows.Forms.ToolStripMenuItem();
            _tblAna = new System.Windows.Forms.TableLayoutPanel();
            _grpAyarOzet = new System.Windows.Forms.GroupBox();
            _tblAyarOzet = new System.Windows.Forms.TableLayoutPanel();
            _lblEdDefterYolu = new System.Windows.Forms.Label();
            _lblDbBilgi = new System.Windows.Forms.Label();
            _lblFirmaBilgi = new System.Windows.Forms.Label();
            _flpButonlar = new System.Windows.Forms.FlowLayoutPanel();
            _btnAnalizEt = new System.Windows.Forms.Button();
            _btnMevcutVeriKontrol = new System.Windows.Forms.Button();
            _tabControl = new System.Windows.Forms.TabControl();
            _tabLoglar = new System.Windows.Forms.TabPage();
            _rtbLog = new System.Windows.Forms.RichTextBox();
            _tabMevcutVeri = new System.Windows.Forms.TabPage();
            _tblMevcutVeri = new System.Windows.Forms.TableLayoutPanel();
            _dgvMevcutVeri = new System.Windows.Forms.DataGridView();
            _flpMevcutVeriAlt = new System.Windows.Forms.FlowLayoutPanel();
            _lblMevcutVeriOzet = new System.Windows.Forms.Label();
            _btnDonemVerisiSil = new System.Windows.Forms.Button();
            _statusStrip = new System.Windows.Forms.StatusStrip();
            _lblDurum = new System.Windows.Forms.ToolStripStatusLabel();
            _progressBar = new System.Windows.Forms.ToolStripProgressBar();
            _bgwIslem = new System.ComponentModel.BackgroundWorker();

            components = new System.ComponentModel.Container();

            _menuStrip.SuspendLayout();
            _tblAna.SuspendLayout();
            _grpAyarOzet.SuspendLayout();
            _tblAyarOzet.SuspendLayout();
            _flpButonlar.SuspendLayout();
            _tabControl.SuspendLayout();
            _tabLoglar.SuspendLayout();
            _tabMevcutVeri.SuspendLayout();
            _tblMevcutVeri.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvMevcutVeri).BeginInit();
            _flpMevcutVeriAlt.SuspendLayout();
            _statusStrip.SuspendLayout();
            SuspendLayout();

            // _menuStrip
            _menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                _tsmDosya,
                _tsmAraclar,
                _tsmYardim
            });
            _menuStrip.Location = new System.Drawing.Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Size = new System.Drawing.Size(900, 24);

            // _tsmDosya
            _tsmDosya.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _tsmCikis });
            _tsmDosya.Name = "_tsmDosya";
            _tsmDosya.Text = "&Dosya";

            _tsmCikis.Name = "_tsmCikis";
            _tsmCikis.Text = "&\u00C7\u0131k\u0131\u015F";
            _tsmCikis.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            _tsmCikis.Click += TsmCikis_Click;

            // _tsmAraclar
            _tsmAraclar.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _tsmAyarlar, _tsmLoglariTemizle });
            _tsmAraclar.Name = "_tsmAraclar";
            _tsmAraclar.Text = "A&ra\u00e7lar";

            _tsmAyarlar.Name = "_tsmAyarlar";
            _tsmAyarlar.Text = "&Ayarlar...";
            _tsmAyarlar.Click += TsmAyarlar_Click;

            _tsmLoglariTemizle.Name = "_tsmLoglariTemizle";
            _tsmLoglariTemizle.Text = "&Loglar\u0131 Temizle";
            _tsmLoglariTemizle.Click += TsmLoglariTemizle_Click;

            // _tsmYardim
            _tsmYardim.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _tsmHakkinda });
            _tsmYardim.Name = "_tsmYardim";
            _tsmYardim.Text = "&Yard\u0131m";

            _tsmHakkinda.Name = "_tsmHakkinda";
            _tsmHakkinda.Text = "&Hakk\u0131nda...";
            _tsmHakkinda.Click += TsmHakkinda_Click;

            // _tblAna
            _tblAna.ColumnCount = 1;
            _tblAna.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblAna.Controls.Add(_grpAyarOzet, 0, 0);
            _tblAna.Controls.Add(_flpButonlar, 0, 1);
            _tblAna.Controls.Add(_tabControl, 0, 2);
            _tblAna.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblAna.Location = new System.Drawing.Point(0, 24);
            _tblAna.Name = "_tblAna";
            _tblAna.Padding = new System.Windows.Forms.Padding(6, 4, 6, 0);
            _tblAna.RowCount = 3;
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblAna.Size = new System.Drawing.Size(900, 576);

            // _grpAyarOzet
            _grpAyarOzet.AutoSize = true;
            _grpAyarOzet.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            _grpAyarOzet.Controls.Add(_tblAyarOzet);
            _grpAyarOzet.Dock = System.Windows.Forms.DockStyle.Fill;
            _grpAyarOzet.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            _grpAyarOzet.Name = "_grpAyarOzet";
            _grpAyarOzet.Padding = new System.Windows.Forms.Padding(8, 2, 8, 4);
            _grpAyarOzet.Size = new System.Drawing.Size(882, 72);
            _grpAyarOzet.Text = "D\u00f6nem Bilgileri";

            // _tblAyarOzet
            _tblAyarOzet.AutoSize = true;
            _tblAyarOzet.ColumnCount = 1;
            _tblAyarOzet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblAyarOzet.Controls.Add(_lblEdDefterYolu, 0, 0);
            _tblAyarOzet.Controls.Add(_lblDbBilgi, 0, 1);
            _tblAyarOzet.Controls.Add(_lblFirmaBilgi, 0, 2);
            _tblAyarOzet.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblAyarOzet.Name = "_tblAyarOzet";
            _tblAyarOzet.RowCount = 3;
            _tblAyarOzet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAyarOzet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAyarOzet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // Info labels
            _lblEdDefterYolu.AutoSize = true;
            _lblEdDefterYolu.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            _lblEdDefterYolu.Name = "_lblEdDefterYolu";
            _lblEdDefterYolu.Text = "E-Defter: ...";

            _lblDbBilgi.AutoSize = true;
            _lblDbBilgi.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            _lblDbBilgi.Name = "_lblDbBilgi";
            _lblDbBilgi.Text = "DB: ...";

            _lblFirmaBilgi.AutoSize = true;
            _lblFirmaBilgi.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            _lblFirmaBilgi.Name = "_lblFirmaBilgi";
            _lblFirmaBilgi.Text = "Firma: ...";

            // _flpButonlar
            _flpButonlar.AutoSize = true;
            _flpButonlar.Dock = System.Windows.Forms.DockStyle.Fill;
            _flpButonlar.Controls.Add(_btnAnalizEt);
            _flpButonlar.Controls.Add(_btnMevcutVeriKontrol);
            _flpButonlar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 4);
            _flpButonlar.Name = "_flpButonlar";

            // _btnAnalizEt
            _btnAnalizEt.Name = "_btnAnalizEt";
            _btnAnalizEt.Size = new System.Drawing.Size(140, 32);
            _btnAnalizEt.Text = "\u25B6 Analiz Et";
            _btnAnalizEt.UseVisualStyleBackColor = true;
            _btnAnalizEt.Click += BtnAnalizEt_Click;

            // _btnMevcutVeriKontrol
            _btnMevcutVeriKontrol.Name = "_btnMevcutVeriKontrol";
            _btnMevcutVeriKontrol.Size = new System.Drawing.Size(170, 32);
            _btnMevcutVeriKontrol.Text = "Mevcut Veri Kontrol";
            _btnMevcutVeriKontrol.UseVisualStyleBackColor = true;
            _btnMevcutVeriKontrol.Click += BtnMevcutVeriKontrol_Click;

            // _tabControl
            _tabControl.Controls.Add(_tabLoglar);
            _tabControl.Controls.Add(_tabMevcutVeri);
            _tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            _tabControl.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            _tabControl.Name = "_tabControl";
            _tabControl.SelectedIndex = 0;

            // _tabLoglar
            _tabLoglar.Controls.Add(_rtbLog);
            _tabLoglar.Name = "_tabLoglar";
            _tabLoglar.Padding = new System.Windows.Forms.Padding(3);
            _tabLoglar.Text = "\u0130\u015Flem Loglar\u0131";

            // _rtbLog
            _rtbLog.BackColor = System.Drawing.Color.White;
            _rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            _rtbLog.Font = new System.Drawing.Font("Consolas", 9F);
            _rtbLog.Name = "_rtbLog";
            _rtbLog.ReadOnly = true;
            _rtbLog.WordWrap = false;

            // _tabMevcutVeri
            _tabMevcutVeri.Controls.Add(_tblMevcutVeri);
            _tabMevcutVeri.Name = "_tabMevcutVeri";
            _tabMevcutVeri.Padding = new System.Windows.Forms.Padding(3);
            _tabMevcutVeri.Text = "Mevcut D\u00f6nem Verisi";

            // _tblMevcutVeri
            _tblMevcutVeri.ColumnCount = 1;
            _tblMevcutVeri.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblMevcutVeri.Controls.Add(_dgvMevcutVeri, 0, 0);
            _tblMevcutVeri.Controls.Add(_flpMevcutVeriAlt, 0, 1);
            _tblMevcutVeri.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblMevcutVeri.Name = "_tblMevcutVeri";
            _tblMevcutVeri.RowCount = 2;
            _tblMevcutVeri.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblMevcutVeri.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // _dgvMevcutVeri
            _dgvMevcutVeri.AllowUserToAddRows = false;
            _dgvMevcutVeri.AllowUserToDeleteRows = false;
            _dgvMevcutVeri.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            _dgvMevcutVeri.BackgroundColor = System.Drawing.SystemColors.Window;
            _dgvMevcutVeri.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _dgvMevcutVeri.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvMevcutVeri.Dock = System.Windows.Forms.DockStyle.Fill;
            _dgvMevcutVeri.Name = "_dgvMevcutVeri";
            _dgvMevcutVeri.ReadOnly = true;
            _dgvMevcutVeri.RowHeadersVisible = false;
            _dgvMevcutVeri.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // _flpMevcutVeriAlt
            _flpMevcutVeriAlt.AutoSize = true;
            _flpMevcutVeriAlt.Controls.Add(_lblMevcutVeriOzet);
            _flpMevcutVeriAlt.Controls.Add(_btnDonemVerisiSil);
            _flpMevcutVeriAlt.Dock = System.Windows.Forms.DockStyle.Fill;
            _flpMevcutVeriAlt.Name = "_flpMevcutVeriAlt";
            _flpMevcutVeriAlt.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);

            // _lblMevcutVeriOzet
            _lblMevcutVeriOzet.Anchor = System.Windows.Forms.AnchorStyles.Left;
            _lblMevcutVeriOzet.AutoSize = true;
            _lblMevcutVeriOzet.Margin = new System.Windows.Forms.Padding(3, 8, 12, 3);
            _lblMevcutVeriOzet.Name = "_lblMevcutVeriOzet";
            _lblMevcutVeriOzet.Text = "Veri kontrol\u00fc yap\u0131lmad\u0131.";

            // _btnDonemVerisiSil
            _btnDonemVerisiSil.Enabled = false;
            _btnDonemVerisiSil.ForeColor = System.Drawing.Color.DarkRed;
            _btnDonemVerisiSil.Name = "_btnDonemVerisiSil";
            _btnDonemVerisiSil.Size = new System.Drawing.Size(170, 30);
            _btnDonemVerisiSil.Text = "D\u00f6nem Verisini Sil";
            _btnDonemVerisiSil.UseVisualStyleBackColor = true;
            _btnDonemVerisiSil.Click += BtnDonemVerisiSil_Click;

            // _statusStrip
            _statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _lblDurum, _progressBar });
            _statusStrip.Location = new System.Drawing.Point(0, 600);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Size = new System.Drawing.Size(900, 22);

            // _lblDurum
            _lblDurum.Name = "_lblDurum";
            _lblDurum.Size = new System.Drawing.Size(680, 17);
            _lblDurum.Spring = true;
            _lblDurum.Text = "Haz\u0131r";
            _lblDurum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // _progressBar
            _progressBar.Name = "_progressBar";
            _progressBar.Size = new System.Drawing.Size(200, 16);

            // _bgwIslem
            _bgwIslem.WorkerReportsProgress = true;
            _bgwIslem.DoWork += BgwIslem_DoWork;
            _bgwIslem.ProgressChanged += BgwIslem_ProgressChanged;
            _bgwIslem.RunWorkerCompleted += BgwIslem_RunWorkerCompleted;

            // MainForm
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(900, 622);
            Controls.Add(_tblAna);
            Controls.Add(_statusStrip);
            Controls.Add(_menuStrip);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            MainMenuStrip = _menuStrip;
            MinimumSize = new System.Drawing.Size(700, 500);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Defter2Fi\u015F \u2014 E-Defter \u2192 Mikro ERP Muhasebe Fi\u015Fi";

            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            _tblAna.ResumeLayout(false);
            _tblAna.PerformLayout();
            _grpAyarOzet.ResumeLayout(false);
            _grpAyarOzet.PerformLayout();
            _tblAyarOzet.ResumeLayout(false);
            _tblAyarOzet.PerformLayout();
            _flpButonlar.ResumeLayout(false);
            _tabControl.ResumeLayout(false);
            _tabLoglar.ResumeLayout(false);
            _tabMevcutVeri.ResumeLayout(false);
            _tblMevcutVeri.ResumeLayout(false);
            _tblMevcutVeri.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvMevcutVeri).EndInit();
            _flpMevcutVeriAlt.ResumeLayout(false);
            _flpMevcutVeriAlt.PerformLayout();
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem _tsmDosya;
        private System.Windows.Forms.ToolStripMenuItem _tsmCikis;
        private System.Windows.Forms.ToolStripMenuItem _tsmAraclar;
        private System.Windows.Forms.ToolStripMenuItem _tsmAyarlar;
        private System.Windows.Forms.ToolStripMenuItem _tsmLoglariTemizle;
        private System.Windows.Forms.ToolStripMenuItem _tsmYardim;
        private System.Windows.Forms.ToolStripMenuItem _tsmHakkinda;
        private System.Windows.Forms.TableLayoutPanel _tblAna;
        private System.Windows.Forms.GroupBox _grpAyarOzet;
        private System.Windows.Forms.TableLayoutPanel _tblAyarOzet;
        private System.Windows.Forms.Label _lblEdDefterYolu;
        private System.Windows.Forms.Label _lblDbBilgi;
        private System.Windows.Forms.Label _lblFirmaBilgi;
        private System.Windows.Forms.FlowLayoutPanel _flpButonlar;
        private System.Windows.Forms.Button _btnAnalizEt;
        private System.Windows.Forms.Button _btnMevcutVeriKontrol;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _tabLoglar;
        private System.Windows.Forms.RichTextBox _rtbLog;
        private System.Windows.Forms.TabPage _tabMevcutVeri;
        private System.Windows.Forms.TableLayoutPanel _tblMevcutVeri;
        private System.Windows.Forms.DataGridView _dgvMevcutVeri;
        private System.Windows.Forms.FlowLayoutPanel _flpMevcutVeriAlt;
        private System.Windows.Forms.Label _lblMevcutVeriOzet;
        private System.Windows.Forms.Button _btnDonemVerisiSil;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _lblDurum;
        private System.Windows.Forms.ToolStripProgressBar _progressBar;
        private System.ComponentModel.BackgroundWorker _bgwIslem;
    }
}
