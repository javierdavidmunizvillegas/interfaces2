using ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ApiFlujoCaja.Controllers
{
    public class FlujoController: ApiController
    {

        [HttpPost]
        [Route("api/IniciaFlujo")]
        public Base IniciaFlujo([FromBody] FlujoRequest datos)
        {
            Base Retorno = new Base();
            Retorno = EjecutaProcesoFacturacion(datos);
            return Retorno;
        }

        [HttpPost]
        [Route("api/IniciaFlujoDynamics")]
        public Base IniciaFlujo([FromBody] ICAJ008Response ICAJ008)
        {
            Base Retorno = new Base();
            FlujoRequest datos = new FlujoRequest();
            string JSONDatos = "{'CodigoCaja':'ALBOR01','SalesID':'V000002386','SalesIDSiac':'5845389','SalesOrigin':'RETAIL','TipoTransaccion':'6','OrigenTransaccion':'DYNAMICS','CodigoCliente':'2073464','Cedula':'0908288558','CodigoEmpresa':'000001','CodigoAlmacen':'ALB01','CodigoTIendaSIAC':'AA','NumeroRecibo':'3975','UsuarioIngreso':'CALDLORE','TerminalIngreso':'192.168.43.229','Monto':556.75,'InvoiceId':'','Motive':'','CPN':'','Voucher':'','MedioPago':[{'FormaPago':'16 - TRANSFERENCIA','Valor':'300.00','AsientoContable':'DCPT-000000072'},{'FormaPago':'01 - EFECTIVO','Valor':'256.75','AsientoContable':'AL01000000382'}]}";
            datos = JsonConvert.DeserializeObject<FlujoRequest>(JSONDatos);
            Retorno = EjecutaProcesoLiquidacion(datos, ICAJ008);
            return Retorno;
        }


        private Base EjecutaProcesoFacturacion(FlujoRequest datos)
        {
            Base Retorno = new Base();
            bool blResultadoFinal = true;
            clsSIAC ObjSIAC = new clsSIAC();
            clsConfirmacionPedidos objConfirmacionPedidos = new clsConfirmacionPedidos();
            clsICAJ008 ObjICAJ008 = new clsICAJ008();
            string JsonResponse = string.Empty;
            try
            {
                if (!ObjSIAC.ConsultaLogs(datos, "Inicia Flujo", ref JsonResponse))
                {
                    ObjSIAC.RegistrarLogs(datos, "Inicia Flujo", JsonConvert.SerializeObject(datos), string.Empty, string.Empty, "FALSE");

                    var resc = objConfirmacionPedidos.ConfirmarPedido(datos);
                    if (resc.CodigoMensaje != "200")
                    {
                        Retorno.CodigoError = 100;
                        Retorno.Mensaje = "Error en la confirmación del pedido.";
                        Retorno.Estado = false;
                        return Retorno;
                    }

                    var respICAJ008 = ObjICAJ008.ICAJ008(datos);
                    if (respICAJ008.statusId == false)
                    {
                        Retorno.CodigoError = 200;
                        Retorno.Mensaje = "Error en la facturación del pedido.";
                        Retorno.Estado = true;
                        return Retorno;
                    }


                    if (respICAJ008.documentInvoiceRequestTableList.Count > 0)
                    {
                        Retorno = EjecutaProcesoLiquidacion(datos, respICAJ008);
                    }
                    else
                    {
                        Retorno.Estado = blResultadoFinal;
                        Retorno.CodigoError = 0;
                        Retorno.Mensaje = "Proceso finalizado con exito.";
                        ObjSIAC.RegistrarLogs(datos, "Inicia Flujo", JsonConvert.SerializeObject(datos), JsonConvert.SerializeObject(Retorno), string.Empty, blResultadoFinal.ToString().ToUpper());
                    }
                }
            }
            catch (Exception Ex)
            {
                Retorno.CodigoError = 90;
                Retorno.Mensaje = string.Concat("EjecutaProcesoFacturacion: ", Ex.Message);
                Retorno.Estado = false;
            }
            return Retorno;
        }
        private Base EjecutaProcesoLiquidacion(FlujoRequest datos, ICAJ008Response RespCAJ008)
        {
            Base Retorno = new Base();
            bool blResultadoFinal = true;
            string JsonResponse = string.Empty;

            clsSIAC ObjSIAC = new clsSIAC();
            clsICOB001 ObjICB001 = new clsICOB001();
            clsGTM004 ObjGTM004 = new clsGTM004();
            clsGeneracionCartera ObjCartera = new clsGeneracionCartera();
            clsComisiones ObjComisiones = new clsComisiones();
            try
            {
                if (!ObjSIAC.ConsultaLogs(datos, "Inicia Flujo", ref JsonResponse))
                {
                    if (datos.OrigenTransaccion != "CAJA")
                        ObjSIAC.RegistrarLogs(datos, "Inicia Flujo", JsonConvert.SerializeObject(datos), string.Empty, string.Empty, "FALSE");

                    var RespICOB001 = ObjICB001.ICOB001(datos, RespCAJ008);
                    if (RespICOB001.StatusId == false)
                    {
                        Retorno.CodigoError = 300;
                        Retorno.Mensaje = "Error en la liquidación de los asientos.";
                        Retorno.Estado = false;
                        return Retorno;
                    }

                    #region "Llamados de servicios secundarios"

                    var RespComisiones = ObjComisiones.GeneraComisiones(datos, RespCAJ008);
                    if (RespComisiones.StatusCode == "ERROR")
                    {
                        Retorno.CodigoError = 400;
                        Retorno.Mensaje = "Error en la generación de comisiones.";
                        Retorno.Estado = false;
                        blResultadoFinal = false;
                    }

                    var RespCartera = ObjCartera.GeneracionCartera(datos, RespCAJ008);
                    if (RespCartera.StatusCode  == "ERROR")
                    {
                        Retorno.CodigoError = 500;
                        Retorno.Mensaje = "Error en la generación de cartera.";
                        Retorno.Estado = false;
                        blResultadoFinal = false;
                    }

                    var RespIGTM004 = ObjGTM004.IGTM004(datos, RespCAJ008);
                    if (RespICOB001.StatusId == false)
                    {
                        Retorno.CodigoError = 600;
                        Retorno.Mensaje = "Error en la notificación de facturación de Moto.";
                        Retorno.Estado = false;
                        blResultadoFinal = false;
                    }
                    #endregion

                    Retorno.Estado = blResultadoFinal;
                    Retorno.CodigoError = 0;
                    Retorno.Mensaje = "Proceso finalizado con exito.";
                    ObjSIAC.RegistrarLogs(datos, "Inicia Flujo", JsonConvert.SerializeObject(datos), JsonConvert.SerializeObject(Retorno), string.Empty, blResultadoFinal.ToString().ToUpper());
                }
            }
            catch (Exception Ex)
            {
                Retorno.CodigoError = 90;
                Retorno.Mensaje = string.Concat("EjecutaProcesoLiquidacion: ", Ex.Message);
                Retorno.Estado = false;
            }
            return Retorno;
        }
    }
}