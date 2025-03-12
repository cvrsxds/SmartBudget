using System.Windows;
using Microsoft.Data.Sqlite;
using SmartBudget.Tables;

namespace SmartBudget.Pages
{
    public partial class EditProfileWindow : Window
    {
        private string dbPath = "SmartBudget.db";
        private int userId;

        public EditProfileWindow(int currentUserId)
        {
            InitializeComponent();
            userId = currentUserId;
            CloseWindow.CloseMainWindow();
            CloseWindow.CloseProfileWindow();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = CurrentPasswordBox.Password.Trim();
            string newUsername = NewUsernameTextBox.Text.Trim();
            string newPassword = NewPasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                MessageBox.Show("Введите текущий пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string checkPasswordQuery = "SELECT Password FROM Users WHERE Id = @UserId";
                using (var command = new SqliteCommand(checkPasswordQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    var result = command.ExecuteScalar();
                    string storedPasswordHash = result as string ?? "";

                    string enteredPasswordHash = Hash.HashPassword(currentPassword);

                    if (storedPasswordHash != enteredPasswordHash)
                    {
                        MessageBox.Show("Неверный текущий пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                var updateParts = new List<string>();
                var parameters = new Dictionary<string, object>();

                if (!string.IsNullOrWhiteSpace(newUsername))
                {
                    updateParts.Add("Username = @NewUsername");
                    parameters["@NewUsername"] = newUsername;
                }
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    updateParts.Add("Password = @NewPassword");
                    parameters["@NewPassword"] = Hash.HashPassword(newPassword);
                }

                if (updateParts.Count == 0)
                {
                    MessageBox.Show("Нет изменений для сохранения.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string updateQuery = $"UPDATE Users SET {string.Join(", ", updateParts)} WHERE Id = @UserId";

                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            var mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }
    }
}
