using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BankProject.WebUI.Helpers
{
    public class DataHelpers
    {
        public static string GetBankName()
        {
            string bankName = "NoNameBank";
            bankName = ConfigurationManager.AppSettings["BankName"];
            return bankName;
        }
        public static bool TCKNCheck(string tckn)
        {
            string kimlikno = tckn;
            kimlikno = kimlikno.Trim();
            if (kimlikno.Length != 11)
            {
                return false;
            }
            int[] arr = new int[11];
            for (int i = 0; i < kimlikno.Length; i++)
            {
                arr[i] = Int32.Parse(kimlikno[i].ToString());
            }
            int toplam = 0;
            for (int i = 0; i < kimlikno.Length - 1; i++)
            {
                toplam += arr[i];
            }
            if (toplam.ToString()[1].ToString() == arr[10].ToString() & arr[10] % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}