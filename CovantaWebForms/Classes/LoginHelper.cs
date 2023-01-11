
using CovantaWebForms.Classes.Helpers;
using System;

namespace CovantaWebForms.Classes
{
    public static class LoginHelper
    {
        public static bool VerifyUser(string email, string password, out string name)
        {
            name = string.Empty;
            FormsUser user = DataHelper.GetUserByEmail(email);
            if (user.Email == email && user.Password == password)
            {
                name = user.Name;
                return true;
            }
            return false;
        }
    }
}