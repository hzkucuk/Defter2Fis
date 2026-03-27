# Kurulum (Installation)

## Gereksinimler
- .NET Framework 4.8 Runtime
- MSSQL Server (Mikro ERP V16 Jump veritabanı erişimi)
- E-Defter Yevmiye XML dosyaları

## Yapılandırma

Ayarları 2 yoldan düzenleyebilirsiniz:

### Yol 1: Uygulama İçinden (Önerilen)
Uygulamayı çalıştırın → **Araçlar > Ayarlar** menüsünden tüm parametreleri düzenleyin.

### Yol 2: App.config Dosyasından

#### Connection String
```xml
<connectionStrings>
    <add name="MikroDB"
         connectionString="Data Source=hzk-pc\sqlexpress;Initial Catalog=MikroDB_V16_PRIMA2022;Integrated Security=True"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

#### E-Defter Ayarları
```xml
<appSettings>
    <add key="EdDefterRootPath" value="C:\Mikro\v16xx\E_DEVLET\E_DEFTER" />
    <add key="SicilNo" value="7330029626" />
    <add key="MaliYilAraligi" value="01.01.2025-31.12.2025" />
    <add key="AyKlasoru" value="04" />
    <add key="FirmaNo" value="0" />
    <add key="SubeNo" value="0" />
    <add key="DBCNo" value="0" />
</appSettings>
```

## Derleme

```bash
msbuild Defter2Fis.slnx /p:Configuration=Debug
```

## Çalıştırma

```bash
Defter2Fis.ForMikro\bin\Debug\Defter2Fis.ForMikro.exe
```

## Kullanım Akışı

1. **Analiz Et** → XML dosyalarını parse et, raporu incele
2. **Mevcut Veri Kontrol** → Dönemde mevcut veri varsa listele
3. Gerekirse **Dönem Verisini Sil** → Çift onay ile mevcut veriyi temizle
4. **Fiş Oluştur** → Muhasebe fişlerini oluştur, cari/stok senkronize et
