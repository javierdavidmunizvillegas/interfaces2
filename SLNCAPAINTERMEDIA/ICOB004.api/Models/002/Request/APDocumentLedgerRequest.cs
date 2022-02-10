using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB004.api.Models._002.Request
{
    public class APDocumentLedgerRequest
    {
        [Required]
        public string APIdentificationList { get; set; }//Identificador de lista
        [Required]
        public string CustAccount { get; set; }//Código del cliente
        [Required]
        public string TransDate { get; set; }//Fecha de diario datetime
        [Required]
        public Decimal AmountDebit { get; set; }//Monto débito
        [Required]
        public Decimal AmountCredit { get; set; }//Monto crédito
        [Required]
        public string PostingProfile { get; set; }//Perfil contable
       
        public string DocumentNegoc { get; set; }//Documento de negociación
       




    }
}
