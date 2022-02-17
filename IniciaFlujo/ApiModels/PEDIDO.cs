﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class PEDIDO
    {
        public decimal INUMEROPEDIDO { get; set; }
        public string CESTADOREGISTRO { get; set; }
        public string CCODIGOEMPRESA { get; set; }
        public string CCODIGOALMACEN { get; set; }
        public string CCODIGOUNIDADNEGOCIO { get; set; }
        public string CCODIGOTIPOIDENTIFICACION { get; set; }
        public string CIDENTIFICACION { get; set; }
        public string CCODIGOMONEDA { get; set; }
        public string CNOMBREPERSONA { get; set; }
        public string CAPELLIDOPATERNO { get; set; }
        public string CAPELLIDOMATERNO { get; set; }
        public string CPRIMERNOMBRE { get; set; }
        public string CSEGUNDONOMBRE { get; set; }
        public Nullable<decimal> ICODIGOCLIENTEMODELO { get; set; }
        public Nullable<decimal> ICODIGOCLIENTE { get; set; }
        public string CEXENTOIMPUESTO { get; set; }
        public string CDOMICILIO1 { get; set; }
        public string CTELEFONOCONVENCIONAL { get; set; }
        public string CNUMEROCELULAR1 { get; set; }
        public string CNUMEROCELULAR2 { get; set; }
        public string CNUMEROCELULAR3 { get; set; }
        public string CDOMICILIO2 { get; set; }
        public string CDOMICILIO3 { get; set; }
        public string CCODIGOVENDEDOR { get; set; }
        public string CCODIGOGRUPOIMP { get; set; }
        public string CDIRECCIONCORREO { get; set; }
        public string CCODIGOPROVINCIA { get; set; }
        public string CCODIGOCIUDAD { get; set; }
        public string COBSERVACION1 { get; set; }
        public string COBSERVACION2 { get; set; }
        public string COBSERVACION3 { get; set; }
        public string CCODIGOGRUPOVTA { get; set; }
        public string CCODIGOCALIFICACIONPROP { get; set; }
        public string CCODIGOFORMALIDAD { get; set; }
        public string CCODIGOTERMINOPAGO { get; set; }
        public string CCODIGOLISTAPRECIO { get; set; }
        public Nullable<decimal> ICUPODISPONIBLE { get; set; }
        public string CCODIGOESTADO { get; set; }
        public string CBILLETEOREGALO { get; set; }
        public Nullable<decimal> IVALORBILLETECOMPRADOR { get; set; }
        public Nullable<decimal> IVALORREGALO { get; set; }
        public Nullable<decimal> ISALDOREGALO { get; set; }
        public string CPREFIJOBCMANUAL { get; set; }
        public Nullable<decimal> INUMEROBCMANUAL { get; set; }
        public Nullable<decimal> IVALORBCMANUAL { get; set; }
        public string CCODIGOPROMOCION { get; set; }
        public Nullable<decimal> IVALORPROMOCION { get; set; }
        public Nullable<decimal> IAPORTEMARCAPROMO { get; set; }
        public string CNECESITAAUTORIZACION { get; set; }
        public Nullable<decimal> IMARGENVTAREQUERIDA { get; set; }
        public Nullable<decimal> IMARGENVENTA { get; set; }
        public Nullable<decimal> ITOTPVP { get; set; }
        public Nullable<decimal> ITOTDSCTCOMBO { get; set; }
        public Nullable<decimal> ITOTDSCTPROMOC { get; set; }
        public Nullable<decimal> ITOTDSCTALMACEN { get; set; }
        public Nullable<decimal> ITOTDSCTPVP { get; set; }
        public Nullable<decimal> ITOTDSCTCUOTAINICIAL { get; set; }
        public Nullable<decimal> ITOTDSCTXPLAZO { get; set; }
        public Nullable<decimal> ITOTDSCTXTARJETA { get; set; }
        public Nullable<decimal> ITOTDSCTPRECIONETO { get; set; }
        public Nullable<decimal> ITOTDSCTIVA { get; set; }
        public Nullable<decimal> ISUBTOTALPAGAR { get; set; }
        public Nullable<decimal> ITOTALSEGURO { get; set; }
        public Nullable<decimal> ITOTALGASTOADMINI { get; set; }
        public Nullable<decimal> ITOTALFLETE { get; set; }
        public Nullable<decimal> ITOTALGENERAL { get; set; }
        public Nullable<decimal> ITOTALAPORTEMARCA { get; set; }
        public Nullable<decimal> IPORCENTDSCTINICIAL { get; set; }
        public Nullable<decimal> IPORCENTDSCTPLAZO { get; set; }
        public Nullable<decimal> IVALORCUOTAINICREQ { get; set; }
        public Nullable<decimal> ITOTDSCTCUOTAINICIALIVA { get; set; }
        public Nullable<decimal> ITOTDSCTXPLAZOIVA { get; set; }
        public Nullable<decimal> ITOTDSCTTARJETAIVA { get; set; }
        public Nullable<decimal> IVALOREFECTIVO { get; set; }
        public Nullable<decimal> IVALOREFECTSEGURO { get; set; }
        public Nullable<decimal> IVALORCREDITO { get; set; }
        public Nullable<decimal> IVALORCREDITOSEGURO { get; set; }
        public Nullable<decimal> IVALORTARJETA { get; set; }
        public Nullable<decimal> IVALORTARJETASEGURO { get; set; }
        public Nullable<decimal> IVALORBONOFP { get; set; }
        public Nullable<decimal> IVALORCUOTAINICIAL { get; set; }
        public Nullable<decimal> IMESPLAZO { get; set; }
        public string CCODIGOFORMAPAGOPRINCI { get; set; }
        public string CCODIGOFORMAPAGOADICIONAL { get; set; }
        public string CCODIGOPLANFINANCPRINCI { get; set; }
        public Nullable<System.DateTime> DFECHAPOSIBLECOMPRA { get; set; }
        public Nullable<decimal> IVALORCUOTA { get; set; }
        public Nullable<decimal> IVALORCUOTAFINAL { get; set; }
        public Nullable<decimal> IPORCENTFINANSEGURO { get; set; }
        public Nullable<decimal> IVALORFINANSEGURO { get; set; }
        public Nullable<System.DateTime> DFECHAAUTORIZACION { get; set; }
        public string CUSUARIOAUTORIZACION { get; set; }
        public string CTERMINALAUTORIZACION { get; set; }
        public Nullable<decimal> INUMEROFACTURA { get; set; }
        public Nullable<System.DateTime> DFECHAFACTURACION { get; set; }
        public string CUSUARIOFACTURACION { get; set; }
        public string CTERMINALFACTURACION { get; set; }
        public string CPINVENDEDOR { get; set; }
        public string CESTADO { get; set; }
        public string CMOTIVOESTADO { get; set; }
        public string CTIPOPEDIDOCOTIZA { get; set; }
        public Nullable<decimal> INUMEROLINEA { get; set; }
        public Nullable<decimal> INUMEROPEDIDOANTERIOR { get; set; }
        public Nullable<decimal> ICIAFACTURA { get; set; }
        public string CPREFIJOFACTURA { get; set; }
        public Nullable<decimal> NNUMEROPEDIDOBPCS { get; set; }
        public Nullable<decimal> NNUMEROFACTURAMANUAL { get; set; }
        public Nullable<decimal> IVALORPROMOCIONALBONO { get; set; }
        public Nullable<decimal> IVALORPROMOCINALAPORTE { get; set; }
        public string CESTADOENVIOBPCS { get; set; }
        public string CESTADORESULTADOBPCS { get; set; }
        public Nullable<decimal> IDSCTTOTALIVA { get; set; }
        public Nullable<decimal> INUMEROCUOTAGRATIS { get; set; }
        public Nullable<decimal> IVALORMATRICULA { get; set; }
        public Nullable<decimal> INUMEROCUOTAMATRICULA { get; set; }
        public Nullable<decimal> IVALORCUOTAMATRICULA { get; set; }
        public Nullable<decimal> IINTERESMATRICULA { get; set; }
        public string CCODIGOPROMOCION2 { get; set; }
        public Nullable<decimal> IVALORPROMOCION2 { get; set; }
        public Nullable<decimal> INUMEROPEDIDOORIGEN { get; set; }
        public string CGENERADAGESTION { get; set; }
        public Nullable<System.DateTime> DFECHANACIMIENTO { get; set; }
        public Nullable<bool> CAPLICOSEGUROS { get; set; }
        public Nullable<bool> CAPLICOGASTOSEGUROS { get; set; }
        public Nullable<bool> CESPEDIDOSEGURO { get; set; }
        public string CCODIGOORIGEN { get; set; }
        public string CTIPOPEDIDO { get; set; }
        public string CIDENTIFICACIONEMPRESA { get; set; }
        public string CCODIGOPEDIDOERP { get; set; }
        public string CCODIGOACUERDOVENTA { get; set; }
        public System.DateTime DFECHAINGRESO { get; set; }
        public string CUSUARIOINGRESO { get; set; }
        public string CTERMINALINGRESO { get; set; }
        public Nullable<System.DateTime> DFECHAMODIFICACION { get; set; }
        public string CUSUARIOMODIFICACION { get; set; }
        public string CTERMINALMODIFICACION { get; set; }
        public Nullable<bool> BFACTURAENVIADACORREOELECTRONICO { get; set; }
        public Nullable<bool> BFACTUROVENDEDOR { get; set; }
        public Nullable<bool> BTABLAAMORTIZACIONENVIADACORREOELECTRONICO { get; set; }
        public Nullable<bool> BCERTIFICADOASISTENCIAFACILITAENVIADACORREOELECTRONICO { get; set; }
        public Nullable<bool> BFUECHEQUEADOPORCREDITO { get; set; }
        public string CRESPUESTAAUMENTOCUPO { get; set; }
        public Nullable<bool> BESORDENVENTAZONAL { get; set; }
        public string CESTADOORDENVENTAZONAL { get; set; }
        public Nullable<System.DateTime> DFECHAORDENVENTAZONAL { get; set; }
        public string CUSUARIOORDENVENTAZONAL { get; set; }
        public Nullable<bool> BESORDENVENTACONTINGENCIA { get; set; }
        public Nullable<bool> BESORDENVENTAPLANILLA { get; set; }
        public string CNUMEROTARJETA { get; set; }
    }
}
