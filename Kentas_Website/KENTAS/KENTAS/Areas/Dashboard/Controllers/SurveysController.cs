using KENTAS.Areas.Dashboard.Data;
using KENTAS.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI.WebControls;

namespace KENTAS.Areas.Dashboard.Controllers
{
    [Authorize]
    public class SurveysController : Controller
    {
        Entities db = new Entities();

        public ActionResult Index()
        {
            return View(db.Anketlers.ToList());
        }
        public ActionResult Questions(int id)
        {
            var AnketId = db.Anketlers.Where(x => x.Id == id).SingleOrDefault();
            var sId = db.AnketSorularis.Find(id);
            return View("Questions", AnketId);
        }
        public ActionResult Answers(int id)
        {
            var SoruId = db.AnketSorularis.Where(x => x.Id == id).SingleOrDefault();
            var sId = db.AnketSorulariSecenekleris.Find(id);
            return View("Answers", SoruId);
        }
        public ActionResult Results()
        {
            return View();
        }

        public ActionResult ResultDetails(int id)
        {
            var AnketId = db.Anketlers.Where(x => x.Id == id).SingleOrDefault();
            var sId = db.Anketlers.Find(id);
            return View("ResultDetails", AnketId);
        }
        public ActionResult ResultDetailsNotInfo(int id)
        {
            var AnketId = db.Anketlers.Where(x => x.Id == id).SingleOrDefault();
            var sId = db.Anketlers.Find(id);
            return View("ResultDetailsNotInfo", AnketId);
        }

        public ActionResult Participants(int id)
        {
            var SoruId = db.AnketSorularis.Where(t => t.Id == id).SingleOrDefault();
            var sId = db.AnketSorularis.Find(id);
            return View("Participants", SoruId);
        }

