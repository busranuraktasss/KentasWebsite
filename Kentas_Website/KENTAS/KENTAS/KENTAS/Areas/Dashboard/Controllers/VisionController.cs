using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class VisionController : Controller
    {
        // GET: Vision
        Entities db = new Entities();
        public ActionResult Index()
        {
            return View(db.Visions.ToList());
        }

        // GET: Vision/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Vision");
            }
            var vizyon = db.Visions.Where(x => x.visionId == id).SingleOrDefault();
            return View(vizyon);
        }

        // POST: Vision/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Vision vision)
        {

            if (ModelState.IsValid)
            {
                var v = db.Visions.Where(x => x.visionId == id).SingleOrDefault();
                v.visionDesc = vision.visionDesc;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vision);

        }


    }
}