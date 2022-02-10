using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ020.api.Models._002.Request
{
    public class APMediaElectronicRequest
    {
        public string StoreId { get; set; } //código de tienda
        public string NumberDocument { get; set; } //número de documento (factura - recibo)
        public string CustAccount { get; set; } //código de cliente
        public string Processor { get; set; } //Procesador
        public string BankAdquiringId { get; set; } //Banco adquiriente
        public string BatchNumber { get; set; } //Número de lote
        public string Reference { get; set; } //referencia
        public string AuthorizationNumber { get; set; } //Número de autorización



    }
}
