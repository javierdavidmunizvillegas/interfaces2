using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC020.Models.Request
{
    public class APInventTransferTable
    {
        
        public string InventLocationIdFrom { get; set; }

        
        public DateTime ShipDate { get; set; }

        
        public APOrderTypeISAC020001 APOrderType { get; set; }

        public string TransferId { get; set; }

        
        public List<APInventTransferLine> APInventTransferLineList { get; set; }

    }
    public enum APOrderTypeISAC020001 { Creacion = 0, Devolucion = 1 }
}
