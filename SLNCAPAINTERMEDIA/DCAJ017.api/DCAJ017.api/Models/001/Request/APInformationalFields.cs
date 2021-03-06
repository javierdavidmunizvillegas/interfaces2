/*
 Objetivo: Almacena variables de APInformationalFields
 Archivo: APInformationalFields.cs
 Versión: 1.0
 Creación: 07/03/2022
 Autor: Solange Moncada
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Models._001.Request
{
    public class APInformationalFields
    {
        public string APBoxCode { get; set; }
        public string APIdentificationList { get; set; }
        public string APNumberOT { get; set; }
        public string APStoreId { get; set; }
        public string APTransactionType { get; set; }
        public string APUserPaym { get; set; }
        public string SalesId { get; set; }
    }
}
