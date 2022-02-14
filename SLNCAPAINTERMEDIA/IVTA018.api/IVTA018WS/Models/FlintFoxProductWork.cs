namespace IVTA018WS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FlintFoxProductWork")]
    public partial class FlintFoxProductWork
    {
        [Key]
        public long RegisterID { get; set; }

        [StringLength(4000)]
        public string APDataAreaId { get; set; }

        [StringLength(4000)]
        public string ItemId { get; set; }

        [StringLength(50)]
        public string inventSerialId { get; set; }
    }
}
