namespace IVTA018WS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_ContiGenPClasificador
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string CCODIGOMODULO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string CCODIGOGRUPO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(6)]
        public string CCODIGO { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string CESTADOREGISTRO { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(100)]
        public string CDESCRIPCION { get; set; }

        [StringLength(3)]
        public string CPREFIJO { get; set; }

        [StringLength(50)]
        public string CDESCRIPCIONCORTA { get; set; }

        [StringLength(1)]
        public string CCODIGOAUTOMATICO { get; set; }

        public DateTime? DFECHAINGRESO { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(25)]
        public string CUSUARIOINGRESO { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(30)]
        public string CTERMINALINGRESO { get; set; }

        public DateTime? DFECHAMODIFICACION { get; set; }

        [StringLength(25)]
        public string CUSUARIOMODIFICACION { get; set; }

        [StringLength(30)]
        public string CTERMINALMODIFICACION { get; set; }

        public int? ICODIGOSECUENCIA { get; set; }
    }
}
