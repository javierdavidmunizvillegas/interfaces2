using Azure.Messaging.ServiceBus;
using ISAC015.api.Infraestructure.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC015.api.Infraestructure.Services
{
    public interface IManejadorRequest<T>
    {
        Task EnviarMensajeAsync(string sesionId, string cadenaConexion, string nombreQueueRequest, T body);
    }
    public class ManejadorRequest<T> : IManejadorRequest<T>
    {
        private static RegistroLog Logger = new RegistroLog();
        public async Task EnviarMensajeAsync(string sesionId, string cadenaConexion, string nombreQueueRequest, T body)
        {
            await using (ServiceBusClient client = new ServiceBusClient(cadenaConexion))
            {
                // create a sender for the queue 
                ServiceBusSender sender = client.CreateSender(nombreQueueRequest);

                // create a message that we can send
                ServiceBusMessage message = new ServiceBusMessage(JsonConvert.SerializeObject(body));
                message.SessionId = sesionId;
                message.ContentType = "application/json";

                var bodyText = message.Body;
                
                // send the message
                await sender.SendMessageAsync(message);

                await sender.CloseAsync();

            }
        }
    }
}
