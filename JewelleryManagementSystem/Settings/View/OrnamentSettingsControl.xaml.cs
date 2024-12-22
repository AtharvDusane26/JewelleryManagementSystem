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

namespace JewelleryManagementSystem.Settings.View
{
    /// <summary>
    /// Interaction logic for OrnamentSettingsControl.xaml
    /// </summary>
    public partial class OrnamentSettingsControl : UserControl
    {
        private OrnamentManager _instance;
        private Ornament _ornament;
        public OrnamentSettingsControl()
        {
            InitializeComponent();
            Loaded += (o, e) => Build();
        }
        public void Build()
        {
            _instance = OrnamentManager.Instance;
            cmbEnumMakingWeightType.ItemsSource = Enum.GetValues<WeightType>();
            cmbMetalList.ItemsSource = MetalManager.Instance.AvailableMetals;
            cmbOrnamentList.ItemsSource = _instance.AvailableOrnaments;

        }
        private void cmbOrnamentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox cmb)
            {
                var item = cmb.SelectedItem as Ornament;
                if (item != null)
                {
                    var presentItem = _instance.AvailableOrnaments.Where(o => o.OrnamentID == item.OrnamentID).FirstOrDefault();
                    if (presentItem != null)
                    {
                        DataContext = _ornament = presentItem.Clone();
                        cmbMetalList.SelectedItem = MetalManager.Instance.AvailableMetals.Where(o => o.MetalID == presentItem.Metal.MetalID).FirstOrDefault();                     
                    }
                }

            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (_instance.AddOrUpdateOrnament(_ornament))
            {
                MessageBox.Show("Updated Ornament List", ProductInformation.OwnerName, MessageBoxButton.OK, MessageBoxImage.Information);
                Build();
            }
            else
                MessageBox.Show("Unable to update Ornament List", ProductInformation.OwnerName, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cmbOrnamentList.SelectedIndex = -1;
            DataContext = _ornament = _instance.GetNewOrnament();
        }
    }
}
