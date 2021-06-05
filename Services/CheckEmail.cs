using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestForGraduates.Services
{
    class CheckEmail
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }

    #region CheckUserName
    //public class CheckUserName
    //{
    //    private char[] ForbiddenCharacker =
    //    {
    //        '1','2','3'
    //    };
    //    public bool CheckName(string name)
    //    {
    //        var passwordChar = name.ToCharArray();
            
    //        for (int i = 0; i < passwordChar.Length; i++)
    //        {
    //            for (int j = 0; j < ForbiddenCharacker.Length; j++)
    //            {
    //                if (passwordChar[i] == ForbiddenCharacker[j])
    //                    return false;
    //            }
    //        }
    //        return true;
    //    }
    //}
    #endregion
}
