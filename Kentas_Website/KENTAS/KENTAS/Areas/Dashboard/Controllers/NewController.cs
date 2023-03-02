using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class NewController : Controller
    {
        private Entities db = new Entities();

        // GET: New
        public ActionResult Index()
        {
            return View(db.News.ToList());
        }


        // GET: New/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "New");
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: New/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: New/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "newsId,newsHeaderDesc,newsDesc,newsImage,newsTabItem")] News news, HttpPostedFileBase[] newsImage)
        {
            if (ModelState.IsValid)
            {
                //var firstimg = "../uploads/default.jpg";
                //news.newsImage = firstimg;
                //db.News.Add(news);
                //db.SaveChanges();

                if (newsImage != null)
                {
                    string newFileName = "";
                    string firstPath = "";

                    foreach (var item in newsImage)
                    {
                        newFileName = Guid.NewGuid().ToString() + "" + Path.GetExtension(item.FileName);

                        if (string.IsNullOrEmpty(firstPath))
                        {
                            firstPath = "/Uploads/News/" + newFileName;

                            news.newsImage = firstPath;

                            db.News.Add(news);

                            db.SaveChanges();
                        }

                        item.SaveAs(Server.MapPath("~/Uploads/News/") + newFileName);

                        //if (firstimg.Contains("/Uploads/default.jpg")) firstimg = "../Uploads/News/" + newFileName;
                        db.News_Images.Add(new News_Images()
                        {
                            nimgUrl = "/Uploads/News/" + newFileName,
                            nbagId = news.newsId,
                            isdelete = false
                        });
                    }
                    //var update = db.News.Where(w => w.newsId == news.newsId).FirstOrDefault();
                    //if (update != null)
                    //{
                    //    update.newsImage = firstimg;
                    //    nt.SaveChanges();
                    //}
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: New/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "New");
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: New/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "newsId,newsHeaderDesc,newsDesc,newsImage,newsTabItem")] News news, HttpPostedFileBase[] newsImage, int id)
        {
            if (ModelState.IsValid)
            {
                var n = db.News.Where(x => x.newsId == id).SingleOrDefault();

                if (newsImage.Count() > 0 && newsImage[0] != null)
                {
                    var newsImages = db.News_Images.Where(x => x.nbagId == n.newsId).ToList();

                    foreach (var newsImg in newsImages)
                    {
                        if (System.IO.File.Exists(Server.MapPath(newsImg.nimgUrl)))
                        {
                            System.IO.File.Delete(Server.MapPath((newsImg.nimgUrl)));
                        }

                        newsImg.isdelete = true;

                        db.Entry(newsImg).State = EntityState.Modified;
                    }

                    n.newsImage = "";
                    //WebImage img = new WebImage(activitiesImage[0].InputStream);
                    //FileInfo imginfo = new FileInfo(activitiesImage[0].FileName);
                    //string actimgname = Guid.NewGuid().ToString() + imginfo.Extension;

                    //img.Save("~/Uploads/Activities/" + actimgname);

                    //a.activitiesImage = ("~/Uploads/Activities/" + actimgname);
                }
                n.newsHeaderDesc = news.newsHeaderDesc;
                n.newsDesc = news.newsDesc;
                n.newsTabItem = news.newsTabItem;
                if (newsImage.Count() > 0 && newsImage[0] != null)
                {
                    foreach (var item in newsImage)
                    {
                        string newFileName = Guid.NewGuid().ToString() + "" + Path.GetExtension(item.FileName);

                        if (string.IsNullOrEmpty(n.newsImage))
                        {
                            n.newsImage = "/Uploads/News/" + newFileName;
                        }

                        item.SaveAs(Server.MapPath("~/Uploads/News/") + newFileName);
                        db.News_Images.Add(new News_Images()
                        {
                            nimgUrl = "/Uploads/News/" + newFileName,
                            nbagId = news.newsId,
                            isdelete = false
                        });
                    }
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(news);
        }


        // GET: New/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "New");
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: New/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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