using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KENTAS.Areas.Dashboard.Data
{
    public class updateQuestionRequest
    {
        public int Id { get; set; }
        public int AnketId { get; set; }
        public string Soru { get; set; }
        public int CevapTipi { get; set; }
    }
}