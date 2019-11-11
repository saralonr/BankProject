namespace BankProject.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public decimal? Balance { get; set; }

        public short AccountNo { get; set; }

        public DateTime? ModifyDate { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? Status { get; set; }
    }
}
