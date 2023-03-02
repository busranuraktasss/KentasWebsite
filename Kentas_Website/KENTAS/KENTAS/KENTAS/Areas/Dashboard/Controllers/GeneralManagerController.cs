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
    public class GeneralManagerController : Controller
    {

        Entities db = new Entities();
        public ActionResult Index()
        {
            return View(db.GeneralManagers.ToList());
        }


        public ActionResult Edit(int id)
        {
           
            var generalManager = db.GeneralManagers.Where(x => x.GeneralManagerId == id).SingleOrDefault();
            return View(generalManager);
        }

        // POST: Dashboard/GeneralManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "GeneralManagerId,GeneralManagerLogo,GeneralManagerDescription,GeneralManagerName")] int id, GeneralManager generalManager, HttpPostedFileBase GeneralManagerLogo)
        {
            if (ModelState.IsValid)
            {
                var g = db.GeneralManagers.Where(x => x.GeneralManagerId == id).SingleOrDefault();
                if (GeneralManagerLogo != null)
                {

                    WebImage img = new WebImage(GeneralManagerLogo.InputStream);
                    FileInfo imginfo = new FileInfo(GeneralManagerLogo.FileName);
                    string generalManagerLogoName = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Save(Server.MapPath("~/Uploads/GeneralManager/") + generalManagerLogoName);

                    g.GeneralManagerLogo = ("~/Uploads/GeneralManager/" + generalManagerLogoName);
                }
                g.GeneralManagerName = generalManager.GeneralManagerName;
                g.GeneralManagerDescription = generalManager.GeneralManagerDescription;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(generalManager);
        }

      
    }
}
