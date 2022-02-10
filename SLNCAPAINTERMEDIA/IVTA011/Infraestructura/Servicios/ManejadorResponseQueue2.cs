using IVTA011.Infraestructura.Configuracion;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IVTA011.Servicios
{
    interface ManejadorResponseQueue2
    {
        public interface IManejadorResponseQueue2<T>
        {
            Task<T> RecibeMensajeSesion(string cadenaConexion, string nombreCola, string sesionId, int segundos, int intentos,string NombreInterface);
        }
        public class ManejadorResponseQueue2<T> : IManejadorResponseQueue2<T>
        {
            public async Task<T> RecibeMensajeSesion(string cadenaConexion, string nombreCola, string sesionId, int segundos, int intentos, string NombreInterface)
            {
                RegistroLog Logger = new RegistroLog();
                int contador = 0;
                T respuesta = default(T);
                var serviceBusConnectionStringBuilder = new ServiceBusConnectionStringBuilder(cadenaConexion);

                var serviceBusConnection = new ServiceBusConnection(serviceBusConnectionStringBuilder);

                var sessionClient = new SessionClient(serviceBusConnection, nombreCola, ReceiveMode.PeekLock, null, 0);

                var messageSession = await sessionClient.AcceptMessageSessionAsync(sesionId, TimeSpan.FromMinutes(1));

                if (messageSession != null)
                {
                    while (contador < intentos)
                    {
                        Message message = await messageSession.ReceiveAsync(TimeSpan.FromSeconds(5));
                        Logger.FileLogger(NombreInterface, "Esperando Respuesta Intento # " + Convert.ToString(contador + 1) + " ," + "Tiempo Transcurrido : " + Convert.ToString(segundos * (contador + 1)) + " Segundos");
                        if (message != null)
                        {
                            var bodyText = System.Text.Encoding.UTF8.GetString(message.Body);
                            respuesta = JsonConvert.DeserializeObject<T>(bodyText);

                            await messageSession.CompleteAsync(message.SystemProperties.LockToken);
                            contador = intentos;
                        }
                        else
                        {
                            await Task.Delay(segundos * 1000);
                        }
                        contador++;
                    }
                }
                await messageSession.CloseAsync();
                await sessionClient.CloseAsync();
                await serviceBusConnection.CloseAsync();

                return respuesta;
            }
        }
    }
}
