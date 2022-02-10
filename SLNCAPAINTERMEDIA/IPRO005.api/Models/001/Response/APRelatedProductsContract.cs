using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._001.Response
{
    public class APRelatedProductsContract
    {
        public string ProductCode  { get; set; }

        public string RelationshipType { get; set; }
        
        public string RelatedProductCode { get; set; }
       
        public string CreationDate { get; set; } //DateTime
    }
}
