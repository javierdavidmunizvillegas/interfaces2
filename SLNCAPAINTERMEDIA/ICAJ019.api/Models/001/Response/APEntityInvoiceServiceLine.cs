using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APEntityInvoiceServiceLine
    {
        public string Invoice { get; set; }//Número de nota de débito
        public DateTime DateLiquidate { get; set; }//Fecha de liquidación
        public decimal Amount { get; set; }//Monto de liquidación
        public string VoucherLiquidate { get; set; }//Asiento del diario de cobro o nota de dédito aplicada a la factura
        public string RecibeSIAC { get; set; }//Recibo de SIAC
        public string MotiveCreditNote { get; set; }//Motivo de la nota de dédito en caso que se liquide con ND
        public string LastSettleVoucher { get; set; }//Asiento de compensación
        public string TransactionType { get; set; }//Tipo de transacción que identifica SIAC o unidad de negocio del cobro
        public string PaymMode { get; set; }//Forma de pago

        public string CreditNote { get; set; }//Número de nota de crédito en caso que se liquide con NC
        public string AmountSettle { get; set; }//Saldo de la factura por liquidar
      //  public string TypeOfDocument { get; set; }//Tipo de documento


    }
}
