# Kırtasiye Otomasyonu
## _C# Başlangıç Projem_

DX ile esnek, kullanıcı dostu arayüz.
Dinamik ekranlarla daha az sınıf, daha fazla kod.

- Proje için DX v22 gerektirir.
- DX için .net 4.8 kullanır.
- SQL Server 16 gerektirir.
## Özellikler

- Kullanıcı dostu satış ekranı (Ürün > Sepet > Onay > Satış)
- Dinamik Ana Ekran (Stok durumu, satışlar ve stok değeri > İleride: dinamik sorgu yazabilme ve uygulama)
- Satış raporları (İki tarih arasında gerçekleşen satışlar, ürün adına ve satış fiyatına göre. Ek: Stok miktarı N değerinden az olanları listeleme)
- Veri tablosu üzerinde read-only aktif filtreleme
- Tek bağlantı sınıfı ve config ile ayarlanabilir DB Connection.

İlk projem olduğu için hata barındırır ve clean code değildir.

## Kurulum

DX version 22 ve SQL Server version 16 gereklidir.

Proje içerisinde bulunan .bak dosyasını sql server'e restore edin.
