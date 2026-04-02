# Changelog

## [2.19.2] - 2025-07-18 — Bug Fix Release

### Düzeltilen
- AyarlarForm: Bağlantı TextBox'undaki `ReadOnly = true` kaldırıldı — artık connection string elle de yazılabilir
- SqlBaglantiForm: `sys.databases` sorgusundaki `state_online` sütun hatası düzeltildi (`state = 0`)
- SqlBaglantiForm: Tüm kontrollere TabIndex eklendi, form açılışında sunucu alanına odak ayarlandı

### Etkilenen Dosyalar
- `Defter2Fis.ForMikro/Forms/AyarlarForm.Designer.cs`
- `Defter2Fis.ForMikro/Forms/SqlBaglantiForm.cs`
- `Defter2Fis.ForMikro/Forms/SqlBaglantiForm.Designer.cs`

---

## [2.19.1] - 2025-07-18 — Veritabanı Listeleme Hatası Düzeltmesi

### Düzeltilen
- SqlBaglantiForm: `sys.databases` sorgusundaki `state_online` sütun adı hatası düzeltildi (`state = 0` olarak güncellendi)

### Etkilenen Dosyalar
- `Defter2Fis.ForMikro/Forms/SqlBaglantiForm.cs`

## [2.19.0] - 2025-07-17 — Özel SQL Bağlantı Formu

### Eklenen
- SqlBaglantiForm: VS bağımlılığı olmayan özel SQL Server bağlantı dialog’u
- Sunucu adı, Windows/SQL kimlik doğrulama, veritabanı listesi (ComboBox ile seçim)
- "Listele" butonu ile sunucudaki veritabanlarını otomatik sorgulama
- Bağlantı testi butonu dialog içinde
- Mevcut connection string’i parse edip forma doldurma

### Kaldırılan
- Microsoft.Data.ConnectionUI / ConnectionUI.Dialog bağımlılığı (VS 2026 runtime bağımlılık sorunu)
- Microsoft.Data.SqlClient / SNI DLL’leri (artık gerekli değil)
- lib/ klasöründeki tüm harici DLL’ler temizlendi

### Değişen
- AyarlarForm: DataConnectionDialog yerine SqlBaglantiForm kullanıyor
- Installer: ConnectionUI/SqlClient DLL’leri kaldırıldı
- Versiyon 2.18.1 → 2.19.0

---

## [2.18.1] - 2025-07-17 — Microsoft.Data.SqlClient Bağımlılık Düzeltmesi

### Düzeltilen
- Microsoft.Data.ConnectionUI.Dialog çalışma zamanı hatası düzeltildi (FileNotFoundException: Microsoft.Data.SqlClient)
- Microsoft.Data.SqlClient 5.2.2 (net462) lib klasörüne eklendi ve proje referansı tanımlandı
- Microsoft.Data.SqlClient.SNI 5.2.0 native DLL'leri build hedefine eklendi (x86/x64/arm64)

### Değişen
- Defter2Fis.ForMikro.csproj: Microsoft.Data.SqlClient referansı ve SNI targets import'u eklendi
- Versiyon 2.18.0 → 2.18.1

---

## [2.18.0] - 2025-07-17 — SQL Connection Builder ve Bağlantı Uyarı Sistemi

### Eklenen
- Microsoft DataConnectionDialog entegrasyonu (SQL Server bağlantı yapılandırma)
- Sunucu tarama, veritabanı seçimi, Windows/SQL kimlik doğrulama desteği
- Bağlantı test butonu (Ayarlar ekranında)
- MainForm'da bağlantı yapılandırılmamışsa görsel uyarı paneli
- Uyarı panelinde doğrudan "Ayarları Aç" bağlantısı
- Bağlantı yoksa veritabanı gerektiren butonlar devre dışı bırakılıyor
- Microsoft.Data.ConnectionUI DLL'leri projeye eklendi (MIT lisanslı)

### Değişen
- App.config: Varsayılan hardcoded connection string kaldırıldı (güvenlik)
- AyarlarForm: "Bağlantı Oluştur" Microsoft dialog'ı açıyor, "Bağlantıyı Test Et" butonu eklendi
- AyarlarForm: Connection string alanı salt-okunur yapıldı
- MainForm: Ayarlar kaydedildikten sonra bağlantı durumu yeniden kontrol ediliyor
- Installer: Microsoft.Data.ConnectionUI DLL'leri dağıtıma eklendi
- Versiyon 2.17.0 → 2.18.0

---

## [2.17.0] - 2025-07-17 — Feragatname ve Kabul Kanıt Sistemi

### Eklenen
- Uygulama başlangıcında hukuki feragatname formu (FeragatnameForm)
- 8 maddelik resmi dilde sorumluluk reddi ve kullanım koşulları metni
- Kabul kanıtı sistemi (DisclaimerService): Windows Registry + gizli dosya + SHA256 imza
- Kabul edilmeden uygulama çalışmaz
- Krypton temalı feragatname UI (dark theme uyumlu)

### Değişen
- Program.cs: MainForm öncesi feragatname kontrolü eklendi
- Versiyon 2.16.0 → 2.17.0

---

## [2.16.0] - 2025-07-17 — Inno Setup Installer

### Eklenen
- Inno Setup installer script (`Installer/Defter2Fis.iss`)
- Kurulum sırasında .NET Framework 4.8 varlık kontrolü
- Türkçe kurulum dili desteği
- Masaüstü kısayolu seçeneği
- GitHub Releases üzerinden installer dağıtımı

### Değişen
- Versiyon 2.15.0 → 2.16.0

---

## [2.15.0] - 2025-08-28 — Model Sınıfları Refactoring

### Değiştirilen
- Model/DTO sınıfları servis dosyalarından ayrı Models/ klasörüne çıkarıldı
- `EvrakBilgisi` → `Models/EvrakBilgisi.cs` (EvrakBilgisiParser.cs'den ayrıldı)
- `OlusturmaSonucu` → `Models/FisOlusturmaModels.cs` (FisOlusturmaServisi.cs nested class'tan ayrıldı)
- `LogSeviye` enum + `LogEventArgs` → `Models/LogModels.cs` (LogService.cs'den ayrıldı)
- `AnalizOzeti`, `DosyaOzeti`, `DbDurumBilgisi`, `EksikHesap`, `DengeSizFis` → `Models/AnalyzerModels.cs` (DefterAnalyzer.cs'den ayrıldı)
- `EdDefterModels.cs` ve `MikroDbModels.cs` XML doc yorumları güncellendi (lineNumberCounter = fis_yevmiye_no eşleşme notu)

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Models\AnalyzerModels.cs (YENİ)
- Defter2Fis.ForMikro\Models\EvrakBilgisi.cs (YENİ)
- Defter2Fis.ForMikro\Models\FisOlusturmaModels.cs (YENİ)
- Defter2Fis.ForMikro\Models\LogModels.cs (YENİ)
- Defter2Fis.ForMikro\Models\EdDefterModels.cs
- Defter2Fis.ForMikro\Models\MikroDbModels.cs
- Defter2Fis.ForMikro\Services\DefterAnalyzer.cs
- Defter2Fis.ForMikro\Services\EvrakBilgisiParser.cs
- Defter2Fis.ForMikro\Services\FisOlusturmaServisi.cs
- Defter2Fis.ForMikro\Services\LogService.cs

## [2.14.1] - 2025-08-28 — lineNumber Kodu Kaldırıldı (Gerçek Veri Analizi)

### Kaldırılan
- Satır numarası (lineNumber) süreklilik kontrolü — gerçek e-Defter verileriyle yapılan analizde `lineNumberCounter` = `fis_yevmiye_no` (yevmiye numarası) olduğu tespit edildi; ayrı bir satır sayacı DEĞİL
- `SatirSureklilkBilgisiGetir` servisi ve SQL sorgusu
- `SatirIcSureklilkKontrol` metodu
- `SatirSureklilkBilgisi` modeli, `SureklilkKontrolSonucu` ve `AyFisBilgisi` satır alanları
- `FisOlusturmaServisi` OrderBy SatirNoSayac sıralaması
- 6 satır sürekliliği birim testi

### Açıklama
- Türk e-Defter XML yapısında `lineNumberCounter` her fiş satırı için ayrı bir global sayaç DEĞİL, `entryNumberCounter` (yevmiye numarası) ile aynı değeri taşır
- Mikro DB eşleşmesi: `fis_yevmiye_no` = `lineNumberCounter`
- Yevmiye sürekliliği kontrolleri (aylar arası + ay içi boşluk) aynen korunuyor

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Models\MikroDbModels.cs
- Defter2Fis.ForMikro\Services\IMikroDbService.cs
- Defter2Fis.ForMikro\Services\MikroDbService.cs
- Defter2Fis.ForMikro\Services\DefterAnalyzer.cs
- Defter2Fis.ForMikro\Services\FisOlusturmaServisi.cs
- Defter2Fis.Tests\DefterAnalyzerMockTests.cs

## [2.14.0] - 2025-08-28 — Satır Numarası (lineNumber) Sürekliliği & Sıralı Yazım (REVERTED in 2.14.1)

> **Not:** Bu sürümdeki satır numarası özellikleri v2.14.1'de kaldırılmıştır.
> Gerçek veri analizi sonucu `lineNumberCounter` = `fis_yevmiye_no` olduğu tespit edilmiştir.

## [2.13.0] - 2025-08-28 — Mikro Uyumlu Fiş Yazma

### Düzeltmeler
- **Ticari tip değerleri düzeltildi** — Cari hareket=2, stok hareket=1 (Mikro ERP uyumlu; önceki sürümde ters atanmıştı)
- **Ticari bilgi tüm satırlara uygulanır** — Mikro gibi, yevmiyenin tüm satırlarına aynı ticari_tip/uid/evraktip atanır (eskiden sadece ilk satır)
- **Hub kolonları eklendi** — fis_ticari_hubid, fis_ticari_hubglbid, fis_ticari_disyazilimid (''), fis_ticari_disyazilim_tip (0) INSERT'e eklendi (NULL yerine Mikro varsayılan değerleri)

### Kaldırılanlar
- Döviz kuru hesaplaması kaldırıldı (meblag1/meblag2 = 0, merkez bankası kuru bilgisi mevcut değil)
- `DonemGunlukKurGetir` metodu kaldırıldı (IMikroDbService + MikroDbService)
- CariHesapHareketi ve StokHareketi modellerinden kur alanları kaldırıldı
- SQL sorgularından kur kolonları kaldırıldı (cha_altd_kur, cha_karsid_kur, sth_alt_doviz_kuru)

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Services\FisOlusturmaServisi.cs (ticari tip düzeltme, tüm satır atama, kur kaldırma)
- Defter2Fis.ForMikro\Services\MikroDbService.cs (hub kolonlar, kur SQL/metot kaldırma)
- Defter2Fis.ForMikro\Services\IMikroDbService.cs (DonemGunlukKurGetir kaldırma)
- Defter2Fis.ForMikro\Models\MikroDbModels.cs (kur alanları kaldırma)

## [2.12.0] - 2025-08-28 — Mükerrer Kontrolü Bilgi Amaçlı + Üzerine Yazma

### Yeni Özellikler
- **DELETE-before-INSERT üzerine yazma** — DB'de mevcut yevmiye fişleri artık atlanmak yerine silinip yeniden yazılır. Tüm e-Defter kayıtları eksiksiz kaydedilir.
- **YevmiyeFisleriniSil metodu** — Belirtilen yevmiye numarasına ait mevcut fiş satırlarını transaction içinde siler.
- **Mükerrer kontrol bilgi amaçlı** — Mükerrer yevmiye sayısı kullanıcıya bilgi olarak gösterilir, ancak kayıt atlanmaz.

### Değişiklikler
- `SimulasyonCalistir`: Mükerrer `continue` (atlama) kaldırıldı — tüm yevmiyeler işlenir
- `TopluFisYaz`: Her fiş INSERT öncesi mevcut kayıtlar `YevmiyeFisleriniSil` ile silinir
- `OnizlemeServisi`: Uyarı mesajı "atlanacak" → "üzerine yazılacak", seviye Uyari → Bilgi
- `SonucRaporla`: "Atlanan (mükerrer)" → "Üzerine yazılan"

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Services\IMikroDbService.cs (YevmiyeFisleriniSil eklendi)
- Defter2Fis.ForMikro\Services\MikroDbService.cs (YevmiyeFisleriniSil eklendi)
- Defter2Fis.ForMikro\Services\FisOlusturmaServisi.cs (mükerrer atlama kaldırıldı, DELETE-before-INSERT)
- Defter2Fis.ForMikro\Services\OnizlemeServisi.cs (uyarı mesajı güncellendi)

## [2.11.1] - 2025-08-27 — Mükerrer Kontrol ve SQL INSERT Düzeltmesi

### Düzeltmeler
- **Mükerrer kontrol unique index uyumu** — `YevmiyeNoMevcutMu` sorgusu `NDX_MUHASEBE_FISLERI_02` unique index'e uyumlu hale getirildi. `fis_tur`, `fis_iptal` ve tarih aralığı filtreleri kaldırıldı; artık farklı fiş türü veya iptal durumundaki mükerrer yevmiye numaraları doğru tespit ediliyor.
- **SQL INSERT parametre düzeltmesi** — `fis_ticari_tip`, `fis_ticari_evraktip`, `fis_tic_evrak_seri`, `fis_tic_evrak_sira` alanları hardcoded değerler yerine model’den parametre olarak gönderiliyor.
- **Hata loglama iyileştirmesi** — `TopluFisYaz` hata yakalayasında SqlException detayları (hata numarası, satır, durum, prosedür) loglanıyor.

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Services\IMikroDbService.cs (imza sadeleştirildi)
- Defter2Fis.ForMikro\Services\MikroDbService.cs (SQL düzeltmeleri)
- Defter2Fis.ForMikro\Services\FisOlusturmaServisi.cs (mükerrer kontrol + hata loglama)
- Defter2Fis.ForMikro\Services\OnizlemeServisi.cs (mükerrer kontrol uyumu)

## [2.11.0] - 2025-08-27 — Cari/Stok Yazma İşlemlerini Kaldırma

### Kaldırılan Özellikler
- **Cari/stok hareket UPDATE işlemleri kaldırıldı** — Uygulama artık CARI_HESAP_HAREKETLERI ve STOK_HAREKETLERI tablolarına yazma yapmaz. Sadece MUHASEBE_FISLERI tablosuna fiş oluşturur.
- `CariHareketMuhFisGuncelle` ve `StokHareketMuhFisGuncelle` metotları kaldırıldı
- `EslesmeCariGuidler` / `EslesmeStokGuidler` Guid listeleri kaldırıldı
- `EslestirilenCariSayisi` / `EslestirilenStokSayisi` sayaçları kaldırıldı
- "Üzerine yazılacak referans" uyarısı kaldırıldı
- `MevcutMuhFisNo`, `AtanacakMuhFisNo`, `UzerineYazilacak` önizleme model özellikleri kaldırıldı
- Cari/stok gridlerinden "Mevcut Fis No", "Yeni Fis No", "Üzerine Yaz" sütunları kaldırıldı

### Korunan Özellikler
- Cari/stok eşleştirme **bilgi amaçlı** olarak önizleme gridinde gösterilmeye devam eder
- Ticari eşleştirme bilgisi (FisTicariTip/FisTicariUid) muhasebe fişi satırına yazılmaya devam eder

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Services\IMikroDbService.cs (metot kaldırıldı)
- Defter2Fis.ForMikro\Services\MikroDbService.cs (metot kaldırıldı)
- Defter2Fis.ForMikro\Services\FisOlusturmaServisi.cs (UPDATE, Guid listeleri, sayaçlar kaldırıldı)
- Defter2Fis.ForMikro\Services\OnizlemeServisi.cs ("Üzerine yazılacak" uyarısı kaldırıldı)
- Defter2Fis.ForMikro\Models\OnizlemeModels.cs (model özellikleri kaldırıldı)
- Defter2Fis.ForMikro\Forms\MainForm.cs (grid sütunları kaldırıldı)

## [2.10.0] - 2025-08-27 — Cari/Stok Eşleştirme: Tarih+FişNo Bazlı

### Yeni Özellikler
- **Tarih+FişNo bazlı eşleştirme** — Cari/stok hareketleri artık `cha_fis_tarih + cha_fis_sirano` (cari) ve `sth_fis_tarihi + sth_fis_sirano` (stok) üzerinden eşleştiriliyor. Evrak seri/sıra bazlı eşleştirme kaldırıldı.
- **Açıklama tam gösterim** — Önizleme gridinde açıklama alanı artık 80 karakterde kesilmiyor, tamamı gösteriliyor.

### Değişiklikler
- `CariHesapHareketi.EvrakAnahtar` → `FisAnahtar` (fis_tarih|fis_sirano)
- `StokHareketi.EvrakAnahtar` → `FisAnahtar` (fis_tarih|fis_sirano)
- SQL ORDER BY: evrak seri/sıra → fis_tarih + fis_sirano
- Cari/stok önizleme gridlerinden EvrakSeri/EvrakSira sütunları kaldırıldı

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Models\MikroDbModels.cs (FisAnahtar)
- Defter2Fis.ForMikro\Models\OnizlemeModels.cs (EvrakSeri/Sira kaldırıldı)
- Defter2Fis.ForMikro\Services\MikroDbService.cs (index + SQL)
- Defter2Fis.ForMikro\Services\OnizlemeServisi.cs (eşleştirme + açıklama)
- Defter2Fis.ForMikro\Services\FisOlusturmaServisi.cs (eşleştirme)
- Defter2Fis.ForMikro\Forms\MainForm.cs (grid sütunları)

## [2.9.2] - 2025-08-26 — Mükerrer Dönem Filtresi Düzeltmesi

### Düzeltmeler
- **Yanlış mükerrer tespiti** — YevmiyeNoMevcutMu sorgusu artık dönem tarih aralığı (fis_tarih >= donemBas AND fis_tarih <= donemBit) ile filtreleniyor. Farklı aylardaki aynı yevmiye numaraları artık mükerrer olarak işaretlenmiyor.

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Services\IMikroDbService.cs (dönem parametreleri eklendi)
- Defter2Fis.ForMikro\Services\MikroDbService.cs (SQL dönem filtresi eklendi)
- Defter2Fis.ForMikro\Services\OnizlemeServisi.cs (dönem parametreleri geçiliyor)
- Defter2Fis.ForMikro\Services\FisOlusturmaServisi.cs (dönem parametreleri geçiliyor)

## [2.9.1] - 2025-08-26 — Sıra No ve Evrak Parse Düzeltmesi

### Düzeltmeler
- **Mükerrer SıraNo = 0 sorunu** — Önizlemede mükerrer fişlere de sıralı numara atanır (mükerrer bayrağı korunur)
- **ETTN evrak parse hatası** — e-Fatura/e-Arşiv ETTN belge numaraları (ör: TA4202500000642) artık doğru parse ediliyor
- **e-Defter açıklama formatı** — "Al.fat. : ...", "C.A.V.D : ..." gibi DetayAciklama formatlarından evrak bilgisi çıkarılıyor

### Yeni Testler
- 12 yeni birim testi: ETTN BelgeNo (4 test), ETTN DetayAciklama (5 test), sayısal DetayAciklama (2 test), Anahtar (1 test)
- Toplam: **84/84 test geçiyor**

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Services\OnizlemeServisi.cs (mükerrer sıra no düzeltmesi)
- Defter2Fis.ForMikro\Services\EvrakBilgisiParser.cs (PatternEttn + PatternEdDefterAciklama)
- Defter2Fis.Tests\EvrakBilgisiParserTests.cs (12 yeni test)

## [2.9.0] - 2025-08-26 — Grid Filtre / Arama / Sıralama

### Yeni Özellikler
- **Grid filtre/arama** — Tüm 6 KryptonDataGridView'e anlık metin arama (tüm kolonlarda case-insensitive)
- **Kolon sıralama** — Kolon başlığına tıklayarak ASC/DESC sıralama (glyph göstergesi)
- **FilterableGridHelper<T>** — Generic LINQ tabanlı filtre + PropertyInfo reflection sıralama yardımcı sınıfı
- **Satır renklendirme korunumu** — Mükerrer (kırmızı) ve uyarı seviye renkleri filtre sonrası da korunur
- **Dark tema filtre kutusu** — Her grid üstüne karanlık temalı arama TextBox'u

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Services\FilterableGridHelper.cs (YENİ)
- Defter2Fis.ForMikro\Forms\MainForm.cs (güncellendi — 6 helper instance, FiltreAlanOlustur, FiltreHelperBaslat)
- Defter2Fis.ForMikro\Forms\MainForm.Designer.cs (güncellendi — 6 filtre TextBox + 5 TLP)

## [2.8.0] - 2025-08-26 — Birim Testleri

### Yeni Özellikler
- **DefterAnalyzerTests** — 17 test: AraSeviyeleriUret, OzetHesapla, BenzersizHesapKodlari, EksikHesaplariTespit, DengeKontrolu
- **EdDefterXmlParserTests** — 14 test: DosyadanOku, KlasordenOku, hata durumları, opsiyonel alanlar
- **DefterAnalyzerMockTests** — 15 test: OncekiAyDogrula, DbDurumGetir (Moq ile)
- **Test projesi modernizasyonu** — packages.config → PackageReference formatına geçiş
- **72/72 test geçiyor** (NUnit 3.14.0 + Moq 4.20.72)

### Etkilenen dosyalar
- Defter2Fis.Tests\DefterAnalyzerTests.cs (YENİ)
- Defter2Fis.Tests\EdDefterXmlParserTests.cs (YENİ)
- Defter2Fis.Tests\DefterAnalyzerMockTests.cs (YENİ)
- Defter2Fis.Tests\Defter2Fis.Tests.csproj (PackageReference + RuntimeIdentifiers)

## [2.7.2] - 2025-08-25 — Stok Hareketleri Kolon Adı Düzeltmesi

### Düzeltmeler
- **Stok hareketleri SQL sorgusu** — Yanlış kolon adları düzeltildi:
  - `sth_tarihi` → `sth_tarih` (Mikro ERP gerçek kolon adı)
  - `sth_evrak_tip` → `sth_evraktip` (Mikro ERP gerçek kolon adı)
- Önizleme ekranında "Invalid column name" hatası giderildi

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Services\MikroDbService.cs (DonemStokHareketleriGetir SQL sorgusu)
- Defter2Fis.ForMikro\Models\MikroDbModels.cs (StokHareketi XML doc yorumları)

## [2.7.1] - 2025-08-25 — Analiz Raporu Birleşik Gösterim

### Değişenler
- **Analiz raporu birleşik gösterim** — Önceki ay ve çalışılan ay bilgileri tek başlık altında alt alta gösterilir
  - `===== E-DEFTER ANALIZ RAPORU =====` başlığı altında: önce önceki ay, sonra çalışılan ay
  - Her iki ay için dosya, firma, dönem, fiş/satır ve yevmiye aralığı bilgisi
  - Önceki ay XML okuma işlemi sessiz yapılır, hata varsa rapor bloğunda uyarı gösterilir

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Forms\MainForm.cs (AnalizCalistir rapor bloğu yeniden yapılandırıldı)

## [2.7.0] - 2025-08-25 — Önceki Ay E-Defter XML Bilgisi Gösterimi

### Eklenenler
- **Önceki ay E-Defter XML bilgisi** — Analiz raporunda çalışılan aydan önceki ayın XML dosyası okunarak gösterilir
  - Dosya adı, firma, dönem, fiş/satır sayısı, yevmiye aralığı
  - Önceki ay klasörü yoksa veya XML bulunamazsa uyarı mesajı gösterilir
  - Mali yıl ilk ayı (01) için önceki ay gösterimi atlanır

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Forms\MainForm.cs (AnalizCalistir — önceki ay XML okuma bloğu)

## [2.6.1] - 2025-08-25 — Yevmiye Sürekliliği: Tarih-Bazlı → Yevmiye-Bazlı Düzeltme

### Düzeltmeler
- **KRİTİK BUG FIX:** Önceki ay doğrulama tarih bazlı sorguda yanlış yevmiye aralığı döndürüyordu
  - Sorun: `postingDate` takvim ayı sınırlarıyla örtüşmüyor; Temmuz fişleri Ağustos tarihli, Ağustos fişleri Temmuz tarihli olabiliyor
  - Örnek: DB Ağustos tarih aralığı sorgusu yevmiye 8085-9033 dönerken, XML Ağustos yevmiyesi 8290-9663
  - Çözüm: Tarih-bazlı `AyFisBilgisiGetir` yerine yevmiye-numarası-bazlı `YevmiyeSureklilkBilgisiGetir` kullanılır
- **İlk ay tespiti düzeltildi:** Takvim ayı karşılaştırması yerine `xmlMinYevmiye == 1` kontrolü

### Eklenenler
- `YevmiyeSureklilkBilgisi` DTO — Mali yıl genelinde yevmiye count/max bilgisi (tarihten bağımsız)
- `IMikroDbService.YevmiyeSureklilkBilgisiGetir()` — Yevmiye numarası bazlı süreklilik sorgusu
- `SureklilkKontrolSonucu.DbMaxYevmiyeNo` — DB'deki mevcut max yevmiye numarası
- `SureklilkKontrolSonucu.DbYevmiyeSayisi` — DB'deki toplam benzersiz yevmiye sayısı

### Değişenler
- `DefterAnalyzer.OncekiAyDogrula()` — Tarih-bazlı → yevmiye-numarası-bazlı süreklilik kontrolü
- `AyFisBilgisiGetir` yalnızca bilgilendirme amaçlı tutuldu (aktif kontrol için kullanılmıyor)
- Onay diyaloğunda `DbMaxYevmiyeNo` gösterilir

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Models\MikroDbModels.cs (yeni DTO + mevcut güncelleme)
- Defter2Fis.ForMikro\Services\IMikroDbService.cs (yeni metot)
- Defter2Fis.ForMikro\Services\MikroDbService.cs (yeni metot implementasyonu)
- Defter2Fis.ForMikro\Services\DefterAnalyzer.cs (OncekiAyDogrula yeniden yazıldı)
- Defter2Fis.ForMikro\Forms\MainForm.cs (onay diyaloğu güncelleme)

## [2.6.0] - 2025-08-24 — Önceki Ay Doğrulama, Yevmiye Sürekliliği, Atomik Ay Operasyonları

### Eklenenler
- **Önceki ay doğrulama** — Analiz aşamasında önceki ayın muhasebe fişlerinin DB'de mevcut olup olmadığı kontrol edilir
- **Yevmiye sürekliliği kontrolü** — Önceki ayın son yevmiye numarasının çalışılan ayın ilk yevmiyesiyle uyumu doğrulanır
- **Ay içi yevmiye boşluk kontrolü** — Yevmiye numaralarının ardışık ve boşluksuz olduğu doğrulanır
- **Simülasyon-önce yaklaşım** — Tüm fişler bellekte oluşturulup doğrulandıktan sonra DB'ye yazılır
- **Atomik ay-seviyesi transaction** — Tüm ayın fişleri tek transaction içinde yazılır; hata durumunda tüm ay rollback yapılır
- **AyFisBilgisi DTO** — Ay bazlı fiş özet bilgisi (fiş sayısı, yevmiye aralığı, tarih aralığı)
- **SureklilkKontrolSonucu DTO** — Süreklilik kontrol sonucu (önceki ay mevcut mu, sürekli mi, aktarım izinli mi)
- `IMikroDbService.AyFisBilgisiGetir()` — Belirtilen ay döneminin DB özet bilgisini döner
- `DefterAnalyzer.OncekiAyDogrula()` — Önceki ay + yevmiye sürekliliği + iç süreklilik kontrolü
- Fiş oluşturma butonunda analiz zorunluluğu ve aktarım engeli kontrolü
- Onay diyaloğunda yevmiye süreklilik bilgisi gösterimi

### Değişenler
- `FisOlusturmaServisi.FisleriOlustur()` — Per-fiş transaction yerine simülasyon + atomik yazım
- `MainForm.AnalizCalistir` — Önceki ay doğrulama adımı eklendi (%90 ilerleme)
- `MainForm.BtnFisOlustur_Click` — Süreklilik kontrolü ve engelleme mantığı
- `MainForm.FisOlusturmaCalistir` — Simülasyon durum mesajları

### Etkilenen dosyalar
- Defter2Fis.ForMikro\Models\MikroDbModels.cs (yeni DTO'lar)
- Defter2Fis.ForMikro\Services\IMikroDbService.cs (yeni metot)
- Defter2Fis.ForMikro\Services\MikroDbService.cs (yeni metot implementasyonu)
- Defter2Fis.ForMikro\Services\DefterAnalyzer.cs (yeni doğrulama metotları)
- Defter2Fis.ForMikro\Services\FisOlusturmaServisi.cs (simülasyon + atomik refactor)
- Defter2Fis.ForMikro\Forms\MainForm.cs (analiz + fiş oluşturma güncelleme)

## [2.5.5] - 2025-08-24 — Akıllı Yedek Sıkıştırma

### Eklenenler
- **Akıllı COMPRESSION** — SQL Server edition otomatik tespit edilir
- Express dışı edition'larda (Standard, Enterprise, Developer) `COMPRESSION` eklenir
- Express'te sıkıştırmasız yedek alır
- `SikistirmaDestekleniyor()` — `SERVERPROPERTY('EngineEdition')` ile edition kontrolü

## [2.5.4] - 2025-08-23 — Cari/Stok Kolon Adı Düzeltmeleri

### Düzeltmeler
- **Kritik:** `CARI_HESAP_HAREKETLERI` ve `STOK_HAREKETLERI` tablolarında yanlış kolon adları düzeltildi
- `cha_subession` → `cha_subeno`
- `cha_muh_fis_no` → `cha_fis_sirano`
- `cha_muh_fis_tarihi` → `cha_fis_tarih`
- `sth_muh_fis_no` → `sth_fis_sirano`
- `sth_muh_fis_tarihi` → `sth_fis_tarihi`
- SELECT, UPDATE sorguları ve model XML doc yorumları güncellendi

## [2.5.3] - 2025-08-23 — SQL Server Express COMPRESSION Uyumluluk Düzeltmesi

### Düzeltmeler
- **Kritik:** `BACKUP DATABASE WITH COMPRESSION` SQL Server Express'te desteklenmez — `COMPRESSION` seçeneği kaldırıldı

## [2.5.2] - 2025-08-23 — Yedekleme: Hedef DB Dizinine Yaz

### Düzeltmeler
- **Kritik:** Yedekleme erişim engeli (OS error 5) kesin çözüm
- Eski strateji (registry BackupDir, master DATA, xp_create_subdir) tümü SQL Server Express'te başarısız oluyordu
- **Yeni strateji:** Hedef veritabanının `.mdf` dosyasının bulunduğu klasöre yazar
- `sys.database_files` sorgusu ile veritabanının fiziksel konumu tespit edilir
- SQL Server bu dizine **kesinlikle** yazabilir çünkü aktif .mdf dosyası orada
- Gereksiz `SqlServerVarsayilanYedekDiziniGetir` ve `SqlServerVeriDiziniGetir` (master) kaldırıldı
- `HedefVeritabaniDiziniGetir()` — `Path.GetDirectoryName` ile temiz dizin çıkarımı

## [2.5.1] - 2025-08-23 — Yedekleme Dizin Erişim Fallback Zinciri

### Düzeltmeler
- **Kritik:** `xp_create_subdir` erişim engeli (error 5) hatası giderildi
- SQL Server servis hesabının dizin oluşturma izni olmadığında 3 aşamalı fallback zinciri devreye girer:
  1. Registry BackupDirectory + `xp_create_subdir`
  2. SQL Server DATA dizini (`master.mdf` klasörü — her zaman mevcut)
  3. Kullanıcı Belgeler\Defter2Fis\Yedekler (çıkış yolu)
- `YedekDiziniHazirla()` — önce `xp_create_subdir`, başarısızsa `Directory.CreateDirectory`
- `SqlServerVeriDiziniGetir()` — `master.sys.database_files`'dan veri dizinini sorgular

## [2.5.0] - 2025-08-23 — Yedekleme Düzeltme + İlerleme ve Bilgilendirme İyileştirmesi

### Düzeltmeler
- **Kritik:** `BACKUP DATABASE` OS error 3 (Yol bulunamadı) hatası giderildi
- SQL Server varsayılan yedek dizini (registry) fiziksel olarak mevcut değilse `xp_create_subdir` ile oluşturuluyor
- Yedek dosya boyutu okunamasa bile (SQL Server dizin erişimi) hata vermez

### Eklenenler
- **Gerçek zamanlı yedekleme ilerleme çubuğu** — `BACKUP DATABASE ... STATS=5` çıktısı `SqlInfoMessage` ile yakalanarak ProgressBar'a yansıtılır
- **İşlem sonuç bilgilendirmesi** — Tüm işlemler (analiz, önizleme, yedek, fiş oluşturma, silme, precheck) tamamlandığında StatusBar'da özet gösterir
- **ProgressBar otomatik sıfırlama** — İşlem bitiminden 2 saniye sonra progress bar sıfırlanır
- `İslemSonucBilgisi` yardımcı sınıfı — BackgroundWorker sonuclarını taşır
- `YedekDiziniOlustur()` — SQL Server `xp_create_subdir` ile dizin oluşturma
- `VeritabaniYedekle` ilerleme callback desteği (`Action<int, string>`)

### Değişiklikler
- `IMikroDbService.VeritabaniYedekle` imzasına `ilerlemeCallback` parametresi eklendi
- `STATS = 10` → `STATS = 5` (daha pürüzsüz ilerleme gösterimi)
- `YedeklemeCalistirSenkron` işlem öncesi yedekte de ilerleme log'lar

## [2.4.1] - 2025-08-23 — Yedekleme Erişim Hatası Düzeltmesi

### Düzeltmeler
- **Kritik:** `BACKUP DATABASE` OS error 5 (Erişim engellendi) hatası giderildi
- SQL Server servis hesabının uygulama dizinine yazma izni olmaması sorunu
- `SqlServerVarsayilanYedekDiziniGetir()` — SQL Server registry'den varsayılan yedek dizinini sorgular
- Yedekler artık SQL Server'ın kendi BackupDirectory'sine yazılır (servis hesabı erişim garantili)
- MainForm caller'ları uygulama dizini yerine null geçerek SQL Server default dizinini kullanır

## [2.4.0] - 2025-08-23 — IMikroDbService Interface Extraction

### Eklenenler
- **IMikroDbService** interface — MikroDbService için soyutlama katmanı
- Tüm 19 public instance method interface'e taşındı
- FisOlusturmaServisi, OnizlemeServisi, DefterAnalyzer artık IMikroDbService'e bağımlı
- YedeklemeSonucu nested class'tan top-level class'a taşındı
- Mock tabanlı unit test altyapısına hazırlık

### Değişiklikler
- MikroDbService artık IMikroDbService implement ediyor
- MainForm._dbService field tipi IMikroDbService olarak güncellendi
- Static yardımcı methodlar (CariIndexOlustur, StokIndexOlustur) concrete class üzerinde kaldı

## [2.3.2] - 2025-08-23 — BGW Eşzamanlılık Düzeltmesi

### Düzeltmeler
- **Kritik:** Tüm buton handler'larına `IslemDurumuAyarla(true)` eklendi (RunWorkerAsync öncesi)
- BGW çalışırken butonların devre dışı kalmaması sorunu giderildi
- Etkilenen handler'lar: BtnAnalizEt, BtnMevcutVeriKontrol, BtnDonemVerisiSil, BtnOnizleme, BtnYedekAl, BtnFisOlustur
- `InvalidOperationException: BackgroundWorker is currently busy` hatası önlendi

## [2.3.1] - 2025-08-23 — Dark Tema

### Eklenenler
- **Dark Tema** - Tum uygulama dark temaya gecirildi
- Krypton palette: Microsoft365BlackDarkMode
- DarkMenuColorTable - MenuStrip icin ozel dark renk tablosu
- Tum non-Krypton kontrollere dark BackColor/ForeColor
- DataGridView dark tema: koyu arka plan, acik metin, mavi secim vurgusu
- Log renkleri dark arka plan icin optimize edildi (LimeGreen, Orange, Tomato)
- AyarlarForm dark tema: GroupBox, TextBox, Button dark renklendirme
- HakkindaForm dark tema: panel, label, buton dark renklendirme
- StatusStrip VS-style mavi arka plan (0, 122, 204)

## [2.3.0] - 2025-08-23 — Veritabanı Yedekleme + Hata Yönetimi + Refactoring

### Eklenenler
- **Veritabanı Yedekleme** — `BACKUP DATABASE` komutu ile tam DB yedek alma (INIT, COMPRESSION, 10dk timeout)
- `MikroDbService.VeritabaniYedekle()` + `YedeklemeSonucu` sınıfı (dosya yolu, boyut, süre)
- `YedekAl` butonu — Tek tıkla veritabanı yedek alma (BackgroundWorker ile)
- **İşlem öncesi otomatik yedek teklifi** — Fiş oluşturma ve dönem silme öncesi Yes/No/Cancel dialog
- `YedeklemeCalistirSenkron()` — İşlem öncesi senkron yedek alma
- **Global hata yönetimi** — `Application.ThreadException` + `AppDomain.UnhandledException` (Program.cs)
- Hata diyalogu — BGW hatalarında kullanıcıya MessageBox ile detaylı bilgi

### İyileştirmeler
- Hata mesajlarına inner exception zincirleme bilgisi eklendi (MikroDbService, FisOlusturmaServisi, MainForm)
- `FisOlusturmaServisi` refactoring — 4 mükerrer private metot kaldırıldı, `MikroDbService` paylaşımlı metotları kullanılıyor
- `OnizlemeServisi` refactoring — 4 mükerrer private metot kaldırıldı, `MikroDbService` paylaşımlı metotları kullanılıyor
- `MikroDbService` paylaşımlı yardımcı metotlar eklendi:
  - `DonemCariHareketleriGetirGuvenli()` / `DonemStokHareketleriGetirGuvenli()` (SqlException 208 güvenli)
  - `CariIndexOlustur()` / `StokIndexOlustur()` (static, Dictionary bazlı evrak index)
- Kullanılmayan `System.Data.SqlClient` using'leri temizlendi (FisOlusturmaServisi, OnizlemeServisi)

## [2.2.0] - 2025-08-22 — Krypton UI + Önizleme/Test Modülü

### Eklenenler
- **Krypton Toolkit** entegrasyonu — Microsoft 365 Blue temalı modern UI
- **Önizleme / Test** modülü — Fiş oluşturma öncesi dry-run simülasyonu
- `OnizlemeServisi` — DB'ye yazmadan tam fiş oluşturma simülasyonu
- `OnizlemeSonucu` model ailesi — Önizleme DTO'ları (fiş, cari, stok, hesap, uyarı)
- Önizleme sekmesi: 5 alt tab (Fişler, Cari Eşleşmeleri, Stok Eşleşmeleri, Eksik Hesaplar, Uyarılar)
- İstatistik özet paneli (fiş/satır/cari/stok/mükerrer/eksik hesap sayıları)
- Mükerrer satırlar kırmızı, kritik uyarılar renkli gösterim
- Tüm formlar KryptonForm base class'a dönüştürüldü

### Değişenler
- `MainForm` — `Form` → `KryptonForm` + Krypton kontrolleri (Button, Label, GroupBox, DataGridView, RichTextBox)
- `AyarlarForm`, `HakkindaForm` — KryptonForm base class
- KryptonManager ile global Microsoft365Blue tema
- Krypton.Toolkit 85.24.6.176 NuGet paketi eklendi (PackageReference)

## [2.1.0] - 2025-08-22 — Fiş Oluşturma ve Cari/Stok Senkronizasyonu

### Eklenenler
- **Fiş Oluştur** butonu — E-Defter verilerinden Mikro ERP muhasebe fişleri oluşturma
- `EvrakBilgisiParser` — E-Defter açıklamalarından evrak seri/no parse (Regex pattern'ler)
- `FisOlusturmaServisi` — Orkestrasyon servisi: hesap planı + fiş yazma + cari/stok senkronizasyonu
- `CariHesapHareketi` / `StokHareketi` DTO modelleri
- `MikroDbService` — Cari/Stok hareketleri dönem sorgulama ve muhasebe fiş referans güncelleme
- Otomatik eksik hesap planı ekleme (fiş oluşturma öncesi)
- Evrak bazlı cari/stok hareket eşleştirme ve muhasebe fiş referansı senkronizasyonu
- Ticari referans alanları (fis_tic_evrak_seri, fis_tic_evrak_sira, fis_ticari_tip) otomatik doldurma
- Onay dialogı ile güvenli fiş oluşturma
- Detaylı sonuç raporu loglama

## [2.0.0] - 2025-08-22 — WinForms UI Dönüşümü

### Değişenler (BREAKING)
- Konsol uygulaması WinForms masaüstü uygulamasına dönüştürüldü (OutputType: Exe → WinExe)
- `Program.cs` WinForms giriş noktası olarak yeniden yazıldı
- `DefterAnalyzer` Yazdir metotları kaldırıldı, veri dönen metotlara dönüştürüldü
- `MikroDbService.BaglantıTest()` imzası değişti (out string hataMesaji eklendi)
- `EdDefterXmlParser.KlasordenOku` Action callback parametresi eklendi

### Eklenenler
- `MainForm` — Ana uygulama formu: menü, ayar özeti, analiz/precheck butonları, log paneli, mevcut veri grid
- `AyarlarForm` — App.config ayarları düzenleme diyalogu (DB, E-Defter, Mikro parametreleri)
- `HakkindaForm` — Hakkında iletişim kutusu
- `LogService` — Event tabanlı merkezi log altyapısı (LogSeviye: Bilgi/Basari/Uyari/Hata)
- `DonemFisOzeti` model — Mevcut dönem veri kontrol DTO'su
- `AnalizOzeti`, `DosyaOzeti`, `DbDurumBilgisi` modelleri — UI analiz sonuç DTO'ları
- Dönem veri kontrol (pre-check): `DonemVerileriGetir` / `DonemVerileriSil` DB metotları
- DataGridView ile mevcut dönem verisi listeleme
- Çift onaylı güvenli dönem verisi silme
- BackgroundWorker ile arka plan işlem desteği + ProgressBar
- RichTextBox tabanlı renkli log paneli

## [1.0.0] - 2025-08-22 — Proje altyapısı ve analiz modülü

### Eklenenler
- E-Defter Yevmiye XML parser (XBRL-GL formatı) — `EdDefterXmlParser.cs`
- Mikro ERP V16 Jump DB servisi (hesap planı, fiş CRUD) — `MikroDbService.cs`
- Analiz ve raporlama servisi (özet, denge, eksik hesap) — `DefterAnalyzer.cs`
- XML parse modelleri (YevmiyeDefteri, YevmiyeFisi, FisDetaySatiri) — `EdDefterModels.cs`
- DB entity modelleri (MuhasebeFisi, MuhasebeHesapPlani) — `MikroDbModels.cs`
- Konsol uygulaması giriş noktası — `Program.cs`
- App.config yapılandırması (connection string, e-Defter yolları)
- Copilot instructions (proje bağlamı)
