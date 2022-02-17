using ApiModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiFlujoCaja
{
    public class clsICRE009:clsSIAC
    {
        public ICRE009Response goConsultaFacturaMoto(string numFactura)
        {
            ICRE009Response res = new ICRE009Response();
            string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlICRE009"];
            try
            {
                #region CAPA INTERMEDIA
                RestClient client = new RestClient(Baseurl);
                var request = new RestRequest(Baseurl, Method.POST, DataFormat.Json);
                var json = new
                {
                    DataAreaId = "000001",
                    apGroupId = "",
                    businnesUnit = "",
                    invoiceId = numFactura
                };
                request.AddParameter("application/json", JsonConvert.SerializeObject(json), ParameterType.RequestBody);
                var response = client.Post(request);
                var content = response.Content;
                res = JsonConvert.DeserializeObject<ICRE009Response>(content);
                #endregion
            }
            catch (Exception e)
            {
            }

            return res;
        }
    }
}