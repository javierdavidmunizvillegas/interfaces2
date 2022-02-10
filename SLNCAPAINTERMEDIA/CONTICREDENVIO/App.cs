using CONTICREDENVIO.Modelos;
using CONTICREDENVIO.Servicios;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CONTICREDENVIO
{
    public class App
    {
        private readonly IConfigurationRoot _config;
        private IContDBConexionServicio _contDB;
        private IContColaServicio _contCola;
        private IContLOG _contLog;
        //private readonly ILogger<App> _logger;

        public App(IConfigurationRoot config
            , IContDBConexionServicio contDB
            ,IContColaServicio contCola
        , IContLOG contLOG)
        
        {
            //_logger = loggerFactory.CreateLogger<App>();
            _config = config;
            _contDB = contDB;
            _contCola = contCola;
            _contLog = contLOG;
        }

        public async Task Run()
        {
            List<Producto> ls = null;
            Parametro Param = null;
            List<List<Producto>> ProductListList = null;
            string Inicio = null;
            // proceso pasar a historico
            // proceso unir tablas y crear tabla de trabajo de productos
            // asegurarse q existan datos enproductos
            Console.WriteLine("PROCESO CONTINGENCIA: inicio de proceso......");
            try
            {
                Inicio = "SI"; // _contDB.ProcValidaInicio(); hsata mismai


                if (Inicio == "SI")
                {
                    try
                    {
                        Console.WriteLine("PROCESO CONTINGENCIA: Respaldo a Històrico...");
                       // hasta mismo _contDB.ProcRespaldaAhistoricos();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                    try 
                    {
                        Console.WriteLine("PROCESO CONTINGENCIA: Crea Productos Generales");
                        _contDB.ProcUnionProductosCalif();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                    try
                    {
                        Console.WriteLine("PROCESO CONTINGENCIA: Recupera Productos");
                        ls = _contDB.LeerProductos();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                    try
                    {
                        Console.WriteLine("PROCESO CONTINGENCIA: Recupera Paràmetros...");
                        Param = _contDB.LeerParametros();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }

                    ProductListList = new List<List<Producto>>();
                    ProductListList = DivideLista.Split(ls, 1000);

                    try
                    {
                        Console.WriteLine("PROCESO CONTINGENCIA: Envìo a Cola...");
                        _contLog.FileLogger("APIVTA018002", "Envio a la Cola  ");
                        await _contCola.EnviarMensajeColaProductos(Param, ProductListList);
                        // finaliza proceso actualiza estado de tabla proceso
                        // _contDB.ProcActualizaEnvio(); hasta mismo

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                }
                else
                    {
                        Console.WriteLine("PROCESO CONTINGENCIA: No puede procesar Envìo, aùn no termina descarga");
                    }
                }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            Console.WriteLine("PROCESO CONTINGENCIA: finalizaciòn de proceso!");
            Console.ReadKey();
            
        }
    }
}
