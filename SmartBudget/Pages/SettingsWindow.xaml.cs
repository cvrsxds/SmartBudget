using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            CloseMainWindow();
            CloseProffileWindow();
            LoadSettings();
        }

        private void LoadSettings()
        {
            string currency = ConfigurationManager.AppSettings["Currency"] ?? "BYN";
            foreach (ComboBoxItem item in CurrencyComboBox.Items)
            {
                if (item.Content.ToString() == currency)
                {
                    CurrencyComboBox.SelectedItem = item;
                    break;
                }
            }

            string theme = ConfigurationManager.AppSettings["Theme"] ?? "Light";
            if (theme == "Dark") DarkThemeRadio.IsChecked = true;
            else LightThemeRadio.IsChecked = true;
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedCurrency = (CurrencyComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "BYN";
            string selectedTheme = LightThemeRadio.IsChecked == true ? "Light" : "Dark";

            SaveSetting("Currency", selectedCurrency);
            SaveSetting("Theme", selectedTheme);
            ApplyTheme(selectedTheme);

            MessageBox.Show("Настройки сохранены!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ApplyTheme(string theme)
        {
            this.Background = theme == "Dark" ? System.Windows.Media.Brushes.DarkGray : System.Windows.Media.Brushes.White;
        }

        private void SaveSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void ClearDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            string dbPath = "SmartBudget.db";
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string clearCostsTable = "DELETE FROM Costs;";
                using (var command = new SqliteCommand(clearCostsTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("База данных очищена!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            var mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }
    }
}
