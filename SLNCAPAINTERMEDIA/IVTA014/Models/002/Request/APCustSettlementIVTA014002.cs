using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA014.Models._002.Request
{
    class APCustSettlementIVTA014002
    {
        public Decimal AmountCuota { get; set; } // Monto Cuota
        public Decimal AmountRecaudo { get; set; } // Valor recaudo
        public string BusinessUnit { get; set; } // Unidad de negocio
        public string CustAccount { get; set; } // Código de cliente
        public string DateRecaudo { get; set; } // Fecha de recaudo datetime
        public string DueDate { get; set; } //Fecha de vencimiento datetime
        public string InvoiceDate { get; set; } // Fecha de la factura
        public string InvoiceId { get; set; } //Factura
        public String MessageError { get; set; } // Error
        public Boolean StatusRegister { get; set; } // Ok éxito / false error
        public string Voucher { get; set; } // ID de asiento
        public string WorkSalesResponsibleCode { get; set; } //Código de Vendedor
        public string WorkSalesResponsibleName { get; set; } //Nombre del vendedor
    }
}
