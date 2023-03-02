using Newtonsoft.Json;
using System.Collections.Generic;

namespace KENTAS.Helper
{
    public class Common
    {
        public static string baseUrl = "https://api.espark.elektrosoft.com.tr";
        public static string token = "k7lFizdIpOicanrvRN3xSXbOOLABkMCK";
        //           EsParkApp-Token: k7lFizdIpOicanrvRN3xSXbOOLABkMCN

        public static string pos_Pass = "i3H8MpQb";
        public static string pos_Terminal = "VP343671";
        
        public static string pos_MerchantId = "000000001713742";

        public static string pos_MerchantId_test = "000100000013506";
        public static string pos_Pass_test = "123456";
        public static string pos_Terminal_test = "VP000579";

        public static string pos_WebSiteId = "612825";

        public static string pos_ServisUrl_test = "https://onlineodemetest.vakifbank.com.tr:4443/VposService/v3/Vposreq.aspx";
        public static string pos_ServisUrl_real = "https://onlineodeme.vakifbank.com.tr:4443/VposService/v3/Vposreq.aspx";

        public static string pos_Enrollment_test = "https://3dsecuretest.vakifbank.com.tr:4443/MPIAPI/MPI_Enrollment.aspx";
        public static string pos_Enrollment_real = "https://3dsecure.vakifbank.com.tr:4443/MPIAPI/MPI_Enrollment.aspx";


        public static readonly string captcha_webKey = "6LcaTHoaAAAAAEq2BBVypfkyljZ_cfvCFUo5f0Yx";
        public static readonly string captcha_screet_webKey = "6LcaTHoaAAAAAMcZ-H7PWaqWrUkxJ40FE-3FduRL";
        // 6LfKURIUAAAAAO50vlwWZkyK_G2ywqE52NU7YO0S -> site key old
        public static User_api GetUser(string user)
        {
            User_api userData = JsonConvert.DeserializeObject<User_api>(user);
            return userData;
        }

        public class User_api
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public int AdminId { get; set; }
            public string Authorization { get; set; }   

        }

        public class Advert_api
        {
            public int adId { get; set; }
            public string adHeaderDesc { get; set; }
            public string adDesc { get; set; }
            public string adTab { get; set; }
            public string adImage { get; set; }
            public List<Adverts_Images_api> Resimler { get; set; }
        }

        public class Adverts_Images_api
        {
            public int Id { get; set; }
            public string imgUrl { get; set; }
            public int? bagId { get; set; }
            public bool? isdelete { get; set; }
        }

        public class Activities_api
        {
            public int activitiesId { get; set; }
            public string activitiesHeaderDesc { get; set; }
            public string activitiesDesc { get; set; }
            public string activitiesImage { get; set; }
            public List<Activities_Images_api> Resimler { get; set; }
        }

        public class Activities_Images_api
        {
            public int fId { get; set; }
            public string fimgUrl { get; set; }
            public int? fbagId { get; set; }
            public bool? isdelete { get; set; }

        }

        public class News_api
        {
            public int newsId { get; set; }
            public string newsHeaderDesc { get; set; }
            public string newsDesc { get; set; }
            public string newsImage { get; set; }
            public string newsTabItem { get; set; }
            public List<News_Images_api> Resimler { get; set; }

        }
        public class News_Images_api
        {
            public int nId { get; set; }
            public string nimgUrl { get; set; }
            public int? nbagId { get; set; }
            public bool? isdelete { get; set; }
        }
    }
}