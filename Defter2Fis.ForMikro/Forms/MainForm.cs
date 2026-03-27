using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Defter2Fis.ForMikro.Models;
using Defter2Fis.ForMikro.Services;

namespace Defter2Fis.ForMikro.Forms
{
    /// <summary>
    /// Defter2Fiş ana uygulama formu.
    /// E-Defter analiz, mevcut veri kontrol, fiş oluşturma işlemlerini yönetir.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly LogService _log = new LogService();
        private MikroDbService _dbService;
        private List<YevmiyeDefteri> _defterler;
        private bool _islemDevam;

        public MainForm()
        {
            InitializeComponent();
            _log.LogEklendi += Log_LogEklendi;
            AyarOzetiniGuncelle();
        }

        #region Ayar Okuma

        private string EdDefterRootPath => ConfigurationManager.AppSettings["EdDefterRootPath"] ?? string.Empty;
        private string SicilNo => ConfigurationManager.AppSettings["SicilNo"] ?? string.Empty;
        private string MaliYilAraligi => ConfigurationManager.AppSettings["MaliYilAraligi"] ?? string.Empty;
        private string AyKlasoru => ConfigurationManager.AppSettings["AyKlasoru"] ?? string.Empty;
        private int FirmaNo => int.TryParse(ConfigurationManager.AppSettings["FirmaNo"], out int v) ? v : 0;
        private int SubeNo => int.TryParse(ConfigurationManager.AppSettings["SubeNo"], out int v) ? v : 0;
        private short DBCNo => short.TryParse(ConfigurationManager.AppSettings["DBCNo"], out short v) ? v : (short)0;

        private string KlasorYolu => Path.Combine(EdDefterRootPath, SicilNo, MaliYilAraligi, AyKlasoru);

        #endregion

        #region Ayar Özeti

        private void AyarOzetiniGuncelle()
        {
            _lblEdDefterYolu.Text = $"E-Defter: {KlasorYolu}";
            _lblDbBilgi.Text = $"DB: {ConfigurationManager.ConnectionStrings["MikroDB"]?.ConnectionString ?? "?"}";
            _lblFirmaBilgi.Text = $"Sicil: {SicilNo}  |  Mali Yıl: {MaliYilAraligi}  |  Ay: {AyKlasoru}  |  Firma: {FirmaNo}/{SubeNo}  DBC: {DBCNo}";
        }

        #endregion

        #region Log Sistemi

        private void Log_LogEklendi(object sender, LogEventArgs e)
        {
            if (_rtbLog.InvokeRequired)
            {
                _rtbLog.BeginInvoke(new Action(() => LogSatiriEkle(e)));
            }
            else
            {
                LogSatiriEkle(e);
            }
        }

        private void LogSatiriEkle(LogEventArgs e)
        {
            Color renk;
            switch (e.Seviye)
            {
                case LogSeviye.Basari: renk = Color.DarkGreen; break;
                case LogSeviye.Uyari: renk = Color.DarkOrange; break;
                case LogSeviye.Hata: renk = Color.Red; break;
                default: renk = Color.Black; break;
            }

            _rtbLog.SelectionStart = _rtbLog.TextLength;
            _rtbLog.SelectionLength = 0;
            _rtbLog.SelectionColor = renk;
            _rtbLog.AppendText(e.Formatla() + Environment.NewLine);
            _rtbLog.ScrollToCaret();
        }

        #endregion

        #region Menü Olayları

