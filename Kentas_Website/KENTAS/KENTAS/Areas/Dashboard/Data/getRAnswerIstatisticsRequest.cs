using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KENTAS.Areas.Dashboard.Data
{
    public class getAnswerIstatisticsRequest
    {
        public int Id { get; set; }
      
        public int SecilmeSayisi { get; set; }
        public int ToplamSecilmeSayisi { get; set; }
        public string Icerik { get; set; }
    }
}
