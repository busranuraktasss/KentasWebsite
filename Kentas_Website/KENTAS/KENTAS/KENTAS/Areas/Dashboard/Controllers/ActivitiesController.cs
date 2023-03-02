using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using KENTAS.Models;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private Entities db = new Entities();

        // GET: Dashboard/Activities
        public ActionResult Index()
        {
            return View(db.Activities.ToList());
        }

        // GET: Dashboard/Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Activities");
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Dashboard/Activities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "activitiesId,activitiesHeaderDesc,activitiesDesc,activitiesImage")] Activity activity, HttpPostedFileBase[] activitiesImage)
        {
            if (ModelState.IsValid)
            {
                //var firstimg = "/Uploads/default.jpg";
                //activity.activitiesImage = firstimg;
                //db.Activities.Add(activity);
                //db.SaveChanges();

                if (activitiesImage != null)
                {
                    string newFileName = "";
                    string firstPath = "";

                    foreach (var item in activitiesImage)
                    {
                        newFileName = Guid.NewGuid().ToString() + "" + Path.GetExtension(item.FileName); //Unique isim verildi ve file sonuna dosya extension(formatı)eklendi.

                        if (string.IsNullOrEmpty(firstPath))
                        {
                            firstPath = "/Uploads/Activities/" + newFileName;

                            activity.activitiesImage = firstPath;

                            db.Activities.Add(activity);

                            db.SaveChanges();
                        }

                        item.SaveAs(Server.MapPath("~/Uploads/Activities/") + newFileName);

                        ////if (firstimg.Contains("/Uploads/default.jpg")) firstimg = "/Uploads/Activities/" + newFileName;
                        db.Activities_Images.Add(new Activities_Images()
                        {
                            fimgUrl = "/Uploads/Activities/" + newFileName,
                            fbagId = activity.activitiesId,
                            isdelete = false
                        });
                    }
                    //var update = db.Activities.Where(w => w.activitiesId == activity.activitiesId).FirstOrDefault();
                    //if (update != null)
                    //{
                    //    update.activitiesImage = "/Uploads/Activities/" + newFileName;
                    //}
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(activity);
        }

        // GET: Dashboard/Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Activities");
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Dashboard/Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "activitiesId,activitiesHeaderDesc,activitiesDesc,activitiesImage")] Activity activity, HttpPostedFileBase[] activitiesImage, int id)
        {
            if (ModelState.IsValid)
            {
                var a = db.Activities.Where(x => x.activitiesId == id).SingleOrDefault();

                if (activitiesImage.Count() > 0 && activitiesImage[0] != null)
                {
                    var activitiesImages = db.Activities_Images.Where(x => x.fbagId == a.activitiesId).ToList();

                    foreach (var actImg in activitiesImages)
                    {
                        if (System.IO.File.Exists(Server.MapPath(actImg.fimgUrl)))
                        {
                            System.IO.File.Delete(Server.MapPath((actImg.fimgUrl)));
                        }

                        actImg.isdelete = true;

                        db.Entry(actImg).State = EntityState.Modified;
                    }

                    a.activitiesImage = "";
                    //WebImage img = new WebImage(activitiesImage[0].InputStream);
                    //FileInfo imginfo = new FileInfo(activitiesImage[0].FileName);
                    //string actimgname = Guid.NewGuid().ToString() + imginfo.Extension;

                    //img.Save("~/Uploads/Activities/" + actimgname);

                    //a.activitiesImage = ("~/Uploads/Activities/" + actimgname);
                }
                a.activitiesHeaderDesc = activity.activitiesHeaderDesc;
                a.activitiesDesc = activity.activitiesDesc;
                if (activitiesImage.Count() > 0 && activitiesImage[0] != null)
                {
                    foreach (var item in activitiesImage)
                    {
                        string newFileName = Guid.NewGuid().ToString() + "" + Path.GetExtension(item.FileName);

                        if (string.IsNullOrEmpty(a.activitiesImage))
                        {
                            a.activitiesImage = "/Uploads/Activities/" + newFileName;
                        }

                        item.SaveAs(Server.MapPath("~/Uploads/Activities/") + newFileName);
                        db.Activities_Images.Add(new Activities_Images()
                        {
                            fimgUrl = "/Uploads/Activities/" + newFileName,
                            fbagId = activity.activitiesId,
                            isdelete = false
                        });
                    }
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // GET: Dashboard/Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Activities");
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Dashboard/Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
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
