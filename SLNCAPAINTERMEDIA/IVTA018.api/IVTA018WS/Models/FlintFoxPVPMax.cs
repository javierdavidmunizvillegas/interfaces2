namespace IVTA018WS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FlintFoxPVPMax")]
    public partial class FlintFoxPVPMax
    {
        [Required]
        [StringLength(255)]
        public string IdProd { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(6)]
        public string Empresa { get; set; }

        [StringLength(255)]
        public string Canal { get; set; }

        [StringLength(255)]
        public string Cliente { get; set; }

        [StringLength(255)]
        public string CalificacionCliente { get; set; }

        [StringLength(255)]
        public string Plazo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string CodProducto { get; set; }

        [StringLength(255)]
        public string EstadoInventario { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(255)]
        public string NumSerie { get; set; }

        [StringLength(300)]
        public string AttString { get; set; }

        [StringLength(255)]
        public string NumAcuerdoCosto { get; set; }

        public int? CodDescripcionCosto { get; set; }

        [StringLength(255)]
        public string DescripcionCosto { get; set; }

        public DateTime? FechaInicioCosto { get; set; }

        public DateTime? FechaFinCosto { get; set; }

        public decimal? ValorTotalCosto { get; set; }

        public decimal? PorcentajeCosto { get; set; }

        [StringLength(255)]
        public string CodErrorCosto { get; set; }

        [StringLength(255)]
        public string DescripcionErrorCosto { get; set; }

        [StringLength(255)]
        public string NumAcuerdoIndCom { get; set; }

        public int? CodDescripcionIndCom { get; set; }

        [StringLength(255)]
        public string DescripcionIndCom { get; set; }

        public DateTime? FechaInicioIndCom { get; set; }

        public DateTime? FechaFinIndCom { get; set; }

        public decimal? ValorTotalIndCom { get; set; }

        public decimal? PorcentajeIndCom { get; set; }

        [StringLength(255)]
        public string CodErrorIndCom { get; set; }

        [StringLength(255)]
        public string DescripcionErrorIndCom { get; set; }

        [StringLength(255)]
        public string NumAcuerdoRecAntPO { get; set; }

        public int? CodDescripcionRecAntPO { get; set; }

        [StringLength(255)]
        public string DescripcionRecAntPO { get; set; }

        public DateTime? FechaInicioRecAntPO { get; set; }

        public DateTime? FechaFinRecAntPO { get; set; }

        public decimal? ValorTotalRecAntPO { get; set; }

        public decimal? PorcentajeRecAntPO { get; set; }

        [StringLength(255)]
        public string CodErrorRecAntPO { get; set; }

        [StringLength(255)]
        public string DescripcionErrorRecAntPO { get; set; }

        [StringLength(255)]
        public string NumAcuerdoPO { get; set; }

        public int? CodDescripcionPO { get; set; }

        [StringLength(255)]
        public string DescripcionPO { get; set; }

        public DateTime? FechaInicioPO { get; set; }

        public DateTime? FechaFinPO { get; set; }

        public decimal? ValorTotalPO { get; set; }

        public decimal? PorcentajePO { get; set; }

        [StringLength(255)]
        public string CodErrorPO { get; set; }

        [StringLength(255)]
        public string DescripcionErrorPO { get; set; }

        [StringLength(255)]
        public string NumAcuerdoIndFinM { get; set; }

        public int? CodDescripcionIndFinM { get; set; }

        [StringLength(255)]
        public string DescripcionIndFinM { get; set; }

        public DateTime? FechaInicioIndFinM { get; set; }

        public DateTime? FechaFinIndFinM { get; set; }

        public decimal? ValorTotalIndFinM { get; set; }

        public decimal? PorcentajeIndFinM { get; set; }

        [StringLength(255)]
        public string CodErrorIndFinM { get; set; }

        [StringLength(255)]
        public string DescripcionErrorIndFinM { get; set; }

        [StringLength(255)]
        public string NumAcuerdoPVPAAlc { get; set; }

        public int? CodDescripcionPVPAAlc { get; set; }

        [StringLength(255)]
        public string DescripcionPVPAAlc { get; set; }

        public DateTime? FechaInicioPVPAAlc { get; set; }

        public DateTime? FechaFinPVPAAlc { get; set; }

        public decimal? ValorTotalPVPAAlc { get; set; }

        public decimal? PorcentajePVPAAlc { get; set; }

        [StringLength(255)]
        public string CodErrorPVPAAlc { get; set; }

        [StringLength(255)]
        public string DescripcionErrorPVPAAlc { get; set; }

        [StringLength(255)]
        public string NumAcuerdoIndAlc { get; set; }

        public int? CodDescripcionIndAlc { get; set; }

        [StringLength(255)]
        public string DescripcionIndAlc { get; set; }

        public DateTime? FechaInicioIndAlc { get; set; }

        public DateTime? FechaFinIndAlc { get; set; }

        public decimal? ValorTotalIndAlc { get; set; }

        public decimal? PorcentajeIndAlc { get; set; }

        [StringLength(255)]
        public string CodErrorIndAlc { get; set; }

        [StringLength(255)]
        public string DescripcionErrorIndAlc { get; set; }

        [StringLength(255)]
        public string NumAcuerdoPVPMax { get; set; }

        public int? CodDescripcionPVPMax { get; set; }

        [StringLength(255)]
        public string DescripcionPVPMax { get; set; }

        public DateTime? FechaInicioPVPMax { get; set; }

        public DateTime? FechaFinPVPMax { get; set; }

        public decimal? ValorTotalPVPMax { get; set; }

        public decimal? PorcentajePVPMax { get; set; }

        [StringLength(255)]
        public string CodErrorPVPMax { get; set; }

        [StringLength(255)]
        public string DescripcionErrorPVPMax { get; set; }

        [StringLength(255)]
        public string NumAcuerdoDescPP { get; set; }

        public int? CodDescripcionDescPP { get; set; }

        [StringLength(255)]
        public string DescripcionDescPPx { get; set; }

        public DateTime? FechaInicioDescPP { get; set; }

        public DateTime? FechaFinDescPP { get; set; }

        public decimal? ValorTotalDescPP { get; set; }

        public decimal? PorcentajeDescPP { get; set; }

        [StringLength(255)]
        public string CodErrorDescPP { get; set; }

        [StringLength(255)]
        public string DescripcionErrorDescPP { get; set; }

        [StringLength(255)]
        public string NumAcuerdoDesWeb { get; set; }

        public int? CodDescripcionDesWeb { get; set; }

        [StringLength(255)]
        public string DescripcionDesWeb { get; set; }

        public DateTime? FechaInicioDesWeb { get; set; }

        public DateTime? FechaFinDesWeb { get; set; }

        public decimal? ValorTotalDesWeb { get; set; }

        public decimal? PorcentajeDesWeb { get; set; }

        [StringLength(255)]
        public string CodErrorDesWeb { get; set; }

        [StringLength(255)]
        public string DescripcionErrorDesWeb { get; set; }

        [StringLength(255)]
        public string NumAcuerdoPVP { get; set; }

        public int? CodDescripcionPVP { get; set; }

        [StringLength(255)]
        public string DescripcionPVP { get; set; }

        public DateTime? FechaInicioPVP { get; set; }

        public DateTime? FechaFinPVP { get; set; }

        public decimal? ValorTotalPVP { get; set; }

        public decimal? PorcentajePVP { get; set; }

        [StringLength(255)]
        public string CodErrorPVP { get; set; }

        [StringLength(255)]
        public string DescripcionErrorPVP { get; set; }

        [StringLength(255)]
        public string NumAcuerdoCuotaIni { get; set; }

        public int? CodDescripcionCuotaIni { get; set; }

        [StringLength(255)]
        public string DescripcionCuotaIni { get; set; }

        public DateTime? FechaInicioCuotaIni { get; set; }

        public DateTime? FechaFinCuotaIni { get; set; }

        public decimal? ValorTotalCuotaIni { get; set; }

        public decimal? PorcentajeCuotaIni { get; set; }

        [StringLength(255)]
        public string CodErrorCuotaIni { get; set; }

        [StringLength(255)]
        public string DescripcionErrorCuotaIni { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        [StringLength(100)]
        public string UsuarioActualizacion { get; set; }

        [StringLength(15)]
        public string IpActualizacion { get; set; }
    }
}
