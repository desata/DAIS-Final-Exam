using Exam.Repository.Interfaces.User;
using Exam.Services.DTOs.Authentication;
using Exam.Services.Helpers;
using Exam.Services.Interfaces.Authentication;
using System.Data.SqlTypes;

namespace Exam.Services.Implementation.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Username and password are required"
                };
            }

            var hashedPassword = SecurityHelper.HashPassword(request.Password);
            var filter = new UserFilter { Username = new SqlString(request.Username) };

            var user = await _userRepository.RetrieveCollectionAsync(filter).SingleOrDefaultAsync();

            if (user is null || user.Password != hashedPassword)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid username or password"
                };
            }

            return new LoginResponse
            {
                IsSuccess = true,
                UserId = user.UserId,
                Name = user.Name
            };
        }
    }
}
