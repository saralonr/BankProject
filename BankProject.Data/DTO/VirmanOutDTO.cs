using BankProject.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject.Data.DTO
{
   public class VirmanOutDTO
    {
        public VirmanOutDTO()
        {
            this.AccountList = new List<Account>();
        }
        public List<Account> AccountList { get; set; }
        public Account Account { get; set; }
    }
}
