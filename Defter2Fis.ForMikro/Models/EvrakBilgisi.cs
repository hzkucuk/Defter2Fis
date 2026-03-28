namespace Defter2Fis.ForMikro.Models
{
    /// <summary>
    /// Parse edilmiş evrak seri ve sıra bilgisi.
    /// </summary>
    public class EvrakBilgisi
    {
        /// <summary>Evrak serisi (ör: "A", "B1")</summary>
        public string Seri { get; }

        /// <summary>Evrak sıra numarası (ör: 123)</summary>
        public int Sira { get; }

        public EvrakBilgisi(string seri, int sira)
        {
            Seri = seri ?? string.Empty;
            Sira = sira;
        }

        /// <summary>Eşleştirme için anahtar: "SERİ-SIRA" formatı</summary>
        public string Anahtar
        {
            get
            {
                return string.IsNullOrEmpty(Seri) ? Sira.ToString() : $"{Seri}-{Sira}";
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Seri) ? Sira.ToString() : $"{Seri} {Sira}";
        }
    }
}
