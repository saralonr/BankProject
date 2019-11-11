using BankProject.Data.DTO;
using BankProject.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject.Data.Services
{
    public interface IExternalService
    {
        HGSCardTypeDTO GetCardTypes();
        List<HGSCard> GetHGSCardList(int customerId);
        HGSCard GetHGSCardById(int Id, int customerId);
        NewHGSCardResultDTO NewHGSCard(NewHGSCardInDTO card);
        HGSPaymentResultDTO HGSPayment(HGSPaymentInDTO dto);
        HGSBalanceQueryResultDTO HGSBalanceQuery(string cardNo);
    }
}
