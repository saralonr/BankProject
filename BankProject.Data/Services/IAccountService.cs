using BankProject.Data.DTO;
using BankProject.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject.Data.Services
{
    public interface IAccountService
    {
        decimal GetAccountBalance(int accountId);
        List<Account> GetAllAccounts(int customerId);
        VirmanOutDTO GetVirmanDTO(int customerId, int accountId);
        TransferOutDTO GetTransferDTO(int accountId);
        TransferResultDTO TransferTransaction(TransferInDTO dto);
        TransferResultDTO TransferTransaction(VirmanInDTO dto);
        NewAccountResultDTO NewAccount(NewAccountInDTO dto);
        DeleteAccountResultDTO DeleteAccount(DeleteAccountInDTO dto);
    }
}
