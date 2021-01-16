using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.Utility
{
    public class Utilities
    {
        public static bool CheckMajority(DateTime birthDate)
        {
            bool result = false;

            if (DateTime.Now.AddYears(-18).CompareTo(birthDate) == 1)
                result = true;

            return result;
        }

        public static bool CheckRegistrationAge(DateTime birthDate)
        {
            bool result = false;

            System.Diagnostics.Debug.WriteLine(DateTime.Now.AddYears(-100).CompareTo(birthDate));

            if (DateTime.Now.AddYears(-13).CompareTo(birthDate) == 1 && DateTime.Now.AddYears(-100).CompareTo(birthDate) == -1)
                result = true;

            return result;
        }
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}