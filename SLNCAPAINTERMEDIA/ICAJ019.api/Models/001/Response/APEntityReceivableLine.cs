using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APEntityReceivableLine
    {
        public string Invoice { get; set; }//Factura o documento liquidado
        public String VoucherLiquidate { get; set; }//Asiento de la factura o documento liquidado
       // public string LastSettleVoucherransType { get; set; }//Asiento de compensación
        public string LastSettleVoucher { get; set; }//Asiento de compensación
        public String VoucherReverse { get; set; }//Asiento de transacción de reverso del cobro
        public DateTime DateLiquidate { get; set; }//Fecha de liquidación
        public decimal AmountLiquidate { get; set; }//Monto de liquidación
        public String MotiveReverse { get; set; }//Motivo del reverso
        public decimal AmountSettle { get; set; }//Saldo pendiente del cobro

    }
}
