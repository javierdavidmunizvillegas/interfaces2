using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APEntityReceivableDevolutionLine
    {
        public DateTime DateLiquidate { get; set; }// Fecha de liquidación
        public Decimal Amount { get; set; }// Monto de liquidación
        public string VoucherLiquidate { get; set; }// Asiento del diario de cobro o nota de crédito aplicada a la factura
        public string RecibeSIAC { get; set; }// Recibo de SIAC
        public string Invoice { get; set; }// Número de Factura o documento 
        public string LastSettleVoucher { get; set; }//Asiento de compesanción
        public decimal AmountSettle { get; set; }//Saldo de la NC que se está liquidando
        public string TypeOfDocument { get; set; }//Tipo de documento
        
    }
}
