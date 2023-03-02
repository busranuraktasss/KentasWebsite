using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Controllers
{
    public class GenelMudurController : Controller
    {
        Entities db = new Entities();
        // GET: GenelMudur
        public ActionResult Index()
        {
            return View(db.GeneralManagers.ToList());
        }
    }
}