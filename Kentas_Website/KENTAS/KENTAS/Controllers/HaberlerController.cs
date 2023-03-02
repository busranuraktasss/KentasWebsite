using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static KENTAS.Helper.Common;

namespace KENTAS.Controllers
{
    public class HaberlerController : Controller
    {
        // GET: Haberler
        Entities db = new Entities();
        public ActionResult Haber()
        {
            return View(db.News.ToList());
        }

        // GET: Haberler/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Haber", "Haberler");
            }
            var news = db.News.Where(w => w.newsId == id).Select(s => new News_api()
            {
                newsId = s.newsId,
                newsHeaderDesc = s.newsHeaderDesc,
                newsDesc = s.newsDesc,
                newsTabItem = s.newsTabItem,
                newsImage = s.newsImage,
                Resimler = db.News_Images.Where(w => w.nbagId == s.newsId && w.isdelete == false).Select(i => new News_Images_api()
                {
                    nbagId = i.nbagId,
                    nimgUrl = i.nimgUrl,
                    nId = i.nId,
                    isdelete = i.isdelete
                }).ToList()

            }).FirstOrDefault();
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }


    }
}
