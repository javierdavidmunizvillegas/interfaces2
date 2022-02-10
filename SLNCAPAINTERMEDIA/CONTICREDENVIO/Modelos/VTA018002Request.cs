using System;
using System.ComponentModel.DataAnnotations;


namespace CONTICREDENVIO.Modelos
{
    public class VTA018002Request
    {
        [Required]
        public string DataAreaId { get; set; }

        public string Enviroment { get; set; }

        public string SessionId { get; set; }

        public int RequestType { get; set; }

        public string EffectiveDate { get; set; } // datetime

        public string CustAccount { get; set; }

        public int Quantity { get; set; }

        public string ItemId { get; set; }

        public decimal ManualPrice { get; set; }

        public string AttString { get; set; }
        public string Registrationid { get; set; }

    }
}

