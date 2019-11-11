using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HGS.WebAPI.Models
{
    public class Setting
    {
        public decimal CardPriceA { get; set; } = 30;
        public decimal CardPriceB { get; set; } = 37.5m;
        public decimal CardPriceC { get; set; } = 50;
    }
}