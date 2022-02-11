using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Interface.Api.Infraestructura.Configuracion
{
    public class Configuracion
    {
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}
