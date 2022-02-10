using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA014.Models._001.Request
{
    public class APCustInvoiceJourIVTA014001
    {
        public List<APCustInvoiceTransIVTA014001> APCustInvoiceTransList { get; set; } // Listado de lineas
        public string InvoiceId { get; set; } //Factura
        public string Voucher { get; set; } // ID de asiento
        public string InvoiceDate { get; set; } // Fecha de la factura
        public string CustAccount { get; set; } // Código de cliente
        public string BusinessUnit { get; set; } // Unidad de negocio
        public Boolean StatusRegister { get; set; } // Ok éxito / false error
        public string MessageError { get; set; } // Error
    }
}
