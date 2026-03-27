using System;
using System.Collections.Generic;
using System.Linq;
using Defter2Fis.ForMikro.Models;

namespace Defter2Fis.ForMikro.Services
{
    /// <summary>
    /// E-Defter verileri üzerinde analiz, doğrulama ve raporlama.
    /// Fiş oluşturma öncesi ön kontrol amaçlı kullanılır.
    /// </summary>
    public class DefterAnalyzer
    {
        /// <summary>
        /// Defterlerin genel özetini hesaplar.
        /// </summary>
        public AnalizOzeti OzetHesapla(List<YevmiyeDefteri> defterler)
        {
            var ozet = new AnalizOzeti();

            foreach (var defter in defterler)
            {
                int satirSayisi = defter.Fisler.Sum(f => f.Satirlar.Count);
                ozet.DosyaSayisi++;
                ozet.ToplamFis += defter.Fisler.Count;
                ozet.ToplamSatir += satirSayisi;

                ozet.DosyaOzetleri.Add(new DosyaOzeti
                {
                    DosyaAdi = defter.DosyaAdi,
                    BenzersizId = defter.BenzersizId,
                    FirmaUnvani = defter.FirmaUnvani,
                    SicilNo = defter.SicilNo,
                    DonemBaslangic = defter.DonemBaslangic,
                    DonemBitis = defter.DonemBitis,
                    FisSayisi = defter.Fisler.Count,
                    SatirSayisi = satirSayisi
                });
            }

            return ozet;
        }

        /// <summary>
        /// Tüm fişlerdeki benzersiz hesap kodlarını (Ana + Alt) toplar.
        /// </summary>
        public HashSet<string> BenzersizHesapKodlari(List<YevmiyeDefteri> defterler)
        {
            var kodlar = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var defter in defterler)
            {
                foreach (var fis in defter.Fisler)
                {
                    foreach (var satir in fis.Satirlar)
                    {
                        // Ana hesap kodu ekle (ör: 770)
                        if (!string.IsNullOrWhiteSpace(satir.AnaHesapKod))
                            kodlar.Add(satir.AnaHesapKod);

                        // Alt hesap kodu ekle (ör: 770.50.0018)
                        if (!string.IsNullOrWhiteSpace(satir.AltHesapKod))
                            kodlar.Add(satir.AltHesapKod);

                        // Ara seviye kodları da ekle (ör: 770.50)
                        if (!string.IsNullOrWhiteSpace(satir.AltHesapKod))
                        {
                            var araKodlar = AraSeviyeleriUret(satir.AltHesapKod);
                            foreach (string araKod in araKodlar)
                                kodlar.Add(araKod);
                        }
                    }
                }
            }

            return kodlar;
        }

        /// <summary>
        /// Hesap kodunun ara seviyelerini üretir.
        /// Ör: "770.50.0018" → ["770", "770.50"]
        /// </summary>
        public List<string> AraSeviyeleriUret(string hesapKod)
        {
            var sonuc = new List<string>();
            if (string.IsNullOrWhiteSpace(hesapKod)) return sonuc;

            string[] parcalar = hesapKod.Split('.');
            string current = string.Empty;

            // Son parça hariç tüm ara seviyeleri ekle
            for (int i = 0; i < parcalar.Length - 1; i++)
            {
                current = i == 0 ? parcalar[i] : current + "." + parcalar[i];
                sonuc.Add(current);
            }

            return sonuc;
        }

        /// <summary>
        /// E-Defter'de olan ama DB'de olmayan hesap kodlarını tespit eder.
        /// Hesap adlarını da eşleştirir.
        /// </summary>
        public List<EksikHesap> EksikHesaplariTespit(
            List<YevmiyeDefteri> defterler,
            HashSet<string> mevcutHesapKodlari)
        {
            // Önce tüm hesap kod-isim eşleştirmesini topla
            var hesapIsimMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var defter in defterler)
            {
                foreach (var fis in defter.Fisler)
                {
                    foreach (var satir in fis.Satirlar)
                    {
                        if (!string.IsNullOrWhiteSpace(satir.AnaHesapKod) &&
                            !hesapIsimMap.ContainsKey(satir.AnaHesapKod))
                        {
                            hesapIsimMap[satir.AnaHesapKod] = satir.AnaHesapAd ?? string.Empty;
                        }

                        if (!string.IsNullOrWhiteSpace(satir.AltHesapKod) &&
                            !hesapIsimMap.ContainsKey(satir.AltHesapKod))
                        {
                            hesapIsimMap[satir.AltHesapKod] = satir.AltHesapAd ?? string.Empty;
                        }
                    }
                }
            }

            // E-Defter'deki tüm benzersiz kodlar
            var edDefterKodlar = BenzersizHesapKodlari(defterler);

            var eksikler = new List<EksikHesap>();

