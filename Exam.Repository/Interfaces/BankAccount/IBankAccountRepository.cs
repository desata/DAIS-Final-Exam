using Exam.Repository.Base;

namespace Exam.Repository.Interfaces.BankAccount
{
    public interface IBankAccountRepository : IBaseRepository<Models.BankAccount, BankAccountFilter, BankAccountUpdate>
    {
    }
}
