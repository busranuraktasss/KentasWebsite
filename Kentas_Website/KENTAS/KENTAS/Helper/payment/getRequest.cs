using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KENTAS.Helper.payment
{
    public class getRequest
    {
        public string Amount { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public int Currency { get; set; }
        public string Cvc { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string ExpiryDate { get; set; }
        public int BrandName { get; set; }
        public string VerifyEnrollmentRequestId => Guid.NewGuid().ToString("N");
    }
}