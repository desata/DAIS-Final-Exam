using Exam.Repository.Base;
using Exam.Repository.Helpers;
using Exam.Repository.Interfaces.Payment;
using Microsoft.Data.SqlClient;

namespace Exam.Repository.Implementation.Payment
{
    public class PaymentRepository : BaseRepository<Models.Payment>, IPaymentRepository
    {

        private const string IdDbFieldEnumeratorName = "PaymentId";
        protected override string GetTableName()
        {
            return "Payments";
        }
        protected override string[] GetColumns() => new string[]
        {
            IdDbFieldEnumeratorName,
            "SenderBankAccountId",
            "SenderUserId",
            "RecieverIBAN",
            "RecieverName",
            "Reference",
            "Amount",
            "StatusId"
        };
        protected override Models.Payment MapEntity(SqlDataReader reader)
        {
            return new Models.Payment
            {
                PaymentId = Convert.ToInt32(reader["PaymentId"]),
                SenderBankAccountId = Convert.ToInt32(reader["SenderBankAccountId"]),
                SenderUserId = Convert.ToInt32(reader["SenderUserId"]),
                RecieverIBAN = Convert.ToString(reader["RecieverIBAN"]),
                RecieverName = Convert.ToString(reader["RecieverName"]),
                Reference = Convert.ToString(reader["Reference"]),
                Amount = Convert.ToDecimal(reader["Amount"]),
                StatusId = Convert.ToInt32(reader["StatusId"])
            };
        }


        public Task<int> CreateAsync(Models.Payment entity)
        {
            return base.CreateAsync(entity, IdDbFieldEnumeratorName);
        }

        public Task<Models.Payment> RetrieveAsync(int objectId)
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<Models.Payment> RetrieveCollectionAsync(PaymentFilter filter)
        {
            Filter commandFilter = new Filter();

            if (filter.SenderUserId is not null)
            {
                commandFilter.AddClause("SenderUserId", filter.SenderUserId);
            }

            return base.RetrieveCollectionAsync(commandFilter);
        }

        public async Task<bool> UpdateAsync(int objectId, PaymentUpdate update)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();

            UpdateCommand updateCommand = new UpdateCommand(
                connection,
                "Payments",
                IdDbFieldEnumeratorName, objectId);

            updateCommand.AddSetClause("@StatusId", update.StatusId);

            return await updateCommand.ExecuteNonQueryAsync() > 0;
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }


    }
}
