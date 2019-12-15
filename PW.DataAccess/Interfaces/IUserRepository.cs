using PW.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PW.DataAccess.Interfaces
{
    public interface IUserRepository : IBaseRepository<PwUser>
    {
        Task<PwUser> GetSingleByEmailAsync(string email);
        Task<PwUser> GetSingleWithTransactionsByEmailAsync(string email);
    }
}
