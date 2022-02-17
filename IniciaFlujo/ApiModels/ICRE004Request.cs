using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class ICRE004Request
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string APPaymModeGeneral { get; set; }
        public List<string> CustAccountList { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public List<int> DocumentStatusList { get; set; }
        public bool PaymentRequest { get; set; }
        public List<string> PurchOrderFormNumList { get; set; }
        public List<string> SalesIdList { get; set; }
        public List<string> SalesResponsiblePersonnalNumberList { get; set; }
        public List<int> SalesStatusList { get; set; }
    }
}
