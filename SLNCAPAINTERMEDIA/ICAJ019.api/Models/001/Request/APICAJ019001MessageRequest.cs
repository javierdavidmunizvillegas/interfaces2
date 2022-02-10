using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Request
{
    public class APICAJ019001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }//Id de la compañía
        public string Enviroment { get; set; }//Ambiente
        public string SessionId { get; set; }//Id de sesión
        [Required]
        public List<int> EntidadList { get; set; }//Descripción
        [Required]
        public string CustAccount { get; set; }//cuentas de clientes
        public DateTime DateStart { get; set; }//Fecha Incio
        public DateTime DateEnd { get; set; }//Fecha Incio
        public string InvoiceId { get; set; }//Fecha Incio
        public string Voucher { get; set; }//Asiento de la Factura o Cobro
        public string APRecibeSIAC { get; set; }//Recibo de Cobro de SIAC
        public string SalesId { get; set; }//Número de Orden de venta
        public string ReturnItemNum { get; set; }//Número de Orden de Devolución
        public string APTransactionType { get; set; }//Tipo de transacción
      
        public string EinvoiceReasonRefund { get; set; }//Motivo de Nota Crédito
        
        public string PaymMode { get; set; }//Forma de pago





    }
}
