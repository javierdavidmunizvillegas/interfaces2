using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ020.api.Models._002.Request
{
    public class APICAJ020002MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; } //Id de la compañía 
        public string Enviroment { get; set; } // Entorno
        public string SessionId { get; set; } // Id de sesión
        [Required]
        public string CustAccount { get; set; } // Código de cliente
        [Required]
        public string Voucher { get; set; } // Número de asiento principal del pago que se va a realizar el reverso
        
        public string PaymMode { get; set; } // Forma de pago
        [Required]
        public decimal Amount { get; set; } // Monto o valor del diario de pago que se va a reversar
        [Required]
        public string ReasonReverse { get; set; } // Motivo del reverso
        [Required]
        public string DateReverse { get; set; } // Fecha en la que se realiza el reverso DateTime
        public string DescriptionReverse { get; set; } // Descripción del diario de reverso cuando sea por medios electrónicos (TC y TD)
      //  public List<APChequeRequest> ChequeRequestList { get; set; } // Documento de cheques
        public List<APMediaElectronicRequest> MediaElectronicRequestList { get; set; } // Documento de medios electrónicos
       //( public List<APDocumentRetentionRequest> DocumentRetentionRequest { get; set; } //Documento de comprobantes de retenciones
        //public List<APLicenseRequest> LicenseRequest { get; set; } //Documento de matrículas y multas
        public APLicenseRequest LicenseRequest { get; set; } //Documento de matrículas y multas




    }
}
