using Exam.Repository.Base;

namespace Exam.Repository.Interfaces.Status
{
    public interface IStatusRepository : IBaseRepository<Models.Status, StatusFilter, StatusUpdate>
    {
    }
}
