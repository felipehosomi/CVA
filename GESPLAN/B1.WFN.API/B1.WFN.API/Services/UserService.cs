using B1.WFN.API.Infrastructure;
using B1.WFN.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace B1.WFN.API.Services
{
    public interface IUserService
    {
        bool IsValidUser(string userName, string password);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IOptions<BasicAuthUser> _user;
        // inject database for user validation
        public UserService(ILogger<UserService> logger, IOptions<BasicAuthUser> user)
        {
            _logger = logger;
            _user = user;
        }

        public bool IsValidUser(string userName, string password)
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
            if (Crypto.Decrypt(_user.Value.UserName) == userName && Crypto.Decrypt(_user.Value.Password) == password)
            {
                return true;
            }

            return false;
        }
    }
}