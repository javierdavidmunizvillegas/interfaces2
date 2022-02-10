using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APEntityReceivableTable
    {
        public int TransType { get; set; }//Tipo de transacción de cobro
        public string TransactionType { get; set; }//Tipo de transacción que identifica SIAC o unidad de negocio del cobro
        public string CustAccount { get; set; }//Código de cliente
        public string SalesId { get; set; }//Número de orden de venta
        public string VoucherReceivable { get; set; }//Asiento de diario de pago
        public string DocumentSIAC { get; set; }//Número de cobro SIAC
        public string PaymMode { get; set; }//Forma de pago
        public string Date { get; set; }//Fecha
        public decimal AmountReceivable { get; set; }//Valor del cobro
        public List<APEntityReceivableLine> APEntityReceivableLineList { get; set; }//Listado de  cobros




    }
}
