namespace IVTA018WS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContiProceso")]
    public partial class ContiProceso
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string NombreProceso { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(15)]
        public string EstDescarga { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(15)]
        public string EstEnvio { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime FechaHoraCreacion { get; set; }

        public DateTime? FechaHoraModificacion { get; set; }
    }
}
