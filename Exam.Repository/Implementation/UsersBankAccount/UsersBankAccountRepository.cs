using Exam.Repository.Base;
using Exam.Repository.Helpers;
using Exam.Repository.Interfaces.UsersBankAccount;
using Microsoft.Data.SqlClient;

namespace Exam.Repository.Implementation.UsersBankAccount
{
    public class UsersBankAccountRepository : BaseRepository<Models.UsersBankAccount>, IUsersBankAccountRepository
    {
        protected override string GetTableName() => "UsersBankAccounts";

        protected override string[] GetColumns() => new[] { "UserId", "BankAccountId" };

        protected override Models.UsersBankAccount MapEntity(SqlDataReader reader)
        {
            return new Models.UsersBankAccount
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                BankAccountId = Convert.ToInt32(reader["BankAccountId"])
            };
        }

        public Task<int> CreateAsync(Models.UsersBankAccount entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }

        public Task<Models.UsersBankAccount> RetrieveAsync(int objectId)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Models.UsersBankAccount> RetrieveCollectionAsync(UsersBankAccountFilter filter)
        {
            //TODO Check if needed another filter by BankAccountId

            Filter commandFilter = new Filter();

            if (filter.UserId.HasValue)
            {
                commandFilter.AddClause("UserId", filter.UserId.Value);
            }

            return base.RetrieveCollectionAsync(commandFilter);
        }

        public Task<bool> UpdateAsync(int objectId, UsersBankAccountUpdate update)
        {
            throw new NotImplementedException();
        }

    }
}
