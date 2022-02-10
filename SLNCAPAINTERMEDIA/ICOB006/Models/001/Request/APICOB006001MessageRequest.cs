using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB006.Models._001.Request
{
    class APICOB006001MessageRequest
    {
        public string DataAreaId { get; set; }//Id de la Compañia
        public string Enviroment { get; set; }//Entorno
        public string SessionId { get; set; }//id de la sesion Guid Sesionid
        public string CustAccount { get; set; }//Código de cliente
        public string JournalNumPaym { get; set; }//Número de lote del diario de pago del depósito
        public string VoucherPaym { get; set; }//Asiento del diario de pago del cliente generado en el proceso del depósito
        public string DocumentNum { get; set; }//Campo Documento del diario de pago del cliente del depósito
        public decimal AmountCheque { get; set; }//Valor del cheque
        public string NumCheque { get; set; }//Número de cheque
        public string NumCtaCheque { get; set; }//Número de cuenta del cheque
        public string BankCheque { get; set; }//Banco del cheque
        public string MotiveCheque { get; set; }//Descripción del motivo por el que se está protestando el cheque
        public string DateProtest { get; set; }//Fecha del protesto del cheque
        public List<APInvoiceListRequest> InvoiceList { get; set; }//Listas de facturas
        public APDocumentTaxRequest DocumentTax { get; set; }//Información de documento fiscal
    }
}
