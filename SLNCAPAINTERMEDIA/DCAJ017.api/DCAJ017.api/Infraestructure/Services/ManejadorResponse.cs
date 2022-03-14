/*
 Objetivo: Recibe la respuesta desde la cola de response.
 Archivo: ManejadorResponse.cs
 Versión: 1.0
 Creación: 07/03/2022
 Autor: Solange Moncada
*/

using DCAJ017.api.Infraestructure.Configuration;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Infraestructure.Services
{
    public interface IManejadorResponse<T>
    {
        Task<T> RecibeMensajeSesion(string cadenaConexion, string nombreCola, string sesionId, int segundos, int intentos, int tiempo, string nombre);
    }
    public class ManejadorResponse<T> : IManejadorResponse<T>
    {
        /*Objeto de la clase RegistroLog*/
        private static RegistroLog Logger = new RegistroLog();

        /*
          Descripción: Este método recibe la respuesta de la cola de response.
          Parámetros de entrada: cadenaConexion(bus request), nombreCola(cola response), sesionId(id de la compañía), segundos, intentos, tiempo, nombre(nombre de la interfaz)
          Parámetros de salida: No aplica
          Último cambio: 08/03/2022
          Autor de último cambio: Solange Moncada
        */
        public async Task<T> RecibeMensajeSesion(string cadenaConexion, string nombreCola, string sesionId, int segundos, int intentos, int tiempo, string nombre)
        {
            /*Variable para contador*/
            int contador = 0;
            /*Variable para respuesta*/
            T respuesta = default(T);

            /*medir tiempo transcurrido en ws*/
            /*Variable para inicio y fin del proceso*/
            long start = 0, end = 0;
            /*Variable restar el fin con el inicio*/
            double diff = 0;
            /*Constante para Segundos*/
            const int SegAMiliS = 1000;
            /*Constante para Milisegundos*/
            const int NanoAMiliS = 10000;


            var serviceBusConnectionStringBuilder = new ServiceBusConnectionStringBuilder(cadenaConexion);

            var serviceBusConnection = new ServiceBusConnection(serviceBusConnectionStringBuilder);

            var sessionClient = new SessionClient(serviceBusConnection, nombreCola, ReceiveMode.PeekLock, null, 0);

            var messageSession = await sessionClient.AcceptMessageSessionAsync(sesionId, TimeSpan.FromMinutes(1));
                        
            if (messageSession != null)
            {
                /*Inicia el proceso*/
                start = Stopwatch.GetTimestamp();

                while (contador < intentos)
                {
                    /*Recepción de la respuesta de Dynamics y la borra de la cola*/
                    Message message = await messageSession.ReceiveAsync(TimeSpan.FromSeconds(tiempo));

                    if (message != null)
                    {
                        var bodyText = System.Text.Encoding.UTF8.GetString(message.Body);

                        /*Respuesta recibida desde Dynamics deserializada*/
                        respuesta = JsonConvert.DeserializeObject<T>(bodyText);
                        await messageSession.CompleteAsync(message.SystemProperties.LockToken);
                        /*Si responde algo salgo*/
                        break;
                    }
                    else
                    {
                        /*Si no responde sigo hasta que se terminen los intentos y tiempo configurados*/
                        await Task.Delay(segundos * SegAMiliS);
                    }
                    contador++;
                }
                /*Fin del proceso*/
                end = Stopwatch.GetTimestamp();
                /*Resta del fin con el inicio*/
                diff = start > 0 ? (end - start) / NanoAMiliS : 0;
                Logger.FileLogger(nombre, "SERVICIO RESPONSE : Número de  intentos realizados : " + Convert.ToString(contador) + $", Tiempo Transcurrido : {diff} ms.");
            }
            await messageSession.CloseAsync();
            await sessionClient.CloseAsync();
            await serviceBusConnection.CloseAsync();

            /*Retorno la respuesta al controlador*/
            return respuesta;
        }
    }


}
