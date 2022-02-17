using ApiModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiFlujoCaja
{
    public class clsICRE006:clsSIAC
    {
        public ICRE006Response ConsultaInfoCliente(string Empresa, string CedulaCliente)
        {
            ICRE006Response res = new ICRE006Response();
            string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlICRE006"];

            ICRE006Request ICRE006 = new ICRE006Request()
            {
                DataAreaId = Empresa,
                Enviroment = "SIT",
                SessionId = "",
                VATNum = CedulaCliente
            };

            var jsonstring = JsonConvert.SerializeObject(ICRE006);

            RestClient cliente = new RestClient();
            var request = new RestRequest(Baseurl, Method.POST, DataFormat.Json);
            request.AddParameter("application/json", jsonstring, ParameterType.RequestBody);
            var response = cliente.Post(request);
            var content = response.Content;
            res = JsonConvert.DeserializeObject<ICRE006Response>(content);

            return res;
        }
    }
}