using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class AboutusController : Controller
    {

        Entities db = new Entities();

        // GET: Aboutas
        public ActionResult Index()
        {

            return View(db.Abouts.ToList());
        }
       
        // GET: Aboutas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Aboutus");
            }
            var hakkimizda = db.Abouts.Where(x => x.aboutId == id).SingleOrDefault();
            return View(hakkimizda);
        }

        // POST: Aboutas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, About about)
        {
            if (ModelState.IsValid)
            {

                var ab = db.Abouts.Where(x => x.aboutId == id).SingleOrDefault();
                ab.aboutHeaderDesc = about.aboutHeaderDesc;
                ab.aboutDesc = about.aboutDesc;
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(about);
        }


    }
}