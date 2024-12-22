using JewelleryManagementSystem.OrnamentManagement.Model;
using JewelleryManagementSystem.Settings.Model;
using JewelleryManagementSystem.StockManagement;
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
    /// Interaction logic for OrnamentStockSetting.xaml
    /// </summary>
    public partial class OrnamentStockSetting : UserControl
    {
        private OrnamentStock _ornamentStock;
        public OrnamentStockSetting()
        {
            InitializeComponent();
            Loaded += (o, e) => Build();
        }
        private void Build()
        {
            cmbOrnaments.Items.Clear();
            cmbOrnaments.ItemsSource = OrnamentManager.Instance.AvailableOrnaments;
            cmbOrnaments.SelectedIndex = 0;
        }
        private void btnUpdateStock_Click(object sender, RoutedEventArgs e)
        {
            if(StockManager.Instance.AddOrUpdateStock(_ornamentStock))
            {
                MessageBox.Show($"Stock Updated for {_ornamentStock.Ornament}",ProductInformation.ProductName,MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else
                MessageBox.Show($"Unable to update for {_ornamentStock.Ornament}", ProductInformation.ProductName, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void cmbOrnaments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is ComboBox cmbOrnaments)
            {
                var selectedItem = cmbOrnaments.SelectedItem as Ornament;
                if (selectedItem != null)
                {
                   var stock = StockManager.Instance.OrnamentStocks.Where(o => o.Ornament == selectedItem.ToString()).FirstOrDefault();
                    if (stock == null)
                        stock = StockManager.Instance.GetNewOrnamentStock(selectedItem);
                    DataContext = _ornamentStock = stock;
                }
            }
        }
    }
}
