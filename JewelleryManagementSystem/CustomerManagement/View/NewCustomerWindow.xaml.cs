using JewelleryManagementSystem.CustomerManagement.Model;
using JewelleryManagementSystem.ModelUtilities;
using JewelleryManagementSystem.RecieptManager;
using JewelleryManagementSystem.RecieptManager.View;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Security.Cryptography;
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
using System.Windows.Xps.Packaging;

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
            Title = ProductInformation.Instance.ShopName;
            _customerManager = customerManager;
            _customer = customerManager.Customer != null ? customerManager.Customer : _customerManager.GetNewCustomer();
            DataContext = _customer;
            _customerManager.OrderManager.Build(_customer);
            UpdateDataGridVisibility();
        }

        private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var noUse = _customerManager.OrderManager.GetNewOrder();
            var newOrderWindow = new NewOrderWindow(_customerManager.OrderManager, Update);
            newOrderWindow.Owner = this;
            newOrderWindow.ShowDialog();
            UpdateDataGridVisibility();
        }
        private void UpdateDataGridVisibility()
        {
            if (_customerManager.Customer is IOrderCustomer orderCustomer)
            {
                if (orderCustomer.OrderList != null && orderCustomer.OrderList.Count > 0)
                    dataGridOrders.Visibility = Visibility.Visible;
                else
                    dataGridOrders.Visibility = Visibility.Collapsed;
            }

        }

        private void dataGridOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                IOrder order = grid.SelectedItem as IOrder;
                if (order != null)
                {
                    if (order.IsCompleted)
                        return;
                    _customerManager.OrderManager.Order = order;
                    var newOrderWindow = new NewOrderWindow(_customerManager.OrderManager, Update);
                    newOrderWindow.Owner = this;
                    newOrderWindow.btnAddOrder.IsEnabled = !_customerManager.OrderManager.Order.IsCompleted;
                    newOrderWindow.btnAddOrder.Content = "Update";
                    newOrderWindow.ShowDialog();
                    UpdateDataGridVisibility();
                }
            }
        }
        private void Update()
        {
            _customerManager.AddOrUpdateCustomer(out string message);
            MessageBox.Show(message, ProductInformation.Instance.ShopName, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void btnCreateCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (_customerManager.Customer == null) return;
            var customer = _customerManager.Customer;
            _customerManager.AddOrUpdateCustomer(out string message);
            MessageBox.Show(message, ProductInformation.Instance.ShopName, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            var order = dataGridOrders.SelectedItem as IOrder;
            if (order != null && !order.IsCompleted)
            {
                var result = MessageBox.Show("Are you sure,do you want to delete order ?", ProductInformation.Instance.ShopName, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (_customer is IOrderCustomer orderCustomer)
                    {
                        orderCustomer.OrderList.Remove(order);
                        MessageBox.Show("Order Deleted", ProductInformation.Instance.ShopName, MessageBoxButton.OK, MessageBoxImage.Information);
                        if (_customer is CommonComponent component)
                            component.OnAllPropertyChanged();
                    }
                }

            }
            else
            {
                if (order == null)
                {
                    MessageBox.Show("Please select the order to be delete", ProductInformation.Instance.ShopName, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (order.IsCompleted)
                    MessageBox.Show("Completed order could not be delete", ProductInformation.Instance.ShopName, MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void btnCreateReciept_Click(object sender, RoutedEventArgs e)
        {
            var order = dataGridOrders.SelectedItem as IOrder;
            if (order != null)
            {
                if (order.PurchasedJewelleries.Any(o => o is INewJewellery))
                {
                    IReciept reciept = null;

                    var result = MessageBox.Show("Do you want to create GST Bill?", ProductInformation.Instance.ShopName, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        order.GSTOrderBillCreated = true;
                        reciept = new GSTReciept(_customer.CustomerName, order.OrderID, _customer.CustomerID)
                        {
                            PurchasedJewelleries = order.PurchasedJewelleries,
                            TotalAmount = order.TotalAmount,
                            RemainingAmount = order.RemainingAmount,
                            DiscountGiven = order.DiscountGiven,
                            OrderStatus = order.OrderStatus,
                            OldJewelleryAmount = order.OldJewelleriesAmount,
                            GstAmount = order.GstAmount
                        };

                    }
                    else if (result == MessageBoxResult.No)
                    {
                        order.GSTOrderBillCreated = false;
                        reciept = new Reciept(_customer.CustomerName, order.OrderID, _customer.CustomerID)
                        {
                            PurchasedJewelleries = order.PurchasedJewelleries,
                            TotalAmount = order.TotalAmount,
                            RemainingAmount = order.RemainingAmount,
                            DiscountGiven = order.DiscountGiven,
                            OrderStatus = order.OrderStatus,
                            OldJewelleryAmount = order.OldJewelleriesAmount,
                        };
                    }
                    var window = new RecieptWindow(reciept);
                    window.Title = ProductInformation.Instance.ShopName;
                    window.Owner = this;
                    window.ShowInTaskbar = false;
                    window.ResizeMode = ResizeMode.CanMinimize;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.Show();

                }
                else
                    MessageBox.Show("Not purchase any item", ProductInformation.Instance.ShopName, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Please select the order to be create reciept", ProductInformation.Instance.ShopName, MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
