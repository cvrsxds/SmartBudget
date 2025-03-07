using Microsoft.Data.Sqlite;
using System;

namespace SmartBudget.Tables
{
    class Database
    {
        public static void InitializeDatabase()
        {
            string dbPath = "SmartBudget.db";

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string createUserTable = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL
                    );";

                string createCostTable = @"
                    CREATE TABLE IF NOT EXISTS Costs (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        Amount REAL NOT NULL,
                        Category TEXT NOT NULL,
                        PurchaseDate DATETIME NOT NULL,
                        FOREIGN KEY (UserId) REFERENCES Users(Id)
                    );";

                string createSettingsTable = @"
                    CREATE TABLE IF NOT EXISTS Settings (
                        Key TEXT PRIMARY KEY,
                        Value TEXT NOT NULL
                    );";

                using (var command = new SqliteCommand(createUserTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqliteCommand(createCostTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqliteCommand(createSettingsTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static string GetSetting(string key, string defaultValue)
        {
            string dbPath = "SmartBudget.db";
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string query = "SELECT Value FROM Settings WHERE Key = @key;";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@key", key);
                    var result = command.ExecuteScalar();
                    return result != null ? result.ToString() : defaultValue;
                }
            }
        }

        public static void SaveSetting(string key, string value)
        {
            string dbPath = "SmartBudget.db";
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Settings (Key, Value) VALUES (@key, @value)
                    ON CONFLICT(Key) DO UPDATE SET Value = excluded.Value;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@key", key);
                    command.Parameters.AddWithValue("@value", value);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}