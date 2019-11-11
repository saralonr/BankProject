using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject.Data.DTO
{
    public class HGSPaymentInDTO
    {
        public int AccountId { get; set; }
        public int CardId { get; set; }
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }
        public int VehicleType { get; set; }
        public int PaymentType { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
