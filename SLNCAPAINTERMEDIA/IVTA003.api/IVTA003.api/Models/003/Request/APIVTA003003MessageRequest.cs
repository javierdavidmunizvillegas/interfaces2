using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA003.api.Models._003.Request
{
    public class APIVTA003003MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public string[] ListItemId { get; set; }
        public string BusinessUnit { get; set; }
        public string InventSerialId { get; set; }
        public string Mark { get; set; }
        public string ItemName { get; set; }
        public string InventLocationId { get; set; }
        public string LineCategory { get; set; }
        public string GroupCategory { get; set; }
        public string SubgroupCategory { get; set; }
        public string CapacityCategory { get; set; }

    }
}
