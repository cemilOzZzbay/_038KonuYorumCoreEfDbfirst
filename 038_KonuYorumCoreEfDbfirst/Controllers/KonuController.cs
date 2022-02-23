using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _038_KonuYorumCoreEfDbfirst.DataAccess;

namespace _038_KonuYorumCoreEfDbfirst.Controllers
{
    public class KonuController : Controller
    {
        private BA_KonuYorumCoreContext _db = new BA_KonuYorumCoreContext(); // default private oluşur
        public IActionResult Index()
        {
            List<Konu> konular = _db.Konu.ToList();
            return View(konular);
        }
        public IActionResult Details(int id)
        {
            Konu konu = _db.Konu.Find(id);
            return View(konu);
        }
        [HttpGet] // Eğer yazılmazsa default olarak her aksiyon HttpGet'tir
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost] // Sunucuya bir form veya başka bir yol ile veri gönderiliyorsa mutlaka HttpPost yazılmalıdır  
        public IActionResult Create(Konu konu)
        {
            if (string.IsNullOrWhiteSpace(konu.Baslik))
            {
                // ViewBag(özellik) ile ViewData(index) birbirleri yerine aynı özellik ve index adları üzerinden kullanılabilir.
                //ViewData["Mesaj"] = "Başlık boş girilemez!";
                ViewBag.Mesaj = "Başlık boş girilemez!";
                
                return View(konu);
            }
            if (konu.Baslik.Length > 100)
            {
                ViewBag.Mesaj = "Başlık en fazla 100 karakter olmalıdır!";
                return View(konu);
            }
            if (!string.IsNullOrWhiteSpace(konu.Aciklama)&& konu.Aciklama.Length > 200)
            {
                ViewBag.Mesaj = "Açıklama en fazla 200 karakter olmalıdır!";
                return View(konu);
            }
            _db.Konu.Add(konu);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet] // Eğer yazılmazsa default olarak her aksiyon HttpGet'tir
        public IActionResult Edit(int id)
        {
            //Konu konu=_db.Konu.Find(id);

            //Konu konu = _db.Konu.First(konu => konu.Id == id);// Eğer belirtilen koşula uygun sonuç dönmezse Exception fırlatır, eğer belirtilen koşula uygun birden çok kayıt dönerse de her zaman ilk kaydı döner

            //Konu konu = _db.Konu.FirstOrDefault(konu => konu.Id == id);// Eğer belirtilen koşula uygun sonuç dönmezse Null döner, eğer belirtilen koşula uygun birden çok kayıt dönerse de her zaman ilk kaydı döner
            //Konu konu = _db.Konu.Last(konu => konu.Id == id);// Eğer belirtilen koşula uygun sonuç dönmezse Exception fırlatır, eğer belirtilen koşula uygun birden çok kayıt dönerse de her zaman son kaydı döner

            //Konu konu = _db.Konu.LastOrDefault(konu => konu.Id == id);// Eğer belirtilen koşula uygun sonuç dönmezse Null döner, eğer belirtilen koşula uygun birden çok kayıt dönerse her zaman son kaydı döner

            //Konu konu = _db.Konu.Single(konu => konu.Id == id);// Eğer belirtilen koşula uygun sonuç dönmezse Exception fırlatır, eğer belirtilen koşula uygun birden çok kayıt dönerse de Exception fıtlatır

            Konu konu = _db.Konu.SingleOrDefault(konu => konu.Id == id); // Eğer belirtilen koşula uygun sonuç dönmezse Null döner, eğer belirtilen koşula uygun birden çok kayıt dönerse de Exception fırlatır.

            // Eğer expression olarak birden çok koşul kullanılmak isteniyorsa bu koşullar and(&&) veya or(||) ile birleştirilebilir, değil işlemi için de not(!) kullanılabilir

            return View(konu);

        }
        [HttpPost] // Sunucuya bir form veya başka bir yol ile veri gönderiliyorsa mutlaka HttpPost yazılmalıdır
        public IActionResult Edit(Konu konu)
        {
            if (string.IsNullOrWhiteSpace(konu.Baslik))
            {
                ViewData["Mesaj"] = "Başlık boş girilemez!";
                return View(konu);
            }
            if (konu.Baslik.Length > 100)
            {
                ViewBag.Mesaj = "Başlık en fazla 100 karakter olmalıdır!";
                return View(konu);
            }
            if (!string.IsNullOrWhiteSpace(konu.Aciklama) && konu.Aciklama.Length > 200)
            {
                ViewBag.Mesaj = "Açıklama en fazla 200 karakter olmalıdır!";
                return View(konu);
            }
            _db.Konu.Update(konu);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            // konu verileriyle birlikte Include kullanarak ilişkili yorum verileri de çekilir
            // eager loading: ihtiyaca göre yükleme, Entity Framework Core'da kullanılır
            // lazy loading: entity framework'ün otomatik olarak ilişkili verileri yüklemesi, Include kullanılmasına gerek yoktur
            Konu konu = _db.Konu.Include(k => k.Yorum).SingleOrDefault(k => k.Id == id);

            // 1.Yöntem: Konu ile birlikte ilişkili yorum kayıtlarının da silinmesi
            //if(konu.Yorum != null && konu.Yorum.Count > 0) // Yorum kayıtları doluysa
            //{
            //   foreach(Yorum yorum in konu.Yorum)
            //   {
            //        _db.Yorum.Remove(yorum);
            //   }
            //}
            //_db.Yorum.RemoveRange(konu.Yorum);
            
            // 2.Yöntem: Konunun ilişkili yorum kayıtları varsa uyarı verilmesi ve silinme işleminin yapılması
            if (konu.Yorum != null && konu.Yorum.Count > 0)
            {
                TempData["Mesaj"] = "Silinmek istenen konu ile ilişkili yorum kayıtları bulunmaktadır";
            }
            else
            {
                _db.Konu.Remove(konu);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        
    }
}
