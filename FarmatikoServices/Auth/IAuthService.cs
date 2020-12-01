using System;
using System.Collections.Generic;
using System.Text;

namespace FarmatikoServices.Auth
{
    public interface IAuthService
    {
        bool IsAnExistingUser(string userName);
        bool IsValidUserCredentials(string userName, string password);
        string GetUserRole(string userName);
    }
}
