using Microsoft.Data.Sqlite;
using SmartBudget.Tables;
using System.Windows;
using System.Windows.Controls;

namespace SmartBudget.Pages
{
    public partial class SettingsWindow : Window
    {
        private int userID;
        public SettingsWindow(int userId)
        {
            InitializeComponent();
            CloseWindow.CloseMainWindow();
            CloseWindow.CloseProfileWindow();
            LoadSettings();
            this.userID = userId;
        }

        private void LoadSettings()
        {
            var (currency, theme) = Database.GetSettings(userID);
            if (!string.IsNullOrEmpty(currency))
            {
                foreach (var item in CurrencyComboBox.Items)
                {
                    if (item is ComboBoxItem comboItem && comboItem.Content.ToString() == currency)
                    {
                        CurrencyComboBox.SelectedItem = comboItem;
                        break;
                    }
                    else if (item.ToString() == currency)
                    {
                        CurrencyComboBox.SelectedItem = item;
                        break;
                    }
                }
            }

            bool isDarkTheme = theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            DarkThemeRadio.IsChecked = isDarkTheme;
            LightThemeRadio.IsChecked = !isDarkTheme;
            ApplyTheme(theme);
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedCurrency = (CurrencyComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "BYN";
            string selectedTheme = LightThemeRadio.IsChecked == true ? "Light" : "Dark";
            Database.SaveSettings(userID, "Currency", selectedCurrency);
            Database.SaveSettings(userID, "Theme", selectedTheme);
            ApplyTheme(selectedTheme);
            MessageBox.Show("Настройки сохранены!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            Back_Button_Click(sender, e);
        }

        private void ApplyTheme(string theme)
        {
            Uri uri;
            if (theme == "Light")
            {
                uri = new Uri(@"Resources/LightTheme.xaml", UriKind.Relative);
            }
            else
            {
                uri = new Uri(@"Resources/DarkTheme.xaml", UriKind.Relative);
            }
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
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

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}