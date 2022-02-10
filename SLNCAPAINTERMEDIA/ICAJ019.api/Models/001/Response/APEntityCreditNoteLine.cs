using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APEntityCreditNoteLine
    {
        public string CreditNote { get; set; }//Número de nota de crédito
        public DateTime DateLiquidate { get; set; }//Fecha de liquidación
        public decimal Amount { get; set; }//Monto de liquidación
        public String Document { get; set; }//Documento, factura o diario de liquidación
        public String VoucherLiquidate { get; set; }//Asiento del documento o factura que se está liquidando
        public String LastSettleVoucher { get; set; }//Asiento de compensación de la NC
       // public String DocumeRecibeSIACnt { get; set; }//Recibo de SIAC
        public String RecibeSIAC { get; set; }//Recibo de SIAC
        public decimal AmountInvoice { get; set; }//Saldo de la factura o documento liquidado
        public decimal AmountSettle { get; set; }//Saldo de la NC por liquidar
       // public String TypeOfDocument { get; set; }//Tipo de documento


    }
}
