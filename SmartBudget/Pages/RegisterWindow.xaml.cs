using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using SmartBudget.Tables;

namespace SmartBudget.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window, IHash
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Все поля обязательны.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Введенные пароли не совпадают.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (RegisterUser(username, password))
                {
                    MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    var startupWindow = new StartWindow();
                    startupWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username уже используется. Пожалуйста выберите другой.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool RegisterUser(string username, string password)
        {
            string dbPath = "SmartBudget.db";

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                var checkCommand = new SqliteCommand("SELECT COUNT(*) FROM Users WHERE Username = @username", connection);
                checkCommand.Parameters.AddWithValue("@username", username);

                int userCount = Convert.ToInt32(checkCommand.ExecuteScalar());
                if (userCount > 0)
                {
                    return false;
                }

                string hashedPassword = IHash.HashPassword(password);
                var insertCommand = new SqliteCommand("INSERT INTO Users (Username, Password) VALUES (@username, @password)", connection);
                insertCommand.Parameters.AddWithValue("@username", username);
                insertCommand.Parameters.AddWithValue("@password", hashedPassword);

                insertCommand.ExecuteNonQuery();
            }

            return true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var startWindow = new StartWindow();
            startWindow.Show();
            this.Close();
        }
    }
}
