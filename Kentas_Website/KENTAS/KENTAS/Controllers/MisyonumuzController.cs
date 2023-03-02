using KENTAS.Models;
using System.Linq;
using System.Web.Mvc;

namespace KENTAS.Controllers
{
    public class MisyonumuzController : Controller
    {
        Entities db = new Entities();

        public ActionResult Misyon()
        {
            return View(db.Missions.ToList());
        }
        // GET: Misyonumuz/Details/5
        public ActionResult Details(int id) 
        { 
            return View(); 
        }
    }
}