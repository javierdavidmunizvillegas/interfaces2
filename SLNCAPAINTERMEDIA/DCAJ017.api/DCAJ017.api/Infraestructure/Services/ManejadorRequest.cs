/*
 Objetivo: Realiza el envío del mensaje de entrada a la cola de request
 Archivo: ManejadorRequest.cs
 Versión: 1.0
 Creación: 07/03/2022
 Autor: Solange Moncada
*/

using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Infraestructure.Services
{
    public interface IManejadorRequest<T>
    {
        Task EnviarMensajeAsync(string sesionId, string cadenaConexion, string nombreQueueRequest, T body);
    }
    public class ManejadorRequest<T> : IManejadorRequest<T>
    {
        /*
          Descripción: Este método realiza el envío del mensaje a la cola de Request.
          Parámetros de entrada: sesionId(id compañia), cadenaConexion(bus request), nombreQueueRequest(cola request), body(json request)
          Parámetros de salida: No aplica
          Último cambio: 08/03/2022
          Autor de último cambio: Solange Moncada
        */
        public async Task EnviarMensajeAsync(string sesionId, string cadenaConexion, string nombreQueueRequest, T body)
        {
            await using (ServiceBusClient client = new ServiceBusClient(cadenaConexion))
            {
                /*Crea un remitente para la cola*/
                ServiceBusSender sender = client.CreateSender(nombreQueueRequest);

                /*Crea el mensaje para ser enviado*/
                ServiceBusMessage message = new ServiceBusMessage(JsonConvert.SerializeObject(body));
                message.SessionId = sesionId;
                message.ContentType = "application/json";
              
                /*Envía el mensaje*/
                await sender.SendMessageAsync(message);

                await sender.CloseAsync();

            }
        }
    }
}
