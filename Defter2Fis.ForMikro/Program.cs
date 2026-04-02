using System;
using System.Threading;
using System.Windows.Forms;
using Defter2Fis.ForMikro.Forms;
using Defter2Fis.ForMikro.Services;

namespace Defter2Fis.ForMikro
{
    /// <summary>
    /// Defter2Fis — E-Defter XML'den Mikro ERP Muhasebe Fişi Oluşturucu
    /// WinForms uygulama giriş noktası.
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // UI thread yakalanmayan hataları
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Tüm thread'lerdeki yakalanmayan hatalar
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Feragatname kontrolü — kabul edilmemişse göster
            if (!DisclaimerService.FeragatnameKabulEdildiMi())
            {
                using (var feragatnameForm = new FeragatnameForm())
                {
                    if (feragatnameForm.ShowDialog() != DialogResult.OK)
                    {
                        // Kullanıcı reddettiğinde uygulama çıkışı
                        return;
                    }

                    // Kabul kanıtını Registry + dosyaya kaydet
                    DisclaimerService.KabulKaydet();
                }
            }

            Application.Run(new MainForm());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                string mesaj = $"Beklenmeyen bir hata olu\u015ftu:\n\n{e.Exception.Message}\n\n" +
                    "Uygulama \u00e7al\u0131\u015fmaya devam edecek. L\u00fctfen i\u015fleminizi tekrar deneyin.";

                MessageBox.Show(mesaj, "Defter2Fis — Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                // Son savunma hattı — MessageBox bile başarısız olursa sessizce devam et
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = e.ExceptionObject as Exception;
                string mesaj = ex != null
                    ? $"Kritik hata olu\u015ftu:\n\n{ex.Message}\n\nUygulama kapat\u0131lacak."
                    : "Bilinmeyen kritik hata olu\u015ftu. Uygulama kapat\u0131lacak.";

                MessageBox.Show(mesaj, "Defter2Fis — Kritik Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch
            {
                // Son savunma hattı
            }
        }
    }
}
