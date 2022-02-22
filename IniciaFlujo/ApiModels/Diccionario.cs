using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public static class Diccionario
    {

        public static class FormaPagoSIAC
        {
            public const string Efectivo = "001";
            public const string ChequeVista = "003";
            public const string TarjetaCredito = "004";
            public const string Anticipo = "005";
            public const string NotaCredito = "006";
            public const string CertificadoDeposito = "007";
            public const string ComprobanteRetencion = "008";
            public const string DepositoEfectivo = "060";
            public const string DepositoCheque = "061";
            public const string CreditoFacilito = "036";
            public const string Transferencia = "016";
            public const string MediosElectronicos = "031";
            public const string BilleteComprador = "033";
            public const string AnticipoCliente = "010";
            public const string ChequeBono = "032";
        }
    }
}
