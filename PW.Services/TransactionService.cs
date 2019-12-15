using PW.DataAccess.Interfaces;
using PW.Entities;
using PW.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Services
{
    public class TransactionService : ITransactionService
    {
        private const string TransactionSizeErrorMessage = "Transaction size exceeds the current balance";
        private const string SendSelfErrorMessage = "You can not send PW self";

        private ITransactionRepository _transactionRepository;
        private IUserRepository _userRepository;

        public TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        public async Task<PwTransaction> CreateTransactionAsync(string payeeEmail, string recipientEmail, int amount)
        {
            var payee = await _userRepository.GetSingleByEmailAsync(payeeEmail);
            var recipient = await _userRepository.GetSingleByEmailAsync(recipientEmail);

            ValidateCreation(payee, recipient, amount);

            payee.Balance -= amount;
            recipient.Balance += amount;

            var transaction = new PwTransaction
            {
                Payee = payee,
                Recipient = recipient,
                ResultingPayeeBalance = payee.Balance,
                ResultingRecipientBalance = recipient.Balance,
                Amount = amount,
                TransactionDateTime = DateTime.Now
            };

            await _transactionRepository.AddAsync(transaction);
            return transaction;
        }

        private void ValidateCreation(PwUser payee, PwUser recipient, int amount)
        {
            if (amount > payee.Balance)
            {
                throw new ArgumentException(TransactionSizeErrorMessage);
            }

            if (payee.Id == recipient.Id)
            {
                throw new ArgumentException(SendSelfErrorMessage);
            }
        }

        public async Task<IOrderedEnumerable<PwTransaction>> GetTransactionsOrderedByDateAsync(string email)
        {
            var result = (await GetTransactionsAsync(email)).OrderByDescending(t => t.TransactionDateTime);
            return result;
        }

        private async Task<IEnumerable<PwTransaction>> GetTransactionsAsync(string email)
        {
            var user = await _userRepository.GetSingleWithTransactionsByEmailAsync(email);
            var result = user.PayeeTransactions.Union(user.RecipientTransactions);
            return result;
        }

        public async Task<IOrderedEnumerable<PwTransaction>> GetPayeeTransactionsOrderedByDateAsync(string email)
        {
            var result = (await GetPayeeTransactionsAsync(email)).OrderByDescending(t => t.TransactionDateTime);
            return result;
        }

        public async Task<IEnumerable<PwTransaction>> GetPayeeTransactionsAsync(string email)
        {
            var user = await _userRepository.GetSingleWithTransactionsByEmailAsync(email);
            var result = user.PayeeTransactions;
            return result;
        }
    }
}
