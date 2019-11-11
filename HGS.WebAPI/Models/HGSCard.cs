using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HGS.WebAPI.Models
{
    public class HGSCard
    {
        public int Id { get; set; }
        public Guid GuidId { get; set; }
        public string CardNo { get; set; }
        public decimal Balance { get; set; }
        public string TCKN { get; set; }
        public string VehiclePlate { get; set; }
        public int VehicleType { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool Status { get; set; }
    }
}