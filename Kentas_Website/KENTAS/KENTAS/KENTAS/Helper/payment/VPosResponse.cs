using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KENTAS.Helper.payment
{
    public class VPosResponse
    {

        public string MerchantId { get; set; }//000000001713742&
        public string Pan { get; set; }//5351771411675143&
        public string Expiry { get; set; }//2607&
        public string PurchAmount { get; set; }//100&
        public string PurchCurrency { get; set; }//949&
        public string VerifyEnrollmentRequestId { get; set; }//1e4de9313e554f6eb09e30eac4db7e97&
        public string Xid { get; set; }//5avnyu7bjgli9n6ht8dg&
        public string SessionInfo { get; set; }//EsParkApp&
        public string Status { get; set; }//Y&
        public string Cavv { get; set; }//ABIBBJYAAD1UAAABAAAAAAAAAAA%3d&
        public string Eci { get; set; }//02&
        public string ExpSign { get; set; }//&
        public string InstallmentCount { get; set; }//&
        public string SubMerchantNo { get; set; }//&
        public string SubMerchantName { get; set; }//&
        public string SubMerchantNumber { get; set; }//&
        public string ErrorCode { get; set; }//&
        public string ErrorMessage { get; set; }//
    }
}