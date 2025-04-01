using System.Windows;
using SmartBudget.Tables;

namespace SmartBudget.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileSettingsWindow.xaml
    /// </summary>
    public partial class ProfileSettingsWindow : Window, ICloseWindow
    {
        private string currentUsername;
        private int userID;
        public ProfileSettingsWindow(string username, int userId)
        {
            InitializeComponent();
            currentUsername = username;
            this.userID = userId;
            UsernameLabel.Text = $"Текущий пользователь: {currentUsername}";
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            var editprofilewindow = new EditProfileWindow(userID);
            var mainwindow = new MainWindow();
            editprofilewindow.Show();
            mainwindow.Close();
            ICloseWindow.CloseMainWindow();
            ICloseWindow.CloseProfileWindow();
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingswindow = new SettingsWindow(userID);
            var mainwindow = new MainWindow();
            settingswindow.Show();
            mainwindow.Close();
            ICloseWindow.CloseMainWindow();
            ICloseWindow.CloseProfileWindow();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var startwindow = new StartWindow();
            var mainwindow = new MainWindow();
            startwindow.Show();
            mainwindow.Close();
            ICloseWindow.CloseMainWindow();
            ICloseWindow.CloseProfileWindow();
        }
    }
}
