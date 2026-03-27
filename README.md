# Defter2Fis — E-Defter → Mikro ERP Muhasebe Fişi Oluşturucu

![Version](https://img.shields.io/badge/version-2.2.0-blue)
![.NET](https://img.shields.io/badge/.NET%20Framework-4.8-purple)

## Açıklama

E-Defter Yevmiye XML dosyalarını (XBRL-GL formatı) parse edip **Mikro ERP V16 Jump** MSSQL veritabanına muhasebe fişleri yazan .NET Framework 4.8 WinForms masaüstü uygulaması.

## Özellikler

- **WinForms masaüstü arayüzü** — Krypton Toolkit temalı modern UI (Microsoft 365 Blue)
- E-Defter Yevmiye XML dosyalarını otomatik bulma ve parse etme (XBRL-GL)
- **Önizleme / Test** — Fiş oluşturma öncesi dry-run simülasyonu (cari/stok eşleşme, uyarılar)
- **Fiş Oluşturma** — E-Defter'den Mikro ERP muhasebe fişleri oluşturma
- **Cari/Stok Senkronizasyonu** — Evrak seri/no ile cari hesap ve stok hareketleri eşleştirme
- Borç-Alacak denge kontrolü
- Eksik hesap planı tespiti ve otomatik ekleme
- Mikro ERP MUHASEBE_FISLERI tablosuna fiş yazma altyapısı
- **Dönem veri ön kontrolü (pre-check)** — İşlem öncesi mevcut veri listeleme ve güvenli silme
- **Çift onaylı veri silme** — Veri bütünlüğü koruması
- Mükerrer yevmiye kontrolü
- **Renkli log paneli** (RichTextBox) + **ProgressBar** ilerleme göstergesi
- **Ayarlar diyalogu** — App.config düzenleme (DB, E-Defter, Mikro parametreleri)
- App.config üzerinden tam yapılandırılabilir

## Gereksinimler

- .NET Framework 4.8
- MSSQL Server (Mikro ERP V16 Jump veritabanı)
- E-Defter Yevmiye XML dosyaları

## Kurulum

1. Projeyi derleyin (`Debug` veya `Release`)
2. `App.config` içindeki ayarları ortamınıza göre düzenleyin (veya uygulama içi **Araçlar > Ayarlar** menüsünden):
   - `MikroDB` connection string
   - `EdDefterRootPath` — E-Defter kök dizini
   - `SicilNo` — Firma sicil numarası
   - `MaliYilAraligi` — Mali yıl aralığı klasör adı
   - `AyKlasoru` — Çalışılacak ay klasörü
3. `Defter2Fis.ForMikro.exe` çalıştırın

## Kullanım

1. **Analiz Et** — XML dosyalarını parse edip analiz raporu oluşturur (DB'ye yazma yok)
2. **Mevcut Veri Kontrol** — Dönemdeki mevcut fiş verilerini listeler
3. **Dönem Verisini Sil** — Mevcut veriyi çift onay ile güvenli siler (opsiyonel)
4. **Önizleme** — Fiş oluşturma simülasyonu: ne oluşacak, ne eşleşecek, hangi uyarılar var
5. **Fiş Oluştur** — Muhasebe fişlerini oluşturur, cari/stok hareketleri ile senkronize eder

## Yapı

```
Defter2Fis.ForMikro/
├── Forms/
│   ├── MainForm.cs/.Designer.cs    # Ana uygulama formu
│   ├── AyarlarForm.cs/.Designer.cs # Ayarlar diyalogu
│   └── HakkindaForm.cs/.Designer.cs# Hakkında diyalogu
├── Models/
│   ├── EdDefterModels.cs           # XML parse DTO'ları
│   └── MikroDbModels.cs            # DB entity modelleri
├── Services/
│   ├── EdDefterXmlParser.cs        # XBRL-GL XML parser
│   ├── MikroDbService.cs           # Mikro ERP DB servisi
│   ├── DefterAnalyzer.cs           # Analiz ve raporlama
│   ├── FisOlusturmaServisi.cs      # Fiş oluşturma orkestrasyon
│   ├── EvrakBilgisiParser.cs       # Evrak seri/no parse
│   └── LogService.cs               # Merkezi log altyapısı
├── Program.cs                       # WinForms giriş noktası
├── App.config                       # Yapılandırma
└── Properties/
    └── AssemblyInfo.cs
```