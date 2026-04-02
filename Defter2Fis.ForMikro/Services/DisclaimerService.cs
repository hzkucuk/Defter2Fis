using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace Defter2Fis.ForMikro.Services
{
    /// <summary>
    /// Feragatname kabul kanıtı yönetim servisi.
    /// Kabul bilgisini Windows Registry ve gizli dosyaya SHA256 imza ile kaydeder.
    /// Bu kayıtlar hukuki kanıt niteliğindedir.
    /// </summary>
    internal static class DisclaimerService
    {
        // Registry yolu — HKCU altında, admin yetkisi gerektirmez
        private const string RegistryKeyPath = @"SOFTWARE\PrimaHavacilik\Defter2Fis";
        private const string RegistryValueName = "DisclaimerAccepted";

        // Gizli dosya — ProgramData altında sadece bizim bildiğimiz konum
        private static readonly string EvidenceDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                          "PrimaHavacilik", "Defter2Fis", "Legal");

        private static readonly string EvidenceFilePath =
            Path.Combine(EvidenceDirectory, ".d2f_acceptance.sig");

        // İmza tuzlama anahtarı — sadece kaynak kodda bilinen gizli değer
        private const string Salt = "D2F-Prima-2025-Legal-Salt-X7k9Qm";

        /// <summary>
        /// Kullanıcının feragatnameyi daha önce kabul edip etmediğini kontrol eder.
        /// Hem Registry hem de dosya kanıtının mevcut ve geçerli olması gerekir.
        /// </summary>
        internal static bool FeragatnameKabulEdildiMi()
        {
            try
            {
                bool registryGecerli = RegistryKanıtıDogrula();
                bool dosyaGecerli = DosyaKanıtıDogrula();

                return registryGecerli && dosyaGecerli;
            }
            catch
            {
                // Herhangi bir hata durumunda kabul edilmemiş say — tekrar göster
                return false;
            }
        }

        /// <summary>
        /// Feragatname kabulünü Registry ve dosyaya imza ile kaydeder.
        /// </summary>
        internal static void KabulKaydet()
        {
            string makineAdi = Environment.MachineName;
            string kullaniciAdi = Environment.UserName;
            string kabulTarihi = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string versiyon = System.Reflection.Assembly.GetExecutingAssembly()
                                    .GetName().Version.ToString();

            string kabulBilgisi = string.Join("|", makineAdi, kullaniciAdi, kabulTarihi, versiyon);
            string imza = ImzaOlustur(kabulBilgisi);
            string kayitIcerigi = kabulBilgisi + "|" + imza;

            RegistryKaydet(kayitIcerigi);
            DosyaKaydet(kayitIcerigi);
        }

        /// <summary>
        /// SHA256 + Salt ile kabul bilgisi imzası oluşturur.
        /// </summary>
        private static string ImzaOlustur(string veri)
        {
            string imzaGirdisi = Salt + "|" + veri + "|" + Salt;

            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(imzaGirdisi));
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>
        /// Kabul bilgisini Windows Registry'ye yazar (HKCU).
        /// </summary>
        private static void RegistryKaydet(string kayitIcerigi)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
            {
                if (key != null)
                {
                    key.SetValue(RegistryValueName, kayitIcerigi, RegistryValueKind.String);
                }
            }
        }

        /// <summary>
        /// Kabul bilgisini gizli dosyaya yazar.
        /// </summary>
        private static void DosyaKaydet(string kayitIcerigi)
        {
            if (!Directory.Exists(EvidenceDirectory))
            {
                Directory.CreateDirectory(EvidenceDirectory);
            }

            File.WriteAllText(EvidenceFilePath, kayitIcerigi, Encoding.UTF8);

            // Dosyayı gizli yap
            File.SetAttributes(EvidenceFilePath, FileAttributes.Hidden | FileAttributes.System);
        }

        /// <summary>
        /// Registry'deki kabul kanıtının bütünlüğünü doğrular.
        /// </summary>
        private static bool RegistryKanıtıDogrula()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                if (key == null)
                {
                    return false;
                }

                string kayitIcerigi = key.GetValue(RegistryValueName) as string;
                return KayitBütünlügünüDogrula(kayitIcerigi);
            }
        }

        /// <summary>
        /// Dosyadaki kabul kanıtının bütünlüğünü doğrular.
        /// </summary>
        private static bool DosyaKanıtıDogrula()
        {
            if (!File.Exists(EvidenceFilePath))
            {
                return false;
            }

            string kayitIcerigi = File.ReadAllText(EvidenceFilePath, Encoding.UTF8);
            return KayitBütünlügünüDogrula(kayitIcerigi);
        }

        /// <summary>
        /// Kayıt içeriğindeki imzanın geçerliliğini kontrol eder.
        /// Format: MakineAdı|KullanıcıAdı|Tarih|Versiyon|SHA256İmza
        /// </summary>
        private static bool KayitBütünlügünüDogrula(string kayitIcerigi)
        {
            if (string.IsNullOrWhiteSpace(kayitIcerigi))
            {
                return false;
            }

            int sonAyiracIndex = kayitIcerigi.LastIndexOf('|');
            if (sonAyiracIndex <= 0)
            {
                return false;
            }

            string veri = kayitIcerigi.Substring(0, sonAyiracIndex);
            string mevcutImza = kayitIcerigi.Substring(sonAyiracIndex + 1);
            string beklenenImza = ImzaOlustur(veri);

            return string.Equals(mevcutImza, beklenenImza, StringComparison.OrdinalIgnoreCase);
        }
    }
}
