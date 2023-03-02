
using System;
using System.Collections.Generic;

namespace KENTAS.Areas.Dashboard.Data
    {
        public class showSurveyRequest
        {
            public int Id { get; set; }
            public string AnketAdi { get; set; }
            public string MekanAdi { get; set; }
            public DateTime KTarihi { get; set; }
            public bool IletisimBilgiMecburiMi { get; set; }
            public int Count { get; set; }

        internal static object FirstOrDefault(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
    }