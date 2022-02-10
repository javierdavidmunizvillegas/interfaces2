using IVTA014.Infraestructura.Configuracion;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace IVTA014.Infraestructura.Servicios
{
    public interface IManejadorResponseQueue2<T>
    {
        Task<T> RecibeMensajeSesion(string cadenaConexion, string nombreCola, string sesionId, int segundos, int intentos, string NombreInterface);
    }
    public class ManejadorResponseQueue2<T> : IManejadorResponseQueue2<T>
    {
        public async Task<T> RecibeMensajeSesion(string cadenaConexion, string nombreCola, string sesionId, int segundos, int intentos, string NombreInterface)
        {
            RegistroLog Logger = new RegistroLog();
            int contador = 0;
            T respuesta = default(T);
            var serviceBusConnectionStringBuilder = new Microsoft.Azure.ServiceBus.ServiceBusConnectionStringBuilder(cadenaConexion);

            var serviceBusConnection = new ServiceBusConnection(serviceBusConnectionStringBuilder);

            var sessionClient = new SessionClient(serviceBusConnection, nombreCola, ReceiveMode.PeekLock, null, 0);

            var messageSession = await sessionClient.AcceptMessageSessionAsync(sesionId, TimeSpan.FromMinutes(1));
            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            const int SegAMiliS = 1000;
            const int NanoAMiliS = 10000;

            if (messageSession != null)

            {
                start = Stopwatch.GetTimestamp();
                while (contador < intentos)
                {
                    Message message = await messageSession.ReceiveAsync(TimeSpan.FromSeconds(5));// TIEMPO DE CADUCIDAD DEL METODO

                    if (message != null)
                    {
                        var bodyText = System.Text.Encoding.UTF8.GetString(message.Body);
                        respuesta = JsonConvert.DeserializeObject<T>(bodyText);
                        await messageSession.CompleteAsync(message.SystemProperties.LockToken);
                        break;
                    }
                    else
                    {
                        await Task.Delay(segundos * SegAMiliS);
                    }
                    contador++;
                }
                end = Stopwatch.GetTimestamp();
                diff = start > 0 ? (end - start) / NanoAMiliS : 0;
                Logger.FileLogger(NombreInterface, "SERVICIO RESPONSE : Número de  intentos realizados : " + Convert.ToString(contador + 1) + $", Tiempo Transcurrido : {diff} ms.");

            }
            await messageSession.CloseAsync();
            await sessionClient.CloseAsync();
            await serviceBusConnection.CloseAsync();

            return respuesta;
        }
    }
}
