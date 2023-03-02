using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KENTAS.Areas.Dashboard.Data
{
    public class getResultsRequest
    {
        public int Id { get; set; }
        public Nullable<int> AnketId { get; set; }
        public string AnketAdi { get; set; }
        public string MekanAdi { get; set;}
        public int SoruId { get; set; }
        public string Soru { get; set; }
        public int AnketCevaplariId { get; set; }
        public string AdiSoyadi { get; set; }
        public string Eposta { get; set; }
        public string Tel { get; set; }
        public string Oneri { get; set; }
        public string Cevap { get; set; }
        public int CevapTipi { get; set; }
        public int SecilmeSayisi { get; set; }
        public int ToplamSecilmeSayisi { get; set; }
        public string Icerik { get; set; }
    }
}
