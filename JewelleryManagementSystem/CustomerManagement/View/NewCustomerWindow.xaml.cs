﻿using JewelleryManagementSystem.CustomerManagement.Model;
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
                    if (order.IsCompleted)
                        return;
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
            if (_customerManager.Customer == null) return;
            var customer = _customerManager.Customer;
            _customerManager.AddOrUpdateCustomer(out string message);
            MessageBox.Show(message, ProductInformation.ProductName, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            var order = dataGridOrders.SelectedItem as IOrder;
            if (order != null && !order.IsCompleted)
            {
                var result = MessageBox.Show("Are you sure,do you want to delete order ?", ProductInformation.ProductName, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _customer.OrderList.Remove(order);
                    MessageBox.Show("Order Deleted", ProductInformation.ProductName, MessageBoxButton.OK, MessageBoxImage.Information);
                    if (_customer is CommonComponent component)
                        component.OnAllPropertyChanged();
                }

            }
            else
            {
                if (order == null)
                {
                    MessageBox.Show("Please select the order to be delete", ProductInformation.ProductName, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (order.IsCompleted)
                    MessageBox.Show("Completed order could not be delete", ProductInformation.ProductName, MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void btnCreateReciept_Click(object sender, RoutedEventArgs e)
        {
            var order = dataGridOrders.SelectedItem as IOrder;
            if (order != null)
            {
                var reciept = new Reciept(_customer.CustomerName, order.OrderID, _customer.CustomerID)
                {
                    PurchasedJewelleries = order.PurchasedJewelleries,
                    TotalAmount = order.TotalAmount,
                    RemainingAmount = order.RemainingAmount,
                    DiscountGiven = order.DiscountGiven,
                    OrderStatus = order.OrderStatus,
                };
                var window = new RecieptWindow(reciept);
                window.Title = ProductInformation.ProductName;
                window.Owner = this;
                window.ShowInTaskbar = false;
                window.ResizeMode = ResizeMode.CanMinimize;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Show();
            }
            else
                MessageBox.Show("Please select the order to be create reciept", ProductInformation.ProductName, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
