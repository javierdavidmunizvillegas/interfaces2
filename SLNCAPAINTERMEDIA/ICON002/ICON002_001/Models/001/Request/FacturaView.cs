using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Request
{
    //OB = OBLIGATORIO
    public class FacturaView
    {
        public List<SRIFacturaDetView> Detalle { get; set; }
        public List<SriFacturaDetImpuestosView> DetalleImpuesto { get; set; }
        public  List<SriFacturaCabTotImpuestosView> ImpuestoCab { get; set; }
        public FacturaCabecera FacturaCabecera { get; set; }
        public SriFacturaCompensacionView FacturaCompensacion { get; set; }
        public SriFacturaMaquinaFiscalView FacturaMaqFiscal { get; set; }
        public  List<SriFacturaReembolsoView> FacturaReemb { get; set; }
        public  List<SriFacturaReembolsoImpView>  FacturReembImp { get; set; }
        public List<SriFacturaFormaPagoView> FormaPago { get; set; }
        public List<SriGuiaDesDetalleView> GuiaDetalle { get; set; }
        public SriGuiaDestinatarioView GuidaDestinatario { get; set; }
        public string Password { get; set; }
        public MidTransactions Transaccion { get; set; }

    }
}
