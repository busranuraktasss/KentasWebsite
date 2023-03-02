using System;

namespace KENTAS.Helper.depts
{
    public class GetCustomerRequest
    {
        public string Type { get; set; }
        public string Sahip { get; set; }
        public string Ad { get; set; }
        public long? TcNo { get; set; }
        public string Tel { get; set; }
        public string Mail { get; set; }
        public string Adres { get; set; }
        public string Cvv { get; set; }
        public string Amount { get; set; }
        public string TransactionId { get; set; }
        public string LicensPlate { get; set; }
    }
}