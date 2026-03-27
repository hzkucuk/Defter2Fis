# Defter2Fis — E-Defter → Mikro ERP Muhasebe Fişi Oluşturucu

![Version](https://img.shields.io/badge/version-1.0.0-blue)
![.NET](https://img.shields.io/badge/.NET%20Framework-4.8-purple)

## Açıklama

E-Defter Yevmiye XML dosyalarını (XBRL-GL formatı) parse edip **Mikro ERP V16 Jump** MSSQL veritabanına muhasebe fişleri yazan .NET Framework 4.8 konsol uygulaması.

## Özellikler

- E-Defter Yevmiye XML dosyalarını otomatik bulma ve parse etme (XBRL-GL)
- Borç-Alacak denge kontrolü
- Eksik hesap planı tespiti ve otomatik ekleme altyapısı
- Mikro ERP MUHASEBE_FISLERI tablosuna fiş yazma altyapısı
- Mükerrer yevmiye kontrolü
- Detaylı konsol raporları (analiz, eksik hesap, denge)
- App.config üzerinden tam yapılandırılabilir

## Gereksinimler

- .NET Framework 4.8
- MSSQL Server (Mikro ERP V16 Jump veritabanı)
- E-Defter Yevmiye XML dosyaları

## Kurulum

1. Projeyi derleyin (`Debug` veya `Release`)
2. `App.config` içindeki ayarları ortamınıza göre düzenleyin:
   - `MikroDB` connection string
   - `EdDefterRootPath` — E-Defter kök dizini
   - `SicilNo` — Firma sicil numarası
   - `MaliYilAraligi` — Mali yıl aralığı klasör adı
   - `AyKlasoru` — Çalışılacak ay klasörü
3. `Defter2Fis.ForMikro.exe` çalıştırın

## Yapı

```
Defter2Fis.ForMikro/
├── Models/
│   ├── EdDefterModels.cs      # XML parse DTO'ları
│   └── MikroDbModels.cs       # DB entity modelleri
├── Services/
│   ├── EdDefterXmlParser.cs   # XBRL-GL XML parser
│   ├── MikroDbService.cs      # Mikro ERP DB servisi
│   └── DefterAnalyzer.cs      # Analiz ve raporlama
├── Program.cs                  # Konsol giriş noktası
├── App.config                  # Yapılandırma
└── Properties/
    └── AssemblyInfo.cs
```