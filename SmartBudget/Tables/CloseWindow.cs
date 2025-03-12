using SmartBudget.Pages;
using System.Windows;

namespace SmartBudget.Tables
{
    class CloseWindow
    {
        public static void CloseMainWindow()
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

        public static void CloseProfileWindow()
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
    }
}
