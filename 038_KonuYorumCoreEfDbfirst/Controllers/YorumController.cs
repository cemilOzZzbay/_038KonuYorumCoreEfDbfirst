using Microsoft.AspNetCore.Mvc;
using _038_KonuYorumCoreEfDbfirst.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _038_KonuYorumCoreEfDbfirst.Controllers
{
    public class YorumController : Controller
    {
        BA_KonuYorumCoreContext _db = new BA_KonuYorumCoreContext(); // default private oluşur
        public IActionResult Index()
        {
            // select * from Yorum order by Puan desc
            //List<Yorum> yorumlar = _db.Yorum.OrderByDescending(yorum => yorum.Puan).ToList(); // OrderBy, OrderByDescending

            // select * from Yorum inner join Konu on Yorum.Konu.Id = order by Puan desc, Yorumcu
            List<Yorum> yorumlar = _db.Yorum.Include(yorum => yorum.Konu).OrderByDescending(yorum => yorum.Puan).ThenBy(yorum => yorum.Yorumcu).ToList(); // ThenBy, ThenByDescending
            
            // Bir OrderBy kullanıldıktan sonra diğerlerinin hepsi ThenBy olmalıdır.
            return View(yorumlar);

        }
        public IActionResult Details(int id)
        {
            Yorum yorum = _db.Yorum.Include(yorum => yorum.Konu).SingleOrDefault(yorum => yorum.Id == id);
            return View(yorum);
        }
        public IActionResult Create()
        {
            List<Konu> konular = _db.Konu.OrderBy(konu => konu.Baslik).ToList();
            
            //ViewBag.KonuId = new SelectList(konular, "Id", "Baslik");
            ViewData["KonuId"] = new SelectList(konular, "Id", "Baslik"); //(SelectList -> DropDownList - MultiSelectList -> ListBox)
            
            // ViewBag ile ViewData birbirlerinin yerine kullanılabilir sadece yazımları farklıdır.
            return View();
        }
        [HttpPost]
        public IActionResult Create(Yorum yorum)
        {
            if (string.IsNullOrWhiteSpace(yorum.Icerik))
            {
                ViewBag.Mesaj = "İçerik boş girilemez!";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (yorum.Icerik.Length > 500)
            {
                ViewBag.Mesaj = "İçerik en fazla 500 karakter olmalıdır";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (string.IsNullOrWhiteSpace(yorum.Yorumcu))
            {
                ViewBag.Mesaj = "Yorumcu boş girilemez!";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (yorum.Yorumcu.Length > 50)
            {
                ViewBag.Mesaj = "Yorumcu en fazla 50 karakter olmalıdır!";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            //if (yorum.Puan != null)
            if (yorum.Puan.HasValue)
            {
                //if (yorum.Puan.Value > 5 || yorum.Puan.Value < 1)
                if (!(yorum.Puan.Value >= 1 && yorum.Puan.Value <= 5))
                {
                    ViewBag.Mesaj = "Puan 1 ile 5 arasında olmalıdır!";
                    ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                    return View(yorum);
                }
            }
            _db.Yorum.Add(yorum);
            _db.SaveChanges();
            TempData["YorumMesaj"] = "Yorum başarıyla eklendi.";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Yorum yorum = _db.Yorum.SingleOrDefault(yorum => yorum.Id == id);
            ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(konu => konu.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
            return View(yorum);
        }
        [HttpPost]
        public IActionResult Edit(Yorum yorum)
        {
            if (string.IsNullOrWhiteSpace(yorum.Icerik))
            {
                ViewBag.Mesaj = "İçerik boş girilemez!";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (yorum.Icerik.Length > 500)
            {
                ViewBag.Mesaj = "İçerik en fazla 500 karakter olmalıdır";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (string.IsNullOrWhiteSpace(yorum.Yorumcu))
            {
                ViewBag.Mesaj = "Yorumcu boş girilemez!";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (yorum.Yorumcu.Length > 50)
            {
                ViewBag.Mesaj = "Yorumcu en fazla 50 karakter olmalıdır!";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (yorum.Puan.HasValue)
            {

                if (!(yorum.Puan.Value >= 1 && yorum.Puan.Value <= 5))
                {
                    ViewBag.Mesaj = "Puan 1 ile 5 arasında olmalıdır!";
                    ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                    return View(yorum);
                }
            }
            Yorum mevcutYorum = _db.Yorum.SingleOrDefault(mevcutYorum => mevcutYorum.Id == yorum.Id);
            mevcutYorum.Icerik = yorum.Icerik;
            mevcutYorum.Yorumcu = yorum.Yorumcu;
            mevcutYorum.Puan = yorum.Puan;
            mevcutYorum.KonuId = yorum.KonuId;
            _db.Yorum.Update(mevcutYorum);
            _db.SaveChanges();
            TempData["YorumMesaj"] = "Yorum başarıyla güncellendi";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id) // ~/Yorum/Delete/1
        {
            Yorum yorum = _db.Yorum.Include(yorum => yorum.Konu).SingleOrDefault(yorum => yorum.Id == id);
            return View(yorum);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id) // ~/Yorum/Delete/
        {
            Yorum yorum = _db.Yorum.Find(id);
            _db.Yorum.Remove(yorum);
            _db.SaveChanges();
            TempData["YorumMesaj"] = "Yorum başarıyla silindi";
            return RedirectToAction("Index");
        }
    }
}
