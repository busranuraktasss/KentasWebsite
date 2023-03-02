using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        Entities db = new Entities();
        // GET: Dashboard/Brand
        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Brand");
            }
            var company = db.Brands.Where(x => x.BrandId == id).SingleOrDefault();
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "BrandId,BrandPhoto,BrandTitle,BrandDescription")] int id, Brand brand, HttpPostedFileBase BrandPhoto)
        {
            if (ModelState.IsValid)
            {
                var b = db.Brands.Where(x => x.BrandId == id).SingleOrDefault();
                if (BrandPhoto != null)
                {

                    WebImage img = new WebImage(BrandPhoto.InputStream);
                    FileInfo imginfo = new FileInfo(BrandPhoto.FileName);
                    string brandPhoto = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Save(Server.MapPath("~/Uploads/Brand/") + brandPhoto);

                    b.BrandPhoto = ("~/Uploads/Brand/" + brandPhoto);
                }
                b.BrandTitle = brand.BrandTitle;
                b.BrandDescription = brand.BrandDescription;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }
    }
}