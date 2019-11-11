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
    public class AccountController : Controller
    {
        // GET: Account
        #region Private Member
        private IAccountService _accountService;
        #endregion
        #region CTOR
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        #endregion
        #region ACTION
        public ActionResult Index()
        {
            var customerId = UserSession.Info.Id;
            List<Account> accounts = _accountService.GetAllAccounts(customerId);
            return View(accounts);
        }
        public ActionResult Transfer()
        {
            string accountId = Request.QueryString["Acc"];
            if (string.IsNullOrWhiteSpace(accountId))
            {
                ViewBag.EmptyAccount = 1;
                return View();
            }
            else
            {
                int accountIntId = Convert.ToInt32(accountId);
            
                TransferOutDTO dto = _accountService.GetTransferDTO(accountIntId);
                return View(dto);
            }
        }
        public ActionResult Virman()
        {
            string accountId = Request.QueryString["Acc"];
            if (string.IsNullOrWhiteSpace(accountId))
            {
                ViewBag.EmptyAccount = 1;
                return View();
            }
            else
            {
                int accountIntId = Convert.ToInt32(accountId);
                var customerId = UserSession.Info.Id;
                VirmanOutDTO dto = _accountService.GetVirmanDTO(customerId, accountIntId);
                return View(dto);
            }
        }
        public ActionResult NewAccount()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DeleteAccount(int Id)
        {
            DeleteAccountInDTO dto = new DeleteAccountInDTO();
            dto.CustomerId = UserSession.Info.Id;
            dto.AccountId = Id;
            DeleteAccountResultDTO result = _accountService.DeleteAccount(dto);
            if (result.IsSuccess)
            {
                TempData["Success"] = result.ResponseMessage;
                return Redirect($"/Account/Index");
            }
            else
            {
                TempData["Error"] = "Hatalı bir işlem yürütüldü.";
                return Redirect($"/Account/Index");
            }
        }
        #endregion
        #region POST
        [HttpPost]
        public ActionResult Transfer(TransferInDTO dto)
        {
            if (dto.FromAccountId == 0 || dto.FromBalance <= 0 || string.IsNullOrWhiteSpace(dto.ToAccountNo))
            {
                TempData["Error"] = "Girdiğiniz bilgileri kontrol ediniz.";
                return Redirect($"/Account/Transfer");
            }

            dto.CustomerId = UserSession.Info.Id;
            TransferResultDTO result = _accountService.TransferTransaction(dto);
            if (result.IsSuccess)
            {
                TempData["Success"] = result.ResponseMessage;
                return Redirect($"/Account/Index");
            }
            else
            {
                TempData["Error"] = "Girdiğiniz bilgileri kontrol ediniz.";
                return Redirect($"/Account/Transfer");
            }

            
        }
        [HttpPost]
        public ActionResult Virman(VirmanInDTO dto)
        {
            if (dto.FromAccountId == 0 || dto.FromBalance <= 0 || dto.ToAccountId == -1)
            {
                TempData["Error"] = "Girdiğiniz bilgileri kontrol ediniz.";
                return Redirect($"/Account/Virman");
            }

            dto.CustomerId = UserSession.Info.Id;
            TransferResultDTO result = _accountService.TransferTransaction(dto);
            if (result.IsSuccess)
            {
                TempData["Success"] = result.ResponseMessage;
                return Redirect($"/Account/Index");
            }
            else
            {
                TempData["Error"] = "Girdiğiniz bilgileri kontrol ediniz.";
                return Redirect($"/Account/Virman");
            }
        }
        [HttpPost]
        public ActionResult NewAccount(NewAccountInDTO dto)
        {
            dto.CustomerId = UserSession.Info.Id;
            NewAccountResultDTO result = _accountService.NewAccount(dto);
            if (result.IsSuccess)
            {
                TempData["Success"] = result.ResponseMessage;
                return Redirect($"/Account/Index");
            }
            else
            {
                TempData["Error"] = "Girdiğiniz bilgileri kontrol ediniz.";
                return Redirect($"/Account/NewAccount");
            }
        }
       
        #endregion
    }
}