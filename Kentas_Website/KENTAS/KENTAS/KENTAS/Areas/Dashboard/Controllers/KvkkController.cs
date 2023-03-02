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
    public class KvkkController : Controller
    {
        Entities db = new Entities();
        // GET: Dashboard/Kvkk
        public ActionResult Index()
        {
            return View(db.Kvkks.ToList());
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("index", "Kvkk");
            }
            var kvkk = db.Kvkks.Where(x => x.KvkkId == id).SingleOrDefault();
            return View(kvkk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Kvkk kvkk)
        {

            if (ModelState.IsValid)
            {
                var k = db.Kvkks.Where(x => x.KvkkId == id).SingleOrDefault();
                k.KvkkDescription = kvkk.KvkkDescription;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kvkk);

        }
    }
}