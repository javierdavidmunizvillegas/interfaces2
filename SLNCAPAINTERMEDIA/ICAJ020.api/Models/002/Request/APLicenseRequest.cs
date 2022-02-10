using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ020.api.Models._002.Request
{
    public class APLicenseRequest
    {
        public string InvoiceMoto { get; set; } //Número de factura de venta de la moto
        public string Cpn { get; set; } //CPN
        public string CustAccount { get; set; } //Código del cliente
        public decimal Amount { get; set; } //monto
        public string Reason { get; set; } //motivo
        public string BusinessUnit { get; set; } //Unidad de negocio
        public string Voucher { get; set; } //Asiento del cobro



    }
}
