
using System;
using System.Collections.Generic;

namespace KENTAS.Areas.Dashboard.Data
{
    public class showFormRequest
    {
        public int Id { get; set; }
        public string tcno { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string Adress { get; set; }
        public string plaka { get; set; }
        public string aractip { get; set; }
        public string phone { get; set; }
        public string ourservice { get; set; }
        public string createdDate { get; set; }
        public int? status { get; set; }
    }
}