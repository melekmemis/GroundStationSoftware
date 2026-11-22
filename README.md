# Roket Yer İstasyonu Yazılımı

Teknofest Roket Yarışması için geliştirdiğim, roket ve görev yükünden seri port üzerinden gelen telemetriyi gerçek zamanlı çözen, gösteren ve kaydeden bir Windows masaüstü uygulaması (C# / Windows Forms).

## Özellikler

- **Çoklu seri port:** Roket, görev yükü ve hakem yer istasyonu (HYI) hattı için ayrı seri portlar. Port adı, baud rate, veri biti, parite ve stop biti arayüzdeki port ayarları penceresinden seçilir.
- **İkili paket çözümleme:** Seri porttan gelen bayt akışı başlık baytı aranarak senkronize edilir, sabit uzunluklu paketler (roket: 70 bayt, görev yükü: 41 bayt) çözülür. Kayan veya bozuk baytlar başlık bulunana kadar atlanır; alım tamponu için taşma sınırı vardır.
- **Canlı telemetri paneli:** Enlem, boylam, GPS irtifası, irtifa, uydu sayısı, zaman, ivme (Ax/Ay/Az), jiroskop (Gx/Gy/Gz), yönelim açıları (X/Y/Z) ve uçuş durumu anlık gösterilir. Görev yükü için nem ve sıcaklık verisi de çözülür.
- **3B yönelim görünümü:** `roket.stl` modeli HelixToolkit ile gösterilir ve telemetrideki açılara göre gerçek zamanlı döndürülür.
- **Harita takibi:** GMap.NET üzerinde roket ve görev yükü iki ayrı işaretçiyle gösterilir, konum 1 saniyede bir güncellenir.
- **Grafikler:** İrtifa ve ivme için zaman eksenli grafikler.
- **Veri kaydı:** Roket ve görev yükü verileri, kaydı başlat/durdur butonlarıyla iki ayrı metin dosyasına yazılır (uçuş sonrası analiz için).
- **Hakem istasyonu paketi:** Roket ve görev yükü verilerinden, sabit başlıklı, sayaçlı ve sağlama toplamlı 78 baytlık bir paket oluşturulur.
- **Özel arayüz:** Çerçevesiz, yuvarlatılmış köşeli, koyu temalı pencere; sürüklenebilir başlık alanı, küçült / büyüt / kapat kontrolleri ve özel kontroller (`customs/`).

## Sistem Akışı

```
Roket ──────────► alıcı modül ──► seri port (R)  ──┐
                                                   │
Görev yükü ─────► alıcı modül ──► seri port (GY) ──┼──► Yer İstasyonu (Windows Forms)
                                                   │       ├─ canlı telemetri paneli
Hakem yer istasyonu ◄── seri port (HYI) ◄──────────┘       ├─ 3B model + harita + grafikler
                                                           └─ metin dosyasına kayıt
```

Uçuş sırasında telemetri 433 MHz LoRa bağlantısıyla ~8 km mesafeden alındı; rampada bekleme ve uçuş boyunca yaklaşık 4.000 paket 5 Hz ile alınıp kaydedildi.

## Telemetri Paket Formatları

Tüm çok baytlı alanlar little-endian'dır (`BitConverter`). Enlem ve boylam, gönderici tarafta 10⁶ ile çarpılmış `float` olarak iletilir; yer istasyonu 1.000.000'a bölerek derece cinsine çevirir.

### Roket paketi (70 bayt)

| Offset | Boyut | Alan |
|-------:|------:|------|
| 0 | 1 | Başlık (`0xAA`) |
| 1 | 4 | Enlem (float) |
| 5 | 4 | Boylam (float) |
| 9 | 4 | GPS irtifası (float) |
| 13 | 4 | İrtifa (float) |
| 17 | 4 | Uydu sayısı (int32) |
| 21 | 4 | Saat (int32) |
| 25 | 4 | Dakika (int32) |
| 29 | 4 | Saniye (int32) |
| 33 | 12 | İvme Ax, Ay, Az (3 × float) |
| 45 | 12 | Jiroskop Gx, Gy, Gz (3 × float) |
| 57 | 12 | Açı X, Y, Z (3 × float) |
| 69 | 1 | Durum (byte) |

### Görev yükü paketi (41 bayt)

| Offset | Boyut | Alan |
|-------:|------:|------|
| 0 | 1 | Başlık (`0x0B`) |
| 1 | 4 | Enlem (float) |
| 5 | 4 | Boylam (float) |
| 9 | 4 | GPS irtifası (float) |
| 13 | 4 | İrtifa (float) |
| 17 | 4 | Uydu sayısı (int32) |
| 21 | 4 | Saat (int32) |
| 25 | 4 | Dakika (int32) |
| 29 | 4 | Saniye (int32) |
| 33 | 4 | Nem (float) |
| 37 | 4 | Sıcaklık (float) |

### Hakem istasyonu paketi (78 bayt)

Paket 4 baytlık sabit başlıkla (`0xFF 0xFF 0x54 0x52`) başlar; ardından takım kimliği, paket sayacı ve roket / görev yükü irtifa-konum değerleri (4 baytlık `float` alanlar) gelir. Sağlama toplamı, 4. bayttan 74. bayta kadar olan baytların toplamının mod 256 değeridir.

## Kullanılan Teknolojiler

| Alan | Teknoloji |
|------|-----------|
| Dil / arayüz | C#, Windows Forms |
| Seri haberleşme | `System.IO.Ports.SerialPort` |
| 3B görüntüleme | WPF + HelixToolkit (`ElementHost` ile Windows Forms içine gömülü) |
| Harita | GMap.NET |
| Grafik | `System.Windows.Forms.DataVisualization.Charting` |
| Veri işleme | Newtonsoft.Json |

## Proje Yapısı

```
├── MainForm.cs / .Designer.cs / .resx   # Ana pencere: port okuma, paket çözme, arayüz, harita, 3B model, kayıt
├── PortSettings.cs / .Designer.cs       # Seri port ayar penceresi
├── Program.cs                           # Giriş noktası
├── customs/                             # Özel arayüz bileşenleri
├── resources/                           # Görsel kaynaklar
├── roket.stl / roket.obj                # 3B roket modeli
└── YerIstasyonuV1.sln / .csproj         # Visual Studio çözümü
```

## Kurulum ve Çalıştırma

**Gereksinimler:** Windows, Visual Studio 2019 veya üzeri (.NET Framework hedefi için `YerIstasyonuV1.csproj` dosyasına bakın).

1. Depoyu klonlayın:
   ```bash
   git clone https://github.com/melekmemis/GroundStationSoftware.git
   ```
2. `YerIstasyonuV1.sln` dosyasını Visual Studio ile açın.
3. NuGet paketlerini geri yükleyin (GMap.NET, HelixToolkit, Newtonsoft.Json).
4. `roket.stl` dosyasının çalıştırılabilir dosyanın yanında (çıktı klasöründe) bulunduğundan emin olun; model göreli yoldan yüklenir.
5. Çalıştırın, **Port Ayarları** penceresinden roket / görev yükü / HYI portlarını seçin ve bağlanın.
6. Veriyi dosyaya yazmak için **Kaydet** butonunu kullanın; iki ayrı dosya yolu (roket ve görev yükü) sorulur.

> Uygulama, doğru formatta paket gönderen bir verici veya seri port kaynağı gerektirir. Donanım olmadan denemek için sanal seri port çifti (ör. com0com) ve paket formatına uygun bir test betiği kullanılabilir.

## Yol Haritası

- [ ] Paketlere sağlama toplamı / CRC ekleyip alıcı tarafında doğrulama
- [ ] Kayıt formatını zaman damgalı, paket başına bir satırlık CSV'ye taşıma
- [ ] Donanımsız test için telemetri simülatörü
- [ ] Ekran görüntüleri ve kısa demo videosu

## Yazar

**Melek Memiş** — Gömülü Sistemler & Aviyonik Mühendisi
[LinkedIn](https://linkedin.com/in/melekmemis) · [GitHub](https://github.com/melekmemis) · [Portfolyo](https://melekmemis.github.io)
