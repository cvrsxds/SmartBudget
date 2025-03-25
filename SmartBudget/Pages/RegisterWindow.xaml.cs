using System.Text.RegularExpressions;
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
            string email = MailBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            if(AgreementCheckBox.IsChecked == true) 
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Все поля обязательны.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!isValidMail(email))
                {
                    MessageBox.Show("Почта введена не верно.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Введенные пароли не совпадают.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    if (RegisterUser(username, email, password))
                    {
                        MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        var startupWindow = new StartWindow();
                        startupWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Логин или почта уже используется.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Подтвердите согласие на обработку данных!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private bool RegisterUser(string username, string email, string password)
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
                var insertCommand = new SqliteCommand("INSERT INTO Users (Username, Mail, Password) VALUES (@username, @mail, @password)", connection);
                insertCommand.Parameters.AddWithValue("@username", username);
                insertCommand.Parameters.AddWithValue("@mail", email);
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

        private bool isValidMail(string mail)
        {
            string patern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(mail, patern);
        }
    }
}
