using KENTAS.Areas.Dashboard.Data;
using KENTAS.Helper;
using KENTAS.Helper.DTO;
using KENTAS.Helper.payment;
using KENTAS.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KENTAS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "";

            using (Entities ct = new Entities())
            {
                var data = ct.PageHeaders.FirstOrDefault();
                

                ViewBag.newsHeader = data.newsHeader;
                ViewBag.aboutHeader = data.aboutHeader;
                ViewBag.whatwedoHeader = data.whatwedoHeader;
                ViewBag.visionHeader = data.visionHeader;
                ViewBag.missionHeader = data.missionHeader;
                ViewBag.photoGaleryHeader = data.photoGaleryHeader;
                ViewBag.contactHeader = data.contactHeader;
                ViewBag.advertsHeader = data.advertsHeader;
                ViewBag.activityHeader = data.activityHeader;

            }

            return View();
        }

        public PartialViewResult _Slider()
        {
            return PartialView("_Slider");
        }

        public PartialViewResult _News()
        {
            return PartialView("_News");
        }

        public PartialViewResult _PhotoGalery()
        {
            return PartialView("_PhotoGalery");
        }

        public PartialViewResult _Contact()
        {
            return PartialView("_Contact");
        }

        public PartialViewResult _Header()
        {
            return PartialView("_Header");
        }

        public PartialViewResult _About()
        {
            return PartialView("_About");
        }

        public PartialViewResult _WhatWeDo()
        {
            return PartialView("_WhatWeDo");
        }

        public PartialViewResult _Mission()
        {
            return PartialView("_Mission");
        }

        public PartialViewResult _Vision()
        {
            return PartialView("_Vision");
        }
        public PartialViewResult _Footer()
        {
            return PartialView("_Footer");
        }

        public PartialViewResult _Adverts()
        {
            return PartialView("_Adverts");
        }

        public PartialViewResult _HeaderContact()
        {
            return PartialView("_HeaderContact");
        }

        public PartialViewResult _HeaderContactAddress()
        {
            return PartialView("_HeaderContactAddress");
        }

        public PartialViewResult _FooterContact()
        {
            return PartialView("_FooterContact");
        }

        [HttpGet]
        public async Task<JsonResult> getMapsMarker()
        {
            using (Entities ct = new Entities())
            {
                var response = await ct.OurServices.Select(s=> new { Lat = s.Lat , Lng =  s.Lng}).ToListAsync();
                return Json(new { status = true, data = response, msg = "Başarılı" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> addBasvuruFormu(getBasvuruFormuRequest request)
        {
            try
            {
                using (Entities ct = new Entities())
                {
                  
                   
                        var newService = ct.SubscriptionForms.Add(new SubscriptionForm()
                        {

                            Ad = request.name,
                            Soyad = request.surname,
                            Adres = request.Adress,
                            AracTipi = request.aractip,
                            Plaka = request.plaka,
                            TcKimlikNo = request.tcno,
                            CepTel = request.phone,
                            CreatedDate = DateTime.Now,
                            Status = 1,
                            
                           
                            
                            
                        });
                         await ct.SaveChangesAsync();

                    for (var aa = 0; aa < request.mekanIds.Length; aa++)
                    {
                        var mekanId = request.mekanIds[aa];

                        var newSubsFormService = ct.SubsFormServices.Add(new SubsFormService()
                        {
                            OurServiceId = mekanId,
                            SubsFormId = newService.ID,
                        });
                        await ct.SaveChangesAsync();
                    }

                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult getSelectOurServices()
        {
            using (Entities ct = new Entities())
            {
                var ListCount = ct.OurServices.Where(w => w.Title != null)
                 .Select(s => new GetOurServicesRequest()
                 {
                     Id = s.Id,
                     Title = s.Title,

                 }).ToList();

                return Json(new { data = ListCount });
            }
               
        }
        
        [HttpPost]
        public JsonResult PlateControl(string plaka)
        {
            using (Entities ct = new Entities())
            {
                var plakaControl = ct.SubscriptionForms.Where(w => w.Plaka == plaka).Count();
                var control = false;
                if(plakaControl > 0)
                {
                    control = true;
                    return Json(new { data = control });
                }
                else
                {
                     control = false;

                    return Json(new { data = control });
                }            
            }
        }
        
        [HttpPost]
        public JsonResult getImage()
        {
            using (Entities ct = new Entities())
            {
                var getImage = ct.PhotoGaleries.Select(s => s.photoGaleryImage).ToList();
                    return Json(new { data = getImage });
                
                           
            }
        }
    }
}