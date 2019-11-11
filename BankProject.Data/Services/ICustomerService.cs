using BankProject.Data.DTO;
using BankProject.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject.Data.Services
{
    public interface ICustomerService
    {
        Customer LoginControl(LoginDTO dto);
        Customer CookieControl(LoginDTO dto);
        bool NewCustomer(Customer customer);
    }
}
