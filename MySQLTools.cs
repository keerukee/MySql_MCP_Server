using MCPServer.Attributes;
using MySqlConnector;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace MySQLMCP;

public class MySQLTools
{
    [McpTool("MySQLExecuteQuery", "Executes a SELECT query on the MySQL database and returns results as JSON.")]
    public static string ExecuteQuery(
        [McpParameter("The MySQL connection string", true)] string connectionString,
        [McpParameter("The SQL SELECT query to execute", true)] string query)
    {
        try
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            
            var results = new List<Dictionary<string, object>>();
            
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                results.Add(row);
            }

            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    [McpTool("MySQLExecuteNonQuery", "Executes an INSERT, UPDATE, or DELETE command. Returns the number of affected rows.")]
    public static string ExecuteNonQuery(
        [McpParameter("The MySQL connection string", true)] string connectionString,
        [McpParameter("The SQL command to execute", true)] string command)
    {
        try
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            using var sqlCommand = new MySqlCommand(command, connection);
            int rowsAffected = sqlCommand.ExecuteNonQuery();
            
            return $"Rows affected: {rowsAffected}";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    [McpTool("MySQLListTables", "Lists all tables in the database.")]
    public static string ListTables(
        [McpParameter("The MySQL connection string", true)] string connectionString)
    {
        try
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            var tables = new List<string>();
            using var command = new MySqlCommand("SHOW TABLES;", connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    tables.Add(reader.GetString(0));
                }
            }

            return JsonSerializer.Serialize(tables, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    [McpTool("MySQLGetTableSchema", "Describes the structure of a specific table.")]
    public static string GetTableSchema(
        [McpParameter("The MySQL connection string", true)] string connectionString,
        [McpParameter("The name of the table to describe", true)] string tableName)
    {
        try
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            var schema = new List<Dictionary<string, object>>();
            // Use parameterized query for table name is tricky in DESCRIBE, usually strict variable is safer.
            // But DESCRIBE ? doesn't work. We should validate table name to avoid injection if possible.
            // For now, naive implementation.
            using var command = new MySqlCommand($"DESCRIBE `{tableName}`;", connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                schema.Add(row);
            }

            return JsonSerializer.Serialize(schema, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}
