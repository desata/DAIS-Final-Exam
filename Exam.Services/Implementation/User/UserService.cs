using Exam.Repository.Interfaces.User;
using Exam.Services.DTOs.User;
using Exam.Services.Interfaces.User;

namespace Exam.Services.Implementation.User
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserInfo>> GetAllAsync()
        {
            var users = await _userRepository.RetrieveCollectionAsync(new UserFilter()).ToListAsync();

            return users.Select(MapUserInfo).ToList();

        }

        public async Task<UserInfo> GetByIdAsync(int userId)
        {
            var user = await _userRepository.RetrieveAsync(userId);
            if (user == null)
            {
                return null;
            }

            return MapUserInfo(user);

        }

        private UserInfo MapUserInfo(Models.User user)
        {
            return new UserInfo
            {
                UserId = user.UserId,
                Name = user.Name
            };
        }
    }
}
