//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KENTAS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PrePayment
    {
        public int Id { get; set; }
        public string CustomerType { get; set; }
        public string Ownership { get; set; }
        public string NameOrTitle { get; set; }
        public Nullable<long> IdentificationNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Mail { get; set; }
        public string LicensPlate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string VerifyEnrollmentRequestId { get; set; }
    }
}
