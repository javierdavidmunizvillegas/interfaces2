using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB003.api.Models._002.Request
{
    public class APCustInvoiceTable
    {
        public string RubroSIAC { get; set; }//Identificador de lista
        public string APIdentificationList { get; set; }//Identificador de lista
        public string CustAccount { get; set; }//Código del Cliente
        public string DateTrans { get; set; }//Fecha de Liquidación Date
        public string NumberSequenceGroupId { get; set; }//Grupo de secuencia 
        public decimal Qty { get; set; }//Cantidad
        public decimal PriceUnit { get; set; }//Precio Unitario
        public decimal Amount { get; set; }//Monto
        public string MotiveId { get; set; }//Motivo
        public string DocumentOrig { get; set; }//Documento de origen (ND)
        public int OutsideSytem { get; set; }//Documento de origen (ND)
        public int TypeOfDocument { get; set; }//Documento de origen (ND)

    }
}
