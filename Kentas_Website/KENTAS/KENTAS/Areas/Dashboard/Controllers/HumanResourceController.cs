using KENTAS.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class HumanResourceController : Controller
    {
        Entities db = new Entities();
        // GET: Dashboard/HumanResource
        public ActionResult Index()
        {
            return View(db.HumanResources.ToList());
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Brand");
            }
            var humanResource = db.HumanResources.Where(x => x.HumanResourceId == id).SingleOrDefault();
            return View(humanResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "HumanResourceId,HumanResourcePhoto,HumanResourceTitle,HumanResourceDescription")] int id, HumanResource humanResource, HttpPostedFileBase HumanResourcePhoto)
        {
            if (ModelState.IsValid)
            {
                var b = db.HumanResources.Where(x => x.HumanResourceId == id).SingleOrDefault();
                if (HumanResourcePhoto != null)
                {

                    WebImage img = new WebImage(HumanResourcePhoto.InputStream);
                    FileInfo imginfo = new FileInfo(HumanResourcePhoto.FileName);
                    string humanResourcePhoto = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Save(Server.MapPath("~/Uploads/HumanResource/") + humanResourcePhoto);

                    b.HumanResourcePhoto = ("~/Uploads/HumanResource/" + humanResourcePhoto);
                }
                b.HumanResourceTitle = humanResource.HumanResourceTitle;
                b.HumanResourceDescription = humanResource.HumanResourceDescription;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(humanResource);
        }
    }
}