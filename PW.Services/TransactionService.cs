using AutoMapper;
using PW.DataAccess.Interfaces;
using PW.DataTransferObjects.Transactions;
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
        private IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateTransactionAsync(string payeeEmail, CreateTransactionDto createTransactionDto)
        {
            var payee = await _userRepository.GetByEmailAsync(payeeEmail);
            var recipient = await _userRepository.GetByEmailAsync(createTransactionDto.UserName);

            ValidateCreation(payee, recipient, createTransactionDto.Amount);

            payee.Balance -= createTransactionDto.Amount;
            recipient.Balance += createTransactionDto.Amount;

            var transaction = new PwTransaction
            {
                Payee = payee,
                Recipient = recipient,
                ResultingPayeeBalance = payee.Balance,
                ResultingRecipientBalance = recipient.Balance,
                Amount = createTransactionDto.Amount,
                TransactionDateTime = DateTime.Now
            };

            await _transactionRepository.AddAsync(transaction);            
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

        public async Task<IOrderedEnumerable<TransactionDto>> GetTransactionsOrderedByDateAsync(string email)
        {
            var result = (await GetTransactionsAsync(email)).OrderByDescending(t => t.Date);
            return result;
        }

        private async Task<IEnumerable<TransactionDto>> GetTransactionsAsync(string email)
        {
            var user = await _userRepository.GetWithTransactionsByEmailAsync(email);
            var payeeDtos = user.PayeeTransactions.Select(t => {
                var transactionDto = _mapper.Map<TransactionDto>(t);
                transactionDto.CorrespondentName = t.Recipient.UserName;
                transactionDto.Amount = -t.Amount;
                transactionDto.ResultBalance = t.ResultingPayeeBalance;
                return transactionDto;
            });
            var recipientDtos = user.RecipientTransactions.Select(t => {
                var transactionDto = _mapper.Map<TransactionDto>(t);
                transactionDto.CorrespondentName = t.Payee.UserName;
                transactionDto.Amount = t.Amount;
                transactionDto.ResultBalance = t.ResultingRecipientBalance;
                return transactionDto;
            });

            var result = payeeDtos.Union(recipientDtos);
            return result;
        }

        //public async Task<IOrderedEnumerable<PwTransaction>> GetPayeeTransactionsOrderedByDateAsync(string email)
        //{
        //    var result = (await GetPayeeTransactionsAsync(email)).OrderByDescending(t => t.TransactionDateTime);
        //    return result;
        //}

        //public async Task<IEnumerable<PwTransaction>> GetPayeeTransactionsAsync(string email)
        //{
        //    var user = await _userRepository.GetSingleWithTransactionsByEmailAsync(email);
        //    var result = user.PayeeTransactions;
        //    return result;
        //}
    }
}
