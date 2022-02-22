using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class CajaItemsFlujo
    {
        public int EsMoto { get; set; }
        public List<Cabecera> Cabecera { get; set; }
        public List<Detalle> Detalle { get; set; }
        
        public CajaItemsFlujo()
        {
            Cabecera = new List<Cabecera>();
            Detalle = new List<Detalle>();
        }
    }

    public partial class Cabecera
    {
        public string CustAccount { get; set; }
        public string SalesId { get; set; }
        public string SalesOrigin { get; set; }
        public string PostingProfile { get; set; }
        public string NumeroPedido { get; set; }
        public int SecuenciaGroup { get; set; }
    }

    public partial class Detalle
    {
        public string NumeroPedido { get; set; }
        public int SecuenciaGroup { get; set; }
        public string ItemId { get; set; }
        public string Serial { get; set; }
        public int Cantidad { get; set; }
        public string EsMoto { get; set; }

    }

}
