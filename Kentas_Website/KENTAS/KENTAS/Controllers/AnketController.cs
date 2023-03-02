using KENTAS.Helper.DTO;
using KENTAS.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using getPlaceRequest = KENTAS.Areas.Dashboard.Data.getPlaceRequest;

namespace KENTAS.Controllers
{
    public class AnketController : Controller
    {
        //GET: Anket
        Entities db = new Entities();
        public ActionResult Index()
        {
            ViewBag.mekanlar = db.Mekanlars.Select(s => new { Id = s.Id, Adi = s.Adi });
            return View(db.Mekanlars.ToList());
        }

        [HttpPost]
        public JsonResult GetSelect1()
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

        [HttpPost]
        public JsonResult GetSelect2(int mekanId)
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
        public JsonResult GetSurveyName(int anketId)
        {
            var anketAdi = db.Anketlers.Where(w => w.Id == anketId).Select(s => s.Adi).FirstOrDefault();
            return Json(new { data = anketAdi });
        }
        [HttpPost]
        public JsonResult SurveyQuestion(int anketId)
        {
            var current = db.AnketSorularis.Where(w => w.AnketId == anketId && w.CevapTipi == 1).Select(s => s.Id).ToList();

            var data1 = new List<int>();

            for (var bb = 0; bb < current.Count(); bb++)
            {
                var _soruId = current[bb];

                var current1 = db.AnketSorulariSecenekleris.Where(w => w.SoruId == _soruId).Select(s => s.Id).Count();

                if (current1 > 1)
                {
                    data1.Add(_soruId);

                }
            }
            var data2 = new List<AnketSorulari>();

            var data3 = db.AnketSorularis.Where(w => w.AnketId == anketId).ToList();
            for (var aa = 0; aa < data3.Count(); aa++)
            {
                if (data3[aa].CevapTipi == 0)
                {
                    data2.Add(data3[aa]);
                }
                else
                {
                    var dd = data1.Contains(data3[aa].Id);
                    if (dd)
                    {
                        data2.Add(data3[aa]);
                    }
                }
            }
            return Json(new { data = data2.Select(s => new { Id = s.Id, Soru = s.Soru, CevapTipi = s.CevapTipi }) });
        }

        [HttpPost]
        public JsonResult QuestionAnswers(int soruID)
        {
            var ListAnswers = db.AnketSorulariSecenekleris.Where(w => w.SoruId == soruID)
                .Select(s => new questionAnswersRequest()
                {
                    Id = s.Id,
                    Icerik = s.Icerik,
                }).ToList();
            return Json(new { Data = ListAnswers });
        }

        [HttpPost]
        public async Task<JsonResult> SaveSurveyAnswer(getAnswerRequest pr)
        {
            try
            {
                var questionListCount = pr.questionList.Count();

                for (int i = 0; i < questionListCount; i++)
                {
                    var question_current = pr.questionList[i];
                    var YorumSorulari =await db.AnketSorularis.Where(w => w.AnketId == pr.surveyId && w.Id == question_current).Select(s => s.CevapTipi).FirstAsync();
                    if (YorumSorulari == 0)
                    {
                        var yorum = pr.answerList[i];

                        var current1 = new AnketCevaplari()
                        {
                            AnketId = pr.surveyId,
                            SoruId = question_current,
                            Yorum = yorum,
                            SecenekId = null,
                        };
                        db.AnketCevaplaris.Add(current1);
                        await db.SaveChangesAsync();

                        var anketCevaplari1 = current1.Id;

                        db.AnketCevaplamaSonus.Add(new AnketCevaplamaSonu()
                        {
                            AnketId = pr.surveyId,
                            AnketCevaplariId = anketCevaplari1,
                            MekanId = pr.locationId,
                            Oneri = pr.Oneri,
                            AdiSoyadi = pr.Ad + ' ' + pr.Soyad,
                            Eposta = pr.Email,
                            TeL = pr.Tel,
                            Ktarihi = DateTime.Now,
                        });
                        await db.SaveChangesAsync();

                    }
                    else
                    {
                        var test = pr.answerList[i].AsInt();
                        if (test != 0)
                        {
                            var current2 = new AnketCevaplari()
                            {
                                AnketId = pr.surveyId,
                                SoruId = question_current,
                                Yorum = null,
                                SecenekId = test,
                            };
                            db.AnketCevaplaris.Add(current2);
                            await db.SaveChangesAsync();

                            var anketCevaplari2 = current2.Id;

                            db.AnketCevaplamaSonus.Add(new AnketCevaplamaSonu()
                            {
                                AnketId = pr.surveyId,
                                AnketCevaplariId = anketCevaplari2,
                                MekanId = pr.locationId,
                                Oneri = pr.Oneri,
                                AdiSoyadi = pr.Ad + ' ' + pr.Soyad,
                                Eposta = pr.Email,
                                TeL = pr.Tel,
                                Ktarihi = DateTime.Now,
                            });
                            await db.SaveChangesAsync();
                        }

                    }

                }
                return Json(new { Status = true, data = "", Messages = "Ekleme işlemi başarılı" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult ContactInformation(int anketId)
        {
            var current = db.Anketlers.Where(w => w.Id == anketId).Select(s => s.IletisimBilgiMecburiMi).FirstOrDefault();
            return Json(new { data = current });

        }
        
        [HttpPost]
        public JsonResult progressBarCount(int anketId)
        {
            var _anketSorulariIds = db.AnketSorularis.Where(w => w.AnketId == anketId).Select(s => s.Id).ToList();

            var QuestionCount = 0;

            for(var aa = 0; aa<_anketSorulariIds.Count(); aa++)
            {
                var _anketSorulariId = _anketSorulariIds[aa];
                var _anketYorumSorulari = db.AnketSorularis.Where(w => w.Id == _anketSorulariId && w.CevapTipi == 0).Count();

                QuestionCount = QuestionCount + _anketYorumSorulari;
            }

            for(var bb = 0;bb<_anketSorulariIds.Count; bb++)
            {
                var _anketSorulariId = _anketSorulariIds[bb];
                var _anketTestSorulariIds = db.AnketSorularis.Where(w => w.Id == _anketSorulariId && w.CevapTipi == 1).Select(s =>s.Id).ToList();

                for(var cc = 0; cc < _anketTestSorulariIds.Count(); cc++)
                {
                    var _anketTestSorulariId = _anketTestSorulariIds[cc];
                    var _anketTestSorulari = db.AnketSorulariSecenekleris.Where(w => w.SoruId == _anketTestSorulariId).Count();
                    
                    if(_anketTestSorulari > 1)
                    {
                        QuestionCount++;
                    }
                }
            }

            return Json(new { data = QuestionCount });

        }


    }
}