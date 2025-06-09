using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OnlinePayments.Repositories.Helpers;

namespace OnlinePayments.Repositories.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected abstract string GetTableName();
        protected abstract string GetIdCollumnName();
        protected abstract string GetProperties();
        protected abstract T MapEntity(SqlDataReader record);


        public virtual async Task<int> Create(T entity)
        {
            var props = typeof(T).GetProperties().Where(p => p.Name != GetIdCollumnName());
            var parameters = string.Join(", ", props.Select(p => $"@{p.Name}"));
            string columns = string.Join(", ", props.Select(p => p.Name));

            using (var connection = await ConnectionFactory.CreateConnectionAsync())
            using (var command = new SqlCommand($@"INSERT INTO {GetTableName()} ({columns})
                                                    VALUES ({parameters});
                                                    SELECT CAST(SCOPE_IDENTITY() AS INT)", connection))
            {
                foreach (var prop in props)
                {
                    command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(entity) ?? DBNull.Value);
                }
                //connection.Open();
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }

        }
        public virtual async Task<T> Retrieve(int objectId)
        {
            using (var connection = await ConnectionFactory.CreateConnectionAsync())
            using (var command = new SqlCommand($"SELECT {GetIdCollumnName()}, {GetProperties()} FROM {GetTableName()} WHERE {GetIdCollumnName()} = @{GetIdCollumnName()}", connection))
            {
                command.Parameters.AddWithValue($"@{GetIdCollumnName()}", objectId);
                //connection.Open();
                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    T result = MapEntity(reader);

                    if (reader.Read())
                    {
                        throw new Exception("Multiple records found for the same ID.");
                    }

                    return result;
                }
                else
                {
                    throw new Exception("No record found for the given ID.");
                }
            }

        }



        public virtual async Task<IEnumerable<T>> RetrieveCollection(Filter filter)
        {
            using var connection = await ConnectionFactory.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            var whereClauses = filter.Conditions
                .Select(c => $"{c.Key} = @{c.Key}")
                .ToList();

            var whereClause = whereClauses.Any()
                ? "WHERE " + string.Join(" AND ", whereClauses)
                : string.Empty;

            command.CommandText = $"SELECT {GetIdCollumnName()}, {GetProperties()} FROM {GetTableName()} {whereClause}";

            foreach (var condition in filter.Conditions)
            {
                command.Parameters.AddWithValue("@" + condition.Key, condition.Value ?? DBNull.Value);
            }

            var results = new List<T>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(MapEntity(reader));
            }
            return results;
        }



        public virtual async Task<bool> Update(int objectId, UpdateCommand update)
        {
            var setClause = string.Join(", ", update.Fields.Select(p => $"{p.Key} = @{p.Key}"));
            using var connection = await ConnectionFactory.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = $"UPDATE {GetTableName()} SET {setClause} WHERE {GetIdCollumnName()} = {objectId}";

            foreach (var field in update.Fields)
            {
                command.Parameters.AddWithValue("@" + field.Key, field.Value ?? DBNull.Value);
            }


            return (await command.ExecuteNonQueryAsync()) > 0;
        }

        public virtual async Task<bool> Delete(int objectId)
        {
            using (var connection = await ConnectionFactory.CreateConnectionAsync())
            using (var command = new SqlCommand($"DELETE FROM {GetTableName()} WHERE {GetIdCollumnName} = @{GetIdCollumnName}", connection))
            {
                command.Parameters.AddWithValue($"@{GetTableName()}", objectId);
                connection.Open();
                return (await command.ExecuteNonQueryAsync()) > 0;
            }
        }
    }
}
