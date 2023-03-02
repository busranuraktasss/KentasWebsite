using KENTAS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using KENTAS.Areas.Dashboard.Data;
using Microsoft.Ajax.Utilities;
using System.Linq;
using System.Security.Cryptography;
using System.Data.Entity.Migrations;

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

        [HttpPost]
        public JsonResult getForms()
        {
            var showFormRequest = new List<showFormRequest>();
            using (Entities ct = new Entities())
            {
                ct.SubscriptionForms.ForEach(s =>
                {
                    var dd = new showFormRequest();
                    var cc = showFormRequest.FirstOrDefault(k => k.plaka == s.Plaka);
                    if (cc != null)
                    {
                        cc.plaka = s.Plaka;

                    }
                    else
                    {
                        dd.Id = s.ID;
                        dd.tcno = s.TcKimlikNo;
                        dd.name = s.Ad;
                        dd.surname = s.Soyad;
                        dd.plaka = s.Plaka;
                        dd.Adress = s.Adres;
                        dd.aractip = s.AracTipi;
                        dd.phone = s.CepTel;
                        dd.createdDate = s.CreatedDate == null ? "-" : s.CreatedDate.ToString();
                        dd.status = s.Status;


                        var showFormServiceRequest = new List<showFormServiceRequest>();

                        var list = ct.SubsFormServices.Where(e => e.SubsFormId == dd.Id).Select(q => q.OurServiceId).ToList();
                        var aaa = 0;
                        ct.SubsFormServices.ForEach(k =>
                        {
                            while (aaa < list.Count())
                            {
                                var nn = new showFormServiceRequest();
                                var mm = showFormServiceRequest.FirstOrDefault(m => m.SubsFormId == dd.Id);

                                if (mm != null)
                                {
                                    var serviceId1 = list[aaa];
                                    mm.ourservice += " , " + ct.OurServices.Where(v => v.Id == serviceId1).Select(t => t.Title).FirstOrDefault();
                                }
                                else
                                {
                                    if (list.Count() == 0)
                                    {
                                        nn.ourservice = "Seçilmedi.";
                                    }
                                    else
                                    {
                                        var serviceId = list[aaa];
                                        nn.ourservice = ct.OurServices.Where(t => t.Id == serviceId).Select(t => t.Title).FirstOrDefault();
                                        nn.SubsFormId = dd.Id;
                                    }


                                    showFormServiceRequest.Add(nn);

                                }
                                aaa++;
                            }
                        });

                        dd.ourservice = showFormServiceRequest.Select(j => j.ourservice).FirstOrDefault();

                        showFormRequest.Add(dd);
                    }





                });
                showFormRequest = (from o in showFormRequest orderby o.Id descending orderby o.status descending select o).ToList();

                return Json(new { msg = "Başarılı", status = true, data = showFormRequest }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult deleteForm(int request)
        {
            try
            {
                using (Entities ct = new Entities())
                {

                    var subsFormServiceIds = ct.SubsFormServices.Where(w => w.SubsFormId == request).Select(s => s.Id).ToList();

                    for (var aa = 0; aa < subsFormServiceIds.Count(); aa++)
                    { 
                        var subsFormServiceId = subsFormServiceIds[aa];
                        var subsFormService = ct.SubsFormServices.Where(w => w.Id == subsFormServiceId);

                        ct.SubsFormServices.RemoveRange(subsFormService);
                        ct.SaveChanges();
                    }


                    var delete_listplate= ct.SubscriptionForms.Where(w => w.ID == request).Select(s => s.Plaka).FirstOrDefault();
                    var delete_listIds= ct.SubscriptionForms.Where(w => w.Plaka == delete_listplate).Select(s => s.ID).ToList();



                    for (var bb = 0; bb < delete_listIds.Count(); bb++)
                    {
                        var delete_subFormId = delete_listIds[bb];


                        var delete_subForm = ct.SubscriptionForms.Where(w => w.ID == delete_subFormId);

                        ct.SubscriptionForms.RemoveRange(delete_subForm);
                        ct.SaveChanges();
                    }




                    return Json(new { msg = "Silme işlemi başarılı", status = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Hata: " + ex.Message, status = false });
            }
        }


         [HttpPost]
        public JsonResult checkForm(int request)
        {
            try
            {
                using (Entities ct = new Entities())
                {

                    var update_listplate = ct.SubscriptionForms.Where(w => w.ID == request).Select(s => s.Plaka).FirstOrDefault();
                    var update_listIds = ct.SubscriptionForms.Where(w => w.Plaka == update_listplate).Select(s => s.ID).ToList();

                    for (var cc = 0; cc < update_listIds.Count(); cc++)
                    {
                        var update_subFormId = update_listIds[cc];


                        var update_subForm = ct.SubscriptionForms.Where(w => w.ID == update_subFormId).FirstOrDefault();

                        if(update_subForm.Status == 1 )
                        {
                            update_subForm.Status = 0;
                        }
                        else
                        {
                            update_subForm.Status = 1;

                        }
                        ct.SubscriptionForms.AddOrUpdate(update_subForm);
                        ct.SaveChanges();
                }
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
        }






    }
}

