using Microsoft.Data.Sqlite;
using System.Windows;
using System.Windows.Controls;
using SmartBudget.Tables;

namespace SmartBudget.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, IHash
    {
        public static string DbPath { get; } = "SmartBudget.db";
        public static string CurrentUser { get; private set; }
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (AuthenticateUser(username, password))
                {
                    MessageBox.Show("Вход выполнен успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    var mainwindow = new MainWindow();
                    mainwindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool AuthenticateUser(string input, string password)
        {
            string connectionString = $"Data Source={DbPath}";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Username, Password FROM Users WHERE Username = @Input OR Mail = @Input";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Input", input);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedUsername = reader.GetString(0);
                            string storedHash = reader.GetString(1);

                            if (storedHash == IHash.HashPassword(password))
                            {
                                CurrentUser = storedUsername;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var startupWindow = new StartWindow();
            startupWindow.Show();
            this.Close();
        }

        private void PasswordReset_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var resetpasswordWindow = new ResetPasswordWindow();
            resetpasswordWindow.Show();
            this.Close();
        }
    }
}