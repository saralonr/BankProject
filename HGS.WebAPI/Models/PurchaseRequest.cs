using HGS.WebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HGS.WebAPI.Models
{
    public class PurchaseRequest
    {
        public string TCKN { get; set; }
        public string VehiclePlate { get; set; }
        public VehicleEnum VehicleType { get; set; }
        public decimal PaymentPrice { get; set; }
        public PaymentEnum PaymentType { get; set; }
        public DateTime RequestDate { get; set; }
    }
}