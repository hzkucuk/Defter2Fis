using System;
using System.Text.RegularExpressions;

namespace Defter2Fis.ForMikro.Services
{
    /// <summary>
    /// E-Defter açıklamalarından evrak seri ve sıra bilgilerini parse eder.
    /// Mikro ERP evrak formatlarını tanır.
    /// </summary>
    public class EvrakBilgisiParser
    {
        // Ortak Mikro e-Defter açıklama formatları:
        // "SERİ:A NO:000123", "SERI:A NO:000123"
        // "A-000123", "A 000123", "A/000123"
        // "EvrakSeri:A EvrakNo:123"
        // "EVRAK SERİ: A EVRAK NO: 000123"
        // ETTN: "TA4202500000642", "GB3202500809960" (e-Fatura/e-Arşiv)
        // e-Defter açıklama: "Al.fat. : TA4202500000642/31.05.2025/..."

        private static readonly Regex PatternSeriNo = new Regex(
            @"(?:SER[İI]\s*[:=]\s*)([A-Za-z0-9]{1,5})\s+(?:NO|SIRA)\s*[:=]\s*(\d+)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex PatternEvrakSeriNo = new Regex(
            @"(?:EVRAK\s*SER[İI]\s*[:=]\s*)([A-Za-z0-9]{1,5})\s+(?:EVRAK\s*NO\s*[:=]\s*)(\d+)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex PatternSeriTireSira = new Regex(
            @"\b([A-Za-z]{1,5})[-/](\d{3,10})\b",
            RegexOptions.Compiled);

        private static readonly Regex PatternSeriBoşlukSira = new Regex(
            @"\b([A-Za-z]{1,3})\s+(\d{4,10})\b",
            RegexOptions.Compiled);

        // ETTN/e-Fatura belge numarası: 3 karakter prefix + 4 haneli yıl + 5+ haneli sıra
        // Ör: TA4202500000642 → Seri=TA4, Sıra=642
        private static readonly Regex PatternEttn = new Regex(
            @"^([A-Za-z]\w{2})(\d{4})(\d{5,})$",
            RegexOptions.Compiled);

        // e-Defter açıklama formatı: <EvrakTipi> : <BelgeNo>/<GG.AA.YYYY>/...
        // Ör: "Al.fat. : TA4202500000642/31.05.2025/...", "C.A.V.D : 6933/31.05.2025/..."
        private static readonly Regex PatternEdDefterAciklama = new Regex(
            @":\s*([A-Za-z0-9]+)/\d{2}\.\d{2}\.\d{4}",
            RegexOptions.Compiled);

        /// <summary>
        /// E-Defter fiş satırından evrak bilgisini parse eder.
        /// Sırasıyla: DetayAciklama, BelgeNo, BelgeReferansi alanlarından çıkarmaya çalışır.
        /// </summary>
        public EvrakBilgisi Parse(string detayAciklama, string belgeNo, string belgeReferansi)
        {
            // 1. Önce detayAciklama'dan dene (en zengin bilgi)
            var sonuc = AciklamadanParse(detayAciklama);
            if (sonuc != null)
                return sonuc;

            // 2. BelgeNo'dan dene (documentNumber)
            sonuc = BelgeNodanParse(belgeNo);
            if (sonuc != null)
                return sonuc;

            // 3. BelgeReferansi'ndan dene (documentReference)
            sonuc = BelgeNodanParse(belgeReferansi);
            if (sonuc != null)
                return sonuc;

            // 4. BelgeNo sadece sayı ise, seri boş + sıra olarak al
            if (!string.IsNullOrWhiteSpace(belgeNo))
            {
                string temiz = belgeNo.Trim();
                if (int.TryParse(temiz, out int sira) && sira > 0)
                {
                    return new EvrakBilgisi(string.Empty, sira);
                }
            }

            return null;
        }

