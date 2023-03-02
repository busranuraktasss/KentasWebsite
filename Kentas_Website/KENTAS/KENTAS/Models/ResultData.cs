using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KENTAS.Models
{
    public class ResultData
    {
        public int PARKINGDEBTID { get; set; }
        public string STARTDATE { get; set; }
        public string OUTDATE { get; set; }
        public string BORC { get; set; }
        public string LOCNAME { get; set; }
        public string REALNAME { get; set; }

    }
}