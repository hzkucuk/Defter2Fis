using System;

namespace Defter2Fis.ForMikro.Models
{
    /// <summary>
    /// Log mesaj seviyesi.
    /// </summary>
    public enum LogSeviye
    {
        Bilgi,
        Basari,
        Uyari,
        Hata
    }

    /// <summary>
    /// Log mesajı event argümanı.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        public string Mesaj { get; }
        public LogSeviye Seviye { get; }
        public DateTime Zaman { get; }

        public LogEventArgs(string mesaj, LogSeviye seviye)
        {
            Mesaj = mesaj ?? string.Empty;
            Seviye = seviye;
            Zaman = DateTime.Now;
        }

        /// <summary>
        /// Formatlanmış log satırı.
        /// </summary>
        public string Formatla()
        {
            string seviyeStr;
            switch (Seviye)
            {
                case LogSeviye.Basari: seviyeStr = "OK"; break;
                case LogSeviye.Uyari: seviyeStr = "UYARI"; break;
                case LogSeviye.Hata: seviyeStr = "HATA"; break;
                default: seviyeStr = "BİLGİ"; break;
            }

            return $"[{Zaman:HH:mm:ss}] [{seviyeStr}] {Mesaj}";
        }
    }
}
