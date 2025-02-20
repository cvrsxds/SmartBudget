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

        private ProfileSettingsWindow _settingsWindow;

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, если окно уже открыто
            if (_settingsWindow != null && _settingsWindow.IsVisible)
            {
                _settingsWindow.Activate(); // Фокусируем уже открытое окно
                return;
            }

            // Создаем экземпляр нового окна
            _settingsWindow = new ProfileSettingsWindow();

            // Вычисляем позицию для отображения окна в правом верхнем углу
            var mainWindowTopLeft = this.PointToScreen(new Point(0, 0));
            double xPosition = mainWindowTopLeft.X + this.Width - _settingsWindow.Width - 20; // 20 для отступа
            double yPosition = mainWindowTopLeft.Y + 40; // Отступ для кнопки

            // Устанавливаем позицию окна
            _settingsWindow.Left = xPosition;
            _settingsWindow.Top = yPosition;

            // Показываем новое окно
            _settingsWindow.Show();
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
