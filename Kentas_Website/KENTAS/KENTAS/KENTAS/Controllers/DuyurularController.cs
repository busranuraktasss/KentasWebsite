using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using static KENTAS.Helper.Common;

namespace KENTAS.Controllers
{
    public class DuyurularController : Controller
    {
        // GET: Duyurular
        Entities db = new Entities();
        public ActionResult Duyuru()
        {
            return View(db.Adverts.ToList());
        }

        // GET: Duyurular/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Duyuru", "Duyurular");
            }

            //Advert advert = db.Adverts.Find(id);
            var advert = db.Adverts.Where(w => w.adId == id).Select(s => new Advert_api()
            {
                adId = s.adId,
                adHeaderDesc = s.adHeaderDesc,
                adDesc = s.adDesc,
                adTab = s.adTab,
                adImage = s.adImage,
                Resimler = db.Adverts_Images.Where(w => w.bagId == s.adId && w.isdelete == false).Select(i => new Adverts_Images_api()
                {
                    bagId = i.bagId,
                    imgUrl = i.imgUrl,
                    Id = i.Id,
                    isdelete = i.isdelete
                }).ToList()

            }).FirstOrDefault();
            if (advert == null)
            {
                return HttpNotFound();
            }
            return View(advert);
        }


    }
}
