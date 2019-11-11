using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HGSCard")]
    public partial class HGSCard
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [Required]
        [StringLength(50)]
        public string CardNo { get; set; }
        public decimal Balance { get; set; }
        [Required]
        [StringLength(50)]
        public string VehiclePlate { get; set; }
        public int VehicleType { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? Status { get; set; }
    }
}
