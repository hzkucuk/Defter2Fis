using System;
using Defter2Fis.ForMikro.Models;

namespace Defter2Fis.ForMikro.Services
{
    /// <summary>
    /// Merkezi log servisi. Servisler ve UI arasında gevşek bağlantılı log iletişimi sağlar.
    /// </summary>
    public class LogService
    {
        /// <summary>
        /// Yeni log mesajı geldiğinde tetiklenir.
        /// </summary>
        public event EventHandler<LogEventArgs> LogEklendi;

        /// <summary>
        /// Bilgi seviyesinde log yazar.
        /// </summary>
        public void Bilgi(string mesaj)
        {
            LogYaz(mesaj, LogSeviye.Bilgi);
        }

        /// <summary>
        /// Başarı seviyesinde log yazar.
        /// </summary>
        public void Basari(string mesaj)
        {
            LogYaz(mesaj, LogSeviye.Basari);
        }

        /// <summary>
        /// Uyarı seviyesinde log yazar.
        /// </summary>
        public void Uyari(string mesaj)
        {
            LogYaz(mesaj, LogSeviye.Uyari);
        }

        /// <summary>
        /// Hata seviyesinde log yazar.
        /// </summary>
        public void Hata(string mesaj)
        {
            LogYaz(mesaj, LogSeviye.Hata);
        }

        private void LogYaz(string mesaj, LogSeviye seviye)
        {
            LogEklendi?.Invoke(this, new LogEventArgs(mesaj, seviye));
        }
    }
}
