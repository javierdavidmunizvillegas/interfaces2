using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB007.Models._002.Request
{
    public class APDacionQualifiedRequest
    {
        public string Dacion { get; set; }//Código de dación
        public string TransDate { get; set; }//Fecha de creación
        public string DocumentRetire { get; set; }//Documento retiro
        public string UserName { get; set; }//Nombre de persona que entrega los artículos
        public string Invoice { get; set; }//factura
        public string InvoiceDate { get; set; }//Fecha de factura
        public string CustAccount { get; set; }//Cuenta de cliente
        public string NameAccount { get; set; }//Nombre de cliente
        public string Status { get; set; }//Estado dación int OJO
        public List<APDacionItemQualifiedRequest> ItemList { get; set; }//Lista de artículos calificados
        



    }
}
