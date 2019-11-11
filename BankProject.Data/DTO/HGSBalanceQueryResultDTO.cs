using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject.Data.DTO
{
    public class HGSBalanceQueryResultDTO
    {
        public string CardNo { get; set; }
        public decimal Balance { get; set; }
        public string TCKN { get; set; }
        public string VehiclePlate { get; set; }
        public int VehicleType { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
