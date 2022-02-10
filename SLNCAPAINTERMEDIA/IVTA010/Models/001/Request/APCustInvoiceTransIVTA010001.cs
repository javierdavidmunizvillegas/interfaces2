using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA010.Models._001.Request
{
    public class APCustInvoiceTransIVTA010001
    {
        public string ItemId { get; set; }//Código del Artículo
        public decimal Qty { get; set; }//Cantidad (en caso de devolución el valor debe ir en negativo)
        public string ItemName { get; set; }//Descripción del artículo
        public decimal LineAmount { get; set; }//Monto de Línea (sin IVA) (en caso de devolución el valor debe ir en negativo)
        public string InvoiceId { get; set; }//Numero de Factura


    }
}
