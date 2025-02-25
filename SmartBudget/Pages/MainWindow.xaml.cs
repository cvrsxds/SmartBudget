using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SmartBudget.Pages
{
    public partial class MainWindow : Window
    {
        private List<Expense> _expenses = new List<Expense>();
        private string dbPath = "SmartBudget.db";

        public MainWindow()
        {
            InitializeComponent();
            LoadExpensesFromDatabase();
        }

        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountTextBox.Text, out decimal amount) && CategoryComboBox.SelectedItem != null)
            {
                string category = (CategoryComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                DateTime purchaseDate = DateTime.Now;
                int userId = GetCurrentUserId();

                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Costs (UserId, Amount, Category, PurchaseDate) VALUES (@UserId, @Amount, @Category, @PurchaseDate)";

                    using (var command = new SqliteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@PurchaseDate", purchaseDate);
                        command.ExecuteNonQuery();
                    }
                }

                LoadExpensesFromDatabase();
                AmountTextBox.Clear();
                CategoryComboBox.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Введите корректную сумму и выберите категорию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadExpensesFromDatabase()
        {
            _expenses.Clear();
            int userId = GetCurrentUserId();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string query = "SELECT Amount, Category, PurchaseDate FROM Costs WHERE UserId = @UserId";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _expenses.Add(new Expense
                            {
                                Amount = reader.GetDecimal(0),
                                Category = reader.GetString(1),
                                Date = reader.GetDateTime(2)
                            });
                        }
                    }
                }
            }

            ExpensesListView.ItemsSource = null;
            ExpensesListView.ItemsSource = _expenses;
            TotalExpensesTextBlock.Text = $"Общая сумма: {_expenses.Sum(e => e.Amount)}";
        }
        private int GetCurrentUserId()
        {
            int userId = -1; // Значение по умолчанию, если пользователь не найден
            string username = LoginWindow.CurrentUser; // Получаем имя текущего пользователя

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Пользователь не авторизован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return userId;
            }

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string query = "SELECT Id FROM Users WHERE Username = @Username LIMIT 1";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
            }
            return userId;
        }

        private ProfileSettingsWindow settingsWindow;

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            string username = GetCurrentUsernameFromDatabase(GetCurrentUserId());
            if (settingsWindow != null && settingsWindow.IsVisible)
            {
                settingsWindow.Activate();
                return;
            }
            settingsWindow = new ProfileSettingsWindow(username);

            var mainWindowTopLeft = this.PointToScreen(new Point(0, 0));
            double xPosition = mainWindowTopLeft.X + this.Width - settingsWindow.Width - 20;
            double yPosition = mainWindowTopLeft.Y + 40;

            settingsWindow.Left = xPosition;
            settingsWindow.Top = yPosition;

            settingsWindow.Show();
        }

        private string GetCurrentUsernameFromDatabase(int userId)
        {
            string username = "Unknown";
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string query = "SELECT Username FROM Users WHERE Id = @UserId LIMIT 1";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        username = result.ToString();
                    }
                }
            }
            return username;
        }

    }

    public class Expense
    {
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
    }
}
