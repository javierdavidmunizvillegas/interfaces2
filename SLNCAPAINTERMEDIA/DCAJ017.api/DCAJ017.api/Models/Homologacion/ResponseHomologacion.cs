/*
 Objetivo: Almacena variables de ResponseHomologacion
 Archivo: ResponseHomologacion.cs
 Versión: 1.0
 Creación: 07/03/2022
 Autor: Solange Moncada
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Models.ResponseHomologacion
{
    public class ResponseHomologacion
    {
        public int StatusCode { get; set; }
        public string Response { get; set; }
        public string DescripcionId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
