using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ005.api.Models._001.Request
{
    public class APLedgerJournalDepositsRequest
    {
        [Required]
        public string ConfirmationDate { get; set; } // datetime
        [Required]
        public string Bank { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Cashbox { get; set; }
        [Required]
        public string NoTicket { get; set; }
        [Required]
        public string ConfirmingUser { get; set; }
        [Required]
        public string DepositNumber { get; set; }
        [Required]
        public string DatePreparation { get; set; } // datetime
    }
}
