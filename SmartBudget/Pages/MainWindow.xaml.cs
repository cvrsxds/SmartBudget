using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SmartBudget.Pages
{
    public partial class MainWindow : Window
    {
        // Список расходов
        private List<Expense> _expenses = new List<Expense>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на ввод корректных данных
            if (decimal.TryParse(AmountTextBox.Text, out decimal amount) && CategoryComboBox.SelectedItem != null)
            {
                string category = (CategoryComboBox.SelectedItem as ComboBoxItem).Content.ToString();

                // Добавление новой записи о расходах
                _expenses.Add(new Expense
                {
                    Amount = amount,
                    Category = category,
                    Date = DateTime.Now
                });

                // Обновление списка расходов
                ExpensesListView.ItemsSource = null; // Очистка источника данных
                ExpensesListView.ItemsSource = _expenses;

                // Обновление общей суммы расходов
                decimal totalAmount = 0;
                foreach (var expense in _expenses)
                {
                    totalAmount += expense.Amount;
                }
                TotalExpensesTextBlock.Text = $"Общая сумма: {totalAmount}";

                // Очистка полей ввода
                AmountTextBox.Clear();
                CategoryComboBox.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Введите корректную сумму и выберите категорию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private ProfileSettingsWindow settingsWindow;

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            string username = GetCurrentUsernameFromDatabase();
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
        private string GetCurrentUsernameFromDatabase()
        {
            string username = "Unknown";
            string dbPath = "SmartBuget.db";

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string query = "SELECT Username FROM Users LIMIT 1";

                using (var command = new SqliteCommand(query, connection))
                {
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

    // Класс для представления расхода
    public class Expense
    {
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
    }
}
