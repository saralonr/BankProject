using HGS.WebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HGS.WebAPI.Models
{
    public class DepositCard
    {
        public string CardNo { get; set; }
        public decimal PaymentPrice { get; set; }
        public PaymentEnum PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}