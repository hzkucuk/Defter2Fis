using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Krypton.Toolkit;
using Defter2Fis.ForMikro.Models;
using Defter2Fis.ForMikro.Services;

namespace Defter2Fis.ForMikro.Forms
{
    /// <summary>
    /// Defter2Fis ana uygulama formu (Krypton temalI).
    /// E-Defter analiz, mevcut veri kontrol, onizleme ve fis olusturma islemlerini yonetir.
    /// </summary>
    public partial class MainForm : KryptonForm
    {
        private readonly LogService _log = new LogService();
        private IMikroDbService _dbService;
        private List<YevmiyeDefteri> _defterler;
        private OnizlemeSonucu _sonOnizleme;
        private SureklilkKontrolSonucu _sureklilkSonucu;
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

        #region Ayar Ozeti

        private void AyarOzetiniGuncelle()
        {
            _lblEdDefterYolu.Values.Text = $"E-Defter: {KlasorYolu}";
            _lblDbBilgi.Values.Text = $"DB: {ConfigurationManager.ConnectionStrings["MikroDB"]?.ConnectionString ?? "?"}";
            _lblFirmaBilgi.Values.Text = $"Sicil: {SicilNo}  |  Mali YIl: {MaliYilAraligi}  |  Ay: {AyKlasoru}  |  Firma: {FirmaNo}/{SubeNo}  DBC: {DBCNo}";
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
                case LogSeviye.Basari: renk = Color.LimeGreen; break;
                case LogSeviye.Uyari: renk = Color.Orange; break;
                case LogSeviye.Hata: renk = Color.Tomato; break;
                default: renk = Color.FromArgb(220, 220, 220); break;
            }

            var rtb = _rtbLog.RichTextBox;
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.SelectionColor = renk;
            rtb.AppendText(e.Formatla() + Environment.NewLine);
            rtb.ScrollToCaret();
        }

        #endregion

        #region Menu Olaylari

        private void TsmAyarlar_Click(object sender, EventArgs e)
        {
            using (var frm = new AyarlarForm())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    AyarOzetiniGuncelle();
                    _log.Bilgi("Ayarlar guncellendi.");
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
            _rtbLog.RichTextBox.Clear();
        }

        #endregion

        #region Analiz Islemi

        private void BtnAnalizEt_Click(object sender, EventArgs e)
        {
            if (_islemDevam) return;
            IslemDurumuAyarla(true);
            _bgwIslem.RunWorkerAsync("analiz");
        }

        private void AnalizCalistir(BackgroundWorker worker, DoWorkEventArgs e)
        {
            worker.ReportProgress(0, "Veritabani baglantisi test ediliyor...");
            _dbService = new MikroDbService();
            if (!_dbService.BaglantıTest(out string hataMesaji))
            {
                _log.Hata($"Veritabanina baglanamadi: {hataMesaji}");
                return;
            }
            _log.Basari("Veritabani baglantisi basarili.");

            worker.ReportProgress(20, "E-Defter XML dosyalari okunuyor...");
            _log.Bilgi($"Klasor: {KlasorYolu}");
            var parser = new EdDefterXmlParser();
            _defterler = parser.KlasordenOku(KlasorYolu, SicilNo, dosya => _log.Basari(dosya));

            worker.ReportProgress(50, "Analiz yapiliyor...");
            var analyzer = new DefterAnalyzer();

            // Önceki ayın XML'ini sessiz oku (varsa)
            List<YevmiyeDefteri> oncekiAyDefterler = null;
            int calislanAyNo = int.TryParse(AyKlasoru, out int ayNo) ? ayNo : 0;
            if (calislanAyNo > 1)
            {
                string oncekiAyKlasoru = (calislanAyNo - 1).ToString("D2");
                string oncekiAyYolu = Path.Combine(EdDefterRootPath, SicilNo, MaliYilAraligi, oncekiAyKlasoru);

                if (Directory.Exists(oncekiAyYolu))
                {
                    try
                    {
                        oncekiAyDefterler = parser.KlasordenOku(oncekiAyYolu, SicilNo);
                    }
                    catch { /* süreklilik bölümünde uyarı verilecek */ }
                }
            }

            // Tek rapor bloğu: önceki ay + çalışılan ay alt alta
            _log.Bilgi("===== E-DEFTER ANALIZ RAPORU =====");

            if (oncekiAyDefterler != null && oncekiAyDefterler.Count > 0)
            {
                var oncekiOzet = analyzer.OzetHesapla(oncekiAyDefterler);
                var oncekiFisler = oncekiAyDefterler.SelectMany(d => d.Fisler).ToList();

                _log.Bilgi("--- Onceki Ay ---");
                foreach (var dosyaOzet in oncekiOzet.DosyaOzetleri)
                {
                    _log.Bilgi($"  Dosya   : {dosyaOzet.DosyaAdi}");
                    _log.Bilgi($"  Firma   : {dosyaOzet.FirmaUnvani} ({dosyaOzet.SicilNo})");
                    _log.Bilgi($"  Donem   : {dosyaOzet.DonemBaslangic:dd.MM.yyyy} - {dosyaOzet.DonemBitis:dd.MM.yyyy}");
                    _log.Bilgi($"  Fis/Satir: {dosyaOzet.FisSayisi:N0} / {dosyaOzet.SatirSayisi:N0}");
                }
                if (oncekiFisler.Count > 0)
                {
                    int oncekiMin = oncekiFisler.Min(f => f.YevmiyeNoSayac);
                    int oncekiMax = oncekiFisler.Max(f => f.YevmiyeNoSayac);
                    _log.Bilgi($"  Yevmiye : {oncekiMin} - {oncekiMax}");
                }
                _log.Bilgi(string.Empty);
            }
            else if (calislanAyNo > 1)
            {
                string oncekiAyKlasoruStr = (calislanAyNo - 1).ToString("D2");
                _log.Uyari($"--- Onceki Ay ({oncekiAyKlasoruStr}) ---");
                _log.Uyari("  Onceki ay E-Defter XML dosyasi bulunamadi veya okunamadi.");
                _log.Bilgi(string.Empty);
            }

            var ozet = analyzer.OzetHesapla(_defterler);
            var calislanFisler = _defterler.SelectMany(d => d.Fisler).ToList();

            _log.Bilgi("--- Calisilan Ay ---");
            foreach (var dosyaOzet in ozet.DosyaOzetleri)
            {
                _log.Bilgi($"  Dosya   : {dosyaOzet.DosyaAdi}");
                _log.Bilgi($"  Firma   : {dosyaOzet.FirmaUnvani} ({dosyaOzet.SicilNo})");
                _log.Bilgi($"  Donem   : {dosyaOzet.DonemBaslangic:dd.MM.yyyy} - {dosyaOzet.DonemBitis:dd.MM.yyyy}");
                _log.Bilgi($"  Fis/Satir: {dosyaOzet.FisSayisi:N0} / {dosyaOzet.SatirSayisi:N0}");
            }
            if (calislanFisler.Count > 0)
            {
                int calislanMin = calislanFisler.Min(f => f.YevmiyeNoSayac);
                int calislanMax = calislanFisler.Max(f => f.YevmiyeNoSayac);
                _log.Bilgi($"  Yevmiye : {calislanMin} - {calislanMax}");
            }
            _log.Bilgi(string.Empty);
            _log.Basari($"TOPLAM: {ozet.DosyaSayisi} dosya, {ozet.ToplamFis:N0} fis, {ozet.ToplamSatir:N0} satir");

            worker.ReportProgress(70, "Borc-Alacak denge kontrolu...");
            var dengesizler = analyzer.DengeKontrolu(_defterler);
            if (dengesizler.Count == 0)
                _log.Basari("Tum fisler dengeli. Borc = Alacak");
            else
            {
                _log.Uyari($"{dengesizler.Count} adet dengesiz fis tespit edildi!");
                foreach (var d in dengesizler)
                    _log.Uyari($"  Yevmiye: {d.YevmiyeNo} (#{d.YevmiyeNoSayac}) — Borc: {d.ToplamBorc:N2} Alacak: {d.ToplamAlacak:N2} Fark: {d.Fark:N2}");
            }

            var ilkDefter = _defterler[0];
            var dbDurum = analyzer.DbDurumGetir(_dbService, ilkDefter.DonemBaslangic, ilkDefter.DonemBitis, ilkDefter.MaliYilBaslangic.Year);
            _log.Bilgi($"DB Hesap Plani: {dbDurum.HesapSayisi:N0} kayit");
            _log.Bilgi($"DB Mevcut Fisler: {dbDurum.FisSayisi:N0} yevmiye ({dbDurum.DonemBaslangic:dd.MM.yyyy} - {dbDurum.DonemBitis:dd.MM.yyyy})");

            worker.ReportProgress(85, "Hesap plani kontrolu...");
            HashSet<string> mevcutKodlar = _dbService.TumHesapKodlariniGetir();
            _log.Bilgi($"DB mevcut hesap sayisi: {mevcutKodlar.Count:N0}");

            var eksikHesaplar = analyzer.EksikHesaplariGetir(_defterler, mevcutKodlar);
            if (eksikHesaplar.Count == 0)
                _log.Basari("Tum hesaplar hesap planinda mevcut.");
            else
            {
                _log.Uyari($"{eksikHesaplar.Count} eksik hesap tespit edildi:");
                foreach (var eksik in eksikHesaplar)
                {
                    string tipAd = eksik.HesapTip == 0 ? "Ana" : eksik.HesapTip == 1 ? "Alt" : $"D{eksik.HesapTip}";
                    _log.Uyari($"  {eksik.HesapKod,-20} {tipAd,-5} {eksik.HesapIsim}");
                }
            }

            // Önceki ay doğrulama ve yevmiye sürekliliği kontrolü
            worker.ReportProgress(90, "Onceki ay ve yevmiye surekliligi kontrol ediliyor...");
            _log.Bilgi(string.Empty);
            _log.Bilgi("===== ONCEKI AY & YEVMIYE SUREKLILIGI =====");

            _sureklilkSonucu = analyzer.OncekiAyDogrula(_dbService, _defterler, FirmaNo, SubeNo);

            foreach (string mesaj in _sureklilkSonucu.Mesajlar)
            {
                if (mesaj.StartsWith("HATA:"))
                    _log.Hata(mesaj);
                else if (mesaj.StartsWith("UYARI:"))
                    _log.Uyari(mesaj);
                else
                    _log.Bilgi($"  {mesaj}");
            }

            if (_sureklilkSonucu.AktarimIzinli)
                _log.Basari("Yevmiye surekliligi kontrolu BASARILI — aktarim yapilabilir.");
            else
                _log.Hata("Yevmiye surekliligi kontrolu BASARISIZ — aktarim ENGELLENDI!");

            worker.ReportProgress(100, "Analiz tamamlandi.");
            string sureklilkDurum = _sureklilkSonucu.AktarimIzinli ? "IZINLI" : "ENGELLI";
            _log.Basari($"ANALIZ TAMAMLANDI — Dengesiz: {dengesizler.Count}, Eksik hesap: {eksikHesaplar.Count}, Aktarim: {sureklilkDurum}");

            e.Result = new IslemSonucBilgisi("analiz",
                $"Analiz tamamlandi — {ozet.ToplamFis:N0} fis, Eksik: {eksikHesaplar.Count}, Aktarim: {sureklilkDurum}");
        }

        #endregion

        #region Mevcut Veri Kontrol (Pre-check)

        private void BtnMevcutVeriKontrol_Click(object sender, EventArgs e)
        {
            if (_islemDevam) return;
            IslemDurumuAyarla(true);
            _bgwIslem.RunWorkerAsync("precheck");
        }

        private void PrecheckCalistir(BackgroundWorker worker, DoWorkEventArgs e)
        {
            worker.ReportProgress(0, "Veritabani kontrol ediliyor...");
            if (_dbService == null) _dbService = new MikroDbService();
            if (!_dbService.BaglantıTest(out string hataMesaji))
            {
                _log.Hata($"Veritabanina baglanamadi: {hataMesaji}");
                return;
            }

            if (_defterler == null || _defterler.Count == 0)
            {
                _log.Uyari("Once 'Analiz Et' ile XML dosyalarini okuyun.");
                return;
            }

            var ilkDefter = _defterler[0];
            int maliYil = ilkDefter.MaliYilBaslangic.Year;
            DateTime donemBas = ilkDefter.DonemBaslangic;
            DateTime donemBit = ilkDefter.DonemBitis;

            worker.ReportProgress(30, "Donem verileri sorgulaniyor...");
            _log.Bilgi($"Donem: {donemBas:dd.MM.yyyy} - {donemBit:dd.MM.yyyy}, Mali Yil: {maliYil}");
            var mevcutVeriler = _dbService.DonemVerileriGetir(maliYil, donemBas, donemBit, FirmaNo, SubeNo);
            worker.ReportProgress(100, "Kontrol tamamlandi.");

            if (mevcutVeriler.Count == 0)
            {
                _log.Basari("Bu donemde mevcut fis verisi yok.");
                e.Result = new IslemSonucBilgisi("precheck", "Donemde mevcut veri yok");
            }
            else
            {
                int toplamSatir = mevcutVeriler.Sum(v => v.SatirSayisi);
                _log.Uyari($"Bu donemde {mevcutVeriler.Count} yevmiye fisi ({toplamSatir:N0} satir) mevcut!");

                BeginInvoke(new Action(() =>
                {
                    MevcutVeriGridYukle(mevcutVeriler);
                    _tabControl.SelectedTab = _tabMevcutVeri;
                }));

                e.Result = new IslemSonucBilgisi("precheck",
                    $"{mevcutVeriler.Count} yevmiye ({toplamSatir:N0} satir) mevcut");
            }
        }

        private void MevcutVeriGridYukle(List<DonemFisOzeti> veriler)
        {
            _dgvMevcutVeri.DataSource = null;
            _dgvMevcutVeri.Columns.Clear();
            var bs = new BindingSource();
            bs.DataSource = veriler;
            _dgvMevcutVeri.DataSource = bs;

            if (_dgvMevcutVeri.Columns.Count > 0)
            {
                _dgvMevcutVeri.Columns["YevmiyeNo"].HeaderText = "Yevmiye No";
                _dgvMevcutVeri.Columns["Tarih"].HeaderText = "Tarih";
                _dgvMevcutVeri.Columns["Tarih"].DefaultCellStyle.Format = "dd.MM.yyyy";
                _dgvMevcutVeri.Columns["SatirSayisi"].HeaderText = "Satir Sayisi";
                _dgvMevcutVeri.Columns["ToplamBorc"].HeaderText = "Toplam Borc";
                _dgvMevcutVeri.Columns["ToplamBorc"].DefaultCellStyle.Format = "N2";
                _dgvMevcutVeri.Columns["ToplamAlacak"].HeaderText = "Toplam Alacak";
                _dgvMevcutVeri.Columns["ToplamAlacak"].DefaultCellStyle.Format = "N2";
                _dgvMevcutVeri.Columns["Aciklama"].HeaderText = "Aciklama";
                _dgvMevcutVeri.Columns["Aciklama"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            _lblMevcutVeriOzet.Values.Text = $"Toplam: {veriler.Count} yevmiye, {veriler.Sum(v => v.SatirSayisi):N0} satir";
            _btnDonemVerisiSil.Enabled = veriler.Count > 0;
        }

        private void BtnDonemVerisiSil_Click(object sender, EventArgs e)
        {
            if (_islemDevam) return;
            if (_defterler == null || _defterler.Count == 0) return;

            var ilkDefter = _defterler[0];
            string donemStr = $"{ilkDefter.DonemBaslangic:dd.MM.yyyy} - {ilkDefter.DonemBitis:dd.MM.yyyy}";

            var sonuc = MessageBox.Show(
                $"DIKKAT: {donemStr} donemine ait tum muhasebe fisleri silinecektir!\n\nBu islem geri alinamaz. Devam?",
                "Donem Verisi Silme Onayi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (sonuc != DialogResult.Yes) return;

            var sonuc2 = MessageBox.Show("Son onay: Veriler kalici olarak silinecek.\n\nEmin misiniz?",
                "Son Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (sonuc2 != DialogResult.Yes) return;

            if (!YedekTeklifEt("Donem Verisi Silme"))
                return;

            IslemDurumuAyarla(true);
            _bgwIslem.RunWorkerAsync("sil");
        }

        private void SilmeCalistir(BackgroundWorker worker, DoWorkEventArgs e)
        {
            worker.ReportProgress(0, "Donem verisi siliniyor...");
            var ilkDefter = _defterler[0];
            int maliYil = ilkDefter.MaliYilBaslangic.Year;

            try
            {
                int silinenSatir = _dbService.DonemVerileriSil(maliYil, ilkDefter.DonemBaslangic, ilkDefter.DonemBitis, FirmaNo, SubeNo);
                worker.ReportProgress(100, "Silme tamamlandi.");
                _log.Basari($"{silinenSatir:N0} fis satiri basariyla silindi.");
                BeginInvoke(new Action(() =>
                {
                    _dgvMevcutVeri.DataSource = null;
                    _lblMevcutVeriOzet.Values.Text = "Donem verisi silindi.";
                    _btnDonemVerisiSil.Enabled = false;
                }));

                e.Result = new IslemSonucBilgisi("sil", $"{silinenSatir:N0} satir silindi");
            }
            catch (Exception ex)
            {
                _log.Hata($"Silme islemi basarisiz (rollback yapildi): {ex.Message}");
            }
        }

        #endregion

        #region Onizleme (Test Modulu)

        private void BtnOnizleme_Click(object sender, EventArgs e)
        {
            if (_islemDevam) return;
            if (_defterler == null || _defterler.Count == 0)
            {
                MessageBox.Show("Once 'Analiz Et' ile XML dosyalarini okuyun.",
                    "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            IslemDurumuAyarla(true);
            _bgwIslem.RunWorkerAsync("onizleme");
        }

        private void OnizlemeCalistir(BackgroundWorker worker, DoWorkEventArgs e)
        {
            worker.ReportProgress(0, "Onizleme baslatiliyor...");
            if (_dbService == null) _dbService = new MikroDbService();
            if (!_dbService.BaglantıTest(out string hataMesaji))
            {
                _log.Hata($"Veritabanina baglanamadi: {hataMesaji}");
                return;
            }

            var servis = new OnizlemeServisi(_dbService, _log);
            _sonOnizleme = servis.Calistir(_defterler, FirmaNo, SubeNo, DBCNo,
                (yuzde, durum) => worker.ReportProgress(yuzde, durum));

            worker.ReportProgress(100, "Onizleme tamamlandi.");

            BeginInvoke(new Action(() =>
            {
                OnizlemeGridleriYukle(_sonOnizleme);
                _tabControl.SelectedTab = _tabOnizleme;
            }));

            e.Result = new IslemSonucBilgisi("onizleme",
                $"{_sonOnizleme.OlusturulacakFisSayisi} fis, {_sonOnizleme.EslesenCariSayisi} cari, {_sonOnizleme.EslesenStokSayisi} stok");
        }

        private void OnizlemeGridleriYukle(OnizlemeSonucu oz)
        {
            // Ozet istatistikler
            _lblOzFisSayisi.Values.Text = $"Fis: {oz.OlusturulacakFisSayisi:N0}";
            _lblOzSatirSayisi.Values.Text = $"Satir: {oz.ToplamSatirSayisi:N0}";
            _lblOzCariEslesen.Values.Text = $"Cari: {oz.EslesenCariSayisi:N0} / {oz.ToplamCariHareket:N0}";
            _lblOzStokEslesen.Values.Text = $"Stok: {oz.EslesenStokSayisi:N0} / {oz.ToplamStokHareket:N0}";
            _lblOzMukerrer.Values.Text = $"Mukerrer: {oz.MukerrerSayisi:N0}";
            _lblOzEksikHesap.Values.Text = $"Eksik Hesap: {oz.EksikHesaplar.Count:N0}";

            // Fisler grid
            GridYukle(_dgvOzFisler, oz.FisKayitlari, col =>
            {
                col["YevmiyeNo"].HeaderText = "Yevmiye No";
                col["Tarih"].HeaderText = "Tarih";
                col["Tarih"].DefaultCellStyle.Format = "dd.MM.yyyy";
                col["SatirSayisi"].HeaderText = "Satir";
                col["ToplamBorc"].HeaderText = "Borc";
                col["ToplamBorc"].DefaultCellStyle.Format = "N2";
                col["ToplamAlacak"].HeaderText = "Alacak";
                col["ToplamAlacak"].DefaultCellStyle.Format = "N2";
                col["AtanacakSiraNo"].HeaderText = "Sira No";
                col["EslesmeDurumu"].HeaderText = "Esleme";
                col["EvrakSeri"].HeaderText = "Evrak Seri";
                col["EvrakSira"].HeaderText = "Evrak No";
                col["Mukerrer"].HeaderText = "Mukerrer";
                col["Aciklama"].HeaderText = "Aciklama";
                col["Aciklama"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            });

            // Mukerrer satirlari kirmizi yap
            foreach (DataGridViewRow row in _dgvOzFisler.Rows)
            {
                if (row.Cells["Mukerrer"].Value is bool m && m)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
            }

            // Cari grid
            GridYukle(_dgvOzCari, oz.CariEslesmeleri, col =>
            {
                col["YevmiyeNo"].HeaderText = "Yevmiye No";
                col["EvrakSeri"].HeaderText = "Evrak Seri";
                col["EvrakSira"].HeaderText = "Evrak No";
                col["CariHesapKod"].HeaderText = "Cari Hesap";
                col["IslemTarihi"].HeaderText = "Tarih";
                col["IslemTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
                col["MevcutMuhFisNo"].HeaderText = "Mevcut Fis No";
                col["AtanacakMuhFisNo"].HeaderText = "Yeni Fis No";
                col["UzerineYazilacak"].HeaderText = "Uzerine Yaz";
            });

            // Stok grid
            GridYukle(_dgvOzStok, oz.StokEslesmeleri, col =>
            {
                col["YevmiyeNo"].HeaderText = "Yevmiye No";
                col["EvrakSeri"].HeaderText = "Evrak Seri";
                col["EvrakSira"].HeaderText = "Evrak No";
                col["IslemTarihi"].HeaderText = "Tarih";
                col["IslemTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
                col["MevcutMuhFisNo"].HeaderText = "Mevcut Fis No";
                col["AtanacakMuhFisNo"].HeaderText = "Yeni Fis No";
                col["UzerineYazilacak"].HeaderText = "Uzerine Yaz";
            });

            // Eksik hesaplar grid
            GridYukle(_dgvOzHesap, oz.EksikHesaplar, col =>
            {
                col["HesapKod"].HeaderText = "Hesap Kodu";
                col["HesapIsim"].HeaderText = "Hesap Adi";
                col["HesapTip"].HeaderText = "Tip";
                col["TipAciklama"].HeaderText = "Tip Aciklama";
            });

            // Uyarilar grid
            GridYukle(_dgvOzUyari, oz.Uyarilar, col =>
            {
                col["SeviyeIkon"].HeaderText = "";
                col["SeviyeIkon"].Width = 30;
                col["Seviye"].HeaderText = "Seviye";
                col["Mesaj"].HeaderText = "Mesaj";
                col["Mesaj"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                if (col.Contains("YevmiyeNo"))
                    col["YevmiyeNo"].HeaderText = "Yevmiye No";
            });

            // Uyari satirlarini renklendir
            foreach (DataGridViewRow row in _dgvOzUyari.Rows)
            {
                if (row.Cells["Seviye"].Value is UyariSeviye sev)
                {
                    switch (sev)
                    {
                        case UyariSeviye.Kritik:
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
                            break;
                        case UyariSeviye.Uyari:
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);
                            break;
                    }
                }
            }

            // Tab basliklarini sayilarla guncelle
            _tabOzFisler.Text = $"Fisler ({oz.FisKayitlari.Count})";
            _tabOzCari.Text = $"Cari ({oz.CariEslesmeleri.Count})";
            _tabOzStok.Text = $"Stok ({oz.StokEslesmeleri.Count})";
            _tabOzHesap.Text = $"Eksik Hesap ({oz.EksikHesaplar.Count})";
            _tabOzUyari.Text = $"Uyarilar ({oz.Uyarilar.Count})";

            _log.Basari($"Onizleme: {oz.OlusturulacakFisSayisi} fis, {oz.EslesenCariSayisi} cari, {oz.EslesenStokSayisi} stok eslesmesi.");
        }

        private void GridYukle<T>(KryptonDataGridView dgv, List<T> veri, Action<DataGridViewColumnCollection> kolonAyarla)
        {
            dgv.DataSource = null;
            dgv.Columns.Clear();
            if (veri == null || veri.Count == 0) return;

            var bs = new BindingSource();
            bs.DataSource = veri;
            dgv.DataSource = bs;

            if (dgv.Columns.Count > 0)
                kolonAyarla(dgv.Columns);
        }

        #endregion

        #region Veritabani Yedekleme

        private void BtnYedekAl_Click(object sender, EventArgs e)
        {
            if (_islemDevam) return;
            IslemDurumuAyarla(true);
            _bgwIslem.RunWorkerAsync("yedek");
        }

        /// <summary>
        /// Kritik islemlerden once kullaniciya yedek teklif eder.
        /// </summary>
        /// <returns>true: islem devam etsin, false: kullanici iptal etti</returns>
        private bool YedekTeklifEt(string islemAdi)
        {
            var sonuc = MessageBox.Show(
                $"'{islemAdi}' islemi oncesinde veritabani yedegi almak ister misiniz?\n\n" +
                "Onerilen: Evet (geri alinamaz islemler icin guvenlik)",
                "Yedek Onerisi",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (sonuc == DialogResult.Cancel)
                return false;

            if (sonuc == DialogResult.Yes)
            {
                try
                {
                    _log.Bilgi("Islem oncesi otomatik yedek aliniyor...");
                    YedeklemeCalistirSenkron();
                    _log.Basari("Yedek basariyla alindi. Islem devam ediyor...");
                }
                catch (Exception ex)
                {
                    var devam = MessageBox.Show(
                        $"Yedek alma basarisiz: {ex.Message}\n\nYedeksiz devam etmek istiyor musunuz?",
                        "Yedek Hatasi",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2);

                    if (devam != DialogResult.Yes)
                        return false;
                }
            }

            return true;
        }

        private void YedeklemeCalistir(BackgroundWorker worker, DoWorkEventArgs e)
        {
            worker.ReportProgress(0, "Veritabani yedegi aliniyor...");

            if (_dbService == null) _dbService = new MikroDbService();
            if (!_dbService.BaglantıTest(out string hataMesaji))
            {
                _log.Hata($"Veritabanina baglanamadi: {hataMesaji}");
                return;
            }

            _log.Bilgi("BACKUP DATABASE komutu calistiriliyor...");

            var sonuc = _dbService.VeritabaniYedekle(
                ilerlemeCallback: (yuzde, durum) => worker.ReportProgress(yuzde, durum));

            _log.Basari($"Veritabani yedegi alindi:");
            _log.Bilgi($"  DB     : {sonuc.VeritabaniAdi}");
            _log.Bilgi($"  Dosya  : {sonuc.DosyaYolu}");
            _log.Bilgi($"  Boyut  : {sonuc.DosyaBoyutuFormatli}");
            _log.Bilgi($"  Sure   : {sonuc.Sure.TotalSeconds:N1} sn");

            e.Result = new IslemSonucBilgisi("yedek", $"Yedek alindi ({sonuc.DosyaBoyutuFormatli}, {sonuc.Sure.TotalSeconds:N1} sn)");
        }

        /// <summary>
        /// Senkron yedekleme (islem oncesi otomatik yedek icin).
        /// </summary>
        private void YedeklemeCalistirSenkron()
        {
            if (_dbService == null) _dbService = new MikroDbService();

            var sonuc = _dbService.VeritabaniYedekle(
                ilerlemeCallback: (yuzde, durum) => _log.Bilgi($"  Yedek: {durum}"));
            _log.Bilgi($"Yedek: {sonuc.DosyaYolu} ({sonuc.DosyaBoyutuFormatli}, {sonuc.Sure.TotalSeconds:N1} sn)");
        }

        #endregion

        #region Fis Olusturma

        private void BtnFisOlustur_Click(object sender, EventArgs e)
        {
            if (_islemDevam) return;
            if (_defterler == null || _defterler.Count == 0)
            {
                MessageBox.Show("Once 'Analiz Et' ile XML dosyalarini okuyun.",
                    "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Süreklilik kontrolü — analiz yapılmamışsa veya aktarım engelliyse
            if (_sureklilkSonucu == null)
            {
                MessageBox.Show(
                    "Once 'Analiz Et' ile onceki ay dogrulamasi yapilmalidir.",
                    "Analiz Gerekli", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_sureklilkSonucu.AktarimIzinli)
            {
                string engel = _sureklilkSonucu.OncekiAyMevcut
                    ? "Yevmiye numarasi surekliligi saglanamadi."
                    : "Onceki ay muhasebe fisleri DB'de bulunamadi.";

                MessageBox.Show(
                    $"Aktarim engellendi:\n\n{engel}\n\nOnce onceki ayin fislerini olusturun, sonra tekrar deneyin.",
                    "Aktarim Engellendi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var ilkDefter = _defterler[0];
            string donemStr = $"{ilkDefter.DonemBaslangic:dd.MM.yyyy} - {ilkDefter.DonemBitis:dd.MM.yyyy}";
            int toplamFis = _defterler.Sum(d => d.Fisler.Count);
            int toplamSatir = _defterler.Sum(d => d.Fisler.Sum(f => f.Satirlar.Count));

            string sureklilkBilgi = _sureklilkSonucu.IlkAy
                ? "\nYevmiye surekliligi: Ilk ay (kontrol gerekmiyor)"
                : $"\nYevmiye surekliligi: DB son={_sureklilkSonucu.DbMaxYevmiyeNo}, " +
                  $"bu ay ilk={_sureklilkSonucu.CalislanAyBilgisi.MinYevmiyeNo}";

            string onizlemeBilgi = _sonOnizleme != null
                ? $"\n\nOnizleme: {_sonOnizleme.EslesenCariSayisi} cari, {_sonOnizleme.EslesenStokSayisi} stok eslesmesi"
                : "\n\n(Onerilen: Once 'Onizleme' ile kontrol edin)";

            var sonuc = MessageBox.Show(
                $"Donem: {donemStr}\nOlusturulacak: {toplamFis:N0} fis, {toplamSatir:N0} satir" +
                $"{sureklilkBilgi}{onizlemeBilgi}" +
                $"\n\nOnce simulasyon yapilacak, sonra atomik yazim baslatilacak.\nDevam?",
                "Fis Olusturma Onayi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (sonuc != DialogResult.Yes) return;

            if (!YedekTeklifEt("Fis Olusturma"))
                return;

            IslemDurumuAyarla(true);
            _bgwIslem.RunWorkerAsync("fisolustur");
        }

        private void FisOlusturmaCalistir(BackgroundWorker worker, DoWorkEventArgs e)
        {
            worker.ReportProgress(0, "Fis olusturma baslatiliyor (simulasyon-once yaklasim)...");
            if (_dbService == null) _dbService = new MikroDbService();
            if (!_dbService.BaglantıTest(out string hataMesaji))
            {
                _log.Hata($"Veritabanina baglanamadi: {hataMesaji}");
                return;
            }

            var servis = new FisOlusturmaServisi(_dbService, _log);
            var sonuc = servis.FisleriOlustur(_defterler, FirmaNo, SubeNo, DBCNo,
                (yuzde, durum) => worker.ReportProgress(yuzde, durum));

            string durumMetni;
            if (!sonuc.SimulasyonBasarili)
                durumMetni = "Simulasyon basarisiz — DB'ye yazim yapilmadi.";
            else if (sonuc.Basarili)
                durumMetni = $"Fis olusturma tamamlandi ({sonuc.OlusturulanFisSayisi} fis, {sonuc.OlusturulanSatirSayisi} satir).";
            else
                durumMetni = "Atomik yazim basarisiz — tum ay ROLLBACK yapildi.";

            worker.ReportProgress(100, durumMetni);
            e.Result = new IslemSonucBilgisi("fisolustur", durumMetni);
        }

        #endregion

        #region BackgroundWorker

        private void BgwIslem_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            string islem = (string)e.Argument;

            switch (islem)
            {
                case "analiz": AnalizCalistir(worker, e); break;
                case "precheck": PrecheckCalistir(worker, e); break;
                case "sil": SilmeCalistir(worker, e); break;
                case "onizleme": OnizlemeCalistir(worker, e); break;
                case "fisolustur": FisOlusturmaCalistir(worker, e); break;
                case "yedek": YedeklemeCalistir(worker, e); break;
            }
        }

        private void BgwIslem_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progressBar.Value = Math.Min(e.ProgressPercentage, 100);
            if (e.UserState is string durum)
                _lblDurum.Text = durum;
        }

        private void BgwIslem_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _islemDevam = false;
            IslemDurumuAyarla(false);

            if (e.Error != null)
            {
                string detay = e.Error.InnerException != null
                    ? $"{e.Error.Message} -> {e.Error.InnerException.Message}"
                    : e.Error.Message;

                _log.Hata($"Islem hatasi: {detay}");
                _lblDurum.Text = "Hata olustu!";
                _progressBar.Value = 0;

                MessageBox.Show(
                    $"Islem sirasinda hata olustu:\n\n{detay}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Result is IslemSonucBilgisi sonuc)
            {
                _lblDurum.Text = $"✔ {sonuc.Ozet}";
                _log.Bilgi($"[TAMAMLANDI] {sonuc.Ozet}");

                // 2 saniye sonra progress bar'i sifirla
                var timer = new Timer { Interval = 2000 };
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    timer.Dispose();
                    if (!_islemDevam)
                    {
                        _progressBar.Value = 0;
                        _lblDurum.Text = "Hazir";
                    }
                };
                timer.Start();
            }
            else
            {
                _lblDurum.Text = "Hazir";
                _progressBar.Value = 0;
            }
        }

        private void IslemDurumuAyarla(bool calisiyor)
        {
            _islemDevam = calisiyor;
            _btnAnalizEt.Enabled = !calisiyor;
            _btnMevcutVeriKontrol.Enabled = !calisiyor;
            _btnOnizleme.Enabled = !calisiyor;
            _btnFisOlustur.Enabled = !calisiyor;
            _btnYedekAl.Enabled = !calisiyor;
            _btnDonemVerisiSil.Enabled = !calisiyor && _dgvMevcutVeri.RowCount > 0;
        }

        #endregion

        #region Form Olaylari

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_islemDevam)
            {
                var sonuc = MessageBox.Show("Islem devam ediyor. Cikmak istiyor musunuz?",
                    "Cikis Onayi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

    /// <summary>
    /// BackgroundWorker islem sonuc bilgisi (e.Result icin).
    /// </summary>
    internal sealed class IslemSonucBilgisi
    {
        public string IslemTipi { get; }
        public string Ozet { get; }

        public IslemSonucBilgisi(string islemTipi, string ozet)
        {
            IslemTipi = islemTipi;
            Ozet = ozet;
        }
    }
}

