# Özellikler (Features)

## v2.2.0 — Krypton UI + Önizleme/Test Modülü

### Krypton Toolkit Entegrasyonu (YENİ)
- **Krypton.Toolkit v85.24.6.176** — Modern Microsoft 365 Blue temalı arayüz
- Tüm formlar `KryptonForm` tabanlı (MainForm, AyarlarForm, HakkındaForm)
- `KryptonManager` ile uygulama geneli tema yönetimi (Microsoft365Blue palette)
- Krypton kontrolleri: KryptonButton, KryptonLabel, KryptonGroupBox, KryptonDataGridView, KryptonRichTextBox

### Önizleme/Test Modülü (YENİ — KRİTİK)
- **Önizleme** butonu — Fiş oluşturma öncesi kuru çalıştırma (dry-run), veritabanına yazma yapılmaz
- `OnizlemeServisi` — FisOlusturmaServisi mantığını simüle eden önizleme servisi
- **Özet İstatistikler** paneli:
  - Oluşturulacak fiş sayısı, fiş satır sayısı
  - Eşleşen cari hareket ve stok hareket sayısı
  - Eksik hesap sayısı, uyarı sayısı
- **5 Detay Sekmesi:**
  - **Fişler** — Oluşturulacak muhasebe fiş kayıtları (yevmiye no, tarih, hesap, tutar, B/A)
  - **Cari Eşleşmeleri** — Cari hareket ↔ muhasebe fişi eşleştirme önizlemesi
  - **Stok Eşleşmeleri** — Stok hareket ↔ muhasebe fişi eşleştirme önizlemesi
  - **Eksik Hesaplar** — Hesap planına eklenecek eksik hesaplar listesi
  - **Uyarılar** — Mükerrer yevmiye, denge hatası, düşük eşleşme oranı, üzerine yazma uyarıları
- Uyarı seviyeleri: Bilgi, Uyarı, Kritik (renk kodlu)

### Önizleme Veri Modelleri (YENİ)
- `OnizlemeSonucu` — Hesaplanmış özellikler (OlusturulacakFisSayisi, EslesenCariSayisi, KritikUyariVar)
- `OnizlemeFisKaydi`, `OnizlemeCariEslesmesi`, `OnizlemeStokEslesmesi`, `OnizlemeEksikHesap`
- `OnizlemeUyari` + `UyariSeviye` enum (Bilgi/Uyari/Kritik)

### UI Geliştirmeleri
- 3 ana sekme yapısı: Loglar / Mevcut Dönem Verisi / Önizleme
- Önizleme sekmesi: Özet panel (üst) + 5 detay sekmesi (alt)
- KryptonDataGridView ile zengin veri gösterimi (sıralama, kolon boyutlandırma)

## v2.1.0 — Fiş Oluşturma ve Cari/Stok Senkronizasyonu

### Fiş Oluşturma (YENİ)
- **Fiş Oluştur** butonu — E-Defter verilerinden Mikro ERP muhasebe fişleri tek tıkla oluşturma
- Otomatik eksik hesap planı tespiti ve ekleme (fiş yazma öncesi)
- Tarih bazlı fiş sıralama ve sıra no atama (MAX+1)
- Mükerrer yevmiye kontrolü ile güvenli fiş yazma
- Transaction bazlı atomik fiş yazma (her yevmiye fişi)
- Onay dialogu ile güvenli işlem başlatma

### Evrak Bilgisi Parse (YENİ)
- E-Defter `detailComment`, `documentNumber`, `documentReference` alanlarından evrak seri/no çıkarma
- Çoklu Regex pattern desteği: `SERİ:A NO:123`, `A-000123`, `A/000123`, `A 000123`, `A000123`
- Mikro ERP evrak formatları ile uyumlu

### Cari/Stok Hareket Senkronizasyonu (YENİ)
- `CARI_HESAP_HAREKETLERI` tablosu dönem sorgulama ve evrak eşleştirme
- `STOK_HAREKETLERI` tablosu dönem sorgulama ve evrak eşleştirme
- Evrak seri+sıra bazlı otomatik eşleştirme (index tabanlı)
- Muhasebe fiş referansı güncelleme (`cha_muh_fis_no`, `sth_muh_fis_no`)
- Ticari referans alanları otomatik doldurma (`fis_tic_evrak_seri`, `fis_tic_evrak_sira`, `fis_ticari_tip`)
- Tablo yoksa güvenli atlanma (SqlException 208 handling)

### Orkestrasyon Servisi (YENİ)
- `FisOlusturmaServisi` — Tüm süreci yöneten merkezi servis
- BackgroundWorker ilerleme raporlama desteği
- Detaylı sonuç raporu (oluşturulan fiş, satır, eşleşen cari/stok, hatalar)

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
