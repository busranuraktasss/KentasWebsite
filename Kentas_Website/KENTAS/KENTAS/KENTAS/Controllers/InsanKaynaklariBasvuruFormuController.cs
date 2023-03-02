using KENTAS.Models;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace KENTAS.Controllers
{
    public class InsanKaynaklariBasvuruFormuController : Controller
    {
        Entities db = new Entities();
        // GET: InsanKaynaklariBasvuruFormu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var applicationForm = new JobApplicationForm();
            return View(applicationForm);
        }

        [HttpPost]
        public ActionResult Create(JobApplicationForm jobApplicationForm)
        {
            var user = db.JobApplicationForms.Any(u => u.CepTel == jobApplicationForm.CepTel);
            if (user)
            {
                ViewBag.Uyari = "Daha önce kayıtlandınız.";
                return View(jobApplicationForm);
            }

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var finfo = new FileInfo(file.FileName);
                    if (finfo.Extension != ".doc" && finfo.Extension != ".pdf" && finfo.Extension != ".docx")
                    {
                        ViewBag.Uyari = "Eklemiş olduğunuz belgenin uzantısı sadece Word(.doc,.docx) veya PDF(.pdf) Olabilir.";
                        return View(jobApplicationForm);
                    }
                    else
                    {
                        var path = Path.Combine(Server.MapPath("~/Uploads/Cv/"), fileName);
                        file.SaveAs(path);
                        jobApplicationForm.CVUrl = $"/Uploads/Cv/{fileName}";
                    }
                }
            }

            db.JobApplicationForms.Add(jobApplicationForm);
            db.SaveChanges();
            TempData["Kayit"] = "Kayıt Oluşturuldu.";

            return RedirectToAction("Create", "InsanKaynaklariBasvuruFormu", TempData);
        }


    }
}
