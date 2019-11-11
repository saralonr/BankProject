using HGS.WebAPI.CacheProvider;
using HGS.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace HGS.WebAPI.Controllers
{
    public class PaymentController : ApiController
    {
        ResponseModel response;
        public PaymentController()
        {
            response = new ResponseModel();
        }
        #region GET
        [HttpPost]
        public IHttpActionResult GetCardTypeList()
        {
            Setting st = new Setting();
            return Json(st);
        }

        [HttpPost]
        public IHttpActionResult GetBalanceFromTCKN(string TCKN)
        {
            return Json(CacheManager.HGSCardList.Where(x => x.TCKN == TCKN && x.Status).ToList());
        }
        [HttpPost]
        public IHttpActionResult GetBalanceFromCardNo(string CardNo)
        {
            return Json(CacheManager.HGSCardList.Where(x => x.CardNo == CardNo && x.Status).ToList());
        }
        #endregion
        #region POST
        [HttpPost]
        public IHttpActionResult BuyNewHGSCard(PurchaseRequest req)
        {
            try
            {
                if (CacheManager.PurchaseRequestList.Any(x => x.TCKN == req.TCKN && x.VehiclePlate == req.VehiclePlate))
                {
                    response.IsSuccess = false;
                    response.Message = "Kişi aynı araç için birden fazla HGS ürününe sahip olamaz";
                    return Json(response);
                }
                if (req.PaymentPrice<0)
                {
                    response.IsSuccess = false;
                    response.Message = "Geçersiz bir işlem yürütüldü.";
                    return Json(response);
                }
                CacheManager.PurchaseRequestList.Add(req);
                HGSCard newCard = new HGSCard()
                {
                    Balance = req.PaymentPrice,
                    CardNo = GenerateCardTokenOptimised(),
                    CreateDate = DateTime.Now,
                    GuidId = Guid.NewGuid(),
                    ModifyDate = DateTime.Now,
                    TCKN = req.TCKN,
                    VehiclePlate = req.VehiclePlate,
                    VehicleType = (int)req.VehicleType,
                    Status = true
                };
                CacheManager.HGSCardList.Add(newCard);
                CacheManager.PaymentList.Add(new Payment()
                {
                    Card = newCard,
                    PaymentDate = DateTime.Now,
                    PaymentType = req.PaymentType
                });

                response.IsSuccess = true;
                response.Message = $"Satın alma başarıyla gerçekleştirildi. Hesabınıza {req.PaymentPrice} TL bakiye yüklendi.";
                response.Data = newCard;
                return Json(response);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = "Girdiğiniz bilgilerin doğruluğunu kontrol ediniz.";
                return Json("");
            }
        }

        [HttpPost]
        public IHttpActionResult DepositHGSCard(DepositCard req)
        {
            try
            {
                if (!CacheManager.HGSCardList.Any(x=>x.CardNo==req.CardNo))
                {
                    response.IsSuccess = false;
                    response.Message = "Kart bilgisi bulunamadı.";
                    return Json(response);
                }
                if (req.PaymentPrice < 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Geçersiz bir işlem yürütüldü.";
                    return Json(response);
                }

                HGSCard card = CacheManager.HGSCardList.Find(x => x.CardNo == req.CardNo);
                card.Balance += req.PaymentPrice;

                CacheManager.PaymentList.Add(new Payment()
                {
                    Card = card,
                    PaymentDate = DateTime.Now,
                    PaymentType = req.PaymentType
                });

                response.IsSuccess = true;
                response.Message = $"Yükleme başarıyla gerçekleştirildi. Hesabınıza {req.PaymentPrice} TL bakiye yüklendi.";
                return Json(response);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = "Girdiğiniz bilgilerin doğruluğunu kontrol ediniz.";
                return Json(response);
            }
        }
        #endregion
        #region METHOD
        [NonAction]
        public string GenerateCardTokenOptimised()
        {
            Random rnd = new Random();
            int[] checkArray = new int[15];

            var cardNum = new int[16];

            for (int d = 14; d >= 0; d--)
            {
                cardNum[d] = rnd.Next(0, 9);
                checkArray[d] = (cardNum[d] * (((d + 1) % 2) + 1)) % 9;
            }

            cardNum[15] = (checkArray.Sum() * 9) % 10;

            var sb = new StringBuilder();

            for (int d = 0; d < 16; d++)
            {
                sb.Append(cardNum[d].ToString());
            }
            return sb.ToString();
        }

        #endregion

    }
}
