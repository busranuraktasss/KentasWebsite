using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class IdentityController : Controller
    {
        // GET: Dashboard/Identity
        private Entities db = new Entities();
        public ActionResult Kimlik()
        {
            return View(db.Identities.ToList());
        }

        // GET: Dashboard/Identity/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Identity");
            }
            var identity = db.Identities.Where(x => x.id == id).SingleOrDefault();
            return View(identity);
        }

        // POST: Dashboard/Identity/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Identity identity)
        {
            if (ModelState.IsValid)
            {
                var k = db.Identities.Where(x => x.id == id).SingleOrDefault();
                k.keyword = identity.keyword;
                k.description = identity.description;
                db.SaveChanges();
                return RedirectToAction("Kimlik");
            }

            return View(identity);

        }

    }
}
