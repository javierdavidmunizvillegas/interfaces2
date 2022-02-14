using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Request
{
    public class MidTransactions
    {
        public Guid TransactionId { get; set; }
        public string AppRefCode { get; set; }
        public Guid SriFacId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public int DocumentEnviroment { get; set; }
        public string DocumentEmisorLegalId { get; set; }
        public int Process { get; set; }
        public DateTime ProcessDt { get; set; }
        public Guid ResponseId { get; set; }//Estaba como datetime como en el documento, pero legado lo tiene como guid
        public string ProcessMessage { get; set; }
        public DateTime InsertedDateTime { get; set; }
        public int Estado { get; set; }


    }
}
