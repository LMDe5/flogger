using Npgsql;
using System.Reflection;

namespace MiniHttpServer.ORM
{
    public class OrmContext(string connectionString) : IOrmContext
    {
        public async Task<int> DeleteAsync(string tableName, int id, CancellationToken cancellationToken = default)
        {
            string sqpExpression = $"DELETE FROM {tableName} WHERE id = @id";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqpExpression;
                    command.Parameters.AddWithValue("@id", id);

                    return await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }

        public async Task<IReadOnlyList<T>> GetAllAsync<T>(string tableName, CancellationToken cancellationToken = default) where T : class, new()
        {
            var res = new List<T>();

            string sqlExpression = $"SELECT * FROM {tableName}";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            PropertyInfo[] properties = typeof(T).GetProperties();

                            while (reader.Read())
                            {
                                var item = new T();

                                for (int i  = 0; i < reader.FieldCount; i++)
                                {
                                    string column = reader.GetName(i);

                                    var property = properties.FirstOrDefault(p => p.Name.Equals(column, StringComparison.OrdinalIgnoreCase));
                                    if(property != null)
                                    {
                                        var value = reader.GetValue(i);
                                        property.SetValue(item, value);
                                    }
                                }

                                res.Add(item);
                            }
                        }
                    }
                }
            }

            return await Task.FromResult<IReadOnlyList<T>>(res);
        }

        public async Task<int> InsertAsync<T>(string tableName, T entity, CancellationToken cancellationToken = default) where T : class
        {
            PropertyInfo[] properties = typeof(T).GetProperties()
                .Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)).ToArray();
            if (properties.Length == 0) { return 0; }

            var columnNames = new List<string>();
            var anotherColumnNames = new List<string>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    foreach(var property in properties)
                    {
                        string column = property.Name;
                        string anotherColumn = "@" + column;

                        columnNames.Add(column);
                        anotherColumnNames.Add(anotherColumn);

                        object? value = property.GetValue(entity);

                        command.Parameters.AddWithValue(anotherColumn, value ?? DBNull.Value);
                    }

                    command.CommandText = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", anotherColumnNames)})";

                    return await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }

        public async Task<int> UpdateAsync<T>(string tableName, int id, T entity, CancellationToken cancellationToken = default) where T : class
        {
            PropertyInfo[] properties = typeof(T).GetProperties().
                Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)).ToArray();

            var pairs = new List<string>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = connection.CreateCommand())
                { 
                    foreach (var property in properties)
                    {
                        string column = property.Name;
                        string anotherColumn = "@" + column;

                        pairs.Add($"{column} = {anotherColumn}");

                        object? value = property.GetValue(entity);

                        command.Parameters.AddWithValue(anotherColumn, value ?? DBNull.Value);
                    }

                    command.Parameters.AddWithValue("@id", id);
                    command.CommandText = $"UPDATE {tableName} SET {string.Join(", ", pairs)} WHERE id = @id";

                    return await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }

        public static string CreateInsertSql(string tableName, PropertyInfo[] properties)
        {
            var columnNames = properties.Select(p => p.Name);
            var anotherColumnNames = properties.Select(p => "@" + p.Name);

            return $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", anotherColumnNames)})";
        }
        public static string CreateUpdateSql(string tableName, PropertyInfo[] properties)
        {
            var pairs = properties.Select(p => $"{p.Name} = @{p.Name}");

            return $"UPDATE {tableName} ({string.Join(", ", pairs)}) WHERE id = @id";
        }
        public static string CreateDeleteSql(string tableName)
        {
            return $"DELETE FROM {tableName} WHERE id = @id";
        }
    }
}
