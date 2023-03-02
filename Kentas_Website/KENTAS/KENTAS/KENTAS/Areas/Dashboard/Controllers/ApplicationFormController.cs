using KENTAS.Models;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class ApplicationFormController : Controller
    {
        // GET: Dashboard/ApplicationForm
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> getForms()
        {
            using (Entities ct = new Entities())
            {
                var response = await ct.SubscriptionForms.ToListAsync();
                return Json(new { msg = "Başarılı", status = true, items = response }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> deleteForm(int request)
        {
            try
            {
                using (Entities ct = new Entities())
                {
                    var deleteService = await ct.SubscriptionForms.FirstOrDefaultAsync(f => f.ID == request);
                    if (deleteService != null)
                    {
                        ct.Entry(deleteService).State = EntityState.Deleted;
                        await ct.SaveChangesAsync();
                        return Json(new { msg = "Silme işlemi başarılı", status = true }, JsonRequestBehavior.AllowGet);

                    }
                    else
                        return Json(new { msg = "Hata: Hizmet bulunamadı", status = false }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { msg = "Hata: " + ex.Message, status = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}