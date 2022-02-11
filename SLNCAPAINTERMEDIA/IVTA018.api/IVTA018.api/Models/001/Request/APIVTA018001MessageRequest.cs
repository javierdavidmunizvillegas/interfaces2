using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA018.api.Models._001.Request
{
    public class APIVTA018001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }

        public string Enviroment { get; set; }

        public string SessionId { get; set; }

        public int RequestType { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string CustAccount { get; set; }

        public int Quantity { get; set; }

        public string ItemId { get; set; }

        public decimal ManualPrice { get; set; }

        public string AttString { get; set; }

    }
}
