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
                        Mail TEXT NOT NULL UNIQUE,
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
                        UserId INTEGER NOT NULL,
                        Key TEXT NOT NULL,
                        Value TEXT NOT NULL,
                        PRIMARY KEY (UserId, Key),
                        FOREIGN KEY (UserId) REFERENCES Users(Id)
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

        public static (string currency, string theme) GetSettings(int userId)
        {
            string dbPath = "SmartBudget.db";
            string currency = "BYN";
            string theme = "Light";

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string currencyQuery = "SELECT Value FROM Settings WHERE UserId = @userId AND Key = 'Currency';";
                using (var command = new SqliteCommand(currencyQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        currency = result.ToString();
                    }
                }

                string themeQuery = "SELECT Value FROM Settings WHERE UserId = @userId AND Key = 'Theme';";
                using (var command = new SqliteCommand(themeQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        theme = result.ToString();
                    }
                }
            }

            return (currency, theme);
        }


        public static void SaveSettings(int userId, string key, string value)
        {
            string dbPath = "SmartBudget.db";
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string query = @"
                INSERT INTO Settings (UserId, Key, Value) 
                VALUES (@userId, @key, @value)
                ON CONFLICT(UserId, Key) DO UPDATE SET Value = excluded.Value;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@key", key);
                    command.Parameters.AddWithValue("@value", value);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}