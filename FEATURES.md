# Özellikler (Features)

## v2.0.0 — WinForms UI

### Masaüstü Arayüzü (YENİ)
- WinForms ana form: menü çubuğu, ayar özeti, işlem butonları
- **Dönem Bilgileri** paneli — Mevcut E-Defter yolu, DB bağlantısı, firma bilgileri
- **İşlem Logları** sekmesi — RichTextBox tabanlı renkli log (Bilgi/Başarı/Uyarı/Hata)
- **Mevcut Dönem Verisi** sekmesi — DataGridView ile mevcut fiş listesi
- BackgroundWorker arka plan işlem desteği + ProgressBar
- StatusStrip durum çubuğu

### Dönem Veri Kontrolü (KRİTİK — YENİ)
- İşlem öncesi mevcut dönem verisi sorgulaması (pre-check)
- Mevcut yevmiye fişlerini DataGridView'de listeleme
- Çift onaylı güvenli silme (2 MessageBox onayı)
- Transaction ile atomik silme (hata durumunda rollback)

### Ayarlar Diyalogu (YENİ)
- Veritabanı bağlantı dizesi düzenleme
- E-Defter kök dizin seçici (FolderBrowserDialog)
- Sicil No, Mali Yıl, Ay parametreleri
- Mikro ERP Firma/Şube/DBC parametreleri
- App.config'e kaydetme ve runtime yenileme

### Hakkında Diyalogu (YENİ)
- Versiyon bilgisi (Assembly'den dinamik)
- Uygulama açıklaması ve telif hakkı

### Log Altyapısı (YENİ)
- Event tabanlı merkezi LogService
- 4 seviye: Bilgi, Başarı, Uyarı, Hata
- Thread-safe UI güncellemesi (BeginInvoke)

## v1.0.0

### E-Defter XML Parse
- XBRL-GL Yevmiye XML dosyalarını otomatik bulma ve parse etme
- `accountingEntries > entryHeader > entryDetail` hiyerarşisi tam destek
- Çoklu dosya desteği (parçalı defterler)
- Namespace: `gl-cor`, `gl-bus`, `xbrli`

### Analiz ve Doğrulama
- Borç-Alacak denge kontrolü (her fiş için)
- Eksik hesap planı tespiti (ana, alt ve ara seviye)
- DB mevcut durum raporu (hesap planı sayısı, mevcut yevmiye sayısı)

### Mikro ERP Entegrasyonu
- MUHASEBE_FISLERI tablosuna yazma altyapısı
- MUHASEBE_HESAP_PLANI tablosuna eksik hesap ekleme altyapısı
- Mükerrer yevmiye no kontrolü
- Transaction desteği
- Hesap tipi otomatik belirleme (kod derinliğine göre)

### Yapılandırma
- App.config üzerinden tüm ayarlar
- Connection string, E-Defter yolu, firma parametreleri
