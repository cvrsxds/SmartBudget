using Microsoft.Data.Sqlite;
using System.Windows;
using SmartBudget.Tables;

namespace SmartBudget.Pages
{
    /// <summary>
    /// Логика взаимодействия для ResetPasswordWindow.xaml
    /// </summary>
    public partial class ResetPasswordWindow : Window, IHash
    {
        private const string DbPath = "SmartBudget.db";
        private string foundUsername = string.Empty;

        public ResetPasswordWindow()
        {
            InitializeComponent();
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void CheckEmailButton_Click(object sender, RoutedEventArgs e)
        {
            string email = MailBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Введите почту.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using(var connection = new SqliteConnection($"Data Source={DbPath}"))
                {
                    connection.Open();
                    string query = "SELECT Username FROM Users WHERE Mail = @Mail";

                    using(var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Mail", email);
                        var result = command.ExecuteScalar();

                        if (result != null)
                        {
                            foundUsername = result.ToString();
                            ShowPasswordFields();
                        }
                        else
                        {
                            MessageBox.Show("Аккаунт с введенной почтой не найден.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке почты: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = PasswordBox.Password.Trim();
            string confirmPassword = ConfirmPasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают. Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string hashedPassword = IHash.HashPassword(newPassword);

            try
            {
                using (var connection = new SqliteConnection($"Data Source={DbPath}"))
                {
                    connection.Open();
                    string query = "UPDATE Users SET Password = @Password WHERE Username = @Username";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Password", hashedPassword);
                        command.Parameters.AddWithValue("@Username", foundUsername);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Пароль успешно изменён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            var loginWindow = new LoginWindow();
                            loginWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при изменении пароля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении пароля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowPasswordFields()
        {
            this.MailBox.IsEnabled = false;
            this.PasswordBlock.Visibility = Visibility.Visible;
            this.PasswordBox.Visibility = Visibility.Visible;
            this.ConfirmPasswordBlock.Visibility = Visibility.Visible;
            this.ConfirmPasswordBox.Visibility = Visibility.Visible;
            this.CheckEmailButton.Visibility = Visibility.Hidden;
            this.CheckEmailButton.IsEnabled = false;
            this.ChangePasswordButton.Visibility = Visibility.Visible;
            this.ChangePasswordButton.IsEnabled = true;
        }
    }
}
