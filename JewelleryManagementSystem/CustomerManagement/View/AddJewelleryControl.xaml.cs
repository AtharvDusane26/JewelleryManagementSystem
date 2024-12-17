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
        public AddJewelleryControl()
        {
            InitializeComponent();
        }
        public AddJewelleryControl(IJewellery jewellery, Action action) : this()
        {
            DataContext = jewellery;
            Loaded += (o, e) =>
            {
                OnRemoveJewellery = action;
                cmbJewellery.Visibility = Visibility.Collapsed;
                txtJewellery.Visibility = Visibility.Visible;
                txtJewellery.Text = jewellery.Ornament.ToString();
                btnAddJewellery.Visibility = Visibility.Collapsed;
                btnRemoveJewellery.Visibility = Visibility.Visible;
                btnRemoveJewellery.Click += (o, e) => OnRemoveJewellery?.Invoke();
            };
        }
    }
}
