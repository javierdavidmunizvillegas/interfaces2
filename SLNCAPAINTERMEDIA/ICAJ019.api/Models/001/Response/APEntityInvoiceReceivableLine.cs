using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APEntityInvoiceReceivableLine
    {
        public DateTime DateLiquidate { get; set; }//Fecha de liquidación
        public Decimal Amount { get; set; }//Monto de liquidación
        public string VoucherLiquidate { get; set; }//Asiento del diario de cobro o nota de crédito aplicada a la factura
        public string RecibeSIAC { get; set; }//RecibeSIAC
        public string MotiveCreditNote { get; set; }//Motivo de la nota de crédito en caso que se liquide con NC
        public string CodeMotiveDevolution { get; set; }//Motivo de devolución en caso de una orden de devolución
        public string TransactionType { get; set; }//Tipo de transacción que identifica SIAC o unidad de negocio del cobro
        public string PaymMode { get; set; }//Forma de pago
        public string LastSettleVoucher { get; set; }//Asiento de compesanción
        public string CreditNote { get; set; }//Número de nota de crédito en caso que se liquide con NC
        public Decimal AmountSettle { get; set; } //Saldo de la factura por liquidar
//        public string TypeOfDocument { get; set; } //Tipo de documento



    }
}
