
using System;
using System.Collections.Generic;

namespace KENTAS.Areas.Dashboard.Data
{
    public class showFormServiceRequest
    {
        public int Id { get; set; }
       
        public int[] ourserviceIds { get; set; }
        public string ourservice { get; set; }
        public int SubsFormId { get; set; }
       
    }
}