# Özellikler (Features)

## v2.12.0 — Mükerrer Kontrolü Bilgi Amaçlı + Üzerine Yazma

### Üzerine Yazma (DELETE-before-INSERT)
- DB'de mevcut yevmiye fişleri atlanmak yerine silinip yeniden yazılır
- `YevmiyeFisleriniSil`: Yevmiye numarasına göre mevcut satırları transaction içinde siler
- Tüm e-Defter kayıtları eksiksiz MUHASEBE_FISLERI tablosuna kaydedilir
- Mükerrer sayısı kullanıcıya bilgi olarak gösterilir (döküm/rapor amaçlı)

### Önizleme Güncelleme
- Mükerrer uyarısı: "atlanacak" → "üzerine yazılacak" (Bilgi seviyesi)
- Sonuç raporu: "Atlanan (mükerrer)" → "Üzerine yazılan"

## v2.11.1 — Mükerrer Kontrol ve SQL INSERT Düzeltmesi

### Mükerrer Kontrol İyileştirmesi
- `YevmiyeNoMevcutMu` sorgusu `NDX_MUHASEBE_FISLERI_02` unique index ile uyumlu
- `fis_tur`, `fis_iptal` ve tarih aralığı filtreleri kaldırıldı (index bunları içermiyor)
- Farklı fiş türü/iptal durumundaki mükerrer yevmiyeler artık doğru tespit ediliyor

### SQL INSERT Düzeltmesi
- `fis_ticari_tip`, `fis_ticari_evraktip`, `fis_tic_evrak_seri`, `fis_tic_evrak_sira` parametrize edildi
- Hata loglama: SqlException detayları (hata no, satır, durum, prosedür) loglanıyor

## v2.11.0 — Cari/Stok Yazma İşlemlerini Kaldırma

### Cari/Stok Salt Okunur Mod
- Cari/stok tablolarına UPDATE yapılmıyor (CARI_HESAP_HAREKETLERI, STOK_HAREKETLERI)
- Sadece MUHASEBE_FISLERI tablosuna fiş oluşturma yapılır
- Cari/stok eşleştirme bilgi amaçlı olarak önizleme gridinde gösterilir
- Ticari eşleştirme bilgisi (FisTicariTip/FisTicariUid) muhasebe fişine yazılır

## v2.10.0 — Cari/Stok Eşleştirme: Tarih+FişNo Bazlı

### Tarih+FişNo Bazlı Eşleştirme (YENİ)
- Cari hareketler: `cha_fis_tarih` + `cha_fis_sirano` üzerinden eşleştirme
- Stok hareketler: `sth_fis_tarihi` + `sth_fis_sirano` üzerinden eşleştirme
- Evrak seri/sıra bazlı eşleştirme kaldırıldı
- Fiş numarası her gün için 1'den başlar

### Açıklama Tam Gösterim
- Önizleme gridinde açıklama artık kesilmeden tamamı gösteriliyor

## v2.9.2 — Mükerrer Dönem Filtresi Düzeltmesi

### Dönem Bazlı Mükerrer Kontrol
- `YevmiyeNoMevcutMu` sorgusu artık dönem tarih aralığı ile filtreleniyor
- Farklı aylardaki aynı yevmiye numaraları yanlış mükerrer olarak işaretlenmiyor
- Önizleme ve fiş oluşturma her ikisi de dönem filtresi kullanıyor

## v2.9.1 — Sıra No ve Evrak Parse Düzeltmesi

### Mükerrer SıraNo Düzeltmesi
- Mükerrer fişlere de sıralı numara atanır (önizlemede 0 yerine gerçek sıra görünür)
- Mükerrer bayrağı korunur — fiş oluşturmada yine atlanır

### ETTN/e-Fatura Evrak Parse (YENİ)
- **PatternEttn** — 3-char prefix + yıl + sıra formatı tanınır (ör: TA4202500000642 → Seri=TA4, Sıra=642)
- **PatternEdDefterAciklama** — DetayAciklama'daki `<EvrakTipi> : <BelgeNo>/<Tarih>/...` formatı parse edilir
- Desteklenen prefixler: TA4, GB3, A16, C01, P01 ve benzerleri
- Sayısal belge numaraları (ör: C.A.V.D : 6933/...) da açıklama formatından yakalanır

### 84/84 Birim Testi
- 12 yeni test eklendi (ETTN + DetayAciklama + Anahtar)

## v2.9.0 — Grid Filtre / Arama / Sıralama

### FilterableGridHelper<T> (YENİ)
- **Generic filtre** — LINQ Where ile tüm public property'lerde case-insensitive metin arama
- **Kolon sıralama** — PropertyInfo.GetValue + OrderBy/OrderByDescending, kolon başlığı tıklama
- **Sıralama göstergesi** — SortGlyphDirection ile ASC/DESC ok işareti
- **Satır renklendirme callback** — VeriYukle'ye opsiyonel Action<DataGridViewRowCollection> parametresi
- **Yükleme flag** — _yukleniyor ile VeriYukle sırasında TextChanged bastirılır

### 6 Grid Filtre Entegrasyonu (YENİ)
- Mevcut Dönem Verisi, Fişler, Cari, Stok, Eksik Hesap, Uyarılar gridlerinin hepsinde filtre
- Her grid üzerinde karanlık temalı KryptonTextBox arama kutusu
- Mükerrer satır (kırmızı) ve uyarı seviye renkleri filtre sonrası da korunur
- FiltreAlanOlustur: TLP + TextBox + Grid layout yardımcısı
- FiltreHelperBaslat: 6 helper instance oluşturma
- Form kapanışında Dispose ile temizlik

## v2.8.0 — Birim Testleri

### Test Projesi (YENİ)
- **DefterAnalyzerTests** — 17 test: AraSeviyeleriUret, OzetHesapla, BenzersizHesapKodlari, EksikHesaplariTespit, DengeKontrolu
- **EdDefterXmlParserTests** — 14 test: DosyadanOku, KlasordenOku, hata durumları, opsiyonel alanlar
- **DefterAnalyzerMockTests** — 15 test: OncekiAyDogrula, DbDurumGetir (Moq ile)
- **72/72 test geçiyor** (NUnit 3.14.0 + Moq 4.20.72)
- Test projesi packages.config → PackageReference formatına geçiş

## v2.7.1 — Analiz Raporu Birleşik Gösterim

### Birleşik Rapor Bloğu (İYİLEŞTİRME)
- **Tek başlık altında alt alta gösterim** — Önceki ay ve çalışılan ay tek rapor bloğunda
- `--- Onceki Ay ---` ve `--- Calisilan Ay ---` alt başlıklarıyla ayrım
- Her iki ay için: dosya, firma, dönem, fiş/satır, yevmiye aralığı
- Önceki ay XML sessiz okunur, hata durumunda rapor içinde uyarı

## v2.7.0 — Önceki Ay E-Defter XML Bilgisi Gösterimi

### Önceki Ay XML Raporu (YENİ)
- Önceki ay E-Defter XML'i otomatik okunur ve analiz raporunda gösterilir
- Mali yıl ilk ayı (01) için önceki ay gösterimi atlanır

## v2.6.1 — Yevmiye Sürekliliği Bug Fix (Tarih → Yevmiye Bazlı)

### Yevmiye-Numarası-Bazlı Süreklilik Kontrolü (DÜZELTİLDİ — KRİTİK)
- **Tarih-bazlı sorgu kaldırıldı** — `postingDate` takvim ayı sınırlarıyla örtüşmediği için yanlış yevmiye aralıkları döndürüyordu
- **Yevmiye-numarası-bazlı sorgu** — `MAX(fis_yevmiye_no)` ve `COUNT(DISTINCT fis_yevmiye_no)` ile `WHERE fis_yevmiye_no < çalışılanMinYevmiye`
- **İlk ay tespiti** — `xmlMinYevmiye == 1` kontrolü (takvim ayı karşılaştırması yerine)
- **YevmiyeSureklilkBilgisi** DTO — Mali yıl genelinde yevmiye count/max (tarihten bağımsız)
- `AyFisBilgisiGetir` yalnızca bilgilendirme amaçlı korundu

## v2.6.0 — Önceki Ay Doğrulama, Yevmiye Sürekliliği, Atomik Ay Operasyonları

### Önceki Ay Doğrulama (YENİ — KRİTİK)
- **Analiz aşamasında önceki dönem kontrolü** — Çalışılan aydan önceki yevmiyelerin DB'de mevcut olup olmadığı kontrol edilir
- **Mali yıl ilk dönem istisnası** — Yevmiye 1'den başlıyorsa önceki dönem kontrolü gerekmez
- **YevmiyeSureklilkBilgisiGetir** DB sorgusu — Yevmiye numarası bazlı: yevmiye sayısı, MAX yevmiye no

### Yevmiye Sürekliliği (YENİ — KRİTİK)
- **Dönemler-arası süreklilik** — DB'deki son yevmiye numarası + 1 = çalışılan ayın ilk yevmiye numarası
- **Ay-içi süreklilik** — Yevmiye numaralarının ardışık ve boşluksuz olduğu doğrulanır
- Detaylı log mesajları: yevmiye aralıkları, beklenen başlangıç, boşluk tespiti

### Simülasyon-Önce Yaklaşım (YENİ)
- **Faz 1 — Simülasyon**: Tüm MuhasebeFisi objeleri bellekte oluşturulur, mükerrer kontrolü yapılır, sıra no hesaplanır
- **Faz 2 — Atomik Yazım**: Simülasyon başarılıysa tüm fişler tek transaction içinde DB'ye yazılır
- Simülasyon başarısızsa DB'ye hiç yazım yapılmaz

### Atomik Ay Operasyonları (YENİ — KRİTİK)
- **Tek transaction** — Tüm ayın fişleri + cari/stok referans güncellemeleri tek transaction içinde
- **Tam rollback** — Herhangi bir hata durumunda tüm ay geri alınır, kısmi veri bırakılmaz
- Eski per-fiş transaction yaklaşımı kaldırıldı

### Fiş Oluşturma Güvenlik Kontrolleri (YENİ)
- Analiz yapılmadan fiş oluşturma engellenir
- Aktarım izni yoksa (süreklilik sağlanamadıysa) fiş oluşturma butonu bloklanır
- Onay diyaloğunda yevmiye süreklilik bilgisi gösterilir

## v2.5.0 — Yedekleme Düzeltme + İlerleme ve Bilgilendirme İyileştirmesi

### Yedekleme Düzeltmesi
- **OS error 3 düzeltmesi** — SQL Server varsayılan yedek dizini fiziksel olarak yoksa `xp_create_subdir` ile oluşturulur
- SQL Server servis hesabı kendi izinleriyle dizin oluşturur (OS izin sorunları önlenir)

### Gerçek Zamanlı İlerleme Çubuğu (YENİ)
- **BACKUP DATABASE ilerleme** — `STATS=5` çıktısı `SqlInfoMessage` event'i ile yakalanarak ProgressBar'a yansıtılır
- `VeritabaniYedekle` methodu `Action<int, string>` callback desteği ile ilerleme bildirir
- Yedekleme sırasında yüzde gösterimi: "Yedekleniyor... %5", "%10", ..., "%95", "Yedek tamamlandi."

### İşlem Sonuç Bilgilendirmesi (YENİ)
- Tüm 6 işlem tamamlandığında StatusBar'da özet gösterir:
  - Analiz: fiş sayısı, eksik hesap sayısı
  - Önizleme: fiş, cari, stok eşleşme sayıları
  - Yedek: dosya boyutu ve süresi
  - Fiş Oluşturma: başarı/hata durumu
  - Silme: silinen satır sayısı
  - Mevcut Veri Kontrol: yevmiye ve satır sayısı
- ProgressBar işlem bitiminden 2 sn sonra otomatik sıfırlanır

## v2.4.0 — IMikroDbService Interface Extraction

### Mimari İyileştirme
- **IMikroDbService** interface — veritabanı servisini soyutlayarak test edilebilirlik sağlar
- Tüm servisler (FisOlusturmaServisi, OnizlemeServisi, DefterAnalyzer) interface'e bağımlı
- Concrete MikroDbService sadece MainForm'da `new MikroDbService()` ile oluşturulur
- Mock tabanlı unit test yazılabilir hale geldi (Moq ile IMikroDbService mock'lanabilir)
- YedeklemeSonucu bağımsız top-level DTO olarak refactor edildi

## v2.3.1 — Dark Tema

### Dark Tema (YENİ)
- **Krypton Microsoft365BlackDarkMode** palette
- Tüm formlar (MainForm, AyarlarForm, HakkindaForm) dark temada
- `DarkMenuColorTable` — MenuStrip özel dark renk tablosu
- DataGridView dark renklendirme (koyu bg, açık text, mavi seçim)
- Log renkleri dark arka plan için optimize (LimeGreen/Orange/Tomato)
- StatusStrip VS-style mavi (0, 122, 204)

## v2.3.0 — Veritabanı Yedekleme + Hata Yönetimi + Refactoring

### Veritabanı Yedekleme (YENİ — KRİTİK)
- **Yedek Al** butonu — Tek tıkla tam veritabanı yedek alma
- `BACKUP DATABASE` SQL komutu (INIT, COMPRESSION, 10 dakika timeout)
- Yedek dosyası: `{UygulamaDizini}\Yedekler\{DBAdı}_{YYYYMMDD_HHmmss}.bak`
- Dosya boyutu ve süre raporlama (insan okunabilir format)
- **İşlem öncesi yedek teklifi** — Fiş oluşturma ve dönem silme öncesi Yes/No/Cancel dialog
  - Evet: Yedek al ve devam
  - Hayır: Yedeksiz devam
  - İptal: İşlemi durdur

### Hata Yönetimi İyileştirmeleri (YENİ)
- **Global exception handler** — `Application.ThreadException` + `AppDomain.UnhandledException`
  - UI thread hataları: Kullanıcıya mesaj göster, uygulama devam etsin
  - Kritik hatalar: Kullanıcıya bilgi ver, uygulama kapatılsın
- BackgroundWorker hata raporlama — Inner exception detayı + MessageBox
- Fiş oluşturma hatalarında inner exception zincirleme (SQL hata detayları)

### Refactoring
- `FisOlusturmaServisi` ve `OnizlemeServisi`'ndeki 4'er mükerrer metot kaldırıldı
- Paylaşımlı metotlar `MikroDbService`'e taşındı:
  - `DonemCariHareketleriGetirGuvenli()` — Tablo yoksa güvenli boş liste döner
  - `DonemStokHareketleriGetirGuvenli()` — Tablo yoksa güvenli boş liste döner
  - `CariIndexOlustur()` (static) — Evrak anahtarı bazlı Dictionary index
  - `StokIndexOlustur()` (static) — Evrak anahtarı bazlı Dictionary index
- Kullanılmayan `using` direktifleri temizlendi

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
