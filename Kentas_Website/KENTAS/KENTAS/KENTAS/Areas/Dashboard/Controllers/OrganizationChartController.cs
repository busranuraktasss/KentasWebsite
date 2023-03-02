using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class OrganizationChartController : Controller
    {
        // GET: Dashboard/OrganizationChart
        Entities db = new Entities();
        public ActionResult Index()
        {
            return View(db.OrganizationCharts.ToList());
        }

        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index", "OrganizationChart");
            }
            var organization = db.OrganizationCharts.Where(x => x.OrganizationChartId == id).SingleOrDefault();
            return View(organization);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "OrganizationChartId,OrganizationChartPhoto")] int id, OrganizationChart organizationChart, HttpPostedFileBase OrganizationChartPhoto)
        {
            if (ModelState.IsValid)
            {
                var o = db.OrganizationCharts.Where(x => x.OrganizationChartId == id).SingleOrDefault();
                if (OrganizationChartPhoto != null)
                {
                   
                    WebImage img = new WebImage(OrganizationChartPhoto.InputStream);
                    FileInfo imginfo = new FileInfo(OrganizationChartPhoto.FileName);
                    string organizationchartPhoto = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Save(Server.MapPath("~/Uploads/OrganizationChart/") + organizationchartPhoto);
                    o.OrganizationChartPhoto = ("~/Uploads/OrganizationChart/" + organizationchartPhoto);
                }
               
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organizationChart);
        }



    }
}