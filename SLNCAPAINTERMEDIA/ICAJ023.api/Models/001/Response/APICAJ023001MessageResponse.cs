using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ023.api.Models._001.Response
{
    public class APICAJ023001MessageResponse
    {
        public string SessionId { get; set; }//Id de sesión
        public string StatusId { get; set; }//Estado
        public List<string> ErrorList { get; set; }//Listado de errores
        public decimal AmountBalance { get; set; }//Saldo de la Caja-Banco luego de realizada la Conciliación Bancaria
        public string DataAreaId { get; set; }//Empresa
        public string CashBankId { get; set; }//Código de Caja-Banco
        public string TransDate { get; set; }//Fecha de conciliación, Date



    }
}
