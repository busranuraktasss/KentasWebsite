using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KENTAS.Models;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Web.Helpers;
using KENTAS.Helper;

namespace KENTAS.Areas.Dashboard.Controllers
{
    public class GaleryTitleAddController : Controller
    {
        // GET: Dashboard/GaleryTitleAdd
        private Entities db = new Entities();
        [_SessionController]

        public ActionResult Index()
        {
            return View(db.GaleriBasliklaris.ToList());
        }

        // GET: Dashboard/GaleryTitleAdd/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dashboard/GaleryTitleAdd/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/GaleryTitleAdd/Create
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title")] GaleriBasliklari galeri)

        {
            if (ModelState.IsValid)
            {
                db.GaleriBasliklaris.Add(galeri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(galeri);
        }

        // GET: Dashboard/GaleryTitleAdd/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GaleriBasliklari galeri = db.GaleriBasliklaris.Find(id);
            if (galeri == null)
            {
                return HttpNotFound();
            }
            return View(galeri);
        }

        // POST: Dashboard/GaleryTitleAdd/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] GaleriBasliklari galeri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(galeri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(galeri);
        }

        // GET: Dashboard/GaleryTitleAdd/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GaleriBasliklari galeri = db.GaleriBasliklaris.Find(id);
            if (galeri == null)
            {
                return HttpNotFound();
            }
            return View(galeri);
        }

        // POST: Dashboard/GaleryTitleAdd/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GaleriBasliklari galeri = db.GaleriBasliklaris.Find(id);
            db.GaleriBasliklaris.Remove(galeri);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
