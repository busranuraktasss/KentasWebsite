using KENTAS.Models;
using System.Linq;
using System.Web.Mvc;

namespace KENTAS.Controllers
{
    public class VizyonumuzController : Controller
    {
         Entities db = new Entities();

        public ActionResult Vizyon() 
        {
            return View(db.Visions.ToList());
        }
            

        // GET: Vizyonumuz/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
            

        // GET: Vizyonumuz/Create
        public ActionResult GetCreate()
        {
            return View();
        }

        // POST: Vizyonumuz/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Vizyonumuz/Edit/5
        public ActionResult Edit(int id) => View();

        // POST: Vizyonumuz/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Vizyonumuz/Delete/5
        public ActionResult Delete(int id) => View();

        // POST: Vizyonumuz/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}