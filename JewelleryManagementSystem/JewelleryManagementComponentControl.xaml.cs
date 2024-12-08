using JewelleryManagementSystem.CustomerManagement.Model;
using JewelleryManagementSystem.CustomerManagement.View;
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

namespace JewelleryManagementSystem
{
    /// <summary>
    /// Interaction logic for JewelleryManagementComponentControl.xaml
    /// </summary>
    public partial class JewelleryManagementComponentControl : UserControl
    {
        private JewelleryManagementSystemComponent _component;
        public JewelleryManagementComponentControl(JewelleryManagementSystemComponent component)
        {
            InitializeComponent();
            this.DataContext = _component = component;
        }

        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            var newCustomerWindow = new NewCustomerWindow(_component.CustomerManager);
            newCustomerWindow.Owner = Window.GetWindow(this);
            newCustomerWindow.ShowDialog();
        }
    }
}
