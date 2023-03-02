using KENTAS.Models;
using System.Linq;
using System.Web.Mvc;
using static KENTAS.Helper.Common;

namespace KENTAS.Controllers
{
    public class FaaliyetlerimizController : Controller
    {
        // GET: Faaliyetlerimiz
        Entities db = new Entities();
        public ActionResult Faaliyet()
        {
            return View(db.Activities.ToList());
        }

        // GET: Faaliyetlerimiz/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Faaliyet", "Faaliyetlerimiz");
            }

            var activity = db.Activities.Where(w => w.activitiesId == id).Select(s => new Activities_api()
            {
                activitiesId = s.activitiesId,
                activitiesHeaderDesc = s.activitiesHeaderDesc,
                activitiesDesc = s.activitiesDesc,
                activitiesImage = s.activitiesImage,
                Resimler = db.Activities_Images.Where(w => w.fbagId == s.activitiesId && w.isdelete == false).Select(i => new Activities_Images_api()
                {
                    fbagId = i.fbagId,
                    fimgUrl = i.fimgUrl,
                    fId = i.fId,
                    isdelete = i.isdelete
                }).ToList()

            }).FirstOrDefault();

            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }


    }
}