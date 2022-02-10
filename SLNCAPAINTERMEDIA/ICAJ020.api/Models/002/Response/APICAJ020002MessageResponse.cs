using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ020.api.Models._002.Response
{
    public class APICAJ020002MessageResponse
    {
        public string SessionId { get; set; } //Id de sesión
        public Boolean StatusId { get; set; } //Descripción "True" o "False"
        public List<string> ErrorList { get; set; } //Listado de errores
        public string Voucher { get; set; } //Asiento del cobro que se reversó
        public string VoucherReverse { get; set; } //Nuevo asiento del diario 
        public decimal Amount { get; set; } //Monto o valor del diario de pago que se va a reversar
        public string ReasonReverse { get; set; } //Motivo del reverso

        public string DateReverse { get; set; } //Fecha en la que se realiza el reverso DateTime


    }
}
