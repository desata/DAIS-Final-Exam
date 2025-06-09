using Exam.Services.DTOs.User;

namespace Exam.Services.Interfaces.User
{
    public interface IUserService
    {
        Task<UserInfo> GetByIdAsync(int userId);
        Task<List<UserInfo>> GetAllAsync();
    }
}
