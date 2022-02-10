using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ023.api.Models._001.Request
{
    public class APAccountingStatementContract
    {
        [Required]
        public string CashBankId { get; set; }//Código de Caja-Banco
        [Required]
        public string TransDate { get; set; }//Fecha de conciliación , Date
        [Required]
        public string PostingDate { get; set; }//Fecha de registro de la transacción, Date
        [Required]
        public string InventTransId { get; set; }//Asiento

        public string Reference { get; set; }//Referencia
        [Required]
        public string TransTypeSIAC { get; set; }//Tipo de transacción de SIAC
        [Required]
        public decimal Amount { get; set; }//Monto
        [Required]
        public string Sign { get; set; }//(+/-)


    }
}
