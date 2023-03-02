using KENTAS.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        // GET: Contact
        Entities db = new Entities();
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }


        // GET: Contact/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Contact");
            }
            var iletisim = db.Contacts.Where(x => x.contactId == id).SingleOrDefault();
            return View(iletisim);
        }

        // POST: Contact/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Contact contact)
        {
            if (ModelState.IsValid)
            {
                var c = db.Contacts.Where(x => x.contactId == id).SingleOrDefault();
                c.contactAddress = contact.contactAddress;
                c.contactFaxNumber = contact.contactFaxNumber;
                c.contactPhoneNumber = contact.contactPhoneNumber;
                c.contactImageSrc = contact.contactImageSrc;
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(contact);
        }


    }
}
