using BankProject.Data.DTO;
using BankProject.Data.Model;
using BankProject.Data.Repository;
using BankProject.Data.Services;
using BankProject.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject.Service
{
    public class CustomerService : ICustomerService
    {
        public Customer LoginControl(LoginDTO dto)
        {
            Customer customer = null;
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<Customer> customerRepository = uow.GetRepository<Customer>();

                string encryptedPass = CryptoService.ConvertToMD5(dto.Password);
                customer = customerRepository.GetAll(x => x.Password == encryptedPass && x.TCKN == dto.TCKN && x.Status == 1).FirstOrDefault();
                return customer;
            }
        }
        public Customer CookieControl(LoginDTO dto)
        {
            Customer customer = null;
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<Customer> customerRepository = uow.GetRepository<Customer>();

                customer = customerRepository.GetAll(x => x.SecretKey == dto.SecretKey && x.TCKN == dto.TCKN && x.Status == 1).FirstOrDefault();
                return customer;
            }
        }

        public bool NewCustomer(Customer customer)
        {
            bool result = false;
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<Customer> customerRepository = uow.GetRepository<Customer>();

                string encryptedPass = CryptoService.ConvertToMD5(customer.Password);
                Customer _customer = customerRepository.GetAll(x => x.TCKN == customer.TCKN && x.Status == 1).FirstOrDefault();
                if (_customer != null)
                {
                    result = false;
                }
                else
                {
                    int acc = customerRepository.GetAll().Max(x => x.CustomerNo);
                    if (acc==0)
                    {
                        acc = 20001;
                    }
                    else
                    {
                        acc += 1;
                    }
                    Guid secret = Guid.NewGuid();
                    customerRepository.Add(new Customer()
                    {
                        Address = customer.Address,
                        CreateDate = DateTime.Now,
                        Firstname = customer.Firstname,
                        Lastname = customer.Lastname,
                        Password = encryptedPass,
                        Phone = customer.Phone,
                        SecretKey = secret,
                        Status = 1,
                        TCKN = customer.TCKN,
                        CustomerNo = acc
                    });
                    uow.SaveChanges();
                    result = true;
                }
            }
            return result;
        }
    }
}
