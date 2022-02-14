using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISAC020.Models.Request
{
    class APISAC020002MessageRequest
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public APInventTransferTableISAC020002 APInventTransfer { get; set; }
    }
}
