using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using System.Text.RegularExpressions;
namespace UserRegistration
{
    public class Tools
    {
        public static string cretaeHash256Password(string password)
        {
            
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Get the hashed string.  
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }

        public static string GetSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
        public static string GenerateUniqueKey()
        {
            string key = GenerateOTP();
            key = cretaeHash256Password(key);
            return key;
        }
        public static bool IsValidEmailID(string emailID)
        {
            try
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (re.IsMatch(emailID))
                {
                    return true;
                }

                return false;
            }

            catch (Exception exception)
            {
                throw exception;
            }
        }
        public static string GenerateOTP()
        {
           string numbers = "1234567890";

            string characters = numbers;
            
            int length = 5 ;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
        public static int ConvertToNumeric(object value)
        {
            int retVal = 0;
            if (IsNumeric(value))
            {
                retVal = Convert.ToInt32(value);
            }
            return retVal;
        }
        public static bool IsNumeric(object value)
        {
            long lTemp;
            bool bVal = true;
            try
            {
                lTemp = System.Convert.ToInt64(value);
            }
            catch
            {
                bVal = false;
            }

            return bVal;
        }
    }
}
