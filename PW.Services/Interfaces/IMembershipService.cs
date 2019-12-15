using PW.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PW.Services.Interfaces
{
    public interface IMembershipService
    {
        ClaimsPrincipal GetUserClaimsPrincipal(PwUser user);
        Task CreateUserAsync(string username, string email, string password);
        Task<PwUser> GetUserAsync(string email, string password);
    }
}
