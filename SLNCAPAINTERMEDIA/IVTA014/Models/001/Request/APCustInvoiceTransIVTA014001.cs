using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA014.Models._001.Request
{
    public class APCustInvoiceTransIVTA014001
    {
        public String APCodeCapacity { get; set; } // Capacidad de articulo
        public String APCodeGroup { get; set; } // Grupo de articulo
        public String APCodeLines { get; set; } // Linea de articulo
        public String APCodeSubGroup { get; set; } // Subgrupo de articulo
        public Decimal APPO { get; set; } //PO
        public Decimal APPVP { get; set; } // PVP
        public string InvoiceDateReturn { get; set; } // Fecha de emisión de factura relacionada NC
        public String InvoiceIdReturn { get; set; } // Número de factura ralacionada a NC
        public string itemId { get; set; } //Código de Articulo
        public Decimal LineAmount { get; set; } // Monto total de linea sin IVA incluido descuentos
        public Decimal LineMargen { get; set; } // Margen de linea de factura
        public String PackingGroupId { get; set; } // Marca de articulo / Conjunto de Embalaje
        public String Payment { get; set; } //Condicion de pago
        public String PayMode { get; set; } // Forma de Pago
        public Decimal Qty { get; set; } // Cantidad
        public String WorkSalesResponsibleCode { get; set; } // Codigo de Vendedor
        public String WorkSalesResponsibleName { get; set; } // Nombre de Vendedor
        public int NumLine { get; set; } // Nombre de Vendedor


    }
}
