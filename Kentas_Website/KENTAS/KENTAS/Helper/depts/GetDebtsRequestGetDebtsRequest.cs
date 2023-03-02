
using Newtonsoft.Json;

namespace KENTAS.Helper.depts
{
    public class GetDebtsRequestGetDebtsRequest
    {
        public string Plaka { get; set; }

        [JsonProperty("g-recaptcha-response")]
        public object g_recaptcha_response { get; set; }
    }
}