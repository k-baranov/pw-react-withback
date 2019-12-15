using PW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<PwTransaction> CreateTransactionAsync(string payeeEmail, string recipientEmail, int amount);
        Task<IOrderedEnumerable<PwTransaction>> GetTransactionsOrderedByDateAsync(string email);
        Task<IOrderedEnumerable<PwTransaction>> GetPayeeTransactionsOrderedByDateAsync(string email);
        Task<IEnumerable<PwTransaction>> GetPayeeTransactionsAsync(string email);
    }
}
