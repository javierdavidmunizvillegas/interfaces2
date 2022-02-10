using System;
using System.Collections.Generic;
using System.Text;

namespace ISAC018.Models._001.Resquest
{
    class APISAC018001MessageRequestDynamic
    {
        public string custAccount { get; set; }//codigo de cliente

        public string vatNum { get; set; }//identificacion de cliente 

        public string creditNoteNum { get; set; }//numero de la nota de credito

        public DateTime creditNoteDate { get; set; } //fecha nota de credito
        public decimal creditNoteAmount { get; set; } //monto total de nota de credito
        public string returnNum { get; set; } //numero de orden de devolucion
        public string invoiceNum { get; set; } //factura origen
        public string reasonRefund { get; set; } //motivo de nota de credito
        public List<APItemReturnISAC018001> itemReturnList { get; set; } //Listado codigo de disposición
        public string Enviroment { get; set; }//Entorno

    }
}
