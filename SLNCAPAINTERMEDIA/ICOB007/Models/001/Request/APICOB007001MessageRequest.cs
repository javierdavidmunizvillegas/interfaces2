using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB007.Models._001.Request
{
    public class APICOB007001MessageRequest
    {
        public string DataAreaId { get; set; }//Id de la compañía
        public string Enviroment { get; set; }//Entorno
        public string SessionId { get; set; }//Id de sesión
        public string Sucursal { get; set; }//Sucursal
        public string TypeAttention { get; set; }//Tipo de atención
        public string PlaceAttention { get; set; }//Lugar de atención
        public string CodeWorkshop { get; set; }//Código taller
        public string ClientType { get; set; }//Tipo de cliente
        public string IdentificationType { get; set; }//Tipo de identificación
        public string IdentificationNum { get; set; }//Número de identificación
        public string NameClient { get; set; }//Nombre de cliente
        public string FirstName { get; set; }//Primer nombre
        public string SecondName { get; set; }//Segundo nombre
        public string LastName { get; set; }//Primer Apellido 
        public string LastSecondName { get; set; }//Segundo apellido
        public string Country { get; set; }//País
        public string State { get; set; }//Provincia
        public string City { get; set; }//Ciudad
        public string Street { get; set; }//Calle
        public string NumberStreet { get; set; }//Número de calle NmberStreet
        public string Phone { get; set; }//Teléfono 
        public string Email { get; set; }//Email 
        public string Warehouse { get; set; }//Almacén 
        public string Invoice { get; set; }//Factura 
        public string InvoiceSRI { get; set; }//Factura SRI
        public string InvoiceDate { get; set; }//Fecha de factura
        public string Dacion { get; set; }//Código de dación
        public string ItemId { get; set; }//Equipo
        public string Serie { get; set; }//Serie
        public string ModelItem { get; set; }//Modelo equipo
        public string DescriptionItem { get; set; }//Descripción equipo
        public string Mark { get; set; }//Marca
        public string Line { get; set; }//Línea
        public string Group { get; set; }//Grupo
        public string SubGroup { get; set; }//SubGrupo
        public string Capacity { get; set; }//Capacidad 
        public string EntAssetObjectID { get; set; }//
        public string Origen { get; set; }//
        public string RecId { get; set; }//
        public Int64 RecIdLine { get; set; }// RecId  línea de la dación
        public string CustAccount { get; set; }//Cuenta de cliente



    }
}
