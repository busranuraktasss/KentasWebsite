using KENTAS.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class JobApplicationListController : Controller
    {
        Entities db = new Entities();
        // GET: Dashboard/JobApplicationList
        public ActionResult Index(int sayfa=1)
        {
            var degerler = db.JobApplicationForms.ToList().ToPagedList(sayfa, 15);

            return View(degerler);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "JobApplicationList");
            }
            var activity = db.JobApplicationForms.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "JobApplicationList");
            }
            var activity = db.JobApplicationForms.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var jobApplicationForm = db.JobApplicationForms.Find(id);
            if (jobApplicationForm != null)
            {
                db.JobApplicationForms.Remove(jobApplicationForm);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}