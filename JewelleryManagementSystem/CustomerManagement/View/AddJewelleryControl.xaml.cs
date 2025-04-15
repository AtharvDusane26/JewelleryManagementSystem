using JewelleryManagementSystem.CustomerManagement.Model;
using JewelleryManagementSystem.ModelUtilities;
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
    /// Interaction logic for AddJewelleryControl.xaml
    /// </summary>
    public partial class AddJewelleryControl : UserControl
    {
        public Action OnRemoveJewellery;
        private IJewellery _jewellery;
        private OrderManager _orderManager;
        public AddJewelleryControl()
        {
            InitializeComponent();
        }
        public OrderManager OrderManager
        {
            get => _orderManager;
            set => _orderManager = value;
        }
        public AddJewelleryControl(IJewellery jewellery, OrderManager orderManager, Action action) : this()
        {
            DataContext = _jewellery = jewellery;
            _orderManager = orderManager;
            Loaded += (o, e) =>
            {
                OnRemoveJewellery = action;
                cmbJewellery.Visibility = Visibility.Collapsed;
                txtJewellery.Visibility = Visibility.Visible;
                txtJewellery.Text = (jewellery as INewJewellery).Ornament.ToString();
                btnAddJewellery.Visibility = Visibility.Collapsed;
                btnRemoveJewellery.Visibility = Visibility.Visible;
                btnRemoveJewellery.Click += (o, e) => OnRemoveJewellery?.Invoke();
            };
        }

        private void btnAddOldJewellery_Click(object sender, RoutedEventArgs e)
        {
            var jewellery = new CustomerOldJewellery();
            var window = new Window();
            var content = new OldJewelleryControl(jewellery, _orderManager, OnRemoveJewellery);
            window.Content = content;
            window.Owner = Window.GetWindow(this);
            window.Title = ProductInformation.Instance.ShopName;
            window.ResizeMode = ResizeMode.NoResize;
            window.MaxHeight = 220;
            window.MaxWidth = 350;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Closing += (o, e) =>
            {
                if (_orderManager?.Order is CommonComponent component)
                    component.OnAllPropertyChanged();
            };
            window.ShowDialog();
        }
    }
}
