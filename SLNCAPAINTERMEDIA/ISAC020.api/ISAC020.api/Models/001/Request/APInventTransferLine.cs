using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC020.Models.Request
{
    public class APInventTransferLine
    {
        
        public string ItemId { get; set; }

        
        public decimal Qty { get; set; }

        
        public DateTime ShipDate { get; set; }

        
        public string style { get; set; }

        
        public string statusInventory { get; set; }
    }
}
