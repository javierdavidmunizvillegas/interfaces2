namespace IVTA018WS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContiParametro")]
    public partial class ContiParametro
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(255)]
        public string CIDGRUPOPARAMETRO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string CIDPARAMETRO { get; set; }

        [Required]
        [StringLength(255)]
        public string CDESCRIPCION { get; set; }

        [Required]
        [StringLength(255)]
        public string CVALOR { get; set; }

        [Required]
        [StringLength(1)]
        public string CESTADOREGISTRO { get; set; }

        public DateTime DFECHAINGRESO { get; set; }

        [Required]
        [StringLength(25)]
        public string CUSUARIOINGRESO { get; set; }

        [StringLength(30)]
        public string CTERMINALINGRESO { get; set; }

        public DateTime? DFECHAMODIFICACION { get; set; }

        [StringLength(25)]
        public string CUSUARIOMODIFICACION { get; set; }

        [StringLength(30)]
        public string CTERMINALMODIFICACION { get; set; }
    }
}
