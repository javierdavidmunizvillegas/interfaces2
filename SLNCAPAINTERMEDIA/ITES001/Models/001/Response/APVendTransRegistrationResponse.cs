using System;
using System.Collections.Generic;
using System.Text;

namespace ITES001.Models._001.Response
{
    class APVendTransRegistrationResponse
    {
        public string DataAreaId { get; set; }//Id de la compañía 
        public string BusinessUnit { get; set; }//Unidad de Negocio
        public string Voucher { get; set; }//ID. De cobros (Recaudo)
        public string InvoiceId { get; set; }//Factura de la moto
        public string Motive { get; set; }//Motivo
        public string UserId { get; set; }//Usuario
        public string PaymMode { get; set; }//Forma de pago
        public string PostedDate { get; set; }//Fecha y hora Datetime
        public decimal Amount { get; set; }//Monto
        public Boolean StatusRegister { get; set; }//Ok éxito / false error
        public string MessageError { get; set; }//Error
        public long RecIdVoucher { get; set; }
    }
}
