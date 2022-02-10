using Azure.Messaging.ServiceBus;
using CONTICREDENVIO.Modelos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace CONTICREDENVIO.Servicios
{
    public interface IContColaServicio
    {
        Task EnviarMensajeColaProductos( Parametro Param,  List<List<Producto>> lsProd);
    }
    public class ContColaServicio : IContColaServicio
    {
        private readonly IConfigurationRoot _config;
        private IContLOG _contLog;
        private string ConeccionCola = null;
        private string Cola = null;
        private string Empresa = null;
        private string Ambiente = null;
        const int NanoAMiliS = 10000;
        public ContColaServicio(IConfigurationRoot config, IContLOG contLOG)
        {
            _config = config;
            ConeccionCola = _config.GetSection("ConectionStringRequest001").Value;
            Cola = _config.GetSection("QueueRequest001").Value;
            Empresa = _config.GetSection("Empresa").Value;
            Ambiente = _config.GetSection("Ambiente").Value;
            _contLog = contLOG;


        }

        public async Task EnviarMensajeColaProductos(Parametro Param, List<List<Producto>> lsProd)
        {
            long start = 0, end = 0;
            double diff = 0;
            List<Task> tasks = new List<Task>();
            foreach (var det in lsProd)
            {
                tasks.Add(Task.Run(async () =>
                {
                    await EnviarMensajeAsync(det, Param, ConeccionCola, Cola);
                }));
            }
            start = Stopwatch.GetTimestamp();
            await _contLog.FileLogger("APIVTA018002", "Inicio envio, desde " + DateTime.Now.ToString());
            
            await Task.WhenAll(tasks);
            end = Stopwatch.GetTimestamp();
            diff = start > 0 ? (end - start) / NanoAMiliS : 0;
            await _contLog.FileLogger("APIVTA018002", "Termina envio, tiempo transcurridos  "+ diff.ToString());
            
            await _contLog.FileLogger("APIVTA018002", "Fin envio, hasta " + DateTime.Now.ToString());

        }
        private async Task EnviarMensajeAsync(
            List<Producto> productos
            , Parametro param
            , string cadenaConexion
            , string nombreQueueRequest
            )
        {
            string sesionid = string.Empty;

            VTA018002Request Request = null;
            if (productos != null && param != null)
            {
                foreach (var elem in productos)
                {
                    Request = new VTA018002Request();
                    Request.ItemId = elem.Itemid;
                    Request.DataAreaId = elem.DataAreaId;
                    Request.Enviroment = Ambiente;
                    sesionid = Guid.NewGuid().ToString();
                    Request.SessionId = sesionid;
                    Request.RequestType = param.RequestType;
                    Request.EffectiveDate = param.EffectiveDate;
                    Request.CustAccount = param.CustAccount;
                    Request.Quantity = param.Quantity;
                    Request.ManualPrice = param.ManualPrice;
                    Request.AttString = param.AttString;
                    if (elem.Serie != null &&  elem.Serie != "" )//string.IsNullOrEmpty(elem.Serie) ) // && !(string.IsNullOrEmpty(elem.Serie) || string.IsNullOrWhiteSpace(elem.Serie)) )
                    Request.AttString = Request.AttString + ","+elem.Serie;
                    if (elem.EstadoInv != null && elem.EstadoInv != "") // && (string.IsNullOrEmpty(elem.EstadoInv) || string.IsNullOrWhiteSpace(elem.EstadoInv)))
                        Request.AttString = Request.AttString + "," + elem.EstadoInv;
                    Request.Registrationid = elem.RegisterID;

                    await using (ServiceBusClient client = new ServiceBusClient(cadenaConexion))
                    {
                        // create a sender for the queue 
                        ServiceBusSender sender = client.CreateSender(nombreQueueRequest);

                        // create a message that we can send
                        ServiceBusMessage message = new ServiceBusMessage(JsonConvert.SerializeObject(Request));
                        message.SessionId = sesionid;
                        message.ContentType = "application/json";
                        //   message.TimeToLive = TimeSpan.FromMinutes(2);

                        // send the message
                        await sender.SendMessageAsync(message);

                        await sender.CloseAsync();

                    }
                    await Task.Delay(100);
                }
            }
            

        }
    }
}
