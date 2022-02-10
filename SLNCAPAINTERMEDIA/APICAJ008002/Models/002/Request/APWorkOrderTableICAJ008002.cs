using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._002.Request
{
    class APWorkOrderTableICAJ008002
    {
        public string Sucursal { get; set; }//Sucursal
        public string TypeService { get; set; }//Tipo de atención
        public string LocationService { get; set; }//Lugar de atención
        public string WorkshopCode { get; set; }//Código de taller
        public string Origin { get; set; }//Origen
        public string TypeCustomer { get; set; }//Tipo de cliente
        public string TypeIdentification { get; set; }//Tipo de Identificación
        public string ID { get; set; }//Cedula
        public string CustomerName { get; set; }//Nombre del cliente
        public string FirstName { get; set; }//Primer Nombre
        public string MiddleName { get; set; }//Segundo Nombre
        public string FirstLastName { get; set; }//Primer Apellido
        public string SecondLastName { get; set; }//Segundo Apellido
        public string Country { get; set; }//País
        public string Province { get; set; }//Provincia
        public string City { get; set; }//Ciudad
        public string Street { get; set; }//Calle
        public string StreetNumber { get; set; }//Número de calle
        public string Phone { get; set; }//Teléfono
        public string Email { get; set; }//Email
        public string Warehouse { get; set; }//Almacén
        public string Invoice { get; set; }//Factura
        public string InvoiceSRI { get; set; }//FacturaSRI
        public string InvoiceDate { get; set; }//Fecha de Factura, Date
        public List<APWorkOrderLinesICAJ008002> DocumentInvoiceLinesList { get; set; }//Listado de líneas de documentos







    }
}
