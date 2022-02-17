using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class GeneracionCarteraRequest
    {
        public string CustAccount { get; set; }
        public string SalesId { get; set; }
        public long SalesIdAccount { get; set; }
        public string Store { get; set; }
        public string PostingProfile { get; set; }
        public decimal TotalAmount { get; set; }
        public string User { get; set; }
        public string Terminal { get; set; }
        public List<CarteraDetalleVM> DocumentInvoiceRequestLinesList { get; set; }
        public List<CarteraDetalleNC> DocumentInvoiceRequestProvisionNCList { get; set; }

        public GeneracionCarteraRequest()
        {
            DocumentInvoiceRequestLinesList = new List<CarteraDetalleVM>();
            DocumentInvoiceRequestProvisionNCList = new List<CarteraDetalleNC>();
        }
    }

    public partial class CarteraDetalleVM
    {
        public int SecuenciaFacturacion { get; set; }
        public string Voucher { get; set; }
        public string InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CarteraDetalleProductos> ItemList { get; set; }
        public CarteraDetalleVM()
        {
            ItemList = new List<CarteraDetalleProductos>();
        }
    }
    public partial class CarteraDetalleNC
    {
        public decimal AmountNC { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceId { get; set; }
        public string VoucherNC { get; set; }
        public string VoucherProvision { get; set; }
    }

    public partial class CarteraDetalleProductos
    {
        public string ItemId { get; set; }
        public decimal Qty { get; set; }
        public decimal AmountLine { get; set; }
        public int OrdenItems { get; set; }
    }
}
