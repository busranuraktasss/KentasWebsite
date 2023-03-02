using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Controllers
{
    public class MarkaController : Controller
    {
        Entities db = new Entities();
        // GET: Marka
        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }
    }
}