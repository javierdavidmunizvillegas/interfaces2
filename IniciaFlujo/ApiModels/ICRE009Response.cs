using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class ICRE009Response
    {
        public string SessionId { get; set; }
        public bool statusId { get; set; }
        public List<string> errorList { get; set; }
        public List<PedidoFacturaDto> APICRE009001InvoiceOrderList { get; set; }
    }

    public class PedidoFacturaDto
    {
        public string APStoreId { get; set; }
        public string DescriptionEstablishment { get; set; }
        public DireccionDto APLogisticsPostalAddress { get; set; }
        public FacturaDto APInvoice { get; set; }
        public string CustGroup { get; set; }
        public string APStoreName { get; set; }
        public PedidoDto APSalesTable { get; set; }
        public List<PedidoDetalleDto> salesLineList { get; set; }
    }

    public class FacturaDto
    {
        public string LastName { get; set; }
        public string AuthorizationNumber { get; set; }
        public string DocumentApplied { get; set; }
        public string email { get; set; }
        public string Establishment { get; set; }
        public string EstablishmentApplied { get; set; }
        public DateTime DateAuthSRI { get; set; }
        public string InvoiceDate { get; set; }
        public string dataAreaId { get; set; }
        public string VatNum { get; set; }
        public string APIdentificationList { get; set; }
        public List<DetalleNotaCredito> APCreditNoteServices { get; set; }
        public string Name { get; set; }
        public string InvoiceId { get; set; }
        public DetalleOrdenVenta APSalesTable { get; set; }
        public string FirstName { get; set; }
        public string EmissionPoint { get; set; }
        public string EmissionPointApplied { get; set; }
        public string SecondName { get; set; }
        public string phoneNumber { get; set; }
        public string Voucher { get; set; }
    }

    public class DetalleNotaCredito
    {
        public int Qty { get; set; }
        public string Description { get; set; }
        public decimal Tax { get; set; }
        public decimal UnitPriece { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }

    public class DetalleOrdenVenta
    {
        public string IndependentEntrepreneurId { get; set; }
        public string CustAccount { get; set; }
        public string WorkSalesResponsible { get; set; }
        public string APStoreId { get; set; }
        public string MotiveReturnDescription { get; set; }
        public string ReasonDescriptionNC { get; set; }
        public decimal TotalDiscount { get; set; }
        public string ChannelDimFinOrigin { get; set; }
        public string BusinessUnitDimFinOrigin { get; set; }
        public string CustGroup { get; set; }
        public string MotiveReturnId { get; set; }
        public decimal TotalIVA { get; set; }
        public List<LineaOrdenVenta> APSalesLineList { get; set; }
        public string ReasonIdNC { get; set; }
        public string WorkSalesResponsibleName { get; set; }
        public string APStoreName { get; set; }
        public string SalesReturn { get; set; }
        public string SalesId { get; set; }
        public string NumberOrdenRef { get; set; }
        public decimal Subtotal { get; set; }
        public decimal AmountInvoice { get; set; }
    }

    public class LineaOrdenVenta
    { 
        public string InventLocationId { get; set; }
        public decimal Qty { get; set; }
        public string APCategoryId { get; set; }
        public string ItemId { get; set; }
        public string Color { get; set; }
        public string ItemName { get; set; }
        public decimal LineDisc { get; set; }
        public string APGroupId { get; set; }
        public decimal Tax { get; set; }
        public string APLineId { get; set; }
        public string WMSLocationId { get; set; }
        public string Marca { get; set; }
        public decimal SalesPrice { get; set; }
        public string Serie { get; set; }
        public string APSubGroupId { get; set; }
        public decimal SubTotalLinea { get; set; }
        public int ItemType { get; set; }
        public decimal LineAmount { get; set; }
    }

    public class PedidoDto
    {
        public string CustAccount { get; set; }
        public string IndependentEntrepreneurId { get; set; }
        public string APProductFinancialCode { get; set; }
        public string WorkSalesResponsible { get; set; }
        public decimal Subtotal { get; set; }
        public string APPayModeSales { get; set; }
        public decimal totalIVA { get; set; }
        public string WorkSalesResponsibleName { get; set; }
        public string SalesId { get; set; }
        public string NumberOrdenRef { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal AmountInvoice { get; set; }
    }

    public class PedidoDetalleDto
    {
        public string InventLocationId { get; set; }
        public decimal Qty { get; set; }
        public string APCategoryId { get; set; }
        public string ItemId { get; set; }
        public string Color { get; set; }
        public string ItemName { get; set; }
        public decimal LineDisc { get; set; }
        public string BusinessUnitDimensionF { get; set; }
        public bool IsHomeDelivery { get; set; }
        public DateTime ReceiptDateRequested { get; set; }
        public string APGroupId { get; set; }
        public string APComboId { get; set; }
        public string IdentificationItem { get; set; }
        public decimal Tax { get; set; }
        public string APLineId { get; set; }
        public string WMSLocationId { get; set; }
        public string Marca { get; set; }
        public MotoDto APSalesVehicle { get; set; }
        public decimal SalesPrice { get; set; }
        public string Serie { get; set; }
        public string APSubGroupId { get; set; }
        public decimal SubTotalLinea { get; set; }
        public int ItemType { get; set; }
        public decimal LineAmount { get; set; }
        public string BusinessUnit { get; set; }
    }

    public class MotoDto
    {
        public string VehicleYear { get; set; }
        public string Displacement { get; set; }
        public string ClassStr { get; set; }
        public string CountryRegionIdOrigin { get; set; }
        public string CPNNumber { get; set; }
        public string Model { get; set; }
        public string ChassisSeries { get; set; }
        public string AxlesNumber { get; set; }
        public string PassengersNumber { get; set; }
        public string WheelsNumber { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
        public string FuelType { get; set; }
        public string Tonnage { get; set; }
    }
}
