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
    public class SssController : Controller
    {
        private Entities db = new Entities();
        // GET: Dashboard/Deneme
        [_SessionController]
        public ActionResult Index()
        {
            return View(db.SSSorulars.ToList());
            
        }
        // GET: Dashboard/Deneme/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SSSorular sSSorular = db.SSSorulars.Find(id);
            if (sSSorular == null)
            {
                return HttpNotFound();
            }
            return View(sSSorular);
        }

        // GET: Dashboard/Deneme/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Deneme/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost,ValidateAntiForgeryToken,ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Tarih,Soru,Cevap")] SSSorular sSSorular)
        {
            if (ModelState.IsValid)
            {
                sSSorular.Tarih = DateTime.Now;
                db.SSSorulars.Add(sSSorular);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sSSorular);
        }

        // GET: Dashboard/Deneme/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SSSorular sSSorular = db.SSSorulars.Find(id);
            if (sSSorular == null)
            {
                return HttpNotFound();
            }
            return View(sSSorular);
        }

        // POST: Dashboard/Deneme/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Tarih,Soru,Cevap")] SSSorular sSSorular)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sSSorular).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sSSorular);
        }

        // GET: Dashboard/Deneme/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SSSorular sSSorular = db.SSSorulars.Find(id);
            if (sSSorular == null)
            {
                return HttpNotFound();
            }
            return View(sSSorular);
        }

        // POST: Dashboard/Deneme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SSSorular sSSorular = db.SSSorulars.Find(id);
            db.SSSorulars.Remove(sSSorular);
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