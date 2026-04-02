using Krypton.Toolkit;

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
            _kryptonManager = new KryptonManager();
            _menuStrip = new System.Windows.Forms.MenuStrip();
            _tsmDosya = new System.Windows.Forms.ToolStripMenuItem();
            _tsmCikis = new System.Windows.Forms.ToolStripMenuItem();
            _tsmAraclar = new System.Windows.Forms.ToolStripMenuItem();
            _tsmAyarlar = new System.Windows.Forms.ToolStripMenuItem();
            _tsmLoglariTemizle = new System.Windows.Forms.ToolStripMenuItem();
            _tsmYardim = new System.Windows.Forms.ToolStripMenuItem();
            _tsmHakkinda = new System.Windows.Forms.ToolStripMenuItem();
            _tblAna = new System.Windows.Forms.TableLayoutPanel();
            _grpAyarOzet = new KryptonGroupBox();
            _tblAyarOzet = new System.Windows.Forms.TableLayoutPanel();
            _lblEdDefterYolu = new KryptonLabel();
            _lblDbBilgi = new KryptonLabel();
            _lblFirmaBilgi = new KryptonLabel();
            _flpButonlar = new System.Windows.Forms.FlowLayoutPanel();
            _btnAnalizEt = new KryptonButton();
            _btnMevcutVeriKontrol = new KryptonButton();
            _btnOnizleme = new KryptonButton();
            _btnFisOlustur = new KryptonButton();
            _btnYedekAl = new KryptonButton();
            _tabControl = new System.Windows.Forms.TabControl();
            _tabLoglar = new System.Windows.Forms.TabPage();
            _rtbLog = new KryptonRichTextBox();
            _tabMevcutVeri = new System.Windows.Forms.TabPage();
            _tblMevcutVeri = new System.Windows.Forms.TableLayoutPanel();
            _dgvMevcutVeri = new KryptonDataGridView();
            _flpMevcutVeriAlt = new System.Windows.Forms.FlowLayoutPanel();
            _lblMevcutVeriOzet = new KryptonLabel();
            _btnDonemVerisiSil = new KryptonButton();
            _tabOnizleme = new System.Windows.Forms.TabPage();
            _tblOnizleme = new System.Windows.Forms.TableLayoutPanel();
            _grpOnizlemeOzet = new KryptonGroupBox();
            _tblOnizlemeOzet = new System.Windows.Forms.TableLayoutPanel();
            _lblOzFisSayisi = new KryptonLabel();
            _lblOzSatirSayisi = new KryptonLabel();
            _lblOzCariEslesen = new KryptonLabel();
            _lblOzStokEslesen = new KryptonLabel();
            _lblOzMukerrer = new KryptonLabel();
            _lblOzEksikHesap = new KryptonLabel();
            _tabOnizlemeDetay = new System.Windows.Forms.TabControl();
            _tabOzFisler = new System.Windows.Forms.TabPage();
            _dgvOzFisler = new KryptonDataGridView();
            _tabOzCari = new System.Windows.Forms.TabPage();
            _dgvOzCari = new KryptonDataGridView();
            _tabOzStok = new System.Windows.Forms.TabPage();
            _dgvOzStok = new KryptonDataGridView();
            _tabOzHesap = new System.Windows.Forms.TabPage();
            _dgvOzHesap = new KryptonDataGridView();
            _tabOzUyari = new System.Windows.Forms.TabPage();
            _dgvOzUyari = new KryptonDataGridView();
            _txtFiltreMevcutVeri = new KryptonTextBox();
            _txtFiltreOzFisler = new KryptonTextBox();
            _txtFiltreOzCari = new KryptonTextBox();
            _txtFiltreOzStok = new KryptonTextBox();
            _txtFiltreOzHesap = new KryptonTextBox();
            _txtFiltreOzUyari = new KryptonTextBox();
            _tblOzFisler = new System.Windows.Forms.TableLayoutPanel();
            _tblOzCari = new System.Windows.Forms.TableLayoutPanel();
            _tblOzStok = new System.Windows.Forms.TableLayoutPanel();
            _tblOzHesap = new System.Windows.Forms.TableLayoutPanel();
            _tblOzUyari = new System.Windows.Forms.TableLayoutPanel();
            _pnlBaglantiUyari = new System.Windows.Forms.Panel();
            _tblUyari = new System.Windows.Forms.TableLayoutPanel();
            _lblUyariIcon = new System.Windows.Forms.Label();
            _lblUyariMesaj = new System.Windows.Forms.Label();
            _lnkAyarlaraGit = new System.Windows.Forms.LinkLabel();
            _statusStrip = new System.Windows.Forms.StatusStrip();
            _lblDurum = new System.Windows.Forms.ToolStripStatusLabel();
            _progressBar = new System.Windows.Forms.ToolStripProgressBar();
            _bgwIslem = new System.ComponentModel.BackgroundWorker();

            components = new System.ComponentModel.Container();

            _pnlBaglantiUyari.SuspendLayout();
            _tblUyari.SuspendLayout();
            _menuStrip.SuspendLayout();
            _tblAna.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grpAyarOzet).BeginInit();
            _grpAyarOzet.Panel.SuspendLayout();
            _grpAyarOzet.SuspendLayout();
            _tblAyarOzet.SuspendLayout();
            _flpButonlar.SuspendLayout();
            _tabControl.SuspendLayout();
            _tabLoglar.SuspendLayout();
            _tabMevcutVeri.SuspendLayout();
            _tblMevcutVeri.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvMevcutVeri).BeginInit();
            _flpMevcutVeriAlt.SuspendLayout();
            _tabOnizleme.SuspendLayout();
            _tblOnizleme.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grpOnizlemeOzet).BeginInit();
            _grpOnizlemeOzet.Panel.SuspendLayout();
            _grpOnizlemeOzet.SuspendLayout();
            _tblOnizlemeOzet.SuspendLayout();
            _tabOnizlemeDetay.SuspendLayout();
            _tabOzFisler.SuspendLayout();
            _tblOzFisler.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvOzFisler).BeginInit();
            _tabOzCari.SuspendLayout();
            _tblOzCari.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvOzCari).BeginInit();
            _tabOzStok.SuspendLayout();
            _tblOzStok.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvOzStok).BeginInit();
            _tabOzHesap.SuspendLayout();
            _tblOzHesap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvOzHesap).BeginInit();
            _tabOzUyari.SuspendLayout();
            _tblOzUyari.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvOzUyari).BeginInit();
            _statusStrip.SuspendLayout();
            SuspendLayout();

            // _kryptonManager
            _kryptonManager.GlobalPaletteMode = PaletteMode.Microsoft365BlackDarkMode;

            // _menuStrip
            _menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                _tsmDosya, _tsmAraclar, _tsmYardim
            });
            _menuStrip.Location = new System.Drawing.Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Size = new System.Drawing.Size(1000, 24);
            _menuStrip.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            _menuStrip.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            _menuStrip.Renderer = new System.Windows.Forms.ToolStripProfessionalRenderer(
                new DarkMenuColorTable());

            _tsmDosya.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _tsmCikis });
            _tsmDosya.Name = "_tsmDosya";
            _tsmDosya.Text = "&Dosya";
            _tsmCikis.Name = "_tsmCikis";
            _tsmCikis.Text = "&\u00C7\u0131k\u0131\u015F";
            _tsmCikis.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            _tsmCikis.Click += TsmCikis_Click;

            _tsmAraclar.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _tsmAyarlar, _tsmLoglariTemizle });
            _tsmAraclar.Name = "_tsmAraclar";
            _tsmAraclar.Text = "A&ra\u00e7lar";
            _tsmAyarlar.Name = "_tsmAyarlar";
            _tsmAyarlar.Text = "&Ayarlar...";
            _tsmAyarlar.Click += TsmAyarlar_Click;
            _tsmLoglariTemizle.Name = "_tsmLoglariTemizle";
            _tsmLoglariTemizle.Text = "&Loglar\u0131 Temizle";
            _tsmLoglariTemizle.Click += TsmLoglariTemizle_Click;

            _tsmYardim.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _tsmHakkinda });
            _tsmYardim.Name = "_tsmYardim";
            _tsmYardim.Text = "&Yard\u0131m";
            _tsmHakkinda.Name = "_tsmHakkinda";
            _tsmHakkinda.Text = "&Hakk\u0131nda...";
            _tsmHakkinda.Click += TsmHakkinda_Click;

            // _pnlBaglantiUyari
            _pnlBaglantiUyari.AutoSize = true;
            _pnlBaglantiUyari.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _pnlBaglantiUyari.BackColor = System.Drawing.Color.FromArgb(120, 60, 0);
            _pnlBaglantiUyari.Controls.Add(_tblUyari);
            _pnlBaglantiUyari.Dock = System.Windows.Forms.DockStyle.Fill;
            _pnlBaglantiUyari.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            _pnlBaglantiUyari.Name = "_pnlBaglantiUyari";
            _pnlBaglantiUyari.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            _pnlBaglantiUyari.Visible = false;

            // _tblUyari
            _tblUyari.AutoSize = true;
            _tblUyari.ColumnCount = 3;
            _tblUyari.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblUyari.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblUyari.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblUyari.Controls.Add(_lblUyariIcon, 0, 0);
            _tblUyari.Controls.Add(_lblUyariMesaj, 1, 0);
            _tblUyari.Controls.Add(_lnkAyarlaraGit, 2, 0);
            _tblUyari.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblUyari.Name = "_tblUyari";
            _tblUyari.RowCount = 1;
            _tblUyari.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // _lblUyariIcon
            _lblUyariIcon.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblUyariIcon.AutoSize = true;
            _lblUyariIcon.Font = new System.Drawing.Font("Segoe UI", 14F);
            _lblUyariIcon.ForeColor = System.Drawing.Color.FromArgb(255, 200, 60);
            _lblUyariIcon.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            _lblUyariIcon.Name = "_lblUyariIcon";
            _lblUyariIcon.Text = "\u26A0";

            // _lblUyariMesaj
            _lblUyariMesaj.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblUyariMesaj.AutoSize = true;
            _lblUyariMesaj.ForeColor = System.Drawing.Color.FromArgb(255, 230, 180);
            _lblUyariMesaj.Name = "_lblUyariMesaj";
            _lblUyariMesaj.Text = "Veritaban\u0131 ba\u011flant\u0131s\u0131 yap\u0131land\u0131r\u0131lmam\u0131\u015F! \u0130\u015Flem yapabilmek i\u00e7in \u00f6nce Ara\u00e7lar > Ayarlar men\u00fcs\u00fcnden SQL Server ba\u011flant\u0131n\u0131z\u0131 olu\u015Fturun.";

            // _lnkAyarlaraGit
            _lnkAyarlaraGit.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lnkAyarlaraGit.AutoSize = true;
            _lnkAyarlaraGit.LinkColor = System.Drawing.Color.FromArgb(100, 200, 255);
            _lnkAyarlaraGit.ActiveLinkColor = System.Drawing.Color.FromArgb(150, 220, 255);
            _lnkAyarlaraGit.Name = "_lnkAyarlaraGit";
            _lnkAyarlaraGit.Text = "Ayarlar\u0131 A\u00e7";
            _lnkAyarlaraGit.Click += LnkAyarlaraGit_Click;

            // _tblAna
            _tblAna.ColumnCount = 1;
            _tblAna.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblAna.Controls.Add(_pnlBaglantiUyari, 0, 0);
            _tblAna.Controls.Add(_grpAyarOzet, 0, 1);
            _tblAna.Controls.Add(_flpButonlar, 0, 2);
            _tblAna.Controls.Add(_tabControl, 0, 3);
            _tblAna.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblAna.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            _tblAna.Location = new System.Drawing.Point(0, 24);
            _tblAna.Name = "_tblAna";
            _tblAna.Padding = new System.Windows.Forms.Padding(6, 4, 6, 0);
            _tblAna.RowCount = 4;
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblAna.Size = new System.Drawing.Size(1000, 626);

            // _grpAyarOzet
            _grpAyarOzet.AutoSize = true;
            _grpAyarOzet.Dock = System.Windows.Forms.DockStyle.Fill;
            _grpAyarOzet.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            _grpAyarOzet.Name = "_grpAyarOzet";
            _grpAyarOzet.Panel.Controls.Add(_tblAyarOzet);
            _grpAyarOzet.Size = new System.Drawing.Size(982, 76);
            _grpAyarOzet.Values.Heading = "D\u00f6nem Bilgileri";

            // _tblAyarOzet
            _tblAyarOzet.AutoSize = true;
            _tblAyarOzet.ColumnCount = 1;
            _tblAyarOzet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblAyarOzet.Controls.Add(_lblEdDefterYolu, 0, 0);
            _tblAyarOzet.Controls.Add(_lblDbBilgi, 0, 1);
            _tblAyarOzet.Controls.Add(_lblFirmaBilgi, 0, 2);
            _tblAyarOzet.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblAyarOzet.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            _tblAyarOzet.Name = "_tblAyarOzet";
            _tblAyarOzet.RowCount = 3;
            _tblAyarOzet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAyarOzet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAyarOzet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            _lblEdDefterYolu.Name = "_lblEdDefterYolu";
            _lblEdDefterYolu.Values.Text = "E-Defter: ...";
            _lblDbBilgi.Name = "_lblDbBilgi";
            _lblDbBilgi.Values.Text = "DB: ...";
            _lblFirmaBilgi.Name = "_lblFirmaBilgi";
            _lblFirmaBilgi.Values.Text = "Firma: ...";

            // _flpButonlar
            _flpButonlar.AutoSize = true;
            _flpButonlar.Dock = System.Windows.Forms.DockStyle.Fill;
            _flpButonlar.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            _flpButonlar.Controls.Add(_btnAnalizEt);
            _flpButonlar.Controls.Add(_btnMevcutVeriKontrol);
            _flpButonlar.Controls.Add(_btnOnizleme);
            _flpButonlar.Controls.Add(_btnFisOlustur);
            _flpButonlar.Controls.Add(_btnYedekAl);
            _flpButonlar.Name = "_flpButonlar";

            _btnAnalizEt.Name = "_btnAnalizEt";
            _btnAnalizEt.Size = new System.Drawing.Size(140, 34);
            _btnAnalizEt.Values.Text = "\u25B6 Analiz Et";
            _btnAnalizEt.Click += BtnAnalizEt_Click;

            _btnMevcutVeriKontrol.Name = "_btnMevcutVeriKontrol";
            _btnMevcutVeriKontrol.Size = new System.Drawing.Size(170, 34);
            _btnMevcutVeriKontrol.Values.Text = "Mevcut Veri Kontrol";
            _btnMevcutVeriKontrol.Click += BtnMevcutVeriKontrol_Click;

            _btnOnizleme.Name = "_btnOnizleme";
            _btnOnizleme.Size = new System.Drawing.Size(140, 34);
            _btnOnizleme.Values.Text = "\uD83D\uDD0D \u00D6nizleme";
            _btnOnizleme.Click += BtnOnizleme_Click;

            _btnFisOlustur.Name = "_btnFisOlustur";
            _btnFisOlustur.Size = new System.Drawing.Size(140, 34);
            _btnFisOlustur.Values.Text = "\u25B6 Fi\u015F Olu\u015Ftur";
            _btnFisOlustur.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(40, 167, 69);
            _btnFisOlustur.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.White;
            _btnFisOlustur.Click += BtnFisOlustur_Click;

            _btnYedekAl.Name = "_btnYedekAl";
            _btnYedekAl.Size = new System.Drawing.Size(140, 34);
            _btnYedekAl.Values.Text = "\uD83D\uDCBE Yedek Al";
            _btnYedekAl.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(23, 162, 184);
            _btnYedekAl.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.White;
            _btnYedekAl.Click += BtnYedekAl_Click;

            // _tabControl
            _tabControl.Controls.Add(_tabLoglar);
            _tabControl.Controls.Add(_tabMevcutVeri);
            _tabControl.Controls.Add(_tabOnizleme);
            _tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            _tabControl.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            _tabControl.Name = "_tabControl";
            _tabControl.SelectedIndex = 0;

            // _tabLoglar
            _tabLoglar.Controls.Add(_rtbLog);
            _tabLoglar.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _tabLoglar.Name = "_tabLoglar";
            _tabLoglar.Padding = new System.Windows.Forms.Padding(3);
            _tabLoglar.Text = "\u0130\u015Flem Loglar\u0131";

            _rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            _rtbLog.Name = "_rtbLog";
            _rtbLog.StateCommon.Content.Font = new System.Drawing.Font("Consolas", 9F);
            _rtbLog.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(30, 30, 30);
            _rtbLog.StateCommon.Content.Color1 = System.Drawing.Color.FromArgb(220, 220, 220);

            // _tabMevcutVeri
            _tabMevcutVeri.Controls.Add(_tblMevcutVeri);
            _tabMevcutVeri.Name = "_tabMevcutVeri";
            _tabMevcutVeri.Padding = new System.Windows.Forms.Padding(3);
            _tabMevcutVeri.Text = "Mevcut D\u00f6nem Verisi";
            _tabMevcutVeri.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);

            _tblMevcutVeri.ColumnCount = 1;
            _tblMevcutVeri.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblMevcutVeri.Controls.Add(_txtFiltreMevcutVeri, 0, 0);
            _tblMevcutVeri.Controls.Add(_dgvMevcutVeri, 0, 1);
            _tblMevcutVeri.Controls.Add(_flpMevcutVeriAlt, 0, 2);
            _tblMevcutVeri.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblMevcutVeri.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _tblMevcutVeri.Name = "_tblMevcutVeri";
            _tblMevcutVeri.RowCount = 3;
            _tblMevcutVeri.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblMevcutVeri.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblMevcutVeri.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            _dgvMevcutVeri.AllowUserToAddRows = false;
            _dgvMevcutVeri.AllowUserToDeleteRows = false;
            _dgvMevcutVeri.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _dgvMevcutVeri.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvMevcutVeri.Dock = System.Windows.Forms.DockStyle.Fill;
            _dgvMevcutVeri.Name = "_dgvMevcutVeri";
            _dgvMevcutVeri.ReadOnly = true;
            _dgvMevcutVeri.RowHeadersVisible = false;
            _dgvMevcutVeri.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            _flpMevcutVeriAlt.AutoSize = true;
            _flpMevcutVeriAlt.Controls.Add(_lblMevcutVeriOzet);
            _flpMevcutVeriAlt.Controls.Add(_btnDonemVerisiSil);
            _flpMevcutVeriAlt.Dock = System.Windows.Forms.DockStyle.Fill;
            _flpMevcutVeriAlt.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _flpMevcutVeriAlt.Name = "_flpMevcutVeriAlt";
            _flpMevcutVeriAlt.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);

            _lblMevcutVeriOzet.Anchor = System.Windows.Forms.AnchorStyles.Left;
            _lblMevcutVeriOzet.Margin = new System.Windows.Forms.Padding(3, 8, 12, 3);
            _lblMevcutVeriOzet.Name = "_lblMevcutVeriOzet";
            _lblMevcutVeriOzet.Values.Text = "Veri kontrol\u00fc yap\u0131lmad\u0131.";

            _btnDonemVerisiSil.Enabled = false;
            _btnDonemVerisiSil.Name = "_btnDonemVerisiSil";
            _btnDonemVerisiSil.Size = new System.Drawing.Size(170, 30);
            _btnDonemVerisiSil.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(220, 53, 69);
            _btnDonemVerisiSil.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.White;
            _btnDonemVerisiSil.Values.Text = "D\u00f6nem Verisini Sil";
            _btnDonemVerisiSil.Click += BtnDonemVerisiSil_Click;

            // _tabOnizleme
            _tabOnizleme.Controls.Add(_tblOnizleme);
            _tabOnizleme.Name = "_tabOnizleme";
            _tabOnizleme.Padding = new System.Windows.Forms.Padding(3);
            _tabOnizleme.Text = "\u00D6nizleme / Test";
            _tabOnizleme.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);

            _tblOnizleme.ColumnCount = 1;
            _tblOnizleme.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblOnizleme.Controls.Add(_grpOnizlemeOzet, 0, 0);
            _tblOnizleme.Controls.Add(_tabOnizlemeDetay, 0, 1);
            _tblOnizleme.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblOnizleme.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _tblOnizleme.Name = "_tblOnizleme";
            _tblOnizleme.RowCount = 2;
            _tblOnizleme.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblOnizleme.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            // _grpOnizlemeOzet
            _grpOnizlemeOzet.AutoSize = true;
            _grpOnizlemeOzet.Dock = System.Windows.Forms.DockStyle.Fill;
            _grpOnizlemeOzet.Name = "_grpOnizlemeOzet";
            _grpOnizlemeOzet.Panel.Controls.Add(_tblOnizlemeOzet);
            _grpOnizlemeOzet.Values.Heading = "\u0130statistik \u00D6zeti";

            _tblOnizlemeOzet.AutoSize = true;
            _tblOnizlemeOzet.ColumnCount = 6;
            _tblOnizlemeOzet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            _tblOnizlemeOzet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            _tblOnizlemeOzet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            _tblOnizlemeOzet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            _tblOnizlemeOzet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            _tblOnizlemeOzet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.7F));
            _tblOnizlemeOzet.Controls.Add(_lblOzFisSayisi, 0, 0);
            _tblOnizlemeOzet.Controls.Add(_lblOzSatirSayisi, 1, 0);
            _tblOnizlemeOzet.Controls.Add(_lblOzCariEslesen, 2, 0);
            _tblOnizlemeOzet.Controls.Add(_lblOzStokEslesen, 3, 0);
            _tblOnizlemeOzet.Controls.Add(_lblOzMukerrer, 4, 0);
            _tblOnizlemeOzet.Controls.Add(_lblOzEksikHesap, 5, 0);
            _tblOnizlemeOzet.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblOnizlemeOzet.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            _tblOnizlemeOzet.Name = "_tblOnizlemeOzet";
            _tblOnizlemeOzet.RowCount = 1;
            _tblOnizlemeOzet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            _lblOzFisSayisi.Name = "_lblOzFisSayisi";
            _lblOzFisSayisi.Values.Text = "Fi\u015F: -";
            _lblOzSatirSayisi.Name = "_lblOzSatirSayisi";
            _lblOzSatirSayisi.Values.Text = "Sat\u0131r: -";
            _lblOzCariEslesen.Name = "_lblOzCariEslesen";
            _lblOzCariEslesen.Values.Text = "Cari: -";
            _lblOzStokEslesen.Name = "_lblOzStokEslesen";
            _lblOzStokEslesen.Values.Text = "Stok: -";
            _lblOzMukerrer.Name = "_lblOzMukerrer";
            _lblOzMukerrer.Values.Text = "M\u00fckerrer: -";
            _lblOzEksikHesap.Name = "_lblOzEksikHesap";
            _lblOzEksikHesap.Values.Text = "Eksik Hesap: -";

            // _tabOnizlemeDetay
            _tabOnizlemeDetay.Controls.Add(_tabOzFisler);
            _tabOnizlemeDetay.Controls.Add(_tabOzCari);
            _tabOnizlemeDetay.Controls.Add(_tabOzStok);
            _tabOnizlemeDetay.Controls.Add(_tabOzHesap);
            _tabOnizlemeDetay.Controls.Add(_tabOzUyari);
            _tabOnizlemeDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            _tabOnizlemeDetay.Name = "_tabOnizlemeDetay";

            _tabOzFisler.Controls.Add(_tblOzFisler);
            _tabOzFisler.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _tabOzFisler.Name = "_tabOzFisler";
            _tabOzFisler.Padding = new System.Windows.Forms.Padding(2);
            _tabOzFisler.Text = "Fi\u015Fler";

            _tabOzCari.Controls.Add(_tblOzCari);
            _tabOzCari.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _tabOzCari.Name = "_tabOzCari";
            _tabOzCari.Padding = new System.Windows.Forms.Padding(2);
            _tabOzCari.Text = "Cari E\u015Fle\u015Fmeleri";

            _tabOzStok.Controls.Add(_tblOzStok);
            _tabOzStok.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _tabOzStok.Name = "_tabOzStok";
            _tabOzStok.Padding = new System.Windows.Forms.Padding(2);
            _tabOzStok.Text = "Stok E\u015Fle\u015Fmeleri";

            _tabOzHesap.Controls.Add(_tblOzHesap);
            _tabOzHesap.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _tabOzHesap.Name = "_tabOzHesap";
            _tabOzHesap.Padding = new System.Windows.Forms.Padding(2);
            _tabOzHesap.Text = "Eksik Hesaplar";

            _tabOzUyari.Controls.Add(_tblOzUyari);
            _tabOzUyari.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _tabOzUyari.Name = "_tabOzUyari";
            _tabOzUyari.Padding = new System.Windows.Forms.Padding(2);
            _tabOzUyari.Text = "Uyar\u0131lar";

            // Filtre TLP ve TextBox ayarlari
            FiltreAlanOlustur(_tblOzFisler, _txtFiltreOzFisler, _dgvOzFisler, "_tblOzFisler", "_txtFiltreOzFisler");
            FiltreAlanOlustur(_tblOzCari, _txtFiltreOzCari, _dgvOzCari, "_tblOzCari", "_txtFiltreOzCari");
            FiltreAlanOlustur(_tblOzStok, _txtFiltreOzStok, _dgvOzStok, "_tblOzStok", "_txtFiltreOzStok");
            FiltreAlanOlustur(_tblOzHesap, _txtFiltreOzHesap, _dgvOzHesap, "_tblOzHesap", "_txtFiltreOzHesap");
            FiltreAlanOlustur(_tblOzUyari, _txtFiltreOzUyari, _dgvOzUyari, "_tblOzUyari", "_txtFiltreOzUyari");
            FiltreTextBoxStilUygula(_txtFiltreMevcutVeri, "_txtFiltreMevcutVeri");
            _txtFiltreMevcutVeri.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtFiltreMevcutVeri.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);

            _dgvOzFisler.AllowUserToAddRows = false;
            _dgvOzFisler.AllowUserToDeleteRows = false;
            _dgvOzFisler.BackgroundColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _dgvOzFisler.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _dgvOzFisler.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(45, 45, 48), ForeColor = System.Drawing.Color.FromArgb(220, 220, 220) };
            _dgvOzFisler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvOzFisler.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(30, 30, 30), ForeColor = System.Drawing.Color.FromArgb(220, 220, 220), SelectionBackColor = System.Drawing.Color.FromArgb(0, 122, 204), SelectionForeColor = System.Drawing.Color.White };
            _dgvOzFisler.Dock = System.Windows.Forms.DockStyle.Fill;
            _dgvOzFisler.EnableHeadersVisualStyles = false;
            _dgvOzFisler.GridColor = System.Drawing.Color.FromArgb(63, 63, 70);
            _dgvOzFisler.Name = "_dgvOzFisler";
            _dgvOzFisler.ReadOnly = true;
            _dgvOzFisler.RowHeadersVisible = false;
            _dgvOzFisler.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            _dgvOzCari.AllowUserToAddRows = false;
            _dgvOzCari.AllowUserToDeleteRows = false;
            _dgvOzCari.BackgroundColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _dgvOzCari.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _dgvOzCari.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(45, 45, 48), ForeColor = System.Drawing.Color.FromArgb(220, 220, 220) };
            _dgvOzCari.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvOzCari.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(30, 30, 30), ForeColor = System.Drawing.Color.FromArgb(220, 220, 220), SelectionBackColor = System.Drawing.Color.FromArgb(0, 122, 204), SelectionForeColor = System.Drawing.Color.White };
            _dgvOzCari.Dock = System.Windows.Forms.DockStyle.Fill;
            _dgvOzCari.EnableHeadersVisualStyles = false;
            _dgvOzCari.GridColor = System.Drawing.Color.FromArgb(63, 63, 70);
            _dgvOzCari.Name = "_dgvOzCari";
            _dgvOzCari.ReadOnly = true;
            _dgvOzCari.RowHeadersVisible = false;
            _dgvOzCari.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            _dgvOzStok.AllowUserToAddRows = false;
            _dgvOzStok.AllowUserToDeleteRows = false;
            _dgvOzStok.BackgroundColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _dgvOzStok.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _dgvOzStok.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(45, 45, 48), ForeColor = System.Drawing.Color.FromArgb(220, 220, 220) };
            _dgvOzStok.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvOzStok.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(30, 30, 30), ForeColor = System.Drawing.Color.FromArgb(220, 220, 220), SelectionBackColor = System.Drawing.Color.FromArgb(0, 122, 204), SelectionForeColor = System.Drawing.Color.White };
            _dgvOzStok.Dock = System.Windows.Forms.DockStyle.Fill;
            _dgvOzStok.EnableHeadersVisualStyles = false;
            _dgvOzStok.GridColor = System.Drawing.Color.FromArgb(63, 63, 70);
            _dgvOzStok.Name = "_dgvOzStok";
            _dgvOzStok.ReadOnly = true;
            _dgvOzStok.RowHeadersVisible = false;
            _dgvOzStok.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            _dgvOzHesap.AllowUserToAddRows = false;
            _dgvOzHesap.AllowUserToDeleteRows = false;
            _dgvOzHesap.BackgroundColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _dgvOzHesap.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _dgvOzHesap.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(45, 45, 48), ForeColor = System.Drawing.Color.FromArgb(220, 220, 220) };
            _dgvOzHesap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvOzHesap.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(30, 30, 30), ForeColor = System.Drawing.Color.FromArgb(220, 220, 220), SelectionBackColor = System.Drawing.Color.FromArgb(0, 122, 204), SelectionForeColor = System.Drawing.Color.White };
            _dgvOzHesap.Dock = System.Windows.Forms.DockStyle.Fill;
            _dgvOzHesap.EnableHeadersVisualStyles = false;
            _dgvOzHesap.GridColor = System.Drawing.Color.FromArgb(63, 63, 70);
            _dgvOzHesap.Name = "_dgvOzHesap";
            _dgvOzHesap.ReadOnly = true;
            _dgvOzHesap.RowHeadersVisible = false;
            _dgvOzHesap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            _dgvOzUyari.AllowUserToAddRows = false;
            _dgvOzUyari.AllowUserToDeleteRows = false;
            _dgvOzUyari.BackgroundColor = System.Drawing.Color.FromArgb(37, 37, 38);
            _dgvOzUyari.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _dgvOzUyari.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(45, 45, 48), ForeColor = System.Drawing.Color.FromArgb(220, 220, 220) };
            _dgvOzUyari.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvOzUyari.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(30, 30, 30), ForeColor = System.Drawing.Color.FromArgb(220, 220, 220), SelectionBackColor = System.Drawing.Color.FromArgb(0, 122, 204), SelectionForeColor = System.Drawing.Color.White };
            _dgvOzUyari.Dock = System.Windows.Forms.DockStyle.Fill;
            _dgvOzUyari.EnableHeadersVisualStyles = false;
            _dgvOzUyari.GridColor = System.Drawing.Color.FromArgb(63, 63, 70);
            _dgvOzUyari.Name = "_dgvOzUyari";
            _dgvOzUyari.ReadOnly = true;
            _dgvOzUyari.RowHeadersVisible = false;
            _dgvOzUyari.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // _statusStrip
            _statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _lblDurum, _progressBar });
            _statusStrip.Location = new System.Drawing.Point(0, 650);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Size = new System.Drawing.Size(1000, 22);
            _statusStrip.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            _statusStrip.ForeColor = System.Drawing.Color.White;

            _lblDurum.Name = "_lblDurum";
            _lblDurum.Size = new System.Drawing.Size(780, 17);
            _lblDurum.Spring = true;
            _lblDurum.Text = "Haz\u0131r";
            _lblDurum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

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
            ClientSize = new System.Drawing.Size(1000, 672);
            Controls.Add(_tblAna);
            Controls.Add(_statusStrip);
            Controls.Add(_menuStrip);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            MainMenuStrip = _menuStrip;
            MinimumSize = new System.Drawing.Size(800, 550);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Defter2Fi\u015F \u2014 E-Defter \u2192 Mikro ERP Muhasebe Fi\u015Fi";

            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            _tblAna.ResumeLayout(false);
            _tblAna.PerformLayout();
            _grpAyarOzet.Panel.ResumeLayout(false);
            _grpAyarOzet.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_grpAyarOzet).EndInit();
            _grpAyarOzet.ResumeLayout(false);
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
            _tabOnizleme.ResumeLayout(false);
            _tblOnizleme.ResumeLayout(false);
            _tblOnizleme.PerformLayout();
            _grpOnizlemeOzet.Panel.ResumeLayout(false);
            _grpOnizlemeOzet.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_grpOnizlemeOzet).EndInit();
            _grpOnizlemeOzet.ResumeLayout(false);
            _tblOnizlemeOzet.ResumeLayout(false);
            _tblOnizlemeOzet.PerformLayout();
            _tabOnizlemeDetay.ResumeLayout(false);
            _tblOzFisler.ResumeLayout(false);
            _tblOzFisler.PerformLayout();
            _tabOzFisler.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_dgvOzFisler).EndInit();
            _tblOzCari.ResumeLayout(false);
            _tblOzCari.PerformLayout();
            _tabOzCari.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_dgvOzCari).EndInit();
            _tblOzStok.ResumeLayout(false);
            _tblOzStok.PerformLayout();
            _tabOzStok.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_dgvOzStok).EndInit();
            _tblOzHesap.ResumeLayout(false);
            _tblOzHesap.PerformLayout();
            _tabOzHesap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_dgvOzHesap).EndInit();
            _tblOzUyari.ResumeLayout(false);
            _tblOzUyari.PerformLayout();
            _tabOzUyari.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_dgvOzUyari).EndInit();
            _tblUyari.ResumeLayout(false);
            _tblUyari.PerformLayout();
            _pnlBaglantiUyari.ResumeLayout(false);
            _pnlBaglantiUyari.PerformLayout();
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private KryptonManager _kryptonManager;
        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem _tsmDosya;
        private System.Windows.Forms.ToolStripMenuItem _tsmCikis;
        private System.Windows.Forms.ToolStripMenuItem _tsmAraclar;
        private System.Windows.Forms.ToolStripMenuItem _tsmAyarlar;
        private System.Windows.Forms.ToolStripMenuItem _tsmLoglariTemizle;
        private System.Windows.Forms.ToolStripMenuItem _tsmYardim;
        private System.Windows.Forms.ToolStripMenuItem _tsmHakkinda;
        private System.Windows.Forms.TableLayoutPanel _tblAna;
        private KryptonGroupBox _grpAyarOzet;
        private System.Windows.Forms.TableLayoutPanel _tblAyarOzet;
        private KryptonLabel _lblEdDefterYolu;
        private KryptonLabel _lblDbBilgi;
        private KryptonLabel _lblFirmaBilgi;
        private System.Windows.Forms.FlowLayoutPanel _flpButonlar;
        private KryptonButton _btnAnalizEt;
        private KryptonButton _btnMevcutVeriKontrol;
        private KryptonButton _btnOnizleme;
        private KryptonButton _btnFisOlustur;
        private KryptonButton _btnYedekAl;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _tabLoglar;
        private KryptonRichTextBox _rtbLog;
        private System.Windows.Forms.TabPage _tabMevcutVeri;
        private System.Windows.Forms.TableLayoutPanel _tblMevcutVeri;
        private KryptonDataGridView _dgvMevcutVeri;
        private System.Windows.Forms.FlowLayoutPanel _flpMevcutVeriAlt;
        private KryptonLabel _lblMevcutVeriOzet;
        private KryptonButton _btnDonemVerisiSil;
        private System.Windows.Forms.TabPage _tabOnizleme;
        private System.Windows.Forms.TableLayoutPanel _tblOnizleme;
        private KryptonGroupBox _grpOnizlemeOzet;
        private System.Windows.Forms.TableLayoutPanel _tblOnizlemeOzet;
        private KryptonLabel _lblOzFisSayisi;
        private KryptonLabel _lblOzSatirSayisi;
        private KryptonLabel _lblOzCariEslesen;
        private KryptonLabel _lblOzStokEslesen;
        private KryptonLabel _lblOzMukerrer;
        private KryptonLabel _lblOzEksikHesap;
        private System.Windows.Forms.TabControl _tabOnizlemeDetay;
        private System.Windows.Forms.TabPage _tabOzFisler;
        private KryptonDataGridView _dgvOzFisler;
        private System.Windows.Forms.TabPage _tabOzCari;
        private KryptonDataGridView _dgvOzCari;
        private System.Windows.Forms.TabPage _tabOzStok;
        private KryptonDataGridView _dgvOzStok;
        private System.Windows.Forms.TabPage _tabOzHesap;
        private KryptonDataGridView _dgvOzHesap;
        private System.Windows.Forms.TabPage _tabOzUyari;
        private KryptonDataGridView _dgvOzUyari;
        private KryptonTextBox _txtFiltreMevcutVeri;
        private KryptonTextBox _txtFiltreOzFisler;
        private KryptonTextBox _txtFiltreOzCari;
        private KryptonTextBox _txtFiltreOzStok;
        private KryptonTextBox _txtFiltreOzHesap;
        private KryptonTextBox _txtFiltreOzUyari;
        private System.Windows.Forms.TableLayoutPanel _tblOzFisler;
        private System.Windows.Forms.TableLayoutPanel _tblOzCari;
        private System.Windows.Forms.TableLayoutPanel _tblOzStok;
        private System.Windows.Forms.TableLayoutPanel _tblOzHesap;
        private System.Windows.Forms.TableLayoutPanel _tblOzUyari;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _lblDurum;
        private System.Windows.Forms.ToolStripProgressBar _progressBar;
        private System.ComponentModel.BackgroundWorker _bgwIslem;
        private System.Windows.Forms.Panel _pnlBaglantiUyari;
        private System.Windows.Forms.TableLayoutPanel _tblUyari;
        private System.Windows.Forms.Label _lblUyariIcon;
        private System.Windows.Forms.Label _lblUyariMesaj;
        private System.Windows.Forms.LinkLabel _lnkAyarlaraGit;
    }
}
