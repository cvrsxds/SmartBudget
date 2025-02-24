using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace SmartBudget.Tables
{
    class Database
    {
        public static void InitializeDatabase()
        {
            string dbPath = "SmartBuget.db";

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
                    )";

                using (var command = new SqliteCommand(createUserTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqliteCommand(createCostTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