        private void TsmAyarlar_Click(object sender, EventArgs e)
        {
            using (var frm = new AyarlarForm())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    AyarOzetiniGuncelle();
                    _log.Bilgi("Ayarlar güncellendi.");
                }
            }
        }

        private void TsmHakkinda_Click(object sender, EventArgs e)
        {
            using (var frm = new HakkindaForm())
            {
                frm.ShowDialog(this);
            }
        }

        private void TsmCikis_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TsmLoglariTemizle_Click(object sender, EventArgs e)
        {
            _rtbLog.Clear();
        }

        #endregion

        #region Analiz İşlemi

        private void BtnAnalizEt_Click(object sender, EventArgs e)
        {
            if (_islemDevam) return;
            _bgwIslem.RunWorkerAsync("analiz");
        }

        private void AnalizCalistir(BackgroundWorker worker, DoWorkEventArgs e)
        {
            // 1. DB bağlantı testi
            worker.ReportProgress(0, "Veritabanı bağlantısı test ediliyor...");
            _dbService = new MikroDbService();
            if (!_dbService.BaglantıTest(out string hataMesaji))
            {
                _log.Hata($"Veritabanına bağlanılamadı: {hataMesaji}");
                return;
            }
            _log.Basari("Veritabanı bağlantısı başarılı.");

            // 2. XML parse
            worker.ReportProgress(20, "E-Defter XML dosyaları okunuyor...");
            _log.Bilgi($"Klasör: {KlasorYolu}");

            var parser = new EdDefterXmlParser();
            _defterler = parser.KlasordenOku(KlasorYolu, SicilNo, dosya => _log.Basari(dosya));

            // 3. Analiz
            worker.ReportProgress(50, "Analiz yapılıyor...");
            var analyzer = new DefterAnalyzer();

            // Özet
            var ozet = analyzer.OzetHesapla(_defterler);
            _log.Bilgi("══════════════════════════════════════════");
            _log.Bilgi("         E-DEFTER ANALİZ RAPORU           ");
            _log.Bilgi("══════════════════════════════════════════");
            foreach (var dosyaOzet in ozet.DosyaOzetleri)
            {
                _log.Bilgi($"  Dosya   : {dosyaOzet.DosyaAdi}");
                _log.Bilgi($"  Firma   : {dosyaOzet.FirmaUnvani} ({dosyaOzet.SicilNo})");
                _log.Bilgi($"  Dönem   : {dosyaOzet.DonemBaslangic:dd.MM.yyyy} - {dosyaOzet.DonemBitis:dd.MM.yyyy}");
                _log.Bilgi($"  Fiş/Satır: {dosyaOzet.FisSayisi:N0} / {dosyaOzet.SatirSayisi:N0}");
                _log.Bilgi(string.Empty);
            }
            _log.Basari($"TOPLAM: {ozet.DosyaSayisi} dosya, {ozet.ToplamFis:N0} fiş, {ozet.ToplamSatir:N0} satır");

            // Borç-Alacak denge kontrolü
            worker.ReportProgress(70, "Borç-Alacak denge kontrolü...");
            var dengesizler = analyzer.DengeKontrolu(_defterler);
            if (dengesizler.Count == 0)
            {
                _log.Basari("Tüm fişler dengeli. Borç = Alacak");
            }
            else
            {
                _log.Uyari($"{dengesizler.Count} adet dengesiz fiş tespit edildi!");
                foreach (var d in dengesizler)
                {
                    _log.Uyari($"  Yevmiye: {d.YevmiyeNo} (#{d.YevmiyeNoSayac}) — Borç: {d.ToplamBorc:N2} Alacak: {d.ToplamAlacak:N2} Fark: {d.Fark:N2}");
                }
            }

            // DB mevcut durum
            var ilkDefter = _defterler[0];
            var dbDurum = analyzer.DbDurumGetir(_dbService, ilkDefter.DonemBaslangic, ilkDefter.DonemBitis, ilkDefter.MaliYilBaslangic.Year);
            _log.Bilgi($"DB Hesap Planı: {dbDurum.HesapSayisi:N0} kayıt");
            _log.Bilgi($"DB Mevcut Fişler: {dbDurum.FisSayisi:N0} yevmiye ({dbDurum.DonemBaslangic:dd.MM.yyyy} - {dbDurum.DonemBitis:dd.MM.yyyy})");

            // Eksik hesap planı
            worker.ReportProgress(85, "Hesap planı kontrolü...");
            HashSet<string> mevcutKodlar = _dbService.TumHesapKodlariniGetir();
            _log.Bilgi($"DB'de mevcut hesap sayısı: {mevcutKodlar.Count:N0}");

            var eksikHesaplar = analyzer.EksikHesaplariGetir(_defterler, mevcutKodlar);
            if (eksikHesaplar.Count == 0)
            {
                _log.Basari("Tüm hesaplar hesap planında mevcut. Eksik hesap yok.");
            }
            else
            {
                _log.Uyari($"{eksikHesaplar.Count} eksik hesap tespit edildi:");
                foreach (var eksik in eksikHesaplar)
                {
                    string tipAd = eksik.HesapTip == 0 ? "Ana" : eksik.HesapTip == 1 ? "Alt" : $"D{eksik.HesapTip}";
                    _log.Uyari($"  {eksik.HesapKod,-20} {tipAd,-5} {eksik.HesapIsim}");
                }
            }

            // Sonuç
            worker.ReportProgress(100, "Analiz tamamlandı.");
            _log.Bilgi("══════════════════════════════════════════");
            _log.Basari($"ANALİZ TAMAMLANDI — Dengesiz: {dengesizler.Count}, Eksik hesap: {eksikHesaplar.Count}");
            _log.Bilgi("══════════════════════════════════════════");
        }

        #endregion

        #region Mevcut Veri Kontrol (Pre-check)

        private void BtnMevcutVeriKontrol_Click(object sender, EventArgs e)
        {
            if (_islemDevam) return;
            _bgwIslem.RunWorkerAsync("precheck");
        }

        private void PrecheckCalistir(BackgroundWorker worker, DoWorkEventArgs e)
        {
            worker.ReportProgress(0, "Veritabanı kontrol ediliyor...");

            if (_dbService == null)
            {
                _dbService = new MikroDbService();
            }

            if (!_dbService.BaglantıTest(out string hataMesaji))
            {
                _log.Hata($"Veritabanına bağlanılamadı: {hataMesaji}");
                return;
            }

            if (_defterler == null || _defterler.Count == 0)
            {
                _log.Uyari("Önce 'Analiz Et' ile XML dosyalarını okuyun.");
                return;
            }

            var ilkDefter = _defterler[0];
            int maliYil = ilkDefter.MaliYilBaslangic.Year;
            DateTime donemBas = ilkDefter.DonemBaslangic;
            DateTime donemBit = ilkDefter.DonemBitis;

            worker.ReportProgress(30, "Dönem verileri sorgulanıyor...");
            _log.Bilgi($"Dönem: {donemBas:dd.MM.yyyy} - {donemBit:dd.MM.yyyy}, Mali Yıl: {maliYil}");

            var mevcutVeriler = _dbService.DonemVerileriGetir(maliYil, donemBas, donemBit, FirmaNo, SubeNo);

            worker.ReportProgress(100, "Kontrol tamamlandı.");

            if (mevcutVeriler.Count == 0)
            {
                _log.Basari("Bu dönemde mevcut fiş verisi yok. İşleme devam edilebilir.");
            }
            else
            {
                int toplamSatir = mevcutVeriler.Sum(v => v.SatirSayisi);
                _log.Uyari($"Bu dönemde {mevcutVeriler.Count} yevmiye fişi ({toplamSatir:N0} satır) mevcut!");
                _log.Bilgi("Mevcut dönem verisi 'Mevcut Veri' sekmesinde listeleniyor...");

                // DataGridView'e yükle (UI thread'de)
                BeginInvoke(new Action(() =>
                {
                    MevcutVeriGridYukle(mevcutVeriler);
                    _tabControl.SelectedTab = _tabMevcutVeri;
                }));
            }
        }

        private void MevcutVeriGridYukle(List<DonemFisOzeti> veriler)
        {
            _dgvMevcutVeri.DataSource = null;
            _dgvMevcutVeri.Columns.Clear();

            var bs = new BindingSource();
            bs.DataSource = veriler;
            _dgvMevcutVeri.DataSource = bs;

            // Kolon başlıklarını Türkçe yap
            if (_dgvMevcutVeri.Columns.Count > 0)
            {
                _dgvMevcutVeri.Columns["YevmiyeNo"].HeaderText = "Yevmiye No";
                _dgvMevcutVeri.Columns["Tarih"].HeaderText = "Tarih";
                _dgvMevcutVeri.Columns["Tarih"].DefaultCellStyle.Format = "dd.MM.yyyy";
                _dgvMevcutVeri.Columns["SatirSayisi"].HeaderText = "Satır Sayısı";
                _dgvMevcutVeri.Columns["ToplamBorc"].HeaderText = "Toplam Borç";
                _dgvMevcutVeri.Columns["ToplamBorc"].DefaultCellStyle.Format = "N2";
                _dgvMevcutVeri.Columns["ToplamAlacak"].HeaderText = "Toplam Alacak";
                _dgvMevcutVeri.Columns["ToplamAlacak"].DefaultCellStyle.Format = "N2";
                _dgvMevcutVeri.Columns["Aciklama"].HeaderText = "Açıklama";

                // Kolon genişlikleri
                _dgvMevcutVeri.Columns["YevmiyeNo"].Width = 90;
                _dgvMevcutVeri.Columns["Tarih"].Width = 90;
                _dgvMevcutVeri.Columns["SatirSayisi"].Width = 80;
                _dgvMevcutVeri.Columns["ToplamBorc"].Width = 110;
                _dgvMevcutVeri.Columns["ToplamAlacak"].Width = 110;
                _dgvMevcutVeri.Columns["Aciklama"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            _lblMevcutVeriOzet.Text = $"Toplam: {veriler.Count} yevmiye, {veriler.Sum(v => v.SatirSayisi):N0} satır";
            _btnDonemVerisiSil.Enabled = veriler.Count > 0;
        }

        private void BtnDonemVerisiSil_Click(object sender, EventArgs e)
        {
            if (_islemDevam) return;
            if (_defterler == null || _defterler.Count == 0) return;

            var ilkDefter = _defterler[0];
            string donemStr = $"{ilkDefter.DonemBaslangic:dd.MM.yyyy} - {ilkDefter.DonemBitis:dd.MM.yyyy}";

            var sonuc = MessageBox.Show(
                $"DİKKAT: {donemStr} dönemine ait tüm muhasebe fişleri (mahsup) silinecektir!\n\n" +
                "Bu işlem geri alınamaz. Devam etmek istiyor musunuz?",
                "Dönem Verisi Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (sonuc != DialogResult.Yes) return;

            // İkinci onay (çift güvenlik)
            var sonuc2 = MessageBox.Show(
                "Son onay: Veriler kalıcı olarak silinecek.\n\nEmin misiniz?",
                "Son Onay",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2);

            if (sonuc2 != DialogResult.Yes) return;

            _bgwIslem.RunWorkerAsync("sil");
        }

        private void SilmeCalistir(BackgroundWorker worker, DoWorkEventArgs e)
        {
            worker.ReportProgress(0, "Dönem verisi siliniyor...");

            var ilkDefter = _defterler[0];
            int maliYil = ilkDefter.MaliYilBaslangic.Year;

            try
            {
                int silinenSatir = _dbService.DonemVerileriSil(
                    maliYil,
                    ilkDefter.DonemBaslangic,
                    ilkDefter.DonemBitis,
                    FirmaNo,
                    SubeNo);

                worker.ReportProgress(100, "Silme tamamlandı.");
                _log.Basari($"{silinenSatir:N0} fiş satırı başarıyla silindi.");

                // Grid'i temizle
                BeginInvoke(new Action(() =>
                {
                    _dgvMevcutVeri.DataSource = null;
                    _lblMevcutVeriOzet.Text = "Dönem verisi silindi.";
                    _btnDonemVerisiSil.Enabled = false;
                }));
            }
            catch (Exception ex)
            {
                _log.Hata($"Silme işlemi başarısız (rollback yapıldı): {ex.Message}");
            }
        }

        #endregion

        #region BackgroundWorker

        private void BgwIslem_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            string islem = (string)e.Argument;

            switch (islem)
            {
                case "analiz":
                    AnalizCalistir(worker, e);
                    break;
                case "precheck":
                    PrecheckCalistir(worker, e);
                    break;
                case "sil":
                    SilmeCalistir(worker, e);
                    break;
            }
        }

        private void BgwIslem_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progressBar.Value = Math.Min(e.ProgressPercentage, 100);
            if (e.UserState is string durum)
            {
                _lblDurum.Text = durum;
            }
        }

        private void BgwIslem_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _islemDevam = false;
            IslemDurumuAyarla(false);

            if (e.Error != null)
            {
                _log.Hata($"İşlem hatası: {e.Error.Message}");
                _lblDurum.Text = "Hata oluştu!";
                _progressBar.Value = 0;
            }
            else
            {
                _lblDurum.Text = "Hazır";
            }
        }

        private void IslemDurumuAyarla(bool calisiyor)
        {
            _islemDevam = calisiyor;
            _btnAnalizEt.Enabled = !calisiyor;
            _btnMevcutVeriKontrol.Enabled = !calisiyor;
            _btnDonemVerisiSil.Enabled = !calisiyor && _dgvMevcutVeri.RowCount > 0;
        }

        #endregion

        #region Form Olayları

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_islemDevam)
            {
                var sonuc = MessageBox.Show(
                    "İşlem devam ediyor. Çıkmak istiyor musunuz?",
                    "Çıkış Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (sonuc != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            base.OnFormClosing(e);
        }

        #endregion
    }
}
