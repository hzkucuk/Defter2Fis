# Defter2Fis — E-Defter → Mikro ERP Muhasebe Fişi Oluşturucu

![Version](https://img.shields.io/badge/version-2.0.0-blue)
![.NET](https://img.shields.io/badge/.NET%20Framework-4.8-purple)

## Açıklama

E-Defter Yevmiye XML dosyalarını (XBRL-GL formatı) parse edip **Mikro ERP V16 Jump** MSSQL veritabanına muhasebe fişleri yazan .NET Framework 4.8 WinForms masaüstü uygulaması.

## Özellikler

- **WinForms masaüstü arayüzü** — Menü, ayar özeti, log paneli, mevcut veri grid
- E-Defter Yevmiye XML dosyalarını otomatik bulma ve parse etme (XBRL-GL)
- Borç-Alacak denge kontrolü
- Eksik hesap planı tespiti ve otomatik ekleme altyapısı
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
│   └── LogService.cs               # Merkezi log altyapısı
├── Program.cs                       # WinForms giriş noktası
├── App.config                       # Yapılandırma
└── Properties/
    └── AssemblyInfo.cs
```