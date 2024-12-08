﻿using JewelleryManagementSystem.CustomerManagement.Model;
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
        public NewCustomerWindow(CustomerManager customerManager)
        {
            InitializeComponent();
            _customerManager = customerManager;
            DataContext = _customerManager.GetNewCustomer();
            _customerManager.OrderManager.Build(_customerManager.Customer);
            UpdateDataGridVisibility();
        }

        private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var newOrderWindow = new NewOrderWindow(_customerManager.OrderManager);
            newOrderWindow.Owner = this;
            newOrderWindow.ShowDialog();
            UpdateDataGridVisibility();
        }
        private void UpdateDataGridVisibility()
        {
            if(_customerManager.Customer.OrderList != null && _customerManager.Customer.OrderList.Count > 0)
                dataGridOrders.Visibility = Visibility.Visible;
            dataGridOrders.Visibility = Visibility.Collapsed;

        }
    }
}