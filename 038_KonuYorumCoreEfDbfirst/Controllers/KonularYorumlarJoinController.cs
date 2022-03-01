using Microsoft.AspNetCore.Mvc;
using _038_KonuYorumCoreEfDbfirst.DataAccess;
using _038_KonuYorumCoreEfDbfirst.Models;

namespace _038_KonuYorumCoreEfDbfirst.Controllers
{
    public class KonularYorumlarJoinController : Controller
    {
        private BA_KonuYorumCoreContext _db = new BA_KonuYorumCoreContext();
        public IActionResult InnerJoin()
        {
            IQueryable<Konu> konuQuery = _db.Konu.AsQueryable(); // select * from Konu
            IQueryable<Yorum> yorumQuery = _db.Yorum.AsQueryable(); // select * from Yorum
            var joinQuery = from konu in konuQuery
                            join yorum in yorumQuery
                            on konu.Id equals yorum.KonuId
                            //where konu.Id == 5
                            orderby yorum.Puan descending, yorum.Yorumcu
                            select new KonuYorumInnerJoinModel()
                            {
                                Baslik = konu.Baslik,
                                Aciklama = konu.Aciklama,
                                Icerik = yorum.Icerik,
                                Yorumcu = yorum.Yorumcu,
                                Puan = yorum.Puan,

                                PuanDurumu = yorum.Puan < 3 ? "Kötü" : yorum.Puan == 3 ? "Orta" : "İyi"
                            };
            var model = joinQuery.ToList();
            return View(model);
        }

        /*select k.Baslik, k.Aciklama, y.Icerik, y.Yorumcu, y.Puan,
        case when y.Puan< 3 then 'Kötü' when y.Puan = 3 then 'Orta' else 'İyi' end as PuanDurumu
        from Konu k left outer  join Yorum y
        on k.Id = y.KonuId
        order by y.Puan desc, y.Yorumcu*/
        public IActionResult LeftOuterJoin()
        {
            IQueryable<Konu> konuQuery = _db.Konu.AsQueryable();
            IQueryable<Yorum> yorumQuery = _db.Yorum.AsQueryable();
            var joinQuery = from konu in konuQuery
                            join yorum in yorumQuery
                            on konu.Id equals yorum.KonuId into konuYorumJoin
                            from subKonuYorumJoin in konuYorumJoin.DefaultIfEmpty()
                            //where konu.Id == 5
                            orderby subKonuYorumJoin.Puan descending, subKonuYorumJoin.Yorumcu
                            select new KonuYorumLeftOuterJoinModel()
                            {
                                Baslik = konu.Baslik,
                                Aciklama = konu.Aciklama,
                                Icerik = subKonuYorumJoin.Icerik,
                                Yorumcu = subKonuYorumJoin.Yorumcu,
                                Puan = subKonuYorumJoin.Puan,
                            };
            var model = joinQuery.ToList();
            return View(model);
        }
    }
}
