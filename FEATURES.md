# Özellikler (Features)

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
- Detaylı konsol çıktısı

### Mikro ERP Entegrasyonu
- MUHASEBE_FISLERI tablosuna yazma altyapısı
- MUHASEBE_HESAP_PLANI tablosuna eksik hesap ekleme altyapısı
- Mükerrer yevmiye no kontrolü
- Transaction desteği
- Hesap tipi otomatik belirleme (kod derinliğine göre)

### Yapılandırma
- App.config üzerinden tüm ayarlar
- Connection string, E-Defter yolu, firma parametreleri
