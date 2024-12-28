using JewelleryManagementSystem.UIUtilities;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JewelleryManagementSystem.Settings.View
{
    /// <summary>
    /// Interaction logic for GeneralSettingsControl.xaml
    /// </summary>
    public partial class GeneralSettingsControl : UserControl
    {
        public GeneralSettingsControl()
        {         
            InitializeComponent();
            DataContext = ProductInformation.Instance;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            ProductInformation.Instance.UpdateProductInformation();
        }
    }
}
