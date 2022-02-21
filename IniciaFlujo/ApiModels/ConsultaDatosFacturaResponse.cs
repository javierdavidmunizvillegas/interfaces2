using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class ConsultaDatosFacturaResponse : Base
    {
        public string NumeroPedido { get; set; }
        public string NumeroOrdenVenta { get; set; }
        public DateTime FechaOrdenVenta { get; set; }
        public string FormaPago { get; set; }
        public bool EsPlanilla { get; set; }
        public string GrupoVenta { get; set; }
        public int CicloCorte { get; set; }
        public decimal ValorInicial { get; set; }
        public List<DetalleFacturacion> DetalleFacturacion { get; set; }

        public ConsultaDatosFacturaResponse()
        {
            DetalleFacturacion = new List<DetalleFacturacion>();
        }
    }

    public partial class DetalleFacturacion
    {
        public int AgrupadorFacturacion { get; set; }
        public int AgrupadorCartera { get; set; }
        public string PlanFinanciero { get; set; }
        public int NumeroCuota { get; set; }
        public int CodigoUso { get; set; }
        public decimal tasaNominal { get; set; }
        public int DiasGracia { get; set; }
        public int DiasPostergacion { get; set; }
        public decimal SaldoFinanciar { get; set; }
        public bool EsAsistenciaFacilita { get; set; }
        public bool EsGarantiaExtendida { get; set; }
        public bool EsMatricula { get; set; }
        public List<Producto> Producto { get; set; }

        public DetalleFacturacion()
        {
            Producto = new List<Producto>();
        }
    }

    public partial class Producto
    {
        public string SecuenciaItem { get; set; }
        public string Marca { get; set; }
        public string Linea { get; set; }
        public string Grupo { get; set; }
        public string CodigoProducto { get; set; }
        public string descripcionProducto { get; set; }
        public string Serie { get; set; }
        public int Cantidad { get; set; }
        public decimal PO { get; set; }
        public decimal PVP { get; set; }
        public bool EsGastoAdministrativoAsistencia { get; set; }
        public bool EsInteres { get; set; }
        public bool ProductoMarketplace { get; set; }
        public bool ProductoMoto { get; set; }
        public bool EsGastoAdministrativoMoto { get; set; }
        public bool EsGastoOperativoMoto { get; set; }
        public bool EsGarantiaExtendida { get; set; }
        public bool EsMatricula { get; set; }
        public decimal SaldoFinanciar { get; set; }
    }
}
