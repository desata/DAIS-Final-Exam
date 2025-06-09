using Exam.Repository.Helpers;
using Microsoft.Data.SqlClient;

namespace Exam.Repository.Base
{
    public abstract class BaseRepository<TObj>
    {
        protected abstract string GetTableName();
        protected abstract string[] GetColumns();
        protected virtual string SelectAllQuery()
        {
            var column = string.Join(", ", GetColumns());

            return $"SELECT {column} FROM {GetTableName()}";
        }
        protected abstract TObj MapEntity(SqlDataReader reader);

        protected async Task<int> CreateAsync(TObj entity, string idDbFieldEnumeratorName = null)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();

            var properties = typeof(TObj).GetProperties()
                .Where(p => p.Name != idDbFieldEnumeratorName)
                .ToList();

            string columns = string.Join(", ", properties.Select(p => p.Name));
            string parameters = string.Join(", ", properties.Select(p => "@" + p.Name));

            command.CommandText = $@"INSERT INTO {GetTableName()} ({columns}) 
                                VALUES ({parameters});
                                SELECT CAST(SCOPE_IDENTITY() as int)";

            foreach (var prop in properties)
            {
                command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(entity) ?? DBNull.Value);
            }

            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }
        protected async Task<TObj> RetrieveAsync(string idDbFieldName, int idDbValue)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand sqlCommand = connection.CreateCommand();

            sqlCommand.CommandText = $"{SelectAllQuery()} WHERE {idDbFieldName} = @{idDbFieldName}";
            sqlCommand.Parameters.AddWithValue($"@{idDbFieldName}", idDbValue);

            using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            if (!reader.Read()) throw new Exception("No record found");

            var result = MapEntity(reader);

            if (reader.Read())
                throw new Exception("More than one record found");

            return result;
        }

        protected async IAsyncEnumerable<TObj> RetrieveCollectionAsync(Filter filter)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand sqlCommand = connection.CreateCommand();

            sqlCommand.CommandText = $"{SelectAllQuery()} WHERE 1=1";

            foreach (var clause in filter.Clauses)
            {
                sqlCommand.CommandText += $" AND {clause.Key} = @{clause.Key}";
                sqlCommand.Parameters.AddWithValue($"@{clause.Key}", clause.Value);
            }

            using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                TObj result = MapEntity(reader);
                yield return result;
            }
        }


        protected async Task<bool> DeleteAsync(string idDbFieldName, int objectId)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();
            using SqlTransaction transaction = command.Connection.BeginTransaction();

            command.CommandText = $"DELETE FROM {GetTableName()} WHERE {idDbFieldName} = @{idDbFieldName}";
            command.Parameters.AddWithValue($"@{idDbFieldName}", objectId);
            command.Transaction = transaction;

            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected != 1)
            {
                throw new Exception($"Just one row should be deleted! Command aborted, because {rowsAffected} could have been deleted!");
            }

            transaction.Commit();
            return true;
        }
    }
}