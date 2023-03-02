using KENTAS.Helper;
using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class SlidersController : Controller
    {
        private Entities db = new Entities();

        // GET: Sliders
      
        public ActionResult Index()
        {
            return View(db.Sliders.ToList());
        }

        // GET: Sliders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sliders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "sliderId,sldierDesc,sliderImage")] Slider slider, HttpPostedFileBase sliderImage)
        {
            if (ModelState.IsValid)
            {
                if (sliderImage != null)
                {
                    sliderImage.SaveAs(Server.MapPath("~/Uploads/Slider/") + sliderImage.FileName);
                    slider.sliderImage = "/Uploads/Slider/" + sliderImage.FileName;
                }
                db.Sliders.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slider);
        }

        // GET: Sliders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Sliders");
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "sliderId,sldierDesc,sliderImage")] Slider slider, HttpPostedFileBase sliderImage, int id)
        {
            if (ModelState.IsValid)
            {
                var s = db.Sliders.Where(x => x.sliderId == id).SingleOrDefault();
                if (sliderImage != null)
                {
                    FileInfo imginfo = new FileInfo(sliderImage.FileName);
                    string sliderimgname = $"{Guid.NewGuid().ToString()}{imginfo.Extension}";
                    sliderImage.SaveAs(Server.MapPath("~/Uploads/Slider/") + sliderimgname);
                    s.sliderImage = ("/Uploads/Slider/" + sliderimgname);
                }
                s.sldierDesc = slider.sldierDesc;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: Sliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Sliders");
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Sliders.Find(id);
            db.Sliders.Remove(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}