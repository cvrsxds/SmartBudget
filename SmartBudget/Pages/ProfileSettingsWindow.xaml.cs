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
using SmartBudget.Models;
using SmartBudget.Pages;

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

        private void Venom_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
