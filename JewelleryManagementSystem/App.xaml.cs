using JewelleryManagementSystem.ModelUtilities;
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
            if (String.IsNullOrWhiteSpace(GeneralSettings.Default.BaseDirectory))
            {
                var window = new DataPath.DataPathWindow();
                window.ShowDialog();
                GeneralSettings.Default.BaseDirectory = window.GetDataPath();
                GeneralSettings.Default.Save();
            }
            var instance = DirectoryPath.Instance;
            var app = new StartApp();
            var desktopWindow = new MainWindow(app.BaseComponent);
            desktopWindow.Show();
        }

    }
    internal class StartApp
    {
        public JewelleryManagementSystemComponent BaseComponent { get; private set; }
        public StartApp()
        {
            BaseComponent = new JewelleryManagementSystemComponent();
            BaseComponent.Build();
        }
        public void Run()
        {

        }
    }

}
