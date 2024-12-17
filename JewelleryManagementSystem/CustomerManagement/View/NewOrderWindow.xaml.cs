using JewelleryManagementSystem.CustomerManagement.Model;
using JewelleryManagementSystem.ModelUtilities;
using JewelleryManagementSystem.OrnamentManagement.Model;
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
    /// Interaction logic for NewOrderWindow.xaml
    /// </summary>
    public partial class NewOrderWindow : Window
    {
        private OrderManager _orderManager;
        private IOrder _order;
        private IJewellery _jewellery = null;
        public NewOrderWindow(OrderManager orderManager)
        {
            InitializeComponent();
            Title = ProductInformation.ProductName;
            _orderManager = orderManager;
            _order = orderManager.Order != null ? orderManager.Order : orderManager.GetNewOrder();
            DataContext = _order;
            Build();
        }
        private void Build()
        {
            jewelleryControl.cmbJewellery.ItemsSource = _orderManager.OrnamentManager.AvailableOrnaments;
            jewelleryControl.cmbJewellery.SelectionChanged += cmbJewellery_SelectionChanged;
            jewelleryControl.btnAddJewellery.Click += btnAddJewellery_Click;
        }

        private void cmbJewellery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                var selectedOrnament = comboBox.SelectedItem as Ornament;
                if ((selectedOrnament != null))
                {
                    if (selectedOrnament.MakingCharges == null)
                        selectedOrnament.UpdatedRateAndMaking();
                    _jewellery = new Jewellery();
                    _jewellery.Ornament = selectedOrnament.Clone();
                    jewelleryControl.DataContext = _jewellery;
                    if (_jewellery is CommonComponent component)
                        component.OnAllPropertyChanged();
                }
            }
        }

        private void btnAddJewellery_Click(object sender, RoutedEventArgs e)
        {
            if (_jewellery != null)
            {
                if (_jewellery.Weight <= 0 || _jewellery.TotalAmount <= 0)
                { return; }
                var result = MessageBox.Show("Do you want to confirm", ProductInformation.ProductName, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    _orderManager.Order.AddJewellery(_jewellery);
                ResetUI();
            }
        }

        private void btnUpdatePaid_Click(object sender, RoutedEventArgs e)
        {
            _orderManager.Order.UpdatePaidAmount();
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                _jewellery = grid.SelectedItem as IJewellery;
                if (_jewellery != null)
                {
                    jewelleryControl.cmbJewellery.SelectedItem = _orderManager.OrnamentManager.AvailableOrnaments.Where(o => o.Equals(_jewellery.Ornament)).FirstOrDefault();
                }
            }
        }
        private void ResetUI()
        {
            jewelleryControl.cmbJewellery.SelectedIndex = -1;
            _jewellery = null;
            jewelleryControl.DataContext = null;
        }

        private void RemoveJewellery()
        {
            if (_jewellery != null)
            {
                _orderManager.Order.RemoveJewellery(_jewellery);
                ResetUI();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_orderManager.Order.PurchasedJewelleries.Count <= 0)
                return;
            var result = MessageBox.Show("Do you want to confirm order?", ProductInformation.ProductName, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (_orderManager.AddOrUpdateOrder())
                    MessageBox.Show("Order Confirmed", ProductInformation.ProductName, MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Order Cancelled", ProductInformation.ProductName, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                _jewellery = grid.SelectedItem as IJewellery;
                if (_jewellery != null)
                {
                    if (_order.IsCompleted)
                        return;
                    var window = new Window();
                    var content = new AddJewelleryControl(_jewellery, RemoveJewellery);
                    window.Content = content;
                    window.Owner = this;
                    window.Title = ProductInformation.ProductName;
                    window.ResizeMode = ResizeMode.NoResize;
                    window.MaxHeight = 200;
                    window.MaxWidth = 300;
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Closing += (o, e) =>
                    {
                        if (_orderManager.Order is CommonComponent component)
                            component.OnAllPropertyChanged();
                    };
                    window.ShowDialog();
                }
            }
        }
    }
}
