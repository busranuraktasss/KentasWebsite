using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using KENTAS.Models;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class AdvertsController : Controller
    {
        private Entities db = new Entities();

        // GET: Dashboard/Adverts
        public ActionResult Index()
        {
            return View(db.Adverts.ToList());
        }

        // GET: Dashboard/Adverts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Adverts");
            }
            Advert advert = db.Adverts.Find(id);
            if (advert == null)
            {
                return HttpNotFound();
            }
            return View(advert);
        }

        // GET: Dashboard/Adverts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Adverts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "adId,adHeaderDesc,adDesc,adTab,adImage")] Advert advert, HttpPostedFileBase[] adImage)
        {
            if (ModelState.IsValid)
            {

                //var firstimg = "/uploads/default.jpg";
                //advert.adImage = firstimg;
                //db.Adverts.Add(advert);
                //db.SaveChanges();

                if (adImage != null)
                {
                    string newFileName = "";
                    string firstPath = "";

                    foreach (var item in adImage)
                    {
                        newFileName = Guid.NewGuid().ToString() + "" + Path.GetExtension(item.FileName);

                        if (string.IsNullOrEmpty(firstPath))
                        {
                            firstPath = "/Uploads/Adverts/" + newFileName;

                            advert.adImage = firstPath;

                            db.Adverts.Add(advert);

                            db.SaveChanges();
                        }

                        item.SaveAs(Server.MapPath("~/Uploads/Adverts/") + newFileName);

                        //if (firstimg.Contains("/uploads/default.jpg")) firstimg = "/Uploads/Adverts/" + newFileName;
                        db.Adverts_Images.Add(new Adverts_Images()
                        {
                            imgUrl = "/Uploads/Adverts/" + newFileName,
                            bagId = advert.adId,
                            isdelete = false
                        });
                    }
                    //var update = ct.Adverts.Where(w => w.adId == advert.adId).FirstOrDefault();
                    //if (update != null)
                    //{
                    //    update.adImage = firstimg;
                    //    ct.SaveChanges();
                    //}
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(advert);
        }

        // GET: Dashboard/Adverts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Adverts");
            }
            Advert advert = db.Adverts.Find(id);
            if (advert == null)
            {
                return HttpNotFound();
            }
            return View(advert);
        }

        // POST: Dashboard/Adverts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "adId,adHeaderDesc,adDesc,adTab,adImage")] Advert advert, HttpPostedFileBase[] adImage, int id)
        {
            if (ModelState.IsValid)
            {
                var adv = db.Adverts.Where(x => x.adId == id).SingleOrDefault();

                if (adImage.Count() > 0 && adImage[0] != null)
                {
                    var Adverts_Images = db.Adverts_Images.Where(x => x.bagId == adv.adId).ToList();

                    foreach (var AdvImg in Adverts_Images)
                    {
                        if (System.IO.File.Exists(Server.MapPath(AdvImg.imgUrl)))
                        {
                            System.IO.File.Delete(Server.MapPath((AdvImg.imgUrl)));
                        }

                        AdvImg.isdelete = true;

                        db.Entry(AdvImg).State = EntityState.Modified;
                    }

                    adv.adImage = "";
                    //WebImage img = new WebImage(activitiesImage[0].InputStream);
                    //FileInfo imginfo = new FileInfo(activitiesImage[0].FileName);
                    //string actimgname = Guid.NewGuid().ToString() + imginfo.Extension;

                    //img.Save("~/Uploads/Activities/" + actimgname);

                    //a.activitiesImage = ("~/Uploads/Activities/" + actimgname);
                }
                adv.adHeaderDesc = advert.adHeaderDesc;
                adv.adDesc = advert.adDesc;
                adv.adTab = advert.adTab;
                if (adImage.Count() > 0 && adImage[0] != null)
                {
                    foreach (var item in adImage)
                    {
                        string newFileName = Guid.NewGuid().ToString() + "" + Path.GetExtension(item.FileName);

                        if (string.IsNullOrEmpty(adv.adImage))
                        {
                            adv.adImage = "/Uploads/Adverts/" + newFileName;
                        }

                        item.SaveAs(Server.MapPath("~/Uploads/Adverts/") + newFileName);
                        db.Adverts_Images.Add(new Adverts_Images()
                        {
                            imgUrl = "/Uploads/Adverts/" + newFileName,
                            bagId = advert.adId,
                            isdelete = false
                        });
                    }
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(advert);
        }

        // GET: Dashboard/Adverts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Adverts");
            }
            Advert advert = db.Adverts.Find(id);
            if (advert == null)
            {
                return HttpNotFound();
            }
            return View(advert);
        }

        // POST: Dashboard/Adverts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Advert advert = db.Adverts.Find(id);
            db.Adverts.Remove(advert);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}