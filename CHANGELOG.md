# Changelog

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
