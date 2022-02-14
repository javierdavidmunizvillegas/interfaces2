using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISAC020.Models.Request
{
    class APInventTransferTableISAC020002
    {
        public string TransferId { get; set; }
        public InventTransferStatus Status { get; set; }
        public APInventTransferLineISAC020002[] APInventTransferLineList { get; set; }

    }
    public enum InventTransferStatus { Created = 0, Shipped = 1, Received=2 }
   
}
