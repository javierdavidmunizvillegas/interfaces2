using IVTA010.Infraestructura.Configuracion;
using IVTA010.Models._001;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IVTA010.Infraestructura.Servicios
{
    public interface IConsumoWebService2
    {
        RespuestaWS PostData(string url, string data);
    }
    public interface IConsumoWebService<T>

    {
        T PostWebService(string uri, string method, string jsonData);

        Task<T> GetWebService(string uri, string method, string urlRequest);
        Task<T> GetHomologacion(string uri, string method, string urlRequest);
    }
    public class ConsumoWebService<T> : IConsumoWebService<T>
    {
        private RestClient client;
        public T PostWebService(string uri, string method, string jsonData)
        {
            client = new RestClient(uri);

            var request = new RestRequest(method, Method.POST, DataFormat.Json);

            request.AddParameter("application/json", jsonData, ParameterType.RequestBody);
            var response = client.Post(request);

            T resultadoApi = default(T);

            if (response == null)
                throw new Exception("Respuesta Nulo");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                resultadoApi = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                if (!(string.IsNullOrEmpty(response.ErrorMessage) || string.IsNullOrWhiteSpace(response.ErrorMessage)))
                    throw new Exception(typeof(ConsumoWebService<T>).ToString() + response.ErrorMessage);

            }

            client = null;
            request = null;
            return resultadoApi;    
        }
        public async Task<T> GetWebService(string uri, string method, string urlRequest)
        {
            T resultadoApi = default(T);
            client = new RestClient(uri);


            string finalPath = $"{method}/{urlRequest}";
            var request = new RestRequest(finalPath, Method.GET);
            ;
            var response = client.Execute(request);
            if (response == null)
                throw new Exception("Respuesta Nulo");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                resultadoApi = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                if (!(string.IsNullOrEmpty(response.ErrorMessage) || string.IsNullOrWhiteSpace(response.ErrorMessage)))
                    throw new Exception(typeof(ConsumoWebService<T>).ToString() + response.ErrorMessage);

            }
            client = null;
            request = null;
            return resultadoApi;
        }

        public async Task<T> GetHomologacion(string uri, string method, string urlRequest)
        {
            T resultadoApi = default(T);
            client = new RestClient(uri);


            string finalPath = $"{method}/{urlRequest}";
            var request = new RestRequest(finalPath, Method.GET);
            ;
            var response = client.Execute(request);
            if (response == null)
                throw new Exception("Respuesta Nulo");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                resultadoApi = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                if (!(string.IsNullOrEmpty(response.ErrorMessage) || string.IsNullOrWhiteSpace(response.ErrorMessage)))
                    throw new Exception(typeof(ConsumoWebService<T>).ToString() + response.ErrorMessage);

            }
            client = null;
            request = null;
            return resultadoApi;
        }

    }

    public class ConsumoWebService2 : IConsumoWebService2
    {
        private static RegistroLog Logger = new RegistroLog();
        public RespuestaWS PostData(string url, string data)
        {
           
            var resultado = string.Empty;
            
           // byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(data);
            byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(data);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "text/xml";

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                resultado = reader.ReadToEnd();
            }
            resultado = WebUtility.HtmlDecode(resultado);
            response.Close();
            if(response != null) response.Dispose();

            //Logger.FileLogger("IVTA010001","Estoy aqui 1");
            
            if (resultado.IndexOf("?>") - 1 > 0)
            {
              //  Logger.FileLogger("IVTA010001", "Estoy aqui 2");
                int posCaracterIntermedio = resultado.IndexOf("?>") + 2;
                int longitudResultado = resultado.Length;
                resultado = resultado.Substring(posCaracterIntermedio, longitudResultado - posCaracterIntermedio);
            }
            //Logger.FileLogger("IVTA010001", "Estoy aqui 3");
            //Logger.FileLogger("IVTA010001", resultado);
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(resultado);
            RespuestaWS response2 = new RespuestaWS();
            string item = string.Empty;

            string numDocumentosSiGenerados = string.Empty;

            if (doc.GetElementsByTagName("numDocSiGenerados") != null && doc.GetElementsByTagName("numDocSiGenerados")[0] != null)
            {
                numDocumentosSiGenerados = doc.GetElementsByTagName("numDocSiGenerados")[0].InnerText;
            }
            
            XmlNodeList Sigenerados = doc.GetElementsByTagName("DetDocSiGenerados");
            XmlNodeList xLista = null;
            if (Sigenerados != null && Sigenerados[0] != null)
            {
                xLista = ((XmlElement)Sigenerados[0]).GetElementsByTagName("item");
            }

            if (xLista != null && xLista.Count > 0)
            {
                response2.DetalleDocumentosSiGenerados = new List<string>();
                foreach (XmlNode xn in xLista)
                {
                    item = xn.InnerText;

                    response2.DetalleDocumentosSiGenerados.Add(item);
                }
            }
            string numDocumentosNoGenerados = string.Empty;
            if (doc.GetElementsByTagName("NoProcesadas") != null && doc.GetElementsByTagName("NoProcesadas")[0] != null)
            {
                numDocumentosNoGenerados = doc.GetElementsByTagName("NoProcesadas")[0].InnerText;

            }

            //XmlNodeList xnList1 = doc.GetElementsByTagName("DetDocNoGenerados/item");
            XmlNodeList Nogenerados = doc.GetElementsByTagName("DetDocNoGenerados");

            XmlNodeList xLista1 = null;
            if (Nogenerados != null && Nogenerados[0] != null)
            { 
                xLista1 = ((XmlElement)Nogenerados[0]).GetElementsByTagName("item");
            }
             
            if (xLista1 != null && xLista1.Count > 0)
            {
                response2.DetalleDocumentosNoGenerados = new List<string>();
                foreach (XmlNode xn in xLista1)
                {
                    item = xn.InnerText;
                    
                    response2.DetalleDocumentosNoGenerados.Add(item); 
                }
            }
            response2.NoProcesadas = numDocumentosNoGenerados;
            response2.numDocumentosSiGenerados = numDocumentosSiGenerados;

            return response2;



        }
    }
}
