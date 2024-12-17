using JewelleryManagementSystem.CustomerManagement.Model;
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
using System.Windows.Shapes;

namespace JewelleryManagementSystem.CustomerManagement.View
{
    /// <summary>
    /// Interaction logic for NewCustomerWindow.xaml
    /// </summary>
    public partial class NewCustomerWindow : Window
    {
        private CustomerManager _customerManager;
        private ICustomer _customer;
        public NewCustomerWindow(CustomerManager customerManager)
        {
            InitializeComponent();
            Title = ProductInformation.ProductName;
            _customerManager = customerManager;
            _customer = customerManager.Customer != null ? customerManager.Customer : _customerManager.GetNewCustomer();
            DataContext = _customer;
            _customerManager.OrderManager.Build(_customer);
            UpdateDataGridVisibility();
        }

        private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var noUse = _customerManager.OrderManager.GetNewOrder();
            var newOrderWindow = new NewOrderWindow(_customerManager.OrderManager);
            newOrderWindow.Owner = this;
            newOrderWindow.ShowDialog();
            UpdateDataGridVisibility();
        }
        private void UpdateDataGridVisibility()
        {
            if (_customerManager.Customer.OrderList != null && _customerManager.Customer.OrderList.Count > 0)
                dataGridOrders.Visibility = Visibility.Visible;
            else
                dataGridOrders.Visibility = Visibility.Collapsed;

        }

        private void dataGridOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                IOrder order = grid.SelectedItem as IOrder;
                if (order != null)
                {
                    _customerManager.OrderManager.Order = order;
                    var newOrderWindow = new NewOrderWindow(_customerManager.OrderManager);
                    newOrderWindow.Owner = this;
                    newOrderWindow.btnAddOrder.IsEnabled = !_customerManager.OrderManager.Order.IsCompleted;
                    newOrderWindow.btnAddOrder.Content = "Update";
                    newOrderWindow.ShowDialog();
                    UpdateDataGridVisibility();
                }
            }
        }

        private void btnCreateCustomer_Click(object sender, RoutedEventArgs e)
        {
            if(_customerManager.Customer == null) return;
            var customer = _customerManager.Customer;
            _customerManager.AddOrUpdateCustomer(out string message);
            MessageBox.Show(message,ProductInformation.ProductName, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
