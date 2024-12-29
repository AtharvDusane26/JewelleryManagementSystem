using JewelleryManagementSystem.CustomerManagement.Model;
using JewelleryManagementSystem.ModelUtilities;
using JewelleryManagementSystem.OrnamentManagement.Model;
using JewelleryManagementSystem.Settings.Model;
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
        private Action UpdateCustomer;
        public NewOrderWindow(OrderManager orderManager, Action updateCustomer)
        {
            InitializeComponent();
            Title = ProductInformation.Instance.ShopName;
            _orderManager = orderManager;
            _order = orderManager.Order != null ? orderManager.Order : orderManager.GetNewOrder();
            DataContext = _order;
            UpdateCustomer = updateCustomer;
            Build();
            Closing += (o, e) =>
            {
                if (_order != null && _order.IsCompleted)
                    _order.UpdateStock();
            };
            jewelleryControl.OrderManager = _orderManager;
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
                    if (StockManager.Instance.CheckStockAvailability(selectedOrnament) == 0)
                    {
                        MessageBox.Show($"{selectedOrnament.Name} is out of stock", ProductInformation.Instance.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
                        jewelleryControl.cmbJewellery.SelectedIndex = -1;
                        return;
                    }
                    _jewellery = new Jewellery();
                    (_jewellery as INewJewellery).Ornament = selectedOrnament.Clone();
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
                if (_jewellery.NetWeight <= 0 || _jewellery.TotalAmount <= 0)
                { return; }
                var result = MessageBox.Show("Do you want to confirm", ProductInformation.Instance.ShopName, MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                    jewelleryControl.cmbJewellery.SelectedItem = _orderManager.OrnamentManager.AvailableOrnaments.Where(o => o.Equals((_jewellery as INewJewellery).Ornament)).FirstOrDefault();
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
            var result = MessageBox.Show("Do you want to confirm order?", ProductInformation.Instance.ShopName, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (_orderManager.AddOrUpdateOrder())
                {
                    MessageBox.Show("Order Confirmed", ProductInformation.Instance.ShopName, MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateCustomer?.Invoke();
                }
                else
                    MessageBox.Show("Order Cancelled", ProductInformation.Instance.ShopName, MessageBoxButton.OK, MessageBoxImage.Information);
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
                    if (_jewellery is INewJewellery newJewellery)
                    {
                        var window = new Window();
                        var content = new AddJewelleryControl(newJewellery, _orderManager, RemoveJewellery);
                        window.Content = content;
                        window.Owner = this;
                        window.Title = ProductInformation.Instance.ShopName;
                        window.ResizeMode = ResizeMode.NoResize;
                        window.MaxHeight = 220;
                        window.MaxWidth = 300;
                        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        window.Closing += (o, e) =>
                        {
                            if (_orderManager.Order is CommonComponent component)
                                component.OnAllPropertyChanged();
                        };
                        window.ShowDialog();
                    }
                    else if (_jewellery is IOldJewellery oldJewellery)
                    {
                        var window = new Window();                       
                        var content = new OldJewelleryControl(oldJewellery,_orderManager, RemoveJewellery);
                        window.Content = content;
                        window.Owner = this;
                        window.Title = ProductInformation.Instance.ShopName;
                        window.ResizeMode = ResizeMode.NoResize;
                        window.MaxHeight = 220;
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
}
