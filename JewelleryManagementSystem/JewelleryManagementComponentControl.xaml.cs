using JewelleryManagementSystem.CustomerManagement.Model;
using JewelleryManagementSystem.CustomerManagement.View;
using JewelleryManagementSystem.Settings.View;
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
            newCustomerWindow.Closing += (o, e) => _component.Build();
            newCustomerWindow.ShowDialog();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(sender is DataGrid grid)
            {
                if (grid.SelectedItem is ICustomer customer)
                {
                    _component.CustomerManager.Customer = customer;
                    var newCustomerWindow = new NewCustomerWindow(_component.CustomerManager);
                    newCustomerWindow.Owner = Window.GetWindow(this);
                    newCustomerWindow.btnCreateCustomer.Content = "Update";
                    newCustomerWindow.Closing += (o, e) => _component.Build();
                    newCustomerWindow.ShowDialog();
                }
            }
        }

        private void _btnSettings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Owner = Window.GetWindow(this);
            settingsWindow.Show();

        }
    }
}
