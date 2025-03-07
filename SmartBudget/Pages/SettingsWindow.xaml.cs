using Microsoft.Data.Sqlite;
using SmartBudget.Tables;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SmartBudget.Pages
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            CloseMainWindow();
            CloseProfileWindow();
            LoadSettings();
        }

        private void LoadSettings()
        {
            string currency = Database.GetSetting("Currency", "BYN");
            foreach (ComboBoxItem item in CurrencyComboBox.Items)
            {
                if (item.Content.ToString() == currency)
                {
                    CurrencyComboBox.SelectedItem = item;
                    break;
                }
            }

            string theme = Database.GetSetting("Theme", "Light");
            if (theme == "Dark") DarkThemeRadio.IsChecked = true;
            else LightThemeRadio.IsChecked = true;

            ApplyTheme(theme);
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedCurrency = (CurrencyComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "BYN";
            string selectedTheme = LightThemeRadio.IsChecked == true ? "Light" : "Dark";

            Database.SaveSetting("Currency", selectedCurrency);
            Database.SaveSetting("Theme", selectedTheme);
            ApplyTheme(selectedTheme);

            MessageBox.Show("Настройки сохранены!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ApplyTheme(string theme)
        {
            this.Background = theme == "Dark" ? System.Windows.Media.Brushes.DarkGray : System.Windows.Media.Brushes.White;
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

        private void CloseProfileWindow()
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
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}