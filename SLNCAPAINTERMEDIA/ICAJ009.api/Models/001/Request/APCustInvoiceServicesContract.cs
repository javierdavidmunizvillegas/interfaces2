using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ009.api.Models._001.Request
{
    public class APCustInvoiceServicesContract
    {
        [Required]
        public string Date { get; set; } //Id de la compañía DateTime
        [Required]
        public String NumericalSequence { get; set; } //Conjunto de secuencia numerica
        [Required]
        public string DocumentNumber { get; set; } //Numero de documento que modifica
       // [Required]
      //  public string ReasonSIAC { get; set; } //Motivo SIAC
        [Required]
        public string ReasonNC { get; set; } //Motivo NC
        [Required]
        public string DocumentDate { get; set; } //Fecha de Documentos
        [Required]
        public string CustAccount { get; set; } //Cuenta de cliente
        [Required]
        public string SourceReceipt { get; set; } //Recibo de origen
        [Required]
        public string ListIdentifier { get; set; } //Cuenta de cliente
        public int TypeOfDocument { get; set; } //Cuenta de cliente
        public int OutsideSytem { get; set; } //Cuenta de cliente

        [Required]
        public List<APCustInvoiceServicesLineContract> APCustInvoiceServicesLineContractList { get; set; } //Recibo de origen


    }
}
