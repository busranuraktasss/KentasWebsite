using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Controllers
{
    public class OrganizasyonSemaController : Controller
    {
        Entities db = new Entities();
        // GET: OrganizasyonSema
        public ActionResult Index()
        {
            return View(db.OrganizationCharts.ToList());
        }
    }
}