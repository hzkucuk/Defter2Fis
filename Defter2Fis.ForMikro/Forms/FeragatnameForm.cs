using System;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace Defter2Fis.ForMikro.Forms
{
    /// <summary>
    /// Uygulama başlangıcında kullanıcıya gösterilen hukuki feragatname ve
    /// sorumluluk reddi formu. Kabul edilmeden uygulama çalışmaz.
    /// </summary>
    public partial class FeragatnameForm : KryptonForm
    {
        public FeragatnameForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Feragatname metnini döndürür. Hukuki kanıt niteliğinde resmi dil kullanılır.
        /// </summary>
        internal static string FeragatnameMetni =>
@"DEFTER2FİS YAZILIMI
KULLANIM KOŞULLARI, SORUMLULUK REDDİ VE FERAGATNAMESİ

Yayımcı: Zafer Bilgisayar
Son Güncelleme: " + DateTime.Now.ToString("dd.MM.yyyy") + @"

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

MADDE 1 — TANIMLAR

1.1. ""Yazılım"": Defter2Fis adlı, E-Defter (XBRL-GL formatı) Yevmiye XML dosyalarını parse ederek Mikro ERP V16 Jump MSSQL veritabanına muhasebe fişleri oluşturan masaüstü uygulamasını ifade eder.
1.2. ""Yayımcı"": Zafer Bilgisayar unvanı altında yazılımı geliştiren ve dağıtan tarafı ifade eder.
1.3. ""Kullanıcı"": Bu yazılımı indiren, kuran, çalıştıran veya herhangi bir şekilde kullanan gerçek veya tüzel kişiyi ifade eder.
1.4. ""Mikro ERP Veritabanı"": Kullanıcının Mikro ERP V16 Jump sistemine ait MSSQL veritabanını ifade eder.
1.5. ""E-Defter Verileri"": GİB (Gelir İdaresi Başkanlığı) tarafından onaylanmış XBRL-GL formatındaki e-Defter Yevmiye XML dosyalarını ifade eder.

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

MADDE 2 — YAZILIMIN İŞLEVİ VE KAPSAMI

2.1. Yazılım, yalnızca E-Defter Yevmiye XML dosyalarını OKUMA (read-only) işlemi gerçekleştirir. E-Defter XML dosyaları üzerinde HİÇBİR ŞEKİLDE yazma, değiştirme, silme veya herhangi bir modifikasyon işlemi YAPMAZ ve YAPAMAZ.
2.2. Yazılım, parse edilen verileri Mikro ERP veritabanının MUHASEBE_FISLERI ve MUHASEBE_HESAP_PLANI tablolarına YAZMA işlemi gerçekleştirir.
2.3. Yazılım, Mikro ERP veritabanı üzerinde INSERT ve DELETE işlemleri yapar. Bu işlemler geri alınamaz (irreversible) nitelikte olabilir.

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

MADDE 3 — VERİTABANI YEDEKLEMESİ (KRİTİK UYARI)

3.1. Kullanıcı, yazılımı kullanmadan ÖNCE Mikro ERP veritabanının MUTLAKA tam bir yedeğini (Full Backup) almakla YÜKÜMLÜDÜR.
3.2. Yedek alınmadan yapılan işlemlerden doğacak her türlü veri kaybı, veri bozulması veya veri tutarsızlığından YALNIZCA KULLANICI sorumludur.
3.3. Yazılım, fiş oluşturma işlemi öncesinde yedek alma teklifi sunmakta olup bu teklif bilgilendirme amaçlıdır; yedek alınıp alınmaması tamamen kullanıcının inisiyatifindedir.
3.4. Yayımcı, yedek alınmadan gerçekleştirilen işlemler sonucu oluşan zararlardan HİÇBİR KOŞULDA sorumlu tutulamaz.

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

MADDE 4 — SORUMLULUK REDDİ

4.1. Yazılım ""OLDUĞU GİBİ"" (AS-IS) sunulmaktadır. Yayımcı, yazılımın hatasız, kesintisiz veya belirli bir amaca uygun çalışacağına dair açık veya zımni HİÇBİR GARANTİ vermez.
4.2. Kullanıcı, yazılımı kendi iradesi ve sorumluluğu altında kullanır. Yazılımın kullanımından doğabilecek:
   a) Doğrudan veya dolaylı mali kayıplar,
   b) Veri kaybı, veri bozulması veya veri tutarsızlığı,
   c) Muhasebe kayıtlarındaki hatalar veya eksiklikler,
   d) Vergi, SGK veya diğer resmi kurumlara karşı doğabilecek cezai ve/veya idari yaptırımlar,
   e) Üçüncü kişilere karşı doğabilecek her türlü zarar
konularında YALNIZCA KULLANICI sorumludur.
4.3. Yayımcı, yukarıda sayılan veya sayılmayan hiçbir zarar türünden dolayı tazminat, zarar-ziyan veya sair nam altında HİÇBİR ÖDEME yükümlülüğü altında değildir.
4.4. Kullanıcı, yazılımın oluşturduğu muhasebe fişlerini Mikro ERP üzerinden KONTROL ETMEKle ve doğruluğunu TEYİT ETMEKle yükümlüdür.

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

MADDE 5 — E-DEFTER VERİLERİNİN KORUNMASI

5.1. Yazılım, GİB onaylı E-Defter XML dosyalarını SALT OKUNUR (read-only) modda işler.
5.2. E-Defter XML dosyaları üzerinde hiçbir yazma, değiştirme, silme, taşıma veya yeniden adlandırma işlemi YAPILMAZ.
5.3. E-Defter dosyalarının bütünlüğü ve güvenliği yazılım tarafından korunur; ancak kullanıcının dosya sistemi üzerinde yapacağı işlemler yazılımın sorumluluğunda değildir.

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

MADDE 6 — FİKRİ MÜLKİYET

6.1. Yazılımın tüm fikri mülkiyet hakları Yayımcıya aittir.
6.2. Kullanıcı, yazılımı tersine mühendislik (reverse engineering), kaynak kodu çıkarma (decompilation), değiştirme veya türev eser oluşturma hakkına sahip değildir.

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

MADDE 7 — KABUL BEYANI

7.1. Kullanıcı, ""Kabul Ediyorum"" butonuna tıklayarak:
   a) Bu feragatnamenin tüm maddelerini okuduğunu ve anladığını,
   b) Veritabanı yedeği almanın kendi sorumluluğunda olduğunu,
   c) Yazılımın kullanımından doğabilecek tüm riskleri üstlendiğini,
   d) Yayımcıyı her türlü sorumluluktan gayri kabili rücu ibra ettiğini,
   e) Bu kabul işleminin tarih, saat ve makine bilgileriyle birlikte kayıt altına alınacağını
KABUL VE BEYAN eder.

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

MADDE 8 — UYGULANACAK HUKUK VE YETKİLİ MAHKEME

8.1. Bu feragatname Türkiye Cumhuriyeti hukukuna tabidir.
8.2. Uyuşmazlıklarda İstanbul Mahkemeleri ve İcra Daireleri yetkilidir.

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

© " + DateTime.Now.Year + @" Zafer Bilgisayar — Tüm hakları saklıdır.";

        private void ChkKabulEdiyorum_CheckedChanged(object sender, EventArgs e)
        {
            _btnKabulEt.Enabled = _chkKabulEdiyorum.Checked;
        }

        private void BtnKabulEt_Click(object sender, EventArgs e)
        {
            if (!_chkKabulEdiyorum.Checked)
            {
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnReddet_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _rtbFeragatname.Text = FeragatnameMetni;
            _btnKabulEt.Enabled = false;
        }
    }
}
