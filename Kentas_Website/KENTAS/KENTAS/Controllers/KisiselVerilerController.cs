using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace KENTAS.Controllers
{
    public class KisiselVerilerController : Controller
    {
        Entities db = new Entities();
        // GET: KisiselVeriler
        public ActionResult Index()
        {
            return View(db.Kvkks.ToList());
        }
    }
}