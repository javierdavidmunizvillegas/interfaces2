using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.Models._003.Request
{
    public class APISAC017003MessageRequest
    {
        public string DataAreaId { get; set; }
        public string SessionId { get; set; }
        public string ReturnItemNum { get; set; }
        public string Enviroment { get; set; }
        public SalesStatus Status { get; set; }
    }
    public enum SalesStatus { none = 0, BackOrder = 1, Delivered = 2, Invoiced = 3, Canceled = 4 }
}
