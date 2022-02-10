using IPRO007.api.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO007.api.Models._001.Response
{
    public class APIPRO007001Error
    {

        public APForecastSales APForecastSales { get; set; } //codigo de articulo
        public List<string> ErrorList { get; set; } //cantidad de ventas
    }
}
