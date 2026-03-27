using System;
using System.Reflection;
using System.Windows.Forms;

namespace Defter2Fis.ForMikro.Forms
{
    /// <summary>
    /// Hakkında iletişim kutusu.
    /// </summary>
    public partial class HakkindaForm : Form
    {
        public HakkindaForm()
        {
            InitializeComponent();
            YukleVersiyonBilgileri();
        }

        private void YukleVersiyonBilgileri()
        {
            var asm = Assembly.GetExecutingAssembly();
            var versiyon = asm.GetName().Version;
            _lblVersiyon.Text = $"Versiyon: {versiyon.Major}.{versiyon.Minor}.{versiyon.Build}";
        }

        private void BtnTamam_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
