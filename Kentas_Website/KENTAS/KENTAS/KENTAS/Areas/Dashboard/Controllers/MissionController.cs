using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class MissionController : Controller
    {
        // GET: Mission
        Entities db = new Entities();
        public ActionResult Index()
        {

            return View(db.Missions.ToList());
        }


        // GET: Mission/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Mission");
            }
            var misyonumuz = db.Missions.Where(x => x.missionId == id).SingleOrDefault();
            return View(misyonumuz);
        }

        // POST: Mission/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Mission mission)
        {

            if (ModelState.IsValid)
            {
                var m = db.Missions.Where(x => x.missionId == id).SingleOrDefault();
                m.missionDesc = mission.missionDesc;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mission);

        }


    }
}