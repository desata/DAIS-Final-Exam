using Exam.Repository.Base;
using Exam.Repository.Helpers;
using Exam.Repository.Interfaces.BankAccount;
using Microsoft.Data.SqlClient;

namespace Exam.Repository.Implementation.BankAccount
{
    public class BankAccountRepository : BaseRepository<Models.BankAccount>, IBankAccountRepository
    {

        private const string IdDbFieldEnumeratorName = "BankAccountId";
        protected override string GetTableName()
        {
            return "BankAccounts";
        }
        protected override string[] GetColumns() => new string[]
        {
            IdDbFieldEnumeratorName,
            "IBAN",
            "Balance"
        };


        protected override Models.BankAccount MapEntity(SqlDataReader reader)
        {
            return new Models.BankAccount
            {
                BankAccountId = Convert.ToInt32(reader[IdDbFieldEnumeratorName]),
                IBAN = Convert.ToString(reader["IBAN"]),
                Balance = Convert.ToDecimal(reader["Balance"])
            };
        }


        public Task<int> CreateAsync(Models.BankAccount entity)
        {
            throw new NotImplementedException();
        }


        public Task<Models.BankAccount> RetrieveAsync(int objectId)
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<Models.BankAccount> RetrieveCollectionAsync(BankAccountFilter filter)
        {
            Filter commandFilter = new Filter();

            if (filter.IBAN is not null)
            {
                commandFilter.AddClause("IBAN", filter.IBAN);
            } 

            return RetrieveCollectionAsync(commandFilter);
        }

        public async Task<bool> UpdateAsync(int objectId, BankAccountUpdate update)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();

            UpdateCommand updateCommand = new UpdateCommand(
                connection,
                "BankAccounts",
                IdDbFieldEnumeratorName, objectId);

            updateCommand.AddSetClause("@Balance", update.Balance);

            return await updateCommand.ExecuteNonQueryAsync() > 0;
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }

    }
}
