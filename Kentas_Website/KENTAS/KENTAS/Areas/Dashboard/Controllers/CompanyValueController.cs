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
    public class CompanyValueController : Controller
    {
        // GET: Dashboard/CompanyValue
        Entities db = new Entities();
        public ActionResult Index()
        {
            return View(db.CompanyValues.ToList());
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "CompanyValue");
            }
            var company = db.CompanyValues.Where(x => x.CompanyValueId == id).SingleOrDefault();
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "CompanyValueId,CompanyValuesPhotos,CompanyValuesTitle,CompanyValuesDescription")] int id, CompanyValue companyValue, HttpPostedFileBase CompanyValuesPhotos)
        {
            if (ModelState.IsValid)
            {
                var cV = db.CompanyValues.Where(x => x.CompanyValueId == id).SingleOrDefault();
                if (CompanyValuesPhotos != null)
                {
                  
                    WebImage img = new WebImage(CompanyValuesPhotos.InputStream);
                    FileInfo imginfo = new FileInfo(CompanyValuesPhotos.FileName);
                    string companyValuePhoto = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Save(Server.MapPath("~/Uploads/CompanyValue/") + companyValuePhoto);

                    cV.CompanyValuesPhotos = ("~/Uploads/CompanyValue/" + companyValuePhoto);
                }
                cV.CompanyValuesTitle = companyValue.CompanyValuesTitle;
                cV.CompanyValuesDescription = companyValue.CompanyValuesDescription;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companyValue);
        }

    }
}