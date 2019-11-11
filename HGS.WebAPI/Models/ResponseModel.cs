using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HGS.WebAPI.Models
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}