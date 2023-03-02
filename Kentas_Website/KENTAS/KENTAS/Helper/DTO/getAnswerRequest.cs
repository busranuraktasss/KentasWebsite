using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KENTAS.Helper.DTO
{
    public class getAnswerRequest
    {

        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Oneri { get; set; }
        public string[] answerList {get; set;}
        public int[] questionList {get; set;}   
        public int surveyId { get; set; }   
        public int locationId { get; set; }   
    }

}
