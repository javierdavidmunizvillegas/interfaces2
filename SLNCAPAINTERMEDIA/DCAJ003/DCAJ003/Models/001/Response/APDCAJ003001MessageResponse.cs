using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ003.Models._001.Response
{
    public class APDCAJ003001MessageResponse
    {
         
        public string SessionId { get; set; }         
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }         
        public APECTypeOfDocument DocumentType { get; set; }         
        public string Invoice { get; set; }       
        public string Message { get; set; }
        
    }
    public enum APECTypeOfDocument { Invoice=18, CreditNote =4}
}
