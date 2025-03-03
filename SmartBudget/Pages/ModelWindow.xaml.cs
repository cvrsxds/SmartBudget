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
    /// Логика взаимодействия для ModelWindow.xaml
    /// </summary>
    public partial class ModelWindow : Window
    {
        public ModelWindow()
        {
            InitializeComponent();
            CloseMainWindow();
            CloseProffileWindow();
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
            foreach (Window window in Application.Current.Windows)
            {
                if (window is ProfileSettingsWindow)
                {
                    window.Close();
                    break;
                }
            }
        }

        private void Back_Buton_Click(object sender, RoutedEventArgs e)
        {
            var mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }
    }
}
