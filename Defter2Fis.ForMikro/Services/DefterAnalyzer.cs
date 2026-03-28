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

        /// <summary>
        /// Yevmiye numarası bazlı süreklilik kontrolü yapar.
        /// DB'deki mevcut yevmiye numaralarının, çalışılan ayın ilk yevmiyesiyle
        /// boşluksuz devam ettiğini doğrular (tarihten bağımsız).
        /// </summary>
        /// <param name="dbService">DB servis arayüzü</param>
        /// <param name="defterler">Çalışılan ayın E-Defter verileri</param>
        /// <param name="firmaNo">Firma numarası</param>
        /// <param name="subeNo">Şube numarası</param>
        public SureklilkKontrolSonucu OncekiAyDogrula(
            IMikroDbService dbService,
            List<YevmiyeDefteri> defterler,
            int firmaNo,
            int subeNo)
        {
            if (dbService == null) throw new ArgumentNullException(nameof(dbService));
            if (defterler == null || defterler.Count == 0)
                throw new ArgumentException("Defter listesi boş.", nameof(defterler));

            var sonuc = new SureklilkKontrolSonucu();
            var ilkDefter = defterler[0];
            int maliYil = ilkDefter.MaliYilBaslangic.Year;
            DateTime donemBas = ilkDefter.DonemBaslangic;
            DateTime donemBit = ilkDefter.DonemBitis;

            // Çalışılan ayın E-Defter yevmiye bilgisi
            var tumFisler = defterler.SelectMany(d => d.Fisler).ToList();
            int calislanMinYevmiye = tumFisler.Min(f => f.YevmiyeNoSayac);
            int calislanMaxYevmiye = tumFisler.Max(f => f.YevmiyeNoSayac);

            // Çalışılan ayın E-Defter satır (lineNumber) bilgisi
            var tumSatirlar = tumFisler.SelectMany(f => f.Satirlar).ToList();
            int calislanMinSatirNo = tumSatirlar.Min(s => s.SatirNoSayac);
            int calislanMaxSatirNo = tumSatirlar.Max(s => s.SatirNoSayac);

            sonuc.XmlMinSatirNo = calislanMinSatirNo;
            sonuc.XmlMaxSatirNo = calislanMaxSatirNo;

            sonuc.CalislanAyBilgisi = new AyFisBilgisi
            {
                DonemBaslangic = donemBas,
                DonemBitis = donemBit,
                FisSayisi = tumFisler.Count,
                SatirSayisi = tumSatirlar.Count,
                MinYevmiyeNo = calislanMinYevmiye,
                MaxYevmiyeNo = calislanMaxYevmiye,
                MinTarih = donemBas,
                MaxTarih = donemBit,
                MinSatirNo = calislanMinSatirNo,
                MaxSatirNo = calislanMaxSatirNo
            };

            sonuc.Mesajlar.Add($"Çalışılan ay yevmiye aralığı: {calislanMinYevmiye} - {calislanMaxYevmiye} ({tumFisler.Count} fiş)");
            sonuc.Mesajlar.Add($"Çalışılan ay satır numarası (lineNumber) aralığı: {calislanMinSatirNo} - {calislanMaxSatirNo} ({tumSatirlar.Count} satır)");

            // İlk ay kontrolü: yevmiye 1'den başlıyorsa ilk aydır
            sonuc.IlkAy = calislanMinYevmiye == 1;

            if (sonuc.IlkAy)
            {
                sonuc.Mesajlar.Add("Mali yıl ilk dönemi — yevmiye 1'den başlıyor, önceki dönem kontrolü gerekmiyor.");
                sonuc.OncekiAyMevcut = true;
                sonuc.Surekli = true;
                sonuc.DbMaxYevmiyeNo = 0;
                sonuc.DbYevmiyeSayisi = 0;
                sonuc.DbToplamSatirSayisi = 0;

                // Satır numarası ilk ay 1'den başlamalı
                sonuc.SatirSurekli = calislanMinSatirNo == 1;
                if (sonuc.SatirSurekli)
                    sonuc.Mesajlar.Add($"Satır numarası sürekliliği OK: İlk ay, lineNumber 1'den başlıyor.");
                else
                    sonuc.Mesajlar.Add($"UYARI: İlk ay lineNumber 1'den başlamıyor! İlk satır numarası={calislanMinSatirNo}");

                // Çalışılan ay içi yevmiye süreklilik kontrolü
                var yevmiyeNolar = tumFisler.Select(f => f.YevmiyeNoSayac).OrderBy(n => n).ToList();
                bool icSurekli = YevmiyeIcSureklilkKontrol(yevmiyeNolar, sonuc.Mesajlar);

                // Çalışılan ay içi satır numarası süreklilik kontrolü
                var satirNolar = tumSatirlar.Select(s => s.SatirNoSayac).OrderBy(n => n).ToList();
                bool icSatirSurekli = SatirIcSureklilkKontrol(satirNolar, sonuc.Mesajlar);

                sonuc.AktarimIzinli = icSurekli && sonuc.SatirSurekli && icSatirSurekli;
                if (sonuc.AktarimIzinli)
                    sonuc.Mesajlar.Add("Tüm kontroller başarılı — aktarıma izin verildi.");
                else
                    sonuc.Mesajlar.Add("Aktarım ENGELLENDI — yevmiye veya satır numaralarında sorun tespit edildi.");

                return sonuc;
            }

            // Yevmiye numarası bazlı süreklilik sorgusu (tarihten bağımsız)
            var dbBilgi = dbService.YevmiyeSureklilkBilgisiGetir(maliYil, calislanMinYevmiye, firmaNo, subeNo);
            sonuc.DbMaxYevmiyeNo = dbBilgi.MaxYevmiyeNo;
            sonuc.DbYevmiyeSayisi = dbBilgi.YevmiyeSayisi;
            sonuc.OncekiAyMevcut = dbBilgi.VeriMevcut;

            // Satır numarası sürekliliği için DB toplam satır sayısı
            var satirBilgi = dbService.SatirSureklilkBilgisiGetir(maliYil, calislanMinYevmiye, firmaNo, subeNo);
            sonuc.DbToplamSatirSayisi = satirBilgi.ToplamSatirSayisi;

            // Bilgilendirme amaçlı önceki ay tarih bazlı bilgisi
            DateTime oncekiAyBas = donemBas.AddMonths(-1);
            DateTime oncekiAyBit = donemBas.AddDays(-1);
            sonuc.OncekiAyBilgisi = dbService.AyFisBilgisiGetir(maliYil, oncekiAyBas, oncekiAyBit, firmaNo, subeNo);

            if (!sonuc.OncekiAyMevcut)
            {
                sonuc.Surekli = false;
                sonuc.SatirSurekli = false;
                sonuc.AktarimIzinli = false;
                sonuc.Mesajlar.Add($"HATA: Yevmiye {calislanMinYevmiye} öncesinde DB'de hiç yevmiye bulunamadı!");
                sonuc.Mesajlar.Add("Aktarım yapılamaz — önce önceki dönemlerin fişleri oluşturulmalıdır.");
                return sonuc;
            }

            // DB bilgilerini logla
            sonuc.Mesajlar.Add($"DB'deki yevmiye sayısı (yevmiye < {calislanMinYevmiye}): {dbBilgi.YevmiyeSayisi:N0}");
            sonuc.Mesajlar.Add($"DB'deki son yevmiye no: {dbBilgi.MaxYevmiyeNo}");
            sonuc.Mesajlar.Add($"DB'deki toplam fiş satır sayısı (yevmiye < {calislanMinYevmiye}): {satirBilgi.ToplamSatirSayisi:N0}");

            // Yevmiye sürekliliği: DB'deki max yevmiye + 1 = çalışılan ayın ilk yevmiyesi
            int beklenenBaslangic = dbBilgi.MaxYevmiyeNo + 1;
            sonuc.Surekli = (calislanMinYevmiye == beklenenBaslangic);

            if (sonuc.Surekli)
            {
                sonuc.Mesajlar.Add($"Yevmiye sürekliliği OK: DB son={dbBilgi.MaxYevmiyeNo}, " +
                                   $"çalışılan ay ilk={calislanMinYevmiye}");
            }
            else
            {
                sonuc.Mesajlar.Add($"UYARI: Yevmiye sürekliliği BOZUK! " +
                                   $"Beklenen başlangıç={beklenenBaslangic}, gerçek={calislanMinYevmiye}");

                int beklenenSayisi = calislanMinYevmiye - 1;
                if (dbBilgi.YevmiyeSayisi < beklenenSayisi)
                {
                    sonuc.Mesajlar.Add($"UYARI: DB'de {beklenenSayisi - dbBilgi.YevmiyeSayisi} adet eksik yevmiye var " +
                                       $"(beklenen: {beklenenSayisi}, mevcut: {dbBilgi.YevmiyeSayisi})");
                }
            }

            // Satır numarası sürekliliği: DB toplam satır + 1 = çalışılan ayın ilk lineNumber'ı
            int beklenenSatirBaslangic = satirBilgi.ToplamSatirSayisi + 1;
            sonuc.SatirSurekli = (calislanMinSatirNo == beklenenSatirBaslangic);

            if (sonuc.SatirSurekli)
            {
                sonuc.Mesajlar.Add($"Satır numarası sürekliliği OK: DB toplam satır={satirBilgi.ToplamSatirSayisi:N0}, " +
                                   $"çalışılan ay ilk lineNumber={calislanMinSatirNo}");
            }
            else
            {
                sonuc.Mesajlar.Add($"UYARI: Satır numarası sürekliliği BOZUK! " +
                                   $"Beklenen başlangıç={beklenenSatirBaslangic}, gerçek={calislanMinSatirNo} " +
                                   $"(DB'de {satirBilgi.ToplamSatirSayisi:N0} satır, fark: {calislanMinSatirNo - beklenenSatirBaslangic})");
            }

            // Çalışılan ay içinde yevmiye numaralarının sıralı ve boşluksuz olduğunu kontrol et
            var yevmiyeNolarList = tumFisler.Select(f => f.YevmiyeNoSayac).OrderBy(n => n).ToList();
            bool icSurekliSonuc = YevmiyeIcSureklilkKontrol(yevmiyeNolarList, sonuc.Mesajlar);

            // Çalışılan ay içinde satır numaralarının sıralı ve boşluksuz olduğunu kontrol et
            var satirNolarList = tumSatirlar.Select(s => s.SatirNoSayac).OrderBy(n => n).ToList();
            bool icSatirSurekliSonuc = SatirIcSureklilkKontrol(satirNolarList, sonuc.Mesajlar);

            sonuc.AktarimIzinli = sonuc.OncekiAyMevcut && sonuc.Surekli && icSurekliSonuc
                                  && sonuc.SatirSurekli && icSatirSurekliSonuc;

            if (sonuc.AktarimIzinli)
            {
                sonuc.Mesajlar.Add("Tüm kontroller başarılı — aktarıma izin verildi.");
            }
            else if (!icSurekliSonuc || !icSatirSurekliSonuc)
            {
                sonuc.Mesajlar.Add("Aktarım ENGELLENDI — yevmiye veya satır numaralarında boşluk tespit edildi.");
            }
            else if (!sonuc.SatirSurekli)
            {
                sonuc.Mesajlar.Add("Aktarım ENGELLENDI — satır numarası (lineNumber) sürekliliği sağlanamadı.");
            }
            else
            {
                sonuc.Mesajlar.Add("Aktarım ENGELLENDI — yevmiye sürekliliği sağlanamadı.");
            }

            return sonuc;
        }

        /// <summary>
        /// Ay içindeki yevmiye numaralarının boşluksuz ve ardışık olduğunu kontrol eder.
        /// </summary>
        private bool YevmiyeIcSureklilkKontrol(List<int> siraliYevmiyeNolar, List<string> mesajlar)
        {
            if (siraliYevmiyeNolar.Count <= 1) return true;

            bool surekli = true;
            for (int i = 1; i < siraliYevmiyeNolar.Count; i++)
            {
                int fark = siraliYevmiyeNolar[i] - siraliYevmiyeNolar[i - 1];
                if (fark != 1)
                {
                    surekli = false;
                    mesajlar.Add($"UYARI: Yevmiye boşluğu: {siraliYevmiyeNolar[i - 1]} → {siraliYevmiyeNolar[i]} (fark: {fark})");
                }
            }

            if (surekli)
            {
                mesajlar.Add($"Ay içi yevmiye sürekliliği OK: {siraliYevmiyeNolar.First()} - {siraliYevmiyeNolar.Last()} (boşluk yok)");
            }

            return surekli;
        }

        /// <summary>
        /// Ay içindeki satır numaralarının (lineNumberCounter) boşluksuz ve ardışık olduğunu kontrol eder.
        /// </summary>
        private bool SatirIcSureklilkKontrol(List<int> siraliSatirNolar, List<string> mesajlar)
        {
            if (siraliSatirNolar.Count <= 1) return true;

            bool surekli = true;
            int boşlukSayisi = 0;

            for (int i = 1; i < siraliSatirNolar.Count; i++)
            {
                int fark = siraliSatirNolar[i] - siraliSatirNolar[i - 1];
                if (fark != 1)
                {
                    surekli = false;
                    boşlukSayisi++;

                    // İlk 3 boşluğu detaylı logla, sonrası özet
                    if (boşlukSayisi <= 3)
                    {
                        mesajlar.Add($"UYARI: Satır numarası boşluğu: {siraliSatirNolar[i - 1]} → {siraliSatirNolar[i]} (fark: {fark})");
                    }
                }
            }

            if (boşlukSayisi > 3)
            {
                mesajlar.Add($"UYARI: Toplam {boşlukSayisi} satır numarası boşluğu tespit edildi (ilk 3'ü gösterildi).");
            }

            if (surekli)
            {
                mesajlar.Add($"Ay içi satır numarası sürekliliği OK: {siraliSatirNolar.First()} - {siraliSatirNolar.Last()} (boşluk yok)");
            }

            return surekli;
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
