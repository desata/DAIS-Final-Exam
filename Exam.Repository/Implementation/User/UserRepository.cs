using Exam.Repository.Base;
using Exam.Repository.Helpers;
using Exam.Repository.Interfaces.User;
using Microsoft.Data.SqlClient;

namespace Exam.Repository.Implementation.User
{
    public class UserRepository : BaseRepository<Models.User>, IUserRepository
    {
        private const string IdDbFieldEnumeratorName = "UserId";
        protected override string GetTableName()
        {
            return "Users";
        }
        protected override string[] GetColumns() => new string[]
        {
            IdDbFieldEnumeratorName,
            "Name",
            "Username",
            "Password"
        };


        public Task<int> CreateAsync(Models.User entity)
        {
            throw new NotImplementedException();
        }

        public Task<Models.User> RetrieveAsync(int objectId)
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<Models.User> RetrieveCollectionAsync(UserFilter filter)
        {
            Filter commandFilter = new Filter();

            if (filter.Username is not null)
            {
                commandFilter.AddClause("Username", filter.Username);
            }

            return RetrieveCollectionAsync(commandFilter);
        }

        public async Task<bool> UpdateAsync(int objectId, UserUpdate update)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }
        protected override Models.User MapEntity(SqlDataReader reader)
        {
            return new Models.User
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                Name = Convert.ToString(reader["Name"]),
                Username = Convert.ToString(reader["Username"]),
                Password = Convert.ToString(reader["Password"])
            };
        }
    }
}