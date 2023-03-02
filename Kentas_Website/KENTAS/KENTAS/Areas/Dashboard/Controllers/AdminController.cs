using KENTAS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace KENTAS.Areas.Dashboard.Controllers
{

    public class AdminController : Controller
    {
        Entities db = new Entities();
        // GET: Admin

        public ActionResult Managment()
        {

            return View();
        }
   
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost, AllowAnonymous]        
        public ActionResult Login(Admin admin, string returnurl)
        {
            var pass = Crypto.Hash(admin.Password, "MD5");
            var login = db.Admins.Where(x => x.Email == admin.Email && x.Password == pass).FirstOrDefault();
            if (login != null)
            {
                string JsonToString = JsonConvert.SerializeObject(login);
                FormsAuthentication.SetAuthCookie(JsonToString, true);
                return RedirectToAction("Index", "Default", new { area = "Dashboard" });
            }
            else
            {
                ViewBag.Warning = "Kullanıcı adı veya şifre hatalı";
                ModelState.AddModelError("Kullanıcı adı veya şifre hatalı", "");
                // return View("login", admin);
                return RedirectToAction("Index", "Default", new { area = "Dashboard" });

            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session.Abandon();
            return RedirectToAction("Login", "Admin", new { area = "Dashboard" });
        }

        public ActionResult Adminler()
        {
            return View(db.Admins.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Admin admin, string Password, string Email)
        {
            if (ModelState.IsValid)
            {
                admin.Password = Crypto.Hash(Password, "MD5");
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(admin);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Adminler", "Admin");
            }
            var a = db.Admins.Where(x => x.AdminId == id).SingleOrDefault();
            return View(a);
        }
        [HttpPost]
        public ActionResult Edit(int id, Admin admin, string Password, string Email)
        {
            if (ModelState.IsValid)
            {
                var a = db.Admins.Where(x => x.AdminId == id).SingleOrDefault();
                a.Password = Crypto.Hash(Password, "MD5");
                a.Email = admin.Email;
                a.Authorization = admin.Authorization;
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(admin);
        }

        public ActionResult Delete(int id)
        {
            var a = db.Admins.Where(x => x.AdminId == id).SingleOrDefault();
            if (a != null)
            {
                db.Admins.Remove(a);
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View();
        }
    }
}