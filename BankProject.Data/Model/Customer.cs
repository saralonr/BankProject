namespace BankProject.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(50)]
        public string Lastname { get; set; }

        [Required]
        [StringLength(11)]
        public string TCKN { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }
        public int CustomerNo { get; set; }
        public Guid? SecretKey { get; set; }

        public DateTime? ModifyDate { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? Status { get; set; }
    }
}
