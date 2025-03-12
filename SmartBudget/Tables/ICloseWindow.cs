using SmartBudget.Pages;
using System.Windows;

namespace SmartBudget.Tables
{
    interface ICloseWindow
    {
        protected static void CloseMainWindow()
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

        protected static void CloseProfileWindow()
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
