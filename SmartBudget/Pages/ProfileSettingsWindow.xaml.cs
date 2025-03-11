using System.Windows;

namespace SmartBudget.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileSettingsWindow.xaml
    /// </summary>
    public partial class ProfileSettingsWindow : Window
    {
        private string currentUsername;
        private int userID;
        public ProfileSettingsWindow(string username, int userId)
        {
            InitializeComponent();
            currentUsername = username;
            this.userID = userId;
            UsernameLabel.Text = $"Текущий пользователь: {currentUsername}";
            this.Deactivated += (s, e) => this.Close();
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            var editprofilewindow = new EditProfileWindow(userID);
            var mainwindow = new MainWindow();
            CloseMainWindow();
            CloseProffileWindow();
            editprofilewindow.Show();
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingswindow = new SettingsWindow(userID);
            var mainwindow = new MainWindow();
            CloseMainWindow();
            CloseProffileWindow();
            settingswindow.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var startwindow = new StartWindow();
            var mainwindow = new MainWindow();
            CloseMainWindow();
            CloseProffileWindow();
            startwindow.Show();
        }

        private void CloseMainWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
        private void CloseProffileWindow()
        {
            foreach(Window window in Application.Current.Windows)
            {
                if(window is ProfileSettingsWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
