using HGS.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HGS.WebAPI.CacheProvider
{
    public class CacheManager
    {
        public static List<HGSCard> HGSCardList { get; set; } = new List<HGSCard>();
        public static List<Payment> PaymentList { get; set; } = new List<Payment>();
        public static List<PurchaseRequest> PurchaseRequestList { get; set; } = new List<PurchaseRequest>();

        public static void CacheClean()
        {
            CacheManager.HGSCardList = new List<HGSCard>();
            CacheManager.PaymentList = new List<Payment>();
            CacheManager.PurchaseRequestList = new List<PurchaseRequest>();
        }
    }
}