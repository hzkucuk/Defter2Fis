# Kurulum (Installation)

## Gereksinimler
- Windows 10/11 (64-bit)
- .NET Framework 4.8 Runtime
- MSSQL Server (Mikro ERP V16 Jump veritabanı erişimi)
- E-Defter Yevmiye XML dosyaları

## Installer ile Kurulum (Önerilen)

1. [Releases](https://github.com/hzkucuk/Defter2Fis/releases) sayfasından `Defter2Fis_Setup_vX.Y.Z.exe` indirin
2. Setup dosyasını çalıştırın
3. Kurulum sihirbazını takip edin
4. Kurulum tamamlandığında uygulamayı başlatın

> **Not:** Installer, .NET Framework 4.8 yüklü olup olmadığını otomatik kontrol eder. Yüklü değilse indirme bağlantısı gösterilir.

## Kaynak Koddan Derleme

```bash
msbuild Defter2Fis.ForMikro\Defter2Fis.ForMikro.csproj /p:Configuration=Release
```

### Installer Yeniden Derleme

Inno Setup 6 kurulu olmalıdır:

```bash
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" Installer\Defter2Fis.iss
```

Çıktı: `Installer\Output\Defter2Fis_Setup_vX.Y.Z.exe`

## Yapılandırma

### İlk Kurulum — Veritabanı Bağlantısı (Zorunlu)

Uygulama ilk açıldığında veritabanı bağlantısı yapılandırılmamıştır. Ekranın üstünde bir uyarı paneli göreceksiniz:

1. **Araçlar > Ayarlar** menüsünü açın (veya uyarı panelindeki "Ayarları Aç" bağlantısına tıklayın)
2. **"Bağlantı Oluştur..."** butonuna tıklayın
3. SQL Server bağlantı bilgilerinizi girin:
   - **Sunucu:** SQL Server adınız (ör: `localhost\sqlexpress`)
   - **Veritabanı:** Mikro ERP veritabanı adı (ör: `MikroDB_V16_FIRMA2024`)
   - **Kimlik Doğrulama:** Windows veya SQL Server Authentication
4. **"Bağlantıyı Test Et"** butonu ile bağlantıyı doğrulayın
5. **Tamam** ile bağlantıyı kaydedin
6. Ayarlar formunda **Kaydet** ile onaylayın

> **Not:** Bağlantı yapılandırılmadan veritabanı gerektiren işlemler (Mevcut Veri Kontrol, Önizleme, Fiş Oluştur, Yedek Al) devre dışıdır.

### Diğer Ayarlar

Ayarları 2 yoldan düzenleyebilirsiniz:

#### Yol 1: Uygulama İçinden (Önerilen)
Uygulamayı çalıştırın → **Araçlar > Ayarlar** menüsünden tüm parametreleri düzenleyin.

#### Yol 2: App.config Dosyasından

#### Connection String
```xml
<connectionStrings>
    <add name="MikroDB"
         connectionString=""
         providerName="System.Data.SqlClient" />
</connectionStrings>
```
> **Not:** Connection string varsayılan olarak boştur. Uygulama içi "Bağlantı Oluştur" ile yapılandırın.

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
4. **Önizleme** → Kuru çalıştırma ile oluşacak kayıtları önizle
5. **Fiş Oluştur** → Muhasebe fişlerini oluştur, cari/stok senkronize et
