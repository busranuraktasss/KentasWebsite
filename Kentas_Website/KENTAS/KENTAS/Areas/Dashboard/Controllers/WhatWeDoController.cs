using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class WhatWeDoController : Controller
    {
        Entities db = new Entities();
        // GET: WhatWeDo
        public ActionResult Index()
        {

            return View(db.Whatwedoes.ToList());
        }


        // GET: WhatWeDo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "WhatWeDo");
            }
            var neleryapiyoruz = db.Whatwedoes.Where(x => x.whatwedoId == id).SingleOrDefault();
            return View(neleryapiyoruz);
        }

        // POST: WhatWeDo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Whatwedo whatwedo)
        {
            if (ModelState.IsValid)
            {
                var wd = db.Whatwedoes.Where(x => x.whatwedoId == id).SingleOrDefault();
                wd.whatwedoHeaderDesc = whatwedo.whatwedoHeaderDesc;
                wd.whatwedoDesc = whatwedo.whatwedoDesc;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(whatwedo);
        }


    }
}