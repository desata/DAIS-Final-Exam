using Exam.Repository.Base;

namespace Exam.Repository.Interfaces.UsersBankAccount
{
    public interface IUsersBankAccountRepository : IBaseRepository<Models.UsersBankAccount, UsersBankAccountFilter, UsersBankAccountUpdate>
    {
    }
}
