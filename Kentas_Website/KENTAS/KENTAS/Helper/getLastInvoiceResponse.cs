namespace KENTAS.Helper
{
    public class getLastInvoiceResponse
    {
        public string InvoiceUrl { get; set; }

        public decimal Amount { get; set; }

        public decimal Vat { get; set; }

        public decimal TotalAmount => Amount + Vat;
    }
}