# Changelog

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
