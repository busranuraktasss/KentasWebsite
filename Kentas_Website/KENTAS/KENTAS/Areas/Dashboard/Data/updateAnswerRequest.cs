using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KENTAS.Areas.Dashboard.Data
{
    public class updateAnswerRequest
    {
        public int id { get; set; }
        public int soruId { get; set; }
        public string cevap { get; set; }
    }
}