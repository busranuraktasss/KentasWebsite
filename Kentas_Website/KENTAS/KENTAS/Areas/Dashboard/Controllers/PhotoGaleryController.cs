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
using KENTAS.Helper;
using KENTAS.Models;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class PhotoGaleryController : Controller
    {
        private Entities db = new Entities();

        // GET: PhotoGalery
        public ActionResult Index(int h = 1)
        {
            var currentData = db.PhotoGaleries.Where(w => w.TitileId == h).Select(s => new getGalryModel()
            {
                TitileId = s.TitileId,
                photoGaleryId = s.photoGaleryId,
                photoGaleryImage = s.photoGaleryImage
            }).ToList();
            return View(currentData);
        }



        // GET: PhotoGalery/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhotoGalery/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "photoGaleryId,photoGaleryImage, TitileId")] PhotoGalery photoGalery, HttpPostedFileBase photoGaleryImage)
        {
            if (ModelState.IsValid)
            {
                if (photoGaleryImage != null)
                {
                    photoGaleryImage.SaveAs(Server.MapPath("~/Uploads/PhotoGalery/") + photoGaleryImage.FileName);
                    photoGalery.photoGaleryImage = "/Uploads/PhotoGalery/" + photoGaleryImage.FileName;

                }
                db.PhotoGaleries.Add(photoGalery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(photoGalery);
        }

        // GET: PhotoGalery/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "PhotoGalery, TitileId");
            }
            PhotoGalery photoGalery = db.PhotoGaleries.Find(id);
            if (photoGalery == null)
            {
                return HttpNotFound();
            }
            return View(photoGalery);
        }

        // POST: PhotoGalery/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "photoGaleryId,photoGaleryImag,TitileIde")] PhotoGalery photoGalery, HttpPostedFileBase photoGaleryImage, int id)
        {
            if (ModelState.IsValid)
            {
                var p = db.PhotoGaleries.Where(x => x.photoGaleryId == id).SingleOrDefault();
                if (photoGaleryImage != null)
                {
                    WebImage img = new WebImage(photoGaleryImage.InputStream);
                    FileInfo imginfo = new FileInfo(photoGaleryImage.FileName);
                    string phimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Save(Server.MapPath("~/Uploads/PhotoGalery/") + phimgname);
                    p.photoGaleryImage = ("~/Uploads/PhotoGalery/" + phimgname);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(photoGalery);
        }

        // GET: PhotoGalery/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "PhotoGalery");
            }
            PhotoGalery photoGalery = db.PhotoGaleries.Find(id);
            if (photoGalery == null)
            {
                return HttpNotFound();
            }
            return View(photoGalery);
        }

        // POST: PhotoGalery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhotoGalery photoGalery = db.PhotoGaleries.Find(id);
            db.PhotoGaleries.Remove(photoGalery);
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


//if (System.IO.File.Exists(Server.MapPath(p.photoGaleryImage)))
//{
//    System.IO.File.Delete(Server.MapPath(p.photoGaleryImage));
//}
//  WebImage img = new WebImage(photoGaleryImage.InputStream);
//FileInfo imginfo = new FileInfo(photoGaleryImage.FileName);
//string phimgname = $"{Guid.NewGuid().ToString() + imginfo.Extension}";
//p.photoGaleryImage = ("~/Uploads/PhotoGalery/" + phimgname);
//  img.Save("~/Uploads/PhotoGalery/" + phimgname);

//p.photoGaleryImage = ("~/Uploads/PhotoGalery/" + phimgname);

//FileInfo imginfo = new FileInfo(photoGaleryImage.FileName);
//string phimgname = $"{Guid.NewGuid().ToString()}{imginfo.Extension}";
//photoGaleryImage.SaveAs(Server.MapPath("~/Uploads/PhotoGalery/") + phimgname);
//p.photoGaleryImage = ("../Uploads/PhotoGalery/" + phimgname);               