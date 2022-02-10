using ICAJ012.api.Models._001.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ012.api.Models._001.Response
{
    public class APICAJ012001MessageResponse
    {
        [Required]
        public string SessionId { get; set; }
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }
        public string APReasonIngressEgressId { get; set; }
        [Required]
        public List<APLedgerJournalTransICAJ012_Response> APLedgerJournalTransList { get; set; }
      //  public string TimeStartEnd { get; set; }

    }
}
