using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MenU.Services
{
    abstract class Validation
    {
        public const string INVALID_CHARS = "&_<>'-+,=";
        public const string INVALID_PASSWORD_CHARS = "@#$%^&+=";
        public static bool IsUsername(string username)
        {
            if (username == null)
                return false;
            bool length = username.Length > 0;
            bool validCharacters = true;
            foreach(char c in INVALID_CHARS)
            {
                if (username.Contains(c.ToString()))
                    validCharacters = false;
            }
            validCharacters = validCharacters && !username.Contains("..");
            bool validEdgeChar = !username.StartsWith(".") && !username.EndsWith(".");
            return length && validCharacters && validEdgeChar;

        }

        public static bool Ispassword(string password)
        {
            if (password == null)
                return false;
            int validConditions = 0;
            foreach (char c in password)
            {
                if (c >= 'a' && c <= 'z')
                {
                    validConditions++;
                    break;
                }
            }
            foreach (char c in password)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 0) return false;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 1) return false;
            if (validConditions == 2)
            {
                char[] special = INVALID_PASSWORD_CHARS.ToCharArray();   
                if (password.IndexOfAny(special) == -1) return false;
            }
            return true;
        }

        public static bool IsEmail(string email)
        {
            if (email == null)
                return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsOfAge(DateTime birthday)
        {
            if (birthday == null)
                return false;
            double age = (DateTime.Now - birthday).Days / 365;
            return age >= 16;
        }
    }
}
