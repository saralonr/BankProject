using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject.Data.DTO
{
    public class TransferInDTO
    {
        public decimal FromBalance { get; set; }
        public int FromAccountId { get; set; }
        public string ToAccountNo { get; set; }
        public int CustomerId { get; set; }
    }
}
