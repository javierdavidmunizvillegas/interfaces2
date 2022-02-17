using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class AxTiendaGrupoSecuenciaDto
    {
        public int Id { get; set; }
        public string EstadoRegistro { get; set; }
        public string CodigoAlmacen { get; set; }
        public string CodigoTienda { get; set; }
        public string Tienda { get; set; }
        public string CodigoEstablecimiento { get; set; }
        public string CiudadMatriculacion { get; set; }
        public string CantonMatriculacion { get; set; }
        public string FacturaElectronica { get; set; }
        public string FacturaManual { get; set; }
        public string NotaCreditoFiscal { get; set; }
        public string NotaCreditoBilleteComprador { get; set; }
        public string NotaCreditoInterna { get; set; }
        public string NotaDebitoFiscal { get; set; }
        public string NotaDebitoInterna { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string UsuarioIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string TerminalModificacion { get; set; }
    }
}
