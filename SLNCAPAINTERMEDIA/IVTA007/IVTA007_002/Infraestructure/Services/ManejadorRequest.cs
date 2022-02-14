using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IVTA007.Services
{
    interface ManejadorRequest
    {

        public interface IManejadorRequest<T>
        {
            Task EnviarMensajeAsync(string sesionId, string cadenaConexion, string nombreQueueRequest, T body);
        }
        public class ManejadorRequest<T> : IManejadorRequest<T>
        {

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
                    //   message.TimeToLive = TimeSpan.FromMinutes(2);

                    // send the message
                    await sender.SendMessageAsync(message);

                    await sender.CloseAsync();

                }

            }
        }
    }
}
