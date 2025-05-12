
# ESA E-Ticaret

Bu proje, ASP.NET (MVC) ve SQLite veritabanı kullanılarak geliştirilen basit bir e-ticaret uygulamasıdır. Kullanıcılar ürünleri görüntüleyebilir, sepete ekleyebilir ve yönetici paneli üzerinden ürün, kategori ve kullanıcı işlemleri yapılabilir.

## Özellikler

- ✅ Ürün listeleme ve detay sayfaları  
- 🛒 Sepete ürün ekleme  
- 🔐 Kullanıcı girişi / çıkışı  
- ⚙️ Admin paneli:  
  - Ürün yönetimi  
  - Kategori yönetimi  
  - Sipariş yönetimi  
  - Kullanıcı yönetimi

## Teknolojiler

- ASP.NET MVC  
- Entity Framework  
- SQLite  
- Bootstrap  
- Razor View Engine

## Kurulum

1. Bu repoyu klonlayın:
   ```bash
   git clone https://github.com/kullaniciadi/esa-e-ticaret.git
   ```

2. Projeyi Visual Studio veya VS Code ile açın.

3. Gerekli NuGet paketlerini yükleyin:
   ```
   Tools > NuGet Package Manager > Package Manager Console
   ```

   Ardından:
   ```bash
   update-database
   ```

4. Uygulamayı çalıştırın (`localhost:5000`'da açılır).

## Admin Giriş Bilgileri

- **Kullanıcı adı:** `Admin`  
- **Şifre:** `Admin+123456`

## Ekran Görüntüleri

### Ana Sayfa
![Ana Sayfa](./Screenshots/ana sayfa.png)

### Admin Paneli
![Admin Paneli](./Screenshots/admin panel.png)

## Lisans

MIT Lisansı altında lisanslanmıştır. Daha fazla bilgi için `LICENSE` dosyasını inceleyin.

