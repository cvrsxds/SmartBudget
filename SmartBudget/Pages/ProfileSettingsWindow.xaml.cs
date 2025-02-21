using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartBudget.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileSettingsWindow.xaml
    /// </summary>
    public partial class ProfileSettingsWindow : Window
    {
        private string currentUsername;
        public ProfileSettingsWindow(string username)
        {
            InitializeComponent();
            currentUsername = username;
            UsernameLabel.Text = $"Текущий пользователь: {currentUsername}";
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            var editprofilewindow = new EditProfileWindow();
            var mainwindow = new MainWindow();
            editprofilewindow.Show();
            CloseMainWindow();
            this.Close();
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingswindow = new SettingsWindow();
            var mainwindow = new MainWindow();
            settingswindow.Show();
            CloseMainWindow();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var startwindow = new StartWindow();
            var mainwindow = new MainWindow();
            startwindow.Show();
            CloseMainWindow();
            this.Close();
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
    }
}