        /// <summary>
        /// Açıklama metninden evrak seri ve sıra bilgisi çıkarır.
        /// </summary>
        private EvrakBilgisi AciklamadanParse(string metin)
        {
            if (string.IsNullOrWhiteSpace(metin))
                return null;

            // Pattern 1: "SERİ:A NO:000123" veya "SERI:A NO:000123"
            Match match = PatternSeriNo.Match(metin);
            if (match.Success)
            {
                return OlusturEvrak(match.Groups[1].Value, match.Groups[2].Value);
            }

            // Pattern 2: "EVRAK SERİ: A EVRAK NO: 000123"
            match = PatternEvrakSeriNo.Match(metin);
            if (match.Success)
            {
                return OlusturEvrak(match.Groups[1].Value, match.Groups[2].Value);
            }

            // Pattern 3: "A-000123" veya "A/000123"
            match = PatternSeriTireSira.Match(metin);
            if (match.Success)
            {
                return OlusturEvrak(match.Groups[1].Value, match.Groups[2].Value);
            }

            // Pattern 4: "A 000123" (seri + boşluk + sıra)
            match = PatternSeriBoşlukSira.Match(metin);
            if (match.Success)
            {
                return OlusturEvrak(match.Groups[1].Value, match.Groups[2].Value);
            }

            // Pattern 5: e-Defter açıklama: "<EvrakTipi> : <BelgeNo>/<GG.AA.YYYY>/..."
            match = PatternEdDefterAciklama.Match(metin);
            if (match.Success)
            {
                string belgeNoYakalanan = match.Groups[1].Value;

                // Sayısal ise doğrudan sıra olarak al
                if (int.TryParse(belgeNoYakalanan, out int sira) && sira > 0)
                {
                    return new EvrakBilgisi(string.Empty, sira);
                }

                // ETTN formatı ise prefix + yıl + sıra olarak parse et
                Match ettnMatch = PatternEttn.Match(belgeNoYakalanan);
                if (ettnMatch.Success)
                {
                    return OlusturEvrak(ettnMatch.Groups[1].Value, ettnMatch.Groups[3].Value);
                }
            }

            return null;
        }

        /// <summary>
        /// BelgeNo alanından evrak bilgisi çıkarır.
        /// </summary>
        private EvrakBilgisi BelgeNodanParse(string belgeNo)
        {
            if (string.IsNullOrWhiteSpace(belgeNo))
                return null;

            string temiz = belgeNo.Trim();

            // "A-000123" veya "A/000123"
            Match match = PatternSeriTireSira.Match(temiz);
            if (match.Success)
            {
                return OlusturEvrak(match.Groups[1].Value, match.Groups[2].Value);
            }

            // "A 000123"
            match = PatternSeriBoşlukSira.Match(temiz);
            if (match.Success)
            {
                return OlusturEvrak(match.Groups[1].Value, match.Groups[2].Value);
            }

            // "A000123" — tek harf seri + kalan sayı
            if (temiz.Length > 1 && char.IsLetter(temiz[0]))
            {
                string olasıSira = temiz.Substring(1);
                if (int.TryParse(olasıSira, out int sira) && sira > 0)
                {
                    return new EvrakBilgisi(temiz.Substring(0, 1).ToUpperInvariant(), sira);
                }
            }

            // ETTN/e-Fatura: 3-char prefix + yıl + sıra
            // Ör: TA4202500000642 → Seri=TA4, Sıra=642
            Match matchEttn = PatternEttn.Match(temiz);
            if (matchEttn.Success)
            {
                return OlusturEvrak(matchEttn.Groups[1].Value, matchEttn.Groups[3].Value);
            }

            return null;
        }

        /// <summary>
        /// Seri ve sıra string değerlerinden EvrakBilgisi oluşturur.
        /// </summary>
        private EvrakBilgisi OlusturEvrak(string seri, string siraStr)
        {
            if (int.TryParse(siraStr, out int sira) && sira > 0)
            {
                return new EvrakBilgisi(seri.Trim().ToUpperInvariant(), sira);
            }

            return null;
        }
    }

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
