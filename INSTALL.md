# Kurulum (Installation)

## Gereksinimler
- .NET Framework 4.8 Runtime
- MSSQL Server (Mikro ERP V16 Jump veritabanı erişimi)
- E-Defter Yevmiye XML dosyaları

## Yapılandırma

`App.config` dosyasında aşağıdaki ayarları ortamınıza göre düzenleyin:

### Connection String
```xml
<connectionStrings>
    <add name="MikroDB"
         connectionString="Data Source=hzk-pc\sqlexpress;Initial Catalog=MikroDB_V16_PRIMA2022;Integrated Security=True"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

### E-Defter Ayarları
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
