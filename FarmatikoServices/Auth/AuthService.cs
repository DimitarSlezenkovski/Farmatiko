using FarmatikoData.FarmatikoRepoInterfaces;
using FarmatikoData.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarmatikoServices.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IRepository _repository;

        private readonly IDictionary<string, string> _users = new Dictionary<string, string>();
        
        // inject your database here for user validation
        public AuthService(ILogger<AuthService> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public bool IsValidUserCredentials(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return _repository.GetUsers().TryGetValue(userName, out var p) && p.Password == password;
        }

        public bool IsAnExistingUser(string userName)
        {
            return _repository.GetUsers().ContainsKey(userName);
        }

        public string GetUserRole(string userName)
        {
            if (!IsAnExistingUser(userName))
            {
                return string.Empty;
            }

            var user = _repository.GetUsers().Where(x => x.Value.Email == userName).FirstOrDefault().Value.UserRole;

            /*if (userName == "admin@farmatiko.mk")
            {
                return nameof(user);
            }*/
            return user.ToString();
        }
    }

    public static class UserRoles
    {
        public const string Admin = nameof(User.Role.Admin);
        public const string PharmacyHead = nameof(User.Role.PharmacyHead);
    }
}

