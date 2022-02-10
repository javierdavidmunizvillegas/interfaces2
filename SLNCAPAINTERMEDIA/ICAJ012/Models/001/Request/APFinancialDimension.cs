using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ012.api.Models._001.Request
{
    public class APFinancialDimension
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Valor { get; set;  }
    }
}
