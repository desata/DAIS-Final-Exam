using Exam.Repository.Base;
using Exam.Repository.Interfaces.Status;
using Microsoft.Data.SqlClient;

namespace Exam.Repository.Implementation.Status
{
    public class StatusRepository : BaseRepository<Models.Status>, IStatusRepository
    {
        private const string IdDbFieldEnumeratorName = "StatusId";

        protected override string GetTableName()
        {
            return "Statuses";
        }

        protected override string[] GetColumns() => new[]
        {
            IdDbFieldEnumeratorName,
            "Description",
};

        protected override Models.Status MapEntity(SqlDataReader reader)
        {
            return new Models.Status
            {
                StatusId = Convert.ToInt32(reader[IdDbFieldEnumeratorName]),
                Description = Convert.ToString(reader["Description"]),
            };
        }


        public Task<int> CreateAsync(Models.Status entity)
        {
            throw new NotImplementedException();
        }


        public Task<Models.Status> RetrieveAsync(int objectId)
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<Models.Status> RetrieveCollectionAsync(StatusFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int objectId, StatusUpdate update)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }

    }
}
