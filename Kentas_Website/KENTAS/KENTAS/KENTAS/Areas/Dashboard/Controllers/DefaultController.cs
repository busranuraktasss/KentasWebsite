using KENTAS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static KENTAS.Helper.Common;


namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        // GET: Dashboard/Default

      
        public ActionResult Index()        
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Default", new { area = "Dashboard" });

            var user = GetUser(HttpContext.User.Identity.Name);
            ViewBag.AdminName = user.Email;

            return View();
        }
    }
}