using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._006.Response
{
    public class APMovementSearchContract
    {
       
        public string ProductCode { get; set; }
       
        public decimal Quantity { get; set; }
       
        public string Origin { get; set; }
        

        public string Destination { get; set; }
       

        public string ShipmentDate { get; set; } // DateTime


        public string SalesNumber { get; set; }
       

        public string Company { get; set; }
    }
}
