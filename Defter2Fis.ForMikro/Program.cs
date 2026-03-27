using System;
using System.Windows.Forms;
using Defter2Fis.ForMikro.Forms;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
