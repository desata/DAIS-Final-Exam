using Exam.Repository.Interfaces.BankAccount;
using Exam.Repository.Interfaces.User;
using Exam.Repository.Interfaces.UsersBankAccount;
using Exam.Services.DTOs.BankAccount;
using Exam.Services.Interfaces.BankAccount;
using System.ComponentModel.DataAnnotations;

namespace Exam.Services.Implementation.BankAccount
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUsersBankAccountRepository _usersBankAccountRepository;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IUserRepository userRepository, IUsersBankAccountRepository usersBankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
            _userRepository = userRepository;
            _usersBankAccountRepository = usersBankAccountRepository;
        }


        public async Task<BankAccountInfo> GetBalanceAsync(int BankAccountId)
        {
            var bankAccount = await _bankAccountRepository.RetrieveAsync(BankAccountId);

            if (bankAccount == null)
            {
                throw new KeyNotFoundException($"Bank account with ID {BankAccountId} not found.");
            }
            return new BankAccountInfo
            {
                BankAccountId = bankAccount.BankAccountId,
                IBAN = bankAccount.IBAN,
                Balance = bankAccount.Balance,
            };
        }

        public async Task<List<BankAccountInfo>> GetBankAccountsByUserIdAsync(int userId)
        {
            var allBankAccounts = await _bankAccountRepository.RetrieveCollectionAsync(new BankAccountFilter()).ToListAsync();
            var userBankAccountIds = await _usersBankAccountRepository.RetrieveCollectionAsync(new UsersBankAccountFilter { UserId = userId }).ToListAsync();

            var userBankAccounts = allBankAccounts
                .Where(account => userBankAccountIds
                .Any(link => link.BankAccountId == account.BankAccountId))
                .ToList();

            var userBankAccountInfos = new List<BankAccountInfo>();

            foreach (var account in userBankAccounts)
            {
                userBankAccountInfos.Add(MapBankAccountInfo(account));
            }
            return userBankAccountInfos;
        }

        public async Task<UpdateBalanceResponse> UpdateBalanceAsync(UpdateBalanceRequest request)
        {
            try
            {
                if (request.NewBalance <= 0)
                    throw new ValidationException("NewBalance cannot be zero or less");

                var update = new BankAccountUpdate
                {
                    Balance = request.NewBalance
                };

                var success = await _bankAccountRepository.UpdateAsync(request.BankAccountId, update);

                return new UpdateBalanceResponse
                {
                    Success = success,
                    Message = "Balance updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new UpdateBalanceResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        private BankAccountInfo MapBankAccountInfo(Models.BankAccount account)
        {
            return new BankAccountInfo
            {
                BankAccountId = account.BankAccountId,
                IBAN = account.IBAN,
                Balance = account.Balance,
            };
        }
    }
}
