﻿1) NuGet'ten Microsoft.EntityFrameworkCore.SqlServer ve Microsoft.EntityFrameworkCore.Tools paketleri indirilir.
2) Package Manager Console'da 
tablolara ulaşmak için oluşturulacak DbSet'lerin sonuna İngilizce çoğulluk için 's' eklenmesi isteniyorsa
Scaffold-DbContext "Server=.\SQLEXPRESS;Database=BA_KonuYorumCore;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir "DataAccess"
tablolara ulaşmak için oluşturulacak DbSet'lerin sonuna Türkçe veritabanı için 's' eklenmesi istenmiyorsa
Scaffold-DbContext "Server=.\SQLEXPRESS;Database=BA_KonuYorumCore;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir "DataAccess" -NoPluralize
komutu connection string, provider ve opsiyonel olarak OutputDir ile NoPluralize belirtilerek çalıştırılır.
Eğer daha önce DataAccess altında entity'ler ve context veritabanı üzerinden oluşturulmuşsa o zaman
Scaffold-DbContext "Server=.\SQLEXPRESS;Database=BA_KonuYorumCore;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir "DataAccess" -NoPluralize -Force
komutu kullanılmalıdır.
3) Bu işlemler bittikten sonra veritabanı tablo yapılarımızı tutan entity'ler ve
veritabanı yapımızı tutan ve veritabanındaki tablolarımıza ulaşmamızı sağlayan context, DataAccess klasörü altındadır.
4) Veritabanı yapısında değişiklik yapıldıkça, mesela tablo yapılarında bir değişiklik yapıldıysa, örneğin bir sütun adı
veya sütun veri tipi değişmiş olabilir, sütun eklenmiş olabilir, sütun silinmiş olabilir,
bir tablo adı değişmiş olabilir, tablo eklenmiş olabilir, tablo silinmiş olabilir, 
Package Manager Console üzerinden yukarıdaki Scaffold-DbContext komutu ile bu değişiklikler kod tarafına tekrar aktarılır.
5) İhtiyacımız olan veri erişim class'larını oluşturduğumuzdan artık uygulamayı geliştirmeye devam edebiliriz.
6) Eğer .NET 6 ile birlikte gelen string ve diğer referans tipler (class, array, vb.) için nullable olarak string? gibi sonda soru işareti kullanılmak istenmiyorsa
Solution Explorer'dan proje seçilip sağ tıklanarak Properties açılır. Soldaki General sekmesi üzerinden sağ tarafta Build -> General altından Nullable disable edilir.
Eğer bu yapılırsa string? yerine string rahatlıkla kullanılabilir. Aynı durum referans tipler için de geçerlidir.

Yapı:
View <-> Controller (Başlangıç noktası) <-> Model (entity -> context -> veritabanı)

Proje geliştirme aşamaları:
1) Konu Controller -> DbContext'ten türeyen context'in new'lenerek kullanılması
1.1) Index Action -> ScaffoldingTemplates altındaki dosyaların ReadMe.txt ile belirtilen klasörlere kopayalanması, konu listesinin (model) context üzerinden Index View'ına gönderilmesi ve Index View'ının oluşturulması (View scaffolding), ~/View/Shared/_Layout.cshtml'de sayfanın yukarısındaki menüye link'in eklenmesi
1.2) Details Action -> LINQ Find methodu
1.3) Create Action -> HttpGet ve HttpPost action method'ları, Create View'ındaki form verilerinin Create Post action'ına model olarak gönderilmesi ve veritabanında yeni kayıt oluşturulması, ViewBag ile ViewData kullanımı
1.4) Edit Action -> LINQ SingleOrDefault, FirstOrDefault ve LastOrDefault methodları
1.5) Delete Action -> LINQ Include methodu ve veritabanında ilişkili kayıtların yönetilmesi, TempData kullanımı
2) Yorum Controller
2.1) Index Action -> OrderBy ve ThenBy kullanımı
2.2) Details Action
2.3) Create Action -> SelectList ve MultiSelectList, post methodunda gelen model üzerinden validasyon işlemleri
2.4) Edit Action
2.5) Delete Action -> Silinmek istenen kaydın önce sayfada gösterilmesi, onaylandıktan sonra silinmesi, ActionName kullanımı
3) KonuScaffoldingController -> Scaffolding ile Entity Framework üzerinden controller ve view'lerin oluşturulması 
4) YorumScaffoldingController
5) KonularYorumlarJoinController
5.1) InnerJoin Action -> LINQ inner join kullanımı ve inner join sonucunun yeni bir model oluşturularak scaffolding ile oluşturulan view'a gönderilmesi, view'de gönderilen modelin kullanılması ve bu modele göre değişikliklerin yapılması
5.2) LeftOuterJoin Action -> LINQ left outer join kullanımı ve left outer join sonucunun yeni bir model oluşturularak scaffolding ile oluşturulan view'a gönderilmesi, view'de gönderilen modelin kullanılması ve bu modele göre değişikliklerin yapılması