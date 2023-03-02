using System.Collections.Generic;

namespace KENTAS.Helper.depts
{
    public class GetDebtsResponseClientResult
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public int MessageType { get; set; }
        public string Message { get; set; }
        public string InternalMessage { get; set; }
        public Data Data { get; set; }
        public List<Validation> Validations { get; set; }
    }
    public class Data
    {
        public string Plaka { get; set; }
        public int Borc { get; set; }
    }

    public class Validation
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}