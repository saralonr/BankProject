using BankProject.Data.DTO;
using BankProject.Data.Model;
using BankProject.Data.Repository;
using BankProject.Data.Services;
using BankProject.Data.UnitOfWork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BankProject.Service
{
    public class ExternalService : IExternalService
    {
        #region PrivateMethods
        private string HttpPostRequest(string url, Dictionary<string, string> postParameters = null, Dictionary<string, string> headers = null)
        {
            string response = string.Empty;
            try
            {
                string baseURL = ConfigurationManager.AppSettings["HGSApiUrl"];
                string postData = "";

                if (postParameters != null)
                {
                    foreach (string key in postParameters.Keys)
                    {
                        postData += HttpUtility.UrlEncode(key) + "="
                              + HttpUtility.UrlEncode(postParameters[key]) + "&";
                    }
                }


                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(baseURL + url);
                myHttpWebRequest.Method = "POST";

                byte[] data = Encoding.ASCII.GetBytes(postData);

                if (headers != null)
                {
                    foreach (var headerItem in headers)
                    {
                        myHttpWebRequest.Headers.Add(headerItem.Key, headerItem.Value);
                    }
                }
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                Stream responseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);

                response = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                responseStream.Close();

                myHttpWebResponse.Close();
                return response;
            }
            catch (Exception ex)
            {
                return "Hatalı istek. -HGS API Services-";
            }
        }
        #endregion
        public HGSCard GetHGSCardById(int Id, int customerId)
        {
            HGSCard card = new HGSCard();
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<HGSCard> cardRepository = uow.GetRepository<HGSCard>();

                card = cardRepository.GetAll(x => x.Id == Id && x.CustomerId == customerId && x.Status == 1).FirstOrDefault();
                return card;
            }
        }
        public HGSCardTypeDTO GetCardTypes()
        {
            HGSCardTypeDTO dto = new HGSCardTypeDTO();
            string raw = HttpPostRequest($"/Payment/GetCardTypeList", null, null);
            dto = JsonConvert.DeserializeObject<HGSCardTypeDTO>(raw);

            return dto;
        }
        public List<HGSCard> GetHGSCardList(int customerId)
        {
            List<HGSCard> cards = new List<HGSCard>();
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<HGSCard> cardRepository = uow.GetRepository<HGSCard>();

                cards = cardRepository.GetAll(x => x.CustomerId == customerId && x.Status == 1).ToList();
                return cards;
            }
        }

        public HGSBalanceQueryResultDTO HGSBalanceQuery(string cardNo)
        {
            HGSCard card = new HGSCard();
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<HGSCard> cardRepository = uow.GetRepository<HGSCard>();
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CardNo", cardNo);
                string raw = HttpPostRequest($"/Payment/GetBalanceFromCardNo?CardNo={cardNo}", parameters, null);
                HGSBalanceQueryResultDTO dto = JsonConvert.DeserializeObject<HGSBalanceQueryResultDTO>(raw);

                card = cardRepository.GetAll(x => x.CardNo == cardNo && x.Status == 1).FirstOrDefault();
                card.Balance = dto.Balance;
                card.ModifyDate = DateTime.Now;
                uow.SaveChanges();

                return dto;
            }
        }

        public HGSPaymentResultDTO HGSPayment(HGSPaymentInDTO payment)
        {
            HGSPaymentResultDTO result = new HGSPaymentResultDTO();
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<HGSCard> cardRepository = uow.GetRepository<HGSCard>();
                IRepository<Account> accountRepository = uow.GetRepository<Account>();

                HGSCard card = cardRepository.GetAll(x => x.Id == payment.CardId && x.Status == 1).FirstOrDefault();
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CardNo", card.CardNo);
                parameters.Add("PaymentPrice", payment.Balance.ToString());
                parameters.Add("PaymentType", payment.PaymentType.ToString());
                parameters.Add("PaymentDate", payment.CreateDate.ToString());

                string raw = HttpPostRequest($"/Payment/DepositHGSCard", parameters, null);
                HGSResultDTO resultDTO = JsonConvert.DeserializeObject<HGSResultDTO>(raw);
                if (resultDTO.IsSuccess)
                {
                    card.Balance += payment.Balance;
                    card.ModifyDate = DateTime.Now;

                    Account acc = accountRepository.GetAll(x => x.Id == payment.AccountId && x.Status == 1).FirstOrDefault();
                    HGSCardTypeDTO cardType = GetCardTypes();
                    acc.Balance -= payment.Balance;

                    uow.SaveChanges();
                }

                result.IsSuccess = true;
                result.ResponseMessage = resultDTO.Message;
                return result;
            }
        }

        public NewHGSCardResultDTO NewHGSCard(NewHGSCardInDTO card)
        {
            NewHGSCardResultDTO result = new NewHGSCardResultDTO();
            using (BaseContext context = ContextFactory.Create())
            {
                IUnitOfWork uow = new BaseUnitOfWork(context);
                IRepository<Customer> customerRepository = uow.GetRepository<Customer>();
                IRepository<HGSCard> cardRepository = uow.GetRepository<HGSCard>();
                IRepository<Account> accountRepository = uow.GetRepository<Account>();

                Customer cus = customerRepository.GetAll(x => x.Id == card.CustomerId && x.Status == 1).FirstOrDefault();

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("TCKN", cus.TCKN);
                parameters.Add("VehiclePlate", card.VehiclePlate);
                parameters.Add("VehicleType", card.VehicleType.ToString());
                parameters.Add("PaymentPrice", card.Balance.ToString());
                parameters.Add("PaymentType", card.PaymentType.ToString());
                parameters.Add("RequestDate", card.RequestDate.ToString());

                string raw = HttpPostRequest($"/Payment/BuyNewHGSCard", parameters, null);
                result = JsonConvert.DeserializeObject<NewHGSCardResultDTO>(raw);
                if (result.IsSuccess)
                {
                    HGSCard newCard = new HGSCard();
                    newCard.Balance = card.Balance;
                    newCard.CardNo = result.Data.CardNo;
                    newCard.ModifyDate = DateTime.Now;
                    newCard.CreateDate = result.Data.CreateDate;
                    newCard.CustomerId = cus.Id;
                    newCard.Status = 1;
                    newCard.VehiclePlate = card.VehiclePlate;
                    newCard.VehicleType = card.VehicleType;
                    cardRepository.Add(newCard);

                    Account acc = accountRepository.GetAll(x => x.Id == card.AccountId && x.Status == 1).FirstOrDefault();
                    HGSCardTypeDTO cardType = GetCardTypes();
                    decimal totalPayment = card.Balance;
                    switch (card.VehicleType)
                    {
                        case 1:
                            totalPayment += cardType.CardPriceA;
                            break;
                        case 2:
                            totalPayment += cardType.CardPriceB;
                            break;
                        case 3:
                            totalPayment += cardType.CardPriceC;
                            break;
                        default:
                            break;
                    }

                    acc.Balance -= totalPayment;
                    uow.SaveChanges();
                }

                return result;
            }
        }
    }
}