            foreach (string kod in edDefterKodlar.OrderBy(k => k))
            {
                if (!mevcutHesapKodlari.Contains(kod))
                {
                    string isim = hesapIsimMap.ContainsKey(kod) ? hesapIsimMap[kod] : string.Empty;

                    // Ara seviye ise ve isim yoksa, üst hesap adından türet
                    if (string.IsNullOrEmpty(isim))
                    {
                        isim = "(Ara seviye)";
                    }

                    eksikler.Add(new EksikHesap
                    {
                        HesapKod = kod,
                        HesapIsim = isim,
                        HesapTip = MuhasebeHesapPlani.HesapTipBelirle(kod)
                    });
                }
            }

            return eksikler;
        }

        /// <summary>
        /// Eksik hesap raporunu döner (UI tarafında gösterilir).
        /// </summary>
        public List<EksikHesap> EksikHesaplariGetir(
            List<YevmiyeDefteri> defterler,
            HashSet<string> mevcutHesapKodlari)
        {
            return EksikHesaplariTespit(defterler, mevcutHesapKodlari);
        }

        /// <summary>
        /// Borç-Alacak dengesini doğrular (her fiş için Borç = Alacak olmalı).
        /// </summary>
        public List<DengeSizFis> DengeKontrolu(List<YevmiyeDefteri> defterler)
        {
            var dengesizler = new List<DengeSizFis>();

            foreach (var defter in defterler)
            {
                foreach (var fis in defter.Fisler)
                {
                    decimal toplamBorc = 0;
                    decimal toplamAlacak = 0;

                    foreach (var satir in fis.Satirlar)
                    {
                        if (satir.BorcAlacakKodu == "D")
                            toplamBorc += satir.Tutar;
                        else if (satir.BorcAlacakKodu == "C")
                            toplamAlacak += satir.Tutar;
                    }

                    // 0.01 tolerans (kuruş farkları)
                    if (Math.Abs(toplamBorc - toplamAlacak) > 0.01m)
                    {
                        dengesizler.Add(new DengeSizFis
                        {
                            DosyaAdi = defter.DosyaAdi,
                            YevmiyeNo = fis.YevmiyeNo,
                            YevmiyeNoSayac = fis.YevmiyeNoSayac,
                            ToplamBorc = toplamBorc,
                            ToplamAlacak = toplamAlacak,
                            Fark = toplamBorc - toplamAlacak
                        });
                    }
                }
            }

            return dengesizler;
        }

        /// <summary>
        /// DB mevcut durum istatistiğini döner.
        /// </summary>
        public DbDurumBilgisi DbDurumGetir(IMikroDbService dbService, DateTime donemBas, DateTime donemBit, int maliYil)
        {
            return new DbDurumBilgisi
            {
                HesapSayisi = dbService.HesapPlaniSayisi(),
                FisSayisi = dbService.MevcutFisSayisi(maliYil, donemBas, donemBit, 0, 0),
                DonemBaslangic = donemBas,
                DonemBitis = donemBit
            };
        }
    }

    /// <summary>
    /// Analiz özet bilgisi.
    /// </summary>
    public class AnalizOzeti
    {
        public int DosyaSayisi { get; set; }
        public int ToplamFis { get; set; }
        public int ToplamSatir { get; set; }
        public List<DosyaOzeti> DosyaOzetleri { get; } = new List<DosyaOzeti>();
    }

    /// <summary>
    /// Tek dosya özet bilgisi.
    /// </summary>
    public class DosyaOzeti
    {
        public string DosyaAdi { get; set; }
        public string BenzersizId { get; set; }
        public string FirmaUnvani { get; set; }
        public string SicilNo { get; set; }
        public DateTime DonemBaslangic { get; set; }
        public DateTime DonemBitis { get; set; }
        public int FisSayisi { get; set; }
        public int SatirSayisi { get; set; }
    }

    /// <summary>
    /// DB mevcut durum bilgisi.
    /// </summary>
    public class DbDurumBilgisi
    {
        public int HesapSayisi { get; set; }
        public int FisSayisi { get; set; }
        public DateTime DonemBaslangic { get; set; }
        public DateTime DonemBitis { get; set; }
    }

    /// <summary>
    /// Eksik hesap bilgisi.
    /// </summary>
    public class EksikHesap
    {
        public string HesapKod { get; set; }
        public string HesapIsim { get; set; }
        public byte HesapTip { get; set; }
    }

    /// <summary>
    /// Borç-Alacak dengesiz fiş bilgisi.
    /// </summary>
    public class DengeSizFis
    {
        public string DosyaAdi { get; set; }
        public string YevmiyeNo { get; set; }
        public int YevmiyeNoSayac { get; set; }
        public decimal ToplamBorc { get; set; }
        public decimal ToplamAlacak { get; set; }
        public decimal Fark { get; set; }
    }
}
