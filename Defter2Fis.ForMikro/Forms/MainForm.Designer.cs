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
            _statusStrip = new System.Windows.Forms.StatusStrip();
            _lblDurum = new System.Windows.Forms.ToolStripStatusLabel();
            _progressBar = new System.Windows.Forms.ToolStripProgressBar();
            _bgwIslem = new System.ComponentModel.BackgroundWorker();

            components = new System.ComponentModel.Container();

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
            ((System.ComponentModel.ISupportInitialize)_dgvOzFisler).BeginInit();
            _tabOzCari.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvOzCari).BeginInit();
            _tabOzStok.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvOzStok).BeginInit();
            _tabOzHesap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvOzHesap).BeginInit();
            _tabOzUyari.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvOzUyari).BeginInit();
            _statusStrip.SuspendLayout();
            SuspendLayout();

            // _kryptonManager
            _kryptonManager.GlobalPaletteMode = PaletteMode.Microsoft365Blue;

            // _menuStrip
            _menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                _tsmDosya, _tsmAraclar, _tsmYardim
            });
            _menuStrip.Location = new System.Drawing.Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Size = new System.Drawing.Size(1000, 24);

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
            _tabLoglar.Name = "_tabLoglar";
            _tabLoglar.Padding = new System.Windows.Forms.Padding(3);
            _tabLoglar.Text = "\u0130\u015Flem Loglar\u0131";

            _rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            _rtbLog.Name = "_rtbLog";
            _rtbLog.StateCommon.Content.Font = new System.Drawing.Font("Consolas", 9F);

            // _tabMevcutVeri
            _tabMevcutVeri.Controls.Add(_tblMevcutVeri);
            _tabMevcutVeri.Name = "_tabMevcutVeri";
            _tabMevcutVeri.Padding = new System.Windows.Forms.Padding(3);
            _tabMevcutVeri.Text = "Mevcut D\u00f6nem Verisi";

            _tblMevcutVeri.ColumnCount = 1;
            _tblMevcutVeri.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblMevcutVeri.Controls.Add(_dgvMevcutVeri, 0, 0);
            _tblMevcutVeri.Controls.Add(_flpMevcutVeriAlt, 0, 1);
            _tblMevcutVeri.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblMevcutVeri.Name = "_tblMevcutVeri";
            _tblMevcutVeri.RowCount = 2;
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

            _tblOnizleme.ColumnCount = 1;
            _tblOnizleme.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblOnizleme.Controls.Add(_grpOnizlemeOzet, 0, 0);
            _tblOnizleme.Controls.Add(_tabOnizlemeDetay, 0, 1);
            _tblOnizleme.Dock = System.Windows.Forms.DockStyle.Fill;
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

            _tabOzFisler.Controls.Add(_dgvOzFisler);
            _tabOzFisler.Name = "_tabOzFisler";
            _tabOzFisler.Padding = new System.Windows.Forms.Padding(2);
            _tabOzFisler.Text = "Fi\u015Fler";

            _tabOzCari.Controls.Add(_dgvOzCari);
            _tabOzCari.Name = "_tabOzCari";
            _tabOzCari.Padding = new System.Windows.Forms.Padding(2);
            _tabOzCari.Text = "Cari E\u015Fle\u015Fmeleri";

            _tabOzStok.Controls.Add(_dgvOzStok);
            _tabOzStok.Name = "_tabOzStok";
            _tabOzStok.Padding = new System.Windows.Forms.Padding(2);
            _tabOzStok.Text = "Stok E\u015Fle\u015Fmeleri";

            _tabOzHesap.Controls.Add(_dgvOzHesap);
            _tabOzHesap.Name = "_tabOzHesap";
            _tabOzHesap.Padding = new System.Windows.Forms.Padding(2);
            _tabOzHesap.Text = "Eksik Hesaplar";

            _tabOzUyari.Controls.Add(_dgvOzUyari);
            _tabOzUyari.Name = "_tabOzUyari";
            _tabOzUyari.Padding = new System.Windows.Forms.Padding(2);
            _tabOzUyari.Text = "Uyar\u0131lar";

            _dgvOzFisler.AllowUserToAddRows = false;
            _dgvOzFisler.AllowUserToDeleteRows = false;
            _dgvOzFisler.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _dgvOzFisler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvOzFisler.Dock = System.Windows.Forms.DockStyle.Fill;
            _dgvOzFisler.Name = "_dgvOzFisler";
            _dgvOzFisler.ReadOnly = true;
            _dgvOzFisler.RowHeadersVisible = false;
            _dgvOzFisler.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            _dgvOzCari.AllowUserToAddRows = false;
            _dgvOzCari.AllowUserToDeleteRows = false;
            _dgvOzCari.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _dgvOzCari.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvOzCari.Dock = System.Windows.Forms.DockStyle.Fill;
            _dgvOzCari.Name = "_dgvOzCari";
            _dgvOzCari.ReadOnly = true;
            _dgvOzCari.RowHeadersVisible = false;
            _dgvOzCari.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            _dgvOzStok.AllowUserToAddRows = false;
            _dgvOzStok.AllowUserToDeleteRows = false;
            _dgvOzStok.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _dgvOzStok.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvOzStok.Dock = System.Windows.Forms.DockStyle.Fill;
            _dgvOzStok.Name = "_dgvOzStok";
            _dgvOzStok.ReadOnly = true;
            _dgvOzStok.RowHeadersVisible = false;
            _dgvOzStok.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            _dgvOzHesap.AllowUserToAddRows = false;
            _dgvOzHesap.AllowUserToDeleteRows = false;
            _dgvOzHesap.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _dgvOzHesap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvOzHesap.Dock = System.Windows.Forms.DockStyle.Fill;
            _dgvOzHesap.Name = "_dgvOzHesap";
            _dgvOzHesap.ReadOnly = true;
            _dgvOzHesap.RowHeadersVisible = false;
            _dgvOzHesap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            _dgvOzUyari.AllowUserToAddRows = false;
            _dgvOzUyari.AllowUserToDeleteRows = false;
            _dgvOzUyari.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _dgvOzUyari.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvOzUyari.Dock = System.Windows.Forms.DockStyle.Fill;
            _dgvOzUyari.Name = "_dgvOzUyari";
            _dgvOzUyari.ReadOnly = true;
            _dgvOzUyari.RowHeadersVisible = false;
            _dgvOzUyari.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // _statusStrip
            _statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _lblDurum, _progressBar });
            _statusStrip.Location = new System.Drawing.Point(0, 650);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Size = new System.Drawing.Size(1000, 22);

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
            _tabOzFisler.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_dgvOzFisler).EndInit();
            _tabOzCari.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_dgvOzCari).EndInit();
            _tabOzStok.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_dgvOzStok).EndInit();
            _tabOzHesap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_dgvOzHesap).EndInit();
            _tabOzUyari.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_dgvOzUyari).EndInit();
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
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _lblDurum;
        private System.Windows.Forms.ToolStripProgressBar _progressBar;
        private System.ComponentModel.BackgroundWorker _bgwIslem;
    }
}
