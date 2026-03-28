using System.Collections.Generic;

namespace Defter2Fis.ForMikro.Models
{
    /// <summary>
    /// Fiş oluşturma sonuç bilgisi.
    /// </summary>
    public class OlusturmaSonucu
    {
        public int OlusturulanFisSayisi { get; set; }
        public int OlusturulanSatirSayisi { get; set; }
        public int EklenenHesapSayisi { get; set; }
        public int AtlananFisSayisi { get; set; }
        public bool SimulasyonBasarili { get; set; }
        public List<string> Hatalar { get; } = new List<string>();
        public bool Basarili { get; set; }
    }
}
