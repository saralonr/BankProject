using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject.Data.DTO
{
    public class LoginDTO
    {
        public string TCKN { get; set; }
        public string Password { get; set; }

        public Guid SecretKey { get; set; }
    }
}
