using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JewelleryManagementSystem.UIUtilities
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private JewelleryManagementSystemComponent _component;
        public MainWindow(JewelleryManagementSystemComponent component)
        {
            InitializeComponent();
            _component = component;
            this.Title = $"{ProductInformation.ProductName} - {ProductInformation.ProductVersion}" ;
            this.Loaded += (o, e) => OnLoaded();
            _component = component;
        }
        private void OnLoaded()
        {
            this.Content = new JewelleryManagementComponentControl(_component);

        }
    }
}