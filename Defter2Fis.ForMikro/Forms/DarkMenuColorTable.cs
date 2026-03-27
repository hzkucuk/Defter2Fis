using System.Drawing;
using System.Windows.Forms;

namespace Defter2Fis.ForMikro.Forms
{
    /// <summary>
    /// MenuStrip ve ToolStrip için dark tema renk tablosu.
    /// ToolStripProfessionalRenderer ile kullanılır.
    /// </summary>
    internal sealed class DarkMenuColorTable : ProfessionalColorTable
    {
        private static readonly Color MenuBg = Color.FromArgb(45, 45, 48);
        private static readonly Color BorderColor = Color.FromArgb(63, 63, 70);
        private static readonly Color ItemSelected = Color.FromArgb(62, 62, 64);
        private static readonly Color ItemPressed = Color.FromArgb(27, 27, 28);
        private static readonly Color CheckBg = Color.FromArgb(51, 51, 52);
        private static readonly Color SeparatorColor = Color.FromArgb(63, 63, 70);
        private static readonly Color ImageMarginBg = Color.FromArgb(45, 45, 48);

        public override Color MenuStripGradientBegin => MenuBg;
        public override Color MenuStripGradientEnd => MenuBg;
        public override Color ToolStripDropDownBackground => MenuBg;
        public override Color MenuItemSelected => ItemSelected;
        public override Color MenuItemSelectedGradientBegin => ItemSelected;
        public override Color MenuItemSelectedGradientEnd => ItemSelected;
        public override Color MenuItemPressedGradientBegin => ItemPressed;
        public override Color MenuItemPressedGradientEnd => ItemPressed;
        public override Color MenuItemPressedGradientMiddle => ItemPressed;
        public override Color MenuBorder => BorderColor;
        public override Color MenuItemBorder => BorderColor;
        public override Color ImageMarginGradientBegin => ImageMarginBg;
        public override Color ImageMarginGradientMiddle => ImageMarginBg;
        public override Color ImageMarginGradientEnd => ImageMarginBg;
        public override Color SeparatorDark => SeparatorColor;
        public override Color SeparatorLight => SeparatorColor;
        public override Color CheckBackground => CheckBg;
        public override Color CheckPressedBackground => CheckBg;
        public override Color CheckSelectedBackground => CheckBg;
        public override Color StatusStripGradientBegin => Color.FromArgb(0, 122, 204);
        public override Color StatusStripGradientEnd => Color.FromArgb(0, 122, 204);
    }
}
