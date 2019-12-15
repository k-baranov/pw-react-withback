using Microsoft.Extensions.Configuration;
using PW.DataAccess.Interfaces;
using PW.Entities;
using PW.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PW.Services
{
    public class MembershipService : IMembershipService
    {
        private const string InvalidUsernameOrPasswordMessage = "Invalid username or password";
        private const string EmailIsAlreadyInUseMessage = "Email is already in use";
        private const string CanNotGetStartBalanceMessage = "Can not get start balance from config";

        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;

        public MembershipService(IConfiguration configuration, IUserRepository userRepository, IEncryptionService encryptionService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        public ClaimsPrincipal GetUserClaimsPrincipal(PwUser user)
        {
            ClaimsPrincipal claimsPrincipal = null;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }

        public async Task<PwUser> GetUserAsync(string email, string password)
        {
            var user = await _userRepository.GetSingleByEmailAsync(email);
            if (user == null || !IsUserValid(user, password))
            {
                throw new ArgumentException(InvalidUsernameOrPasswordMessage);
            }
            return user;
        }

        public async Task CreateUserAsync(string username, string email, string password)
        {
            var existingUser = await _userRepository.GetSingleByEmailAsync(email);

            if (existingUser != null)
            {
                throw new ArgumentException(EmailIsAlreadyInUseMessage);
            }

            var passwordSalt = _encryptionService.CreateSalt();
            int startBalance;
            try
            {
                startBalance = int.Parse(_configuration.GetSection("User:StartBalance").Value);
            }
            catch (Exception)
            {
                throw new Exception(CanNotGetStartBalanceMessage);
            }

            var user = new PwUser()
            {
                UserName = username,
                Salt = passwordSalt,
                Email = email,
                PasswordHash = _encryptionService.EncryptPassword(password, passwordSalt),
                Balance = startBalance
            };

            await _userRepository.AddAsync(user);            
        }

        private bool IsPasswordValid(PwUser user, string password)
        {
            return string.Equals(_encryptionService.EncryptPassword(password, user.Salt), user.PasswordHash);
        }

        private bool IsUserValid(PwUser user, string password)
        {
            var result = IsPasswordValid(user, password);
            return result;
        }
    }
}
