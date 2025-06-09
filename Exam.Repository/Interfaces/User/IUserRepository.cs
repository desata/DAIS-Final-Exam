using Exam.Repository.Base;

namespace Exam.Repository.Interfaces.User
{
    public interface IUserRepository : IBaseRepository<Models.User, UserFilter, UserUpdate>
    {

    }
}