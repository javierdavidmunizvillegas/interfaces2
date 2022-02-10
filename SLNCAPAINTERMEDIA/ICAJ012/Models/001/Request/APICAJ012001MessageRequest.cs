using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ012.api.Models._001.Request
{
    public class APICAJ012001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }
       
        public string Enviroment { get; set; }
       
        public string SessionId { get; set; }
        [Required]
        public string APReasonIngressEgressId { get; set; }
        
        public  List<APLedgerJournalTransICAJ012> APLedgerJournalTransList { get; set; }
    }
}
