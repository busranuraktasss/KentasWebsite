using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Controllers
{
    public class SirketDegerController : Controller
    {
        Entities db = new Entities();
        // GET: SirketDeger
        public ActionResult Index()
        {
            return View(db.CompanyValues.ToList());
        }
    }
}