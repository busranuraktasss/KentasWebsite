namespace KENTAS.Helper
{
    public class getLastInvoiceRequest
    {
        public int JobId { get; set; }

        public bool? IsLicensePlate { get; set; } = false;

        public string LicensePlate { get; set; } = "";

        public bool? IsInvoiceNumber { get; set; } = false;

        public string InvoiceNumber { get; set; } = "";

        public bool? IsRecordId { get; set; } = false;
        public int RecordId { get; set; }
    }
}