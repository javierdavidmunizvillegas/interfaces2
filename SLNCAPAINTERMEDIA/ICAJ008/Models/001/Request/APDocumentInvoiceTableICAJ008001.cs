using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ008.api.Models._001.Request
{
    public class APDocumentInvoiceTableICAJ008001
    {
        [Required]
        public string SalesId { get; set; }//Número de la Orden de Venta
        [Required]
        public string CustAccount { get; set; }//Cuenta del cliente
        [Required]
        public string SalesOrigin { get; set; }//Origen de la venta
        public string InvoiceId { get; set; }//Numero de factura de contingencia
        [Required]
        public string InvoiceDate { get; set; }//Fecha de factura de contingencia, Date
        [Required]
        public string NumberSecuence { get; set; }//Código de Secuencia
        [Required]
        public string DocumentDate { get; set; }//Fecha del documento, Date
        [Required]
        public string PostingProfile { get; set; }//Perfil de contabilizacion
        [Required]
        public List<APDocumentInvoiceLinesICAJ008001> DocumentInvoiceLinesList { get; set; }//Listado de líneas de documentos


    }
}
