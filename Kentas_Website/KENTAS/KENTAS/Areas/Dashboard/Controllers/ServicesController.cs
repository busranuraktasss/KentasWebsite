using KENTAS.Areas.Dashboard.Data;
using KENTAS.Models;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        // GET: Dashboard/Services
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> getServices()
        {
            using (Entities ct = new Entities())
            {
                var response = await ct.OurServices.ToListAsync();
                return Json(new { msg = "Başarılı", status = true, items = response }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> addService(getServiceRequest request)
        {
            try
            {
                using (Entities ct = new Entities())
                {
                    if (request.Id == 0)
                    {
                        var newService = ct.OurServices.Add(new OurService()
                        {
                            Adress = request.Adress,
                            Lat = request.Lat,
                            Lng = request.Lng,
                            Subtitile = request.Subtitile,
                            Title = request.Title,
                            WorkingHours = request.Sabahsaat + " / " + request.Aksamsaat
                        });
                        await ct.SaveChangesAsync();
                        return Json(new { msg = "Yeni hizmet eklendi", status = true }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {

                        var editService = await ct.OurServices.FirstOrDefaultAsync(f => f.Id == request.Id);
                        if (editService != null)
                        {
                            editService.Adress = request.Adress;
                            editService.Lat = request.Lat;
                            editService.Lng = request.Lng;
                            editService.Subtitile = request.Subtitile;
                            editService.Title = request.Title;
                            editService.WorkingHours = request.Sabahsaat + " / " + request.Aksamsaat;
                            await ct.SaveChangesAsync();
                        }
                        return Json(new { msg = "Hizmet düzenlendi.", status = true }, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Hata: " + ex.Message, status = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> deleteService(int request)
        {
            try
            {
                using (Entities ct = new Entities())
                {
                    var deleteService = await ct.OurServices.FirstOrDefaultAsync(f => f.Id == request);
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

        [HttpGet]
        public async Task<JsonResult> getOneService(int request)
        {
            try
            {
                using (Entities ct = new Entities())
                {
                    var oneService = await ct.OurServices.FirstOrDefaultAsync(f => f.Id == request);
                    return Json(new { msg = "Başarılı", status = true, data = oneService }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Hata: " + ex.Message, status = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}