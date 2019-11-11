using BankProject.Data.DTO;
using BankProject.Data.Model;
using BankProject.Data.Services;
using BankProject.WebUI.Attributes;
using BankProject.WebUI.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankProject.WebUI.Controllers
{
    [Auth]
    public class HGSController : Controller
    {
        #region Private Member
        private IExternalService _externalService;
        private IAccountService _accountService;
        #endregion
        #region CTOR
        public HGSController(IExternalService externalService, IAccountService accountService)
        {
            _externalService = externalService;
            _accountService = accountService;
        }
        #endregion
        #region ACTION
        public ActionResult Index()
        {
            var customerId = UserSession.Info.Id;
            List<HGSCard> cards = _externalService.GetHGSCardList(customerId);
            return View(cards);
        }
        public ActionResult Detail(int cardId)
        {
            var customerId = UserSession.Info.Id;
            ViewBag.Accounts = _accountService.GetAllAccounts(customerId);
            HGSCard card = _externalService.GetHGSCardById(cardId, customerId);
            return View(card);
        }
        public ActionResult NewCard()
        {
            var customerId = UserSession.Info.Id;
            ViewBag.Accounts = _accountService.GetAllAccounts(customerId);
            return View();
        }
        #endregion
        #region POST
        [HttpPost]
        public ActionResult NewCard(NewHGSCardInDTO dto)
        {
            try
            {
                if (dto.Balance < 0 || dto.VehicleType == 0 || string.IsNullOrWhiteSpace(dto.VehiclePlate))
                {
                    TempData["Error"] = "Girdiğiniz bilgileri kontrol ediniz.";
                    return Redirect($"/Hgs/NewCard");
                }

                HGSCardTypeDTO card = _externalService.GetCardTypes();
                decimal customerBalance = _accountService.GetAccountBalance(dto.AccountId);
                decimal totalPayment = dto.Balance;
                switch (dto.VehicleType)
                {
                    case 1:
                        totalPayment += card.CardPriceA;
                        break;
                    case 2:
                        totalPayment += card.CardPriceB;
                        break;
                    case 3:
                        totalPayment += card.CardPriceC;
                        break;
                    default:
                        break;
                }
                if (customerBalance < totalPayment)
                {
                    TempData["Error"] = "Hesabınızda yeterli bakiye yok.";
                    return Redirect($"/Hgs/NewCard");
                }

                dto.CustomerId = UserSession.Info.Id;
                dto.PaymentType = 1;
                dto.RequestDate = DateTime.Now;

                NewHGSCardResultDTO result = _externalService.NewHGSCard(dto);
                if (result.IsSuccess)
                {
                    TempData["Success"] = result.Message;
                    return Redirect($"/Hgs/Index");
                }
                else
                {
                    TempData["Error"] = "Girdiğiniz bilgileri kontrol ediniz.";
                    return Redirect($"/Hgs/NewCard");
                }
            }
            catch (Exception ex)
            {

                TempData["Error"] = "Girdiğiniz bilgileri kontrol ediniz.";
                return Redirect($"/Hgs/NewCard");
            }
        }
        [HttpPost]
        public ActionResult CardPayment(HGSPaymentInDTO dto)
        {
            try
            {
                if (dto.Balance < 0)
                {
                    TempData["Error"] = "Girdiğiniz bilgileri kontrol ediniz.";
                    return Redirect($"/Hgs/Detail/{dto.CardId}");
                }

                var customerId = UserSession.Info.Id;
                HGSCard card = _externalService.GetHGSCardById(dto.CardId, customerId);
                decimal customerBalance = _accountService.GetAccountBalance(dto.AccountId);

                if (customerBalance < dto.Balance)
                {
                    TempData["Error"] = "Hesabınızda yeterli bakiye yok.";
                    return Redirect($"/Hgs/Detail/{dto.CardId}");
                }

                dto.VehicleType = card.VehicleType;
                dto.CustomerId = customerId;
                dto.CreateDate = DateTime.Now;
                dto.PaymentType = 1;

                HGSPaymentResultDTO result = _externalService.HGSPayment(dto);
                if (result.IsSuccess)
                {
                    TempData["Success"] = result.ResponseMessage;
                    return Redirect($"/Hgs/Index");
                }
                else
                {
                    TempData["Error"] = "Girdiğiniz bilgileri kontrol ediniz.";
                    return Redirect($"/Hgs/Detail/{dto.CardId}");
                }
            }
            catch (Exception)
            {

                TempData["Error"] = "Girdiğiniz bilgileri kontrol ediniz.";
                return Redirect($"/Hgs/Index");
            }
        }
        #endregion
    }
}