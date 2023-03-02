using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Controllers
{
    public class SSS : Controller
    {
        // GET: SikcaSorulanSorular
        Entities db = new Entities();
        public ActionResult Index()
        {
            return View(db.SSSorulars.ToList());
        }
    }
}