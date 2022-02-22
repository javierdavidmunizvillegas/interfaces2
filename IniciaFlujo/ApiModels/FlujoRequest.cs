using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class FlujoRequest
    {
        public string CodigoCaja { get; set; }
        public string SalesID { get; set; }
        public string SalesIDSiac { get; set; }
        public string SalesOrigin { get; set; }
        public int TipoTransaccion { get; set; }
        public string OrigenTransaccion { get; set; }
        public string CodigoCliente { get; set; }
        public string Cedula { get; set; }
        public string CodigoAlmacen { get; set; }
        public string CodigoTIendaSIAC { get; set; }
        public string CodigoEmpresa { get; set; }
        public string NumeroRecibo { get; set; }
        public string UsuarioIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public decimal Monto { get; set; }
        public string InvoiceId { get; set; }
        public string Motive { get; set; }
        public string CPN { get; set; }
        public string Voucher { get; set; }


        public List<MEDIOPAGO> MedioPago { get; set; }

        public FlujoRequest()
        {
            MedioPago = new List<MEDIOPAGO>();
        }
    }

    public class MEDIOPAGO
    {
        public string FormaPago { get; set; }
        public decimal Valor { get; set; }
        public string AsientoContable { get; set; }
    }
}
