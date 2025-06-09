using Exam.Services.DTOs.BankAccount;

namespace Exam.Services.Interfaces.BankAccount
{
    public interface IBankAccountService
    {
        Task<BankAccountInfo> GetBalanceAsync(int BankAccountId);
        Task<List<BankAccountInfo>> GetBankAccountsByUserIdAsync(int userId);
        Task<UpdateBalanceResponse> UpdateBalanceAsync(UpdateBalanceRequest request);
    }
}
