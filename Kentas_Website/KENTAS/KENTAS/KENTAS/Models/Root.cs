using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KENTAS.Models
{
    public class Root
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public int MessageType { get; set; }
        public string Message { get; set; }
        public object InternalMessage { get; set; }
        public Data Data { get; set; }
        public object Validations { get; set; }

    }
}