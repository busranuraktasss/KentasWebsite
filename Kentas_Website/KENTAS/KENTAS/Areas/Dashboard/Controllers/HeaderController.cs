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
    public class HeaderController : Controller
    {
        // GET: Dashboard/Header
        Entities db = new Entities();
        public ActionResult Index()
        {
            return View(db.Headers.ToList());
        }

   
        // GET: Dashboard/Header/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Header");
            }
            var header = db.Headers.Where(x => x.headerId == id).SingleOrDefault();
            return View(header);
        }

        // POST: Dashboard/Header/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "headerPresidentName,headerPresidentDescription,headerId,headerPresidentLogo")] int id, Header header, HttpPostedFileBase headerPresidentLogo)
        {
            if (ModelState.IsValid)
            {
                var h = db.Headers.Where(x => x.headerId == id).SingleOrDefault();
                if (headerPresidentLogo != null)
                {
                    //if (System.IO.File.Exists(Server.MapPath(h.headerPresidentLogo)))
                    //{
                    //    System.IO.File.Delete(Server.MapPath(h.headerPresidentLogo));
                    //}
                    WebImage img = new WebImage(headerPresidentLogo.InputStream);
                    FileInfo imginfo = new FileInfo(headerPresidentLogo.FileName);
                    string headerlogoname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Save(Server.MapPath("~/Uploads/President/") + headerlogoname);

                    h.headerPresidentLogo = ("~/Uploads/President/"+ headerlogoname);
                }
                h.headerPresidentName = header.headerPresidentName;
                h.headerPresidentDescription = header.headerPresidentDescription;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(header);
        }

      
    }
}
