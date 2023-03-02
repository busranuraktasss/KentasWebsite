using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KENTAS.Helper.depts
{
    public class GetDeptsApiModel
    {
        public string Plaka { get; set; }

        public int[] DebtsIds { get; set; }

        public string RequestId { get; set; }
    }
}