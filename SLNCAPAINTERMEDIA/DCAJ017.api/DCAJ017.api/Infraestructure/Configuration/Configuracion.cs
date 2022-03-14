/*
 Objetivo: Contiene la llamada a las rutas del appsettings.json
 Archivo: Configuracion.cs
 Versión: 1.0
 Creación: 08/03/2022
 Autor: Solange Moncada
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DCAJ017.api.Infraestructure.Configuration
{
    public class Configuracion
    {
        /*
          Descripción: En este método se utiliza para obtener la ruta o valor que se desea buscar en el archivo appsettings.json
          Parámetros de entrada: No aplica
          Parámetros de salida: No aplica
          Último cambio: 08/03/2022
          Autor de último cambio: Solange Moncada
        */
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}
