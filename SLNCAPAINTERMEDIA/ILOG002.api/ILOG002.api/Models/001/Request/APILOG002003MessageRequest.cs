using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ILOG002.api.Models
{
    public class APILOG002003MessageRequest
    {
        
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }       
        public string SessionId { get; set; }        
        public APCurrierShippifyList[] CurrierShippifyList { get; set; }
    }
}