        // POST: Surveys/SavePlace
        [HttpPost]
        public JsonResult SavePlace(string MekanAdi)
        {
            try
            {
                var MekanControl = db.Mekanlars.Where(w => w.Adi == MekanAdi).Count();

                if (MekanControl > 0)
                {
                    return Json(new { Status = false, data = MekanControl, Messages = "Aynı isme sahip anket var." });
                }
                else
                {
                    db.Mekanlars.Add(new Mekanlar()
                    {
                        Adi = MekanAdi
                    });
                    db.SaveChangesAsync();
                    return Json(new { Status = true, Messages = "Ekleme işlemi başarılı" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }
        // POST: Surveys/SelectedLoc
        [HttpPost]
        public JsonResult SelectedLoc(int sId)
        {
            try
            {
                var current = db.Anketlers.Where(w => w.Id == sId).Select(s => s.Adi).FirstOrDefault();

                var current2 = db.Anketlers.Where(w => w.Adi == current).Select(s => s.MekanId).ToList();
                var data2 = new List<Mekanlar>();
                var data3 = db.Mekanlars.ToList();
                foreach (var item in data3 )
                {
                    var dd = current2.Contains(item.Id);
                        if (!dd)
                    {
                        data2.Add(item);
                    }
                        
                }
                
                return Json(new
                {
                    data = data2.Select(s => new { adi= s.Adi ,Id= s.Id})
                });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }


        // POST: Surveys/SaveSurvey
        [HttpPost]
        public JsonResult SaveSurvey(string AnketAdi, int[] MekanIds, bool bilgi)
        {
            try
            {
                var anketId = db.Anketlers.Where(w => w.Adi == AnketAdi).Select(s => s.Id).FirstOrDefault();
                var count = MekanIds.Length;
                var k = 0;

                while (k < MekanIds.Length)
                {

                    var current1 = db.Anketlers.Add(new Anketler()
                    {
                        Adi = AnketAdi,
                        KTarihi = DateTime.Now,
                        IletisimBilgiMecburiMi = bilgi,
                        MekanId = MekanIds[k]
                    });
                    db.SaveChanges();

                    var current3 = db.AnketSorularis.Where(w => w.AnketId == anketId).Select(s => new { s.Soru, s.CevapTipi, s.Id }).ToList();
                    for (var y = 0; y < current3.Count(); y++)
                    {
                        var AnketId = current1.Id;
                        var Soru = current3[y].Soru;
                        var CevapTipi = current3[y].CevapTipi;
                        var current4 = db.AnketSorularis.Add(new AnketSorulari()
                        {
                            AnketId = AnketId,
                            Soru = Soru,
                            CevapTipi = CevapTipi,
                        });
                        db.SaveChanges();

                        var soruıd = current3[y].Id;

                        var SoruId = current4.Id;

                        var current5 = db.AnketSorulariSecenekleris.Where(w => w.SoruId == soruıd).Select(s => s.Icerik).ToList();

                        for (var z = 0; z < current5.Count(); z++)
                        {
                            var Icerik = current5[z];
                            db.AnketSorulariSecenekleris.Add(new AnketSorulariSecenekleri()
                            {
                                SoruId = SoruId,
                                Icerik = Icerik,
                            });

                            db.SaveChanges();
                        }
                    }
                    k++;
                }
                return Json(new { Status = true, Messages = "Ekleme işlemi başarılı" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        // POST: Surveys/SaveQuestion
        [HttpPost]
        public JsonResult SaveQuestion(int anketId, string soru, int cevapTipi)
        {
            try
            {
                var _samequestion = db.AnketSorularis.Where(w => w.Soru == soru).Count();
                if(_samequestion > 0)
                {
                    return Json(new { Status = false , data = 1});
                }
                else
                {
                    var anketAdi = db.Anketlers.Where(w => w.Id == anketId).Select(s => s.Adi).FirstOrDefault();
                    var AnketIds = db.Anketlers.Where(w => w.Adi == anketAdi).Select(s => s.Id).ToList();

                    for (var aa = 0; aa < AnketIds.Count(); aa++)
                    {
                        var AnketId = AnketIds[aa];
                        db.AnketSorularis.Add(new AnketSorulari()
                        {
                            AnketId = AnketId,
                            Soru = soru,
                            CevapTipi = cevapTipi,
                        });
                        db.SaveChanges();

                    }
                    return Json(new { Status = true, Messages = "Ekleme işlemi başarılı" });
                }


                
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        // POST: Surveys/SaveAnswer
        [HttpPost]
        public JsonResult SaveAnswer(int soruId, string cevap)
        {
            try
            {
                var _sameanswer = db.AnketSorulariSecenekleris.Where(w => w.Icerik == cevap).Count();
                if(_sameanswer > 1)
                {
                    return Json(new { Status = false, data= 1 });

                }
                else
                {
                    db.AnketSorulariSecenekleris.Add(new AnketSorulariSecenekleri()
                    {
                        SoruId = soruId,
                        Icerik = cevap
                    });
                    db.SaveChangesAsync();
                    return Json(new { Status = true, Messages = "Ekleme işlemi başarılı" });
                }


                
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetSelect1()
        {
            var ListCount = db.Mekanlars.Where(w => w.Adi != null)
                .Select(s => new getPlaceRequest()
                {
                    Id = s.Id,
                    MekanAdi = db.Mekanlars.Where(t => t.Id == s.Id).Select(v => v.Adi).FirstOrDefault(),

                }).ToList();

            return Json(new { data = ListCount });
        }

        //ShowSurveyTable
        [HttpPost]
        public JsonResult ShowSurveyAjax()
        {
            try
            {
                var showSurveyRequest = new List<showSurveyRequest>();
                db.Anketlers.ForEach(s =>
                    {
                        var dd = new showSurveyRequest();
                        var cc = showSurveyRequest.FirstOrDefault(k => k.AnketAdi == s.Adi);
                        if (cc != null)
                        {
                            cc.MekanAdi += " , " + db.Mekanlars.Where(t => t.Id == s.MekanId).Select(v => v.Adi).FirstOrDefault();
                        }
                        else
                        {
                            dd.Id = s.Id;
                            dd.AnketAdi = s.Adi;
                            dd.MekanAdi = db.Mekanlars.Where(t => t.Id == s.MekanId).Select(v => v.Adi).FirstOrDefault();
                            dd.KTarihi = s.KTarihi;
                            dd.IletisimBilgiMecburiMi = s.IletisimBilgiMecburiMi;
                            dd.Count = db.AnketSorularis.Where(w => w.AnketId == s.Id).Select(v => v.Id).Count();
                            showSurveyRequest.Add(dd);
                        }


                    });
                var recordsTotal = showSurveyRequest.Count();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = showSurveyRequest
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //ShowLocationTable
        [HttpPost]
        public JsonResult ShowLocationAjax()
        {
            try
            {
                var recordsTotal = db.Mekanlars.Count();
                var showLocationRequest = db.Mekanlars
                    .Select(s => new showSurveyRequest()
                    {
                        Id = s.Id,
                        MekanAdi = s.Adi,

                    }).ToList();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = showLocationRequest
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ShowQuestionAjax(int anketId)
        {
            try
            {
                var recordsTotal = db.AnketSorularis.Where(w => w.AnketId == anketId).Count();
                var QuestionListCount = db.AnketSorularis.Where(w => w.AnketId == anketId)
                    .Select(s => new showQuestionRequest()
                    {
                        Id = s.Id,
                        AnketId = s.AnketId,
                        Soru = s.Soru,
                        CevapTipi = s.CevapTipi,
                        Count = db.AnketSorulariSecenekleris.Where(w => w.SoruId == s.Id).Select(t => t.Id).Count()


                    }).ToList();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = QuestionListCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ShowAnswerAjax(int SoruId)
        {
            try
            {
                var recordsTotal = db.AnketSorulariSecenekleris.Where(w => w.SoruId == SoruId).Count();
                var AnswersListCount = db.AnketSorulariSecenekleris.Where(w => w.SoruId == SoruId)
                    .Select(s => new showAnswerRequest()
                    {
                        Id = s.Id,
                        Soru = db.AnketSorularis.Where(w => w.Id == s.SoruId).Select(n => n.Soru).FirstOrDefault(),
                        Icerik = s.Icerik,
                    }).ToList();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = AnswersListCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        //Delete Survey
        [HttpGet]
        public JsonResult DeleteSurvey(int pr)
        {
            try
            {
                var anketAdi = db.Anketlers.Where(w => w.Id == pr).Select(s => s.Adi).FirstOrDefault();
                var anketIds = db.Anketlers.Where(w => w.Adi == anketAdi).Select(s => s.Id).ToList();

                for (var aa = 0; aa < anketIds.Count(); aa++)
                {
                    var anketId = anketIds[aa];

                    //AnketCevaplamaSonu Tablosu Delete İşlemi
                    var AnketCevaplaSonu = db.AnketCevaplamaSonus.Where(w => w.AnketId == anketId);
                    if (AnketCevaplaSonu == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                    db.AnketCevaplamaSonus.RemoveRange(AnketCevaplaSonu);//Hard Delete
                    db.SaveChanges();

                    //AnketCevaplama Tablosu Delete İşlemi
                    var AnketCevapları = db.AnketCevaplaris.Where(w => w.AnketId == anketId);
                    if (AnketCevapları == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                    db.AnketCevaplaris.RemoveRange(AnketCevapları);//Hard Delete
                    db.SaveChanges();

                    //AnketSoruSecenekleri Tablosu Delete İşlemi
                    var SoruIds = db.AnketSorularis.Where(w => w.AnketId == anketId).Select(w => w.Id).ToList();

                    for (var s = 0; s < SoruIds.Count(); s++)
                    {
                        var SoruId = SoruIds[s];
                        var AnketSoruSecenekleri = db.AnketSorulariSecenekleris.Where(w => w.SoruId == SoruId);
                        if (AnketSoruSecenekleri == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                        db.AnketSorulariSecenekleris.RemoveRange(AnketSoruSecenekleri);
                        db.SaveChanges();
                    }

                    //AnketSorulari Tablosu Delete İşlemi
                    var AnketSorulari = db.AnketSorularis.Where(w => w.AnketId == anketId);
                    if (AnketSorulari == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                    db.AnketSorularis.RemoveRange(AnketSorulari);//Hard Delete
                    db.SaveChanges();

                    //Anket Tablosu Delete İşlemi
                    var Anket = db.Anketlers.Where(w => w.Id == anketId);
                    if (Anket == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                    db.Anketlers.RemoveRange(Anket);//Hard Delete
                    db.SaveChanges();

                }


                return Json(new { Status = true, data = "" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", messages = ex.Message });
            }
        }

        //DeleteQuestion
        [HttpGet]
        public JsonResult DeleteQuestion(int request)
        {
            try
            {
                var AnketCevaplarıIds = db.AnketCevaplaris.Where(w => w.SoruId == request).Select(w => w.Id).ToList();

                for (var ss = 0; ss < AnketCevaplarıIds.Count(); ss++)
                {
                    var AnketCevaplariId = AnketCevaplarıIds[ss];

                    //AnketCevaplamaSonu Tablosu Delete İşlemi
                    var AnketCevaplaSonu = db.AnketCevaplamaSonus.Where(w => w.AnketCevaplariId == AnketCevaplariId);
                    if (AnketCevaplaSonu == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });
                    db.AnketCevaplamaSonus.RemoveRange(AnketCevaplaSonu);
                    db.SaveChanges();

                    //AnketCevaplama Tablosu Delete İşlemi
                    var AnketCevapları = db.AnketCevaplaris.Where(w => w.Id == AnketCevaplariId);
                    if (AnketCevapları == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });
                    db.AnketCevaplaris.RemoveRange(AnketCevapları);//Hard Delete
                    db.SaveChanges();

                }

                var AnketSoruSecenekleriIds = db.AnketSorulariSecenekleris.Where(w => w.SoruId == request).Select(w => w.Id).ToList();

                for (var kk = 0; kk < AnketSoruSecenekleriIds.Count(); kk++)
                {
                    var AnketSoruSecenekleriId = AnketSoruSecenekleriIds[kk];

                    //AnketSoruSecenekleri Tablosu Delete İşlemi
                    var AnketSoruSecenekleri = db.AnketSorulariSecenekleris.Where(w => w.Id == AnketSoruSecenekleriId);
                    if (AnketSoruSecenekleri == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                    db.AnketSorulariSecenekleris.RemoveRange(AnketSoruSecenekleri);
                    db.SaveChanges();
                }

                //AnketSorulari Tablosu Delete İşlemi
                var AnketSorulari = db.AnketSorularis.Where(w => w.Id == request);
                if (AnketSorulari == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                db.AnketSorularis.RemoveRange(AnketSorulari);//Hard Delete
                db.SaveChanges();

                return Json(new { Status = true, data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", messages = ex.Message });
            }
        }

        //DeleteAnswer
        [HttpGet]
        public JsonResult DeleteAnswer(int request)
        {
            try
            {
                var AnketCevaplarıIds = db.AnketCevaplaris.Where(w => w.SecenekId == request).Select(w => w.Id).ToList();

                for (var ss = 0; ss < AnketCevaplarıIds.Count(); ss++)
                {
                    var AnketCevaplariId = AnketCevaplarıIds[ss];

                    //AnketCevaplamaSonu Tablosu Delete İşlemi
                    var AnketCevaplaSonu = db.AnketCevaplamaSonus.Where(w => w.AnketCevaplariId == AnketCevaplariId);
                    if (AnketCevaplaSonu == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });
                    db.AnketCevaplamaSonus.RemoveRange(AnketCevaplaSonu);
                    db.SaveChanges();

                    //AnketCevaplama Tablosu Delete İşlemi
                    var AnketCevapları = db.AnketCevaplaris.Where(w => w.Id == AnketCevaplariId);
                    if (AnketCevapları == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });
                    db.AnketCevaplaris.RemoveRange(AnketCevapları);//Hard Delete
                    db.SaveChanges();

                }

                //AnketSoruSecenekleri Tablosu Delete İşlemi
                var AnketSoruSecenekleri = db.AnketSorulariSecenekleris.Where(w => w.Id == request);
                if (AnketSoruSecenekleri == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                db.AnketSorulariSecenekleris.RemoveRange(AnketSoruSecenekleri);
                db.SaveChanges();

                return Json(new { Status = true, data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", messages = ex.Message });
            }
        }

        //Update Survey
        [HttpGet]
        public JsonResult GetSetSurvey(int sId)
        {
            var currentData = db.Anketlers.Where(f => f.Id == sId).Select(s => new { s.Id, s.Adi, s.IletisimBilgiMecburiMi, s.KTarihi, s.MekanId }).FirstOrDefault();
            return Json(new { Status = true, data = currentData }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateSurvey(updateSurveyRequest pr)
        {
            try
            {
                var currnet2 = db.Anketlers.Where(w => w.Id == pr.Id).Select(s => s.Adi).FirstOrDefault();
                var current = db.Anketlers.Where(w => w.Adi == currnet2).Select(s => s.Id).ToList();
                var current_count = current.Count();
                if (current == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });
                var x = 0;

                while (x < current_count)
                {
                    var currentId = current[x];
                    var current1 = db.Anketlers.Where(w => w.Id == currentId).FirstOrDefault();
                    if (current1 == null) return Json(new { Status = false, data = "", Messages = "Soru bulunamadı." });
                    current1.Adi = pr.Adi;
                    current1.KTarihi = DateTime.Now;


                    db.Anketlers.AddOrUpdate(current1);
                    db.SaveChanges();


                    x++;
                }

                return Json(new { Status = true, Messages = "Değiştirme işlemi başarılı" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        //Update Question
        [HttpGet]
        public JsonResult GetSetQuestion(int sId)
        {
            var currentData = db.AnketSorularis.Where(f => f.Id == sId).Select(s => new { s.Id, s.AnketId, s.Soru, s.CevapTipi }).FirstOrDefault();
            return Json(new { Status = true, data = currentData }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateQuestion(updateQuestionRequest pr)
        {
            try
            {
                var current = await db.AnketSorularis.Where(w => w.Id == pr.Id && w.AnketId == pr.AnketId).FirstOrDefaultAsync();
                if (current == null) return Json(new { Status = false, data = "", Messages = "Soru bulunamadı." });
                current.Soru = pr.Soru;
                current.CevapTipi = pr.CevapTipi;

                if (pr.CevapTipi == 0)
                {
                    var AnswerIds = db.AnketSorulariSecenekleris.Where(w => w.SoruId == pr.Id).Select(s => s.Id).ToList();
                    for (var y = 0; y < AnswerIds.Count(); y++)
                    {
                        var answerId = AnswerIds[y];
                        var AnketCevaplariIds = db.AnketCevaplaris.Where(w => w.SecenekId == answerId).Select(s => s.Id).ToList();

                        for (var f = 0; f < AnketCevaplariIds.Count(); f++)
                        {
                            var AnketCevaplariId = AnketCevaplariIds[f];

                            //AnketCevaplamaSonu Tablosu Delete İşlemi
                            var AnketCevaplaSonu = db.AnketCevaplamaSonus.Where(w => w.AnketCevaplariId == AnketCevaplariId);
                            if (AnketCevaplaSonu == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                            db.AnketCevaplamaSonus.RemoveRange(AnketCevaplaSonu);//Hard Delete
                            db.SaveChanges();

                            //AnketCevaplama Tablosu Delete İşlemi
                            var AnketCevapları = db.AnketCevaplaris.Where(w => w.Id == AnketCevaplariId);
                            if (AnketCevapları == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                            db.AnketCevaplaris.RemoveRange(AnketCevapları);//Hard Delete
                            db.SaveChanges();
                        }

                        var SoruSecenekleri = db.AnketSorulariSecenekleris.Where(w => w.Id == answerId);
                        if (SoruSecenekleri == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                        db.AnketSorulariSecenekleris.RemoveRange(SoruSecenekleri);
                        db.SaveChanges();
                    }
                }

                db.AnketSorularis.AddOrUpdate(current);
                await db.SaveChangesAsync();

                return Json(new { Status = true, Messages = "Değiştirme işlemi başarılı" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        //Update Question
        [HttpGet]
        public JsonResult GetSetAnswer(int sId)
        {
            var currentData = db.AnketSorulariSecenekleris.Where(f => f.Id == sId).Select(s => new { s.Id, s.Icerik, s.SoruId }).FirstOrDefault();
            return Json(new { Status = true, data = currentData }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAnswer(updateAnswerRequest pr)
        {
            try
            {
                var current = await db.AnketSorulariSecenekleris.Where(w => w.Id == pr.id && w.SoruId == pr.soruId).FirstOrDefaultAsync();
                if (current == null) return Json(new { Status = false, data = "", Messages = "Soru bulunamadı." });
                current.Icerik = pr.cevap;

                db.AnketSorulariSecenekleris.AddOrUpdate(current);
                await db.SaveChangesAsync();

                return Json(new { Status = true, Messages = "Değiştirme işlemi başarılı" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult CheckBilgi(int request)
        {
            try
            {
                var currnet2 = db.Anketlers.Where(w => w.Id == request).Select(s => s.Adi).FirstOrDefault();
                var current = db.Anketlers.Where(w => w.Adi == currnet2).Select(s => s.Id).ToList();
                var current_count = current.Count();
                if (current == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });
                var x = 0;

                while (x < current_count)
                {
                    var currentId = current[x];
                    var current1 = db.Anketlers.Where(w => w.Id == currentId).FirstOrDefault();
                    if (current1 == null) return Json(new { Status = false, data = "", Messages = "Soru bulunamadı." });

                    if (current1.IletisimBilgiMecburiMi == true)
                    {
                        current1.IletisimBilgiMecburiMi = false;
                    }
                    else
                    {
                        current1.IletisimBilgiMecburiMi = true;
                    }
                    db.Anketlers.AddOrUpdate(current1);

                    db.SaveChanges();

                    x++;
                }

                return Json(new { Status = true, data = "", Messages = "" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", messages = ex.Message });

            }
        }


        //Results Datatable
        [HttpPost]
        public JsonResult getResultsAjax(int sId)
        {
            try
            {
                var recordsTotal = db.AnketSorularis.Where(w => w.AnketId == sId).Count();
                var ResultListCount = db.AnketSorularis.Where(w => w.AnketId == sId)
                    .Select(s => new getResultsRequest()
                    {
                        Id = s.Id,
                        Soru = s.Soru,
                        CevapTipi = s.CevapTipi,
                    }).ToList();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = ResultListCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        //Participiants Datatable
        [HttpPost]
        public JsonResult getParticipantsAjax(int sId)
        {
            try
            {
                var anketCevaplariIds = db.AnketCevaplaris.Where(w => w.SoruId == sId).Select(s => s.Id).ToList();
                var recordsTotal = db.AnketCevaplamaSonus.Where(w => anketCevaplariIds.Contains(w.AnketCevaplariId)).Count();
                var ParticipiantListCount = db.AnketCevaplamaSonus.Where(w => anketCevaplariIds.Contains(w.AnketCevaplariId) && w.AdiSoyadi != " ")
                    .Select(s => new getResultsRequest()
                    {
                        Id = s.Id,
                        AdiSoyadi = s.AdiSoyadi,
                        Eposta = s.Eposta,
                        Tel = s.TeL,
                        Cevap = db.AnketCevaplaris.Where(w => w.Id == s.AnketCevaplariId).Select(a => a.SecenekId).FirstOrDefault() == null ? db.AnketCevaplaris.Where(w => w.Id == s.AnketCevaplariId).Select(a => a.Yorum).FirstOrDefault() : (from x in db.AnketCevaplaris join y in db.AnketSorulariSecenekleris on x.SecenekId equals y.Id where x.Id == s.AnketCevaplariId select y.Icerik).FirstOrDefault(),
                        Oneri = s.Oneri,


                    }).ToList();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = ParticipiantListCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult getParticipantsCommentAll(int sId)
        {
            try
            {
                var anketCevaplariIds = db.AnketCevaplaris.Where(w => w.SoruId == sId).Select(s => s.Id).ToList();
                var recordsTotal = db.AnketCevaplamaSonus.Where(w => anketCevaplariIds.Contains(w.AnketCevaplariId)).Count();
                var ParticipiantListCount = db.AnketCevaplamaSonus.Where(w => anketCevaplariIds.Contains(w.AnketCevaplariId))
                    .Select(s => new getResultsRequest()
                    {
                        Id = s.Id,
                        AdiSoyadi = s.AdiSoyadi,
                        Eposta = s.Eposta,
                        Tel = s.TeL,
                        Cevap = db.AnketCevaplaris.Where(w => w.Id == s.AnketCevaplariId).Select(a => a.SecenekId).FirstOrDefault() == null ? db.AnketCevaplaris.Where(w => w.Id == s.AnketCevaplariId).Select(a => a.Yorum).FirstOrDefault() : (from x in db.AnketCevaplaris join y in db.AnketSorulariSecenekleris on x.SecenekId equals y.Id where x.Id == s.AnketCevaplariId select y.Icerik).FirstOrDefault(),
                        Oneri = s.Oneri,


                    }).ToList();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = ParticipiantListCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult getParticipantsCommentAjax(int sId)
        {
            try
            {
                var anketCevaplariIds = db.AnketCevaplaris.Where(w => w.SoruId == sId).Select(s => s.Id).ToList();
                var recordsTotal = db.AnketCevaplamaSonus.Where(w => anketCevaplariIds.Contains(w.AnketCevaplariId)).Count();
                var ParticipiantListCount = db.AnketCevaplamaSonus.Where(w => anketCevaplariIds.Contains(w.AnketCevaplariId))
                    .Select(s => new getResultsRequest()
                    {
                        Id = s.Id,
                        AdiSoyadi = s.AdiSoyadi,
                        Eposta = s.Eposta,
                        Tel = s.TeL,
                        Cevap = db.AnketCevaplaris.Where(w => w.Id == s.AnketCevaplariId).Select(a => a.SecenekId).FirstOrDefault() == null ? db.AnketCevaplaris.Where(w => w.Id == s.AnketCevaplariId).Select(a => a.Yorum).FirstOrDefault() : (from x in db.AnketCevaplaris join y in db.AnketSorulariSecenekleris on x.SecenekId equals y.Id where x.Id == s.AnketCevaplariId select y.Icerik).FirstOrDefault(),
                        Oneri = s.Oneri,


                    }).ToList();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = ParticipiantListCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult getLocationDelete(int sId)
        {
            try
            {
                var AnketAd = db.Anketlers.Where(w => w.Id == sId).Select(s => s.Adi).FirstOrDefault();
                var LocationListCount = db.Anketlers.Where(w => w.Adi == AnketAd)
                    .Select(s => new getResultsRequest()
                    {
                        AnketId = s.Id,
                        MekanAdi = db.Mekanlars.Where(t => t.Id == s.MekanId).Select(v => v.Adi).FirstOrDefault(),

                    }).ToList();

                return Json(new
                {
                    data = LocationListCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult deleteSurveyLoc(int[] request)
        {
            var loc_delete = db.Anketlers.Where(w => request.Contains(w.Id)).ToList();

            for (var aa = 0; aa < loc_delete.Count(); aa++)
            {
                var _anket_id = loc_delete[aa].Id;

                var _anket_cevaplama_sonu_ids = db.AnketCevaplamaSonus.Where(w => w.AnketId == _anket_id).Select(s => s.Id).ToList();
                for (var bb = 0; bb < _anket_cevaplama_sonu_ids.Count(); bb++)
                {
                    var _anket_cevaplama_sonu_id = _anket_cevaplama_sonu_ids[bb];

                    //AnketCevaplamaSonu Tablosu Delete İşlemi
                    var AnketCevaplaSonu = db.AnketCevaplamaSonus.Where(w => w.Id == _anket_cevaplama_sonu_id);
                    if (AnketCevaplaSonu == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                    db.AnketCevaplamaSonus.RemoveRange(AnketCevaplaSonu);//Hard Delete
                    db.SaveChanges();
                }

                var _anket_cevaplari_ids = db.AnketCevaplaris.Where(w => w.AnketId == _anket_id).Select(s => s.Id).ToList();
                for (var cc = 0; cc < _anket_cevaplari_ids.Count(); cc++)
                {
                    var _anket_cevaplari_id = _anket_cevaplari_ids[cc];

                    //AnketCevaplama Tablosu Delete İşlemi
                    var AnketCevapları = db.AnketCevaplaris.Where(w => w.Id == _anket_cevaplari_id);
                    if (AnketCevapları == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                    db.AnketCevaplaris.RemoveRange(AnketCevapları);//Hard Delete
                    db.SaveChanges();
                }

                //AnketSoruSecenekleri Tablosu Delete İşlemi
                var SoruIds = db.AnketSorularis.Where(w => w.AnketId == _anket_id).Select(w => w.Id).ToList();

                for (var dd = 0; dd < SoruIds.Count(); dd++)
                {
                    var SoruId = SoruIds[dd];
                    var AnketSoruSecenekleri = db.AnketSorulariSecenekleris.Where(w => w.SoruId == SoruId);
                    if (AnketSoruSecenekleri == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                    db.AnketSorulariSecenekleris.RemoveRange(AnketSoruSecenekleri);
                    db.SaveChanges();
                }

                var _anket_sorulari_Ids = db.AnketSorularis.Where(w => w.AnketId == _anket_id).Select(s => s.Id).ToList();
                for (var ee = 0; ee < _anket_sorulari_Ids.Count(); ee++)
                {
                    var _anket_sorulari_Id = _anket_sorulari_Ids[ee];
                    //AnketSorulari Tablosu Delete İşlemi
                    var AnketSorulari = db.AnketSorularis.Where(w => w.Id == _anket_sorulari_Id);
                    if (AnketSorulari == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                    db.AnketSorularis.RemoveRange(AnketSorulari);//Hard Delete
                    db.SaveChanges();
                }

                //Anket Tablosu Delete İşlemi
                var Anket = db.Anketlers.Where(w => w.Id == _anket_id);
                if (Anket == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                db.Anketlers.RemoveRange(Anket);//Hard Delete
                db.SaveChanges();
            }
            return Json(new { Status = true, data = "" }, JsonRequestBehavior.AllowGet);       
        }

        [HttpPost]
        public JsonResult deletePlace(int[] request)
        {
            for (var ff = 0; ff < request.Count(); ff++)
            {
                var _mekan_id = request[ff];

                var _anket_ids = db.Anketlers.Where(w => w.MekanId == _mekan_id).Select(t => t.Id).ToList();

                for (var aa = 0; aa < _anket_ids.Count(); aa++)
                {
                    var _anket_id = _anket_ids[aa];

                    var _anket_cevaplama_sonu_ids = db.AnketCevaplamaSonus.Where(w => w.AnketId == _anket_id).Select(s => s.Id).ToList();
                    for (var bb = 0; bb < _anket_cevaplama_sonu_ids.Count(); bb++)
                    {
                        var _anket_cevaplama_sonu_id = _anket_cevaplama_sonu_ids[bb];

                        //AnketCevaplamaSonu Tablosu Delete İşlemi
                        var AnketCevaplaSonu = db.AnketCevaplamaSonus.Where(w => w.Id == _anket_cevaplama_sonu_id);
                        if (AnketCevaplaSonu == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                        db.AnketCevaplamaSonus.RemoveRange(AnketCevaplaSonu);//Hard Delete
                        db.SaveChanges();
                    }

                    var _anket_cevaplari_ids = db.AnketCevaplaris.Where(w => w.AnketId == _anket_id).Select(s => s.Id).ToList();
                    for (var cc = 0; cc < _anket_cevaplari_ids.Count(); cc++)
                    {
                        var _anket_cevaplari_id = _anket_cevaplari_ids[cc];

                        //AnketCevaplama Tablosu Delete İşlemi
                        var AnketCevapları = db.AnketCevaplaris.Where(w => w.Id == _anket_cevaplari_id);
                        if (AnketCevapları == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                        db.AnketCevaplaris.RemoveRange(AnketCevapları);//Hard Delete
                        db.SaveChanges();
                    }

                    //AnketSoruSecenekleri Tablosu Delete İşlemi
                    var SoruIds = db.AnketSorularis.Where(w => w.AnketId == _anket_id).Select(w => w.Id).ToList();

                    for (var dd = 0; dd < SoruIds.Count(); dd++)
                    {
                        var SoruId = SoruIds[dd];
                        var AnketSoruSecenekleri = db.AnketSorulariSecenekleris.Where(w => w.SoruId == SoruId);
                        if (AnketSoruSecenekleri == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                        db.AnketSorulariSecenekleris.RemoveRange(AnketSoruSecenekleri);
                        db.SaveChanges();
                    }

                    var _anket_sorulari_Ids = db.AnketSorularis.Where(w => w.AnketId == _anket_id).Select(s => s.Id).ToList();
                    for (var ee = 0; ee < _anket_sorulari_Ids.Count(); ee++)
                    {
                        var _anket_sorulari_Id = _anket_sorulari_Ids[ee];
                        //AnketSorulari Tablosu Delete İşlemi
                        var AnketSorulari = db.AnketSorularis.Where(w => w.Id == _anket_sorulari_Id);
                        if (AnketSorulari == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                        db.AnketSorularis.RemoveRange(AnketSorulari);//Hard Delete
                        db.SaveChanges();
                    }

                    //Anket Tablosu Delete İşlemi
                    var Anket = db.Anketlers.Where(w => w.Id == _anket_id);
                    if (Anket == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                    db.Anketlers.RemoveRange(Anket);//Hard Delete
                    db.SaveChanges();

                }

                //Mekan Tablosu Delete İşlemi
                var Mekan = db.Mekanlars.Where(w => w.Id == _mekan_id);
                if (Mekan == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                db.Mekanlars.RemoveRange(Mekan);//Hard Delete
                db.SaveChanges();

            }
            return Json(new { Status = true, data = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost] //GetSelect1
        public JsonResult SelectLocation()
        {
            var ListCount2 = db.Anketlers.Where(w => w.AnketSorularis.Count != 0).Select(s => s.MekanId).ToList();
            var ListCount = db.Mekanlars.Where(t => t.Adi != null && t.Anketlers.Count != 0 && ListCount2.Contains(t.Id))
                .Select(s => new getPlaceRequest()
                {
                    Id = s.Id,
                    Adi = s.Adi,
                }).ToList();

            return Json(new { data = ListCount });

        }

        [HttpPost] //GetSelect2
        public JsonResult GetSurveySelf(int mekanId)
        {
            var ListCount = db.Anketlers.Where(t => t.MekanId == mekanId && t.Adi != null && t.AnketSorularis.Count != 0)
                .Select(s => new getPlaceRequest()
                {
                    Id = s.Id,
                    Adi = s.Adi,
                    MekanId = s.MekanId == null ? 0 : s.MekanId.Value
                }).ToList();

            return Json(new { data = ListCount });
        }

        [HttpPost]
        public JsonResult ResultDetailsInfoControl(int anketId)
        {
            var Info = db.Anketlers.Where(w => w.Id == anketId).Select(s => s.IletisimBilgiMecburiMi).FirstOrDefault();
            return Json(new { data = Info });
        }

        [HttpPost]
        public JsonResult getAnswerIstatistics(int sId)
        {
            try
            {
                var recordsTotal = db.AnketSorulariSecenekleris.Where(w => w.SoruId == sId).Count();
                var ParticipiantListCount = db.AnketSorulariSecenekleris.Where(w => w.SoruId == sId)
                    .Select(s => new getAnswerIstatisticsRequest()
                    {
                        Id = s.Id,
                        Icerik = s.Icerik,
                        SecilmeSayisi = db.AnketCevaplaris.Where(w => w.SecenekId == s.Id).Count(),
                        ToplamSecilmeSayisi = db.AnketCevaplaris.Where(w => w.SoruId == sId).Count(),


                    }).ToList();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    data = ParticipiantListCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult getParticipantAnswers(int request)
        {
            try
            {
                var ParticipiantAnswerListCount = db.AnketSorularis.Where(w => w.AnketId == request)
                    .Select(s => new getAnswerAjaxRequest()
                    {
                        Id = s.Id,
                        Soru = s.Soru,
                    }).ToList();

                return Json(new { data = ParticipiantAnswerListCount });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult getParticipantAnswersAjax(int sId)
        {
            try
            {
                var getParticipantAnswerAjaxRequest = new List<getParticipantAnswerAjaxRequest>();

                db.AnketCevaplamaSonus.Where(w => w.AnketId == sId).ForEach(s =>
                {
                    var current = new getParticipantAnswerAjaxRequest();
                    var current1 = getParticipantAnswerAjaxRequest.FirstOrDefault(k => k.AdiSoyadi == s.AdiSoyadi && k.Tel == s.TeL);
                    if (current1 != null)
                    {
                        current1.AdiSoyadi = s.AdiSoyadi;
                    }
                    else
                    {
                        current.Id = s.Id;
                        current.AdiSoyadi = s.AdiSoyadi == null ? "Anonim" : s.AdiSoyadi;
                        current.Tel = s.TeL;
                        current.Oneri = s.Oneri;
                        current.Eposta = s.Eposta;
                        getParticipantAnswerAjaxRequest.Add(current);
                    }
                });
                var recordsTotal = getParticipantAnswerAjaxRequest.Count();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = getParticipantAnswerAjaxRequest
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult getAnswerAjax(int sId)
        {
            try
            {
                var sNameTel = db.AnketCevaplamaSonus.Where(w => w.Id == sId).Select(s => new { s.AdiSoyadi, s.TeL }).FirstOrDefault();

                var getAnswerAjaxRequest = new List<getAnswerAjaxRequest>();
                var anketCevaplariIds = db.AnketCevaplamaSonus.Where(w => w.AdiSoyadi == sNameTel.AdiSoyadi && w.TeL == sNameTel.TeL).Select(s => s.AnketCevaplariId).ToList();
                var recordsTotal = db.AnketCevaplamaSonus.Where(w => anketCevaplariIds.Contains(w.AnketCevaplariId)).Count();

                for (var aa = 0; aa < anketCevaplariIds.Count(); aa++)
                {
                    var anketcevaplariId = anketCevaplariIds[aa];
                    var soruId = db.AnketCevaplaris.Where(w => w.Id == anketcevaplariId).Select(s => s.SoruId).FirstOrDefault();
                    var current = new getAnswerAjaxRequest();

                    current.Soru = db.AnketSorularis.Where(w => w.Id == soruId).Select(s => s.Soru).FirstOrDefault();
                    current.Cevap = db.AnketCevaplaris.Where(w => w.Id == anketcevaplariId).Select(a => a.SecenekId).FirstOrDefault() == null ? db.AnketCevaplaris.Where(w => w.Id == anketcevaplariId).Select(a => a.Yorum).FirstOrDefault() : (from x in db.AnketCevaplaris join y in db.AnketSorulariSecenekleris on x.SecenekId equals y.Id where x.Id == anketcevaplariId select y.Icerik).FirstOrDefault();

                    getAnswerAjaxRequest.Add(current);
                }

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = getAnswerAjaxRequest
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult GetParticipantInfo(int sId)
        {
            try
            {
                var participantName = db.AnketCevaplamaSonus.Where(w => w.Id == sId).Select(s => s.AdiSoyadi).FirstOrDefault();



                return Json(new { data = participantName });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ParticipantsControl(int sId)
        {
            try
            {
                var control = db.AnketSorularis.Where(w => w.Id == sId).Select(s => s.AnketId).FirstOrDefault();

                var control1 = db.AnketCevaplamaSonus.Where(w => w.AnketId == control).Count();

                if (control1 > 0)
                {
                    return Json(new { data = true });
                }
                else
                {
                    return Json(new { data = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult getQuestionName(int sId)
        {
            try
            {
                var _questionName = db.AnketSorularis.Where(w => w.Id == sId).Select(s => s.Soru).FirstOrDefault();

                
                    return Json(new { data = _questionName });
                
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }
    }
}

