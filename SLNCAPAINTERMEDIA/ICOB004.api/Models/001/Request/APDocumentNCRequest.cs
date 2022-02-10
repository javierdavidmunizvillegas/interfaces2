using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB004.api.Models._001.Request
{
    public class APDocumentNCRequest
    {
        [Required]
        public string APIdentificationList { get; set; }//Identificador de lista
        [Required]
        public string CustAccount { get; set; }//Código del cliente
        [Required]
        public string TransDate { get; set; }//Fecha de emisión de la nota de crédito //datetime
        [Required]
        public string NumberSequenceGroup { get; set; }//Conjunto de secuencia numérica
        [Required]
        public string DocumentApplies { get; set; }//Número de documento que aplica
        [Required]
        public string PostingProfile { get; set; }//Perfil contable
        [Required]
        public string APRubroSIAC { get; set; }//Rubro SIAC
        [Required]
        public Decimal Qty { get; set; }//Cantidad
        [Required]
        public Decimal PriceUnit { get; set; }//Precio unitario
        [Required]
        public Decimal Neto { get; set; }//Monto neto
        [Required]

        public string MotiveNC { get; set; }//Motivo de la NC
        public int OutsideSytem { get; set; }//Documento de origen (ND)
        public int TypeOfDocument { get; set; }//Documento de origen (ND)



    }
}
