using JewelleryManagementSystem.UIUtilities;
using System.Configuration;
using System.Data;
using System.Windows;

namespace JewelleryManagementSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var desktopWindo = new MainWindow();
            desktopWindo.Show();
        }

    }

}
