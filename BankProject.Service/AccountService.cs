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
    public class AccountService : IAccountService
    {
        
        public List<Account> GetAllAccounts(int customerId)
        {
            List<Account> accounts = new List<Account>();
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<Account> accountRepository = uow.GetRepository<Account>();

                accounts = accountRepository.GetAll(x => x.CustomerId == customerId && x.Status == 1).ToList();
                return accounts;
            }
        }

        public VirmanOutDTO GetVirmanDTO(int customerId, int accountId)
        {
            VirmanOutDTO dto = new VirmanOutDTO();
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<Account> accountRepository = uow.GetRepository<Account>();

                dto.Account = accountRepository.GetAll(x => x.Id == accountId && x.Status == 1).FirstOrDefault();
                dto.AccountList = accountRepository.GetAll(x => x.CustomerId == customerId && x.Status == 1 && x.Id != accountId).ToList();

                return dto;
            }
        }

        public TransferOutDTO GetTransferDTO(int accountId)
        {
            TransferOutDTO dto = new TransferOutDTO();
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<Account> accountRepository = uow.GetRepository<Account>();

                dto.Account = accountRepository.GetAll(x => x.Id == accountId && x.Status == 1).FirstOrDefault();

                return dto;
            }
        }
        public decimal GetAccountBalance(int accountId)
        {
            decimal balance = 0;
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<Account> accountRepository = uow.GetRepository<Account>();

                Account acc = accountRepository.GetAll(x => x.Id == accountId && x.Status == 1).FirstOrDefault();
                if (acc!=null)
                {
                    balance = (decimal)acc.Balance;
                }

                return balance;
            }
        }

        public NewAccountResultDTO NewAccount(NewAccountInDTO inDto)
        {
            NewAccountResultDTO dto = new NewAccountResultDTO();
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<Account> accountRepository = uow.GetRepository<Account>();

                var acc = accountRepository.GetAll(x => x.CustomerId == inDto.CustomerId).ToList();
                short newAcc;
                if (acc.Any())
                {
                    newAcc = Convert.ToInt16(acc.Max(x => x.AccountNo) + 1);
                }
                else
                {
                    newAcc = 1001;
                }

                accountRepository.Add(new Account()
                {
                    AccountNo = newAcc,
                    Balance = inDto.Balance,
                    CreateDate = DateTime.Now,
                    Status=1,
                    CustomerId= inDto.CustomerId
                });
                int result =uow.SaveChanges();
                if (result>0)
                {
                    dto.IsSuccess = true;
                    dto.ResponseMessage = "Hesap başarıyla oluşturuldu.";
                }
                else
                {
                    dto.IsSuccess = false;
                    dto.ResponseMessage = "Geçersiz bir işlem yürütüldü. Lütfen bilgileri kontrol ederek tekrar deneyiniz.";
                }
                return dto;
            }
        }
        public DeleteAccountResultDTO DeleteAccount(DeleteAccountInDTO dto)
        {
            DeleteAccountResultDTO result = new DeleteAccountResultDTO();
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<Account> accountRepository = uow.GetRepository<Account>();

                Account acc = accountRepository.GetAll(x => x.Id == dto.AccountId && x.CustomerId==dto.CustomerId && x.Status == 1).FirstOrDefault();
                if (acc==null)
                {
                    result.IsSuccess = false;
                    result.ResponseMessage = "Geçersiz bir işlem yürütüldü. Lütfen bilgileri kontrol ederek tekrar deneyiniz.";
                }
                else
                {
                    acc.Status = 0;
                    accountRepository.Update(acc);
                    uow.SaveChanges();

                    result.IsSuccess = true;
                    result.ResponseMessage = "Hesap başarıyla silindi.";
                }

                return result;
            }
        }

        public TransferResultDTO TransferTransaction(VirmanInDTO dto)
        {
            TransferResultDTO result = new TransferResultDTO();
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<Account> accountRepository = uow.GetRepository<Account>();

                var fromAccount = accountRepository.GetAll(x => x.Id == dto.FromAccountId && x.CustomerId==dto.CustomerId && x.Status == 1).FirstOrDefault();
                var toAccount = accountRepository.GetAll(x => x.Id == dto.ToAccountId && x.Status == 1).FirstOrDefault();

                if (fromAccount == null || toAccount == null)
                {
                    result.IsSuccess = false;
                    result.ResponseMessage = "Geçersiz bir işlem yürütüldü.";
                }
                else if (fromAccount.Balance < dto.FromBalance)
                {
                    result.IsSuccess = false;
                    result.ResponseMessage = "Hesabınızda yeteri kadar bakiye bulunmamaktadır.";
                }
                else if (fromAccount.CustomerId!=dto.CustomerId)
                {
                    result.IsSuccess = false;
                    result.ResponseMessage = "Fraud işlem algılandı. İşlem iptal edildi.";
                }
                else
                {
                    fromAccount.Balance -= dto.FromBalance;
                    toAccount.Balance += dto.FromBalance;
                    uow.SaveChanges();

                    result.IsSuccess = true;
                    result.ResponseMessage = "Transfer başarıyla gerçekleştirildi.";
                }

                return result;
            }
        }

        public TransferResultDTO TransferTransaction(TransferInDTO dto)
        {
            TransferResultDTO result = new TransferResultDTO();
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<Account> accountRepository = uow.GetRepository<Account>();
                IRepository<Customer> customerRepository = uow.GetRepository<Customer>();

                var fromAccount = accountRepository.GetAll(x => x.Id == dto.FromAccountId && x.CustomerId==dto.CustomerId && x.Status == 1).FirstOrDefault();
                var accountNo = dto.ToAccountNo.Substring(0, 4);
                var accountIntNo = Convert.ToInt32(accountNo);
                var customerNo = dto.ToAccountNo.Substring(4, 5);
                var customerIntNo = Convert.ToInt32(customerNo);

                var toCustomer = customerRepository.GetAll(x => x.CustomerNo == customerIntNo && x.Status == 1).FirstOrDefault();
                var toAccount = accountRepository.GetAll(x => x.AccountNo == accountIntNo && x.CustomerId== toCustomer.Id && x.Status == 1).FirstOrDefault();

                if (fromAccount == null || toAccount == null)
                {
                    result.IsSuccess = false;
                    result.ResponseMessage = "Geçersiz bir işlem yürütüldü. Girdiğiniz bilgileri kontrol ediniz.";
                }
                else if (fromAccount.Balance < dto.FromBalance)
                {
                    result.IsSuccess = false;
                    result.ResponseMessage = "Hesabınızda yeteri kadar bakiye bulunmamaktadır.";
                }
                else if (fromAccount.CustomerId != dto.CustomerId)
                {
                    result.IsSuccess = false;
                    result.ResponseMessage = "Fraud işlem algılandı. İşlem iptal edildi.";
                }
                else
                {
                    fromAccount.Balance -= dto.FromBalance;
                    toAccount.Balance += dto.FromBalance;
                    uow.SaveChanges();

                    result.IsSuccess = true;
                    result.ResponseMessage = "Transfer başarıyla gerçekleştirildi.";
                }

                return result;
            }
        }
    }
}
