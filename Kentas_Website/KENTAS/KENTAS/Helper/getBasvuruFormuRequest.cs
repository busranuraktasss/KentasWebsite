using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KENTAS.Helper
{
    public class getBasvuruFormuRequest
    {
        public string tcno { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string Adress { get; set; }
        public string plaka { get; set; }
        public string aractip { get; set; }
        public string phone { get; set; }

        public int[] mekanIds { get; set; }

    }
}