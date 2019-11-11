using HGS.WebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HGS.WebAPI.Models
{
    public class Payment
    {
        public int CardId { get; set; }
        public HGSCard Card { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentEnum PaymentType { get; set; }
    }
}