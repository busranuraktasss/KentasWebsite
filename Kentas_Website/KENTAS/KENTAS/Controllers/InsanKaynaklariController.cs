using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Controllers
{
    public class InsanKaynaklariController : Controller
    {
        Entities db = new Entities();
        // GET: InsanKaynaklari
        public ActionResult Index()
        {
            return View(db.HumanResources.ToList());
        }
    }
}