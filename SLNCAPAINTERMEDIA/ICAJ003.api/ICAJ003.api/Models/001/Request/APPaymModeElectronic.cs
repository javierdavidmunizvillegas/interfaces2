using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ003.api.Models._001.Request
{
    public class APPaymModeElectronic
    {
        public string SequeceBankAcqId { get; set; }
        public string APStoreId { get; set; }        
        public string DocumentNumber { get; set; }        
        public string AccountNum { get; set; }        
        public string AccountId { get; set; }        
        public string TarjetaHabiente { get; set; }        
        public int IdTarjetaHabiente { get; set; }        
        public string Processor { get; set; }        
        public string BankAdquiring { get; set; }        
        public string NumberEncrypt { get; set; }        
        public string codeFinancialPLan { get; set; }        
        public string TypePlan { get; set; }        
        public int Plazo { get; set; }        
        public string NumberBatch { get; set; }        
        public string Reference { get; set; }        
        public string Authorization { get; set; }        
        public DateTime DateCash { get; set; }        
        public string UserDocument { get; set; }        
        public string TypeTransaction { get; set; }        
        public string LedgerNumber { get; set; }        
        public string Voucher { get; set; }        
        public decimal TotalDocument { get; set; }        
        public Status Status { get; set; }
    }
    public enum Status { ingresado = 0, abonado = 1, pagado = 2, anulado = 3 }
}
