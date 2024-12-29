using JewelleryManagementSystem.CustomerManagement.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JewelleryManagementSystem.CustomerManagement.View
{
    /// <summary>
    /// Interaction logic for OldJewelleryControl.xaml
    /// </summary>
    public partial class OldJewelleryControl : UserControl
    {
        private IJewellery _jewellery;
        private OrderManager _orderManager;
        private Action OnRemoveJewellery;
        public OldJewelleryControl(IJewellery jewellery, OrderManager orderManager, Action onRemoveJewellery)
        {
            InitializeComponent();
            DataContext = _jewellery = jewellery;
            _orderManager = orderManager;
            OnRemoveJewellery = onRemoveJewellery;
            cmbMetal.ItemsSource = MetalManager.Instance.AvailableMetals.Where(o => o is IOldMetal).ToList();
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
               // ResetUI();
            }
        }
        private void ResetUI()
        {
            cmbMetal.SelectedIndex = -1;
            _jewellery = null;
            DataContext = null;
        }

        private void btnRemoveJewellery_Click(object sender, RoutedEventArgs e)
        {
            OnRemoveJewellery?.Invoke();
        }

        private void cmbMetal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is ComboBox cmbMetal)
            {
                var metal = cmbMetal.SelectedItem as IOldMetal;
                (_jewellery as CustomerOldJewellery).Metal = metal;
            }
        }
    }
}
