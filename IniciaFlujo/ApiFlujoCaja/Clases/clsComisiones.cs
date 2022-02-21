using ApiModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiFlujoCaja
{
    public class clsComisiones:clsSIAC
    {
        public ResponseComisiones GeneraComisiones(FlujoRequest Datos, ICAJ008Response RespCAJ008)
        {
            ResponseComisiones res = new ResponseComisiones();
            RestClient client = null;
            string JsonResponse = string.Empty;
            string Baseurl = string.Empty ;
            try
            {
                if (!ConsultaLogs(Datos, "Servicio Comisiones", ref JsonResponse))
                {
                    Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlComisiones"];
                    client = new RestClient(Baseurl);

                }
            }
            catch (Exception Ex) { }
            return res;
        }
    }
}