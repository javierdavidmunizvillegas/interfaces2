using System;
using System.Collections.Generic;
using System.Text;

namespace CONTICREDENVIO.Modelos
{
    public class Parametro
    {
        public int RequestType { get; set; }

        public string EffectiveDate { get; set; } //DateTime

        public string CustAccount { get; set; }

        public int Quantity { get; set; }

        public decimal ManualPrice { get; set; }

        public string AttString { get; set; }
        public string Registrationid { get; set; }
    }
}
