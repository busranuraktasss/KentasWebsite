using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KENTAS.Helper.DTO
{
    public class surveyQuestionRequest
    {
   
        public int Id { get; set; }
        public string Soru { get; set; }
        public int CevapTipi { get; set; }  

    }

}
