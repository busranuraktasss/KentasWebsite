using KENTAS.Helper;
using KENTAS.Models;
using System.Linq;
using System.Web.Mvc;

namespace KENTAS.Controllers
{
    public class GaleriController : Controller
    {
        Entities db = new Entities();

        public ActionResult Index()
        {
            return View(db.PhotoGaleries.ToList());
        }

        // GET: Galeri/Details/5
        public ActionResult ShowDetails(int h = 1) 
        {
            var currentData = db.PhotoGaleries.Where(w => w.TitileId == h).Select(s => new getGalryModel()
            {
                TitileId = s.TitileId,
                photoGaleryId = s.photoGaleryId,
                photoGaleryImage = s.photoGaleryImage
            }).ToList();
            return View(currentData);
        }
    }
       
}