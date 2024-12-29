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
    /// Interaction logic for MetalSettingsControl.xaml
    /// </summary>
    public partial class MetalSettingsControl : UserControl
    {
        private IMetal _metal;
        private MetalManager _instance;
        public MetalSettingsControl()
        {
            InitializeComponent();
            Loaded +=(o,e)=> Build();
        }
        private void Build()
        {
            _instance = MetalManager.Instance;
            cmbEnumMakingWeightType.ItemsSource = Enum.GetValues<WeightType>();
            cmbEnumRateWeightType.ItemsSource = Enum.GetValues<WeightType>();
            cmbMetalList.ItemsSource = _instance.AvailableMetals;
        }
        
        private void cmbMetalList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is ComboBox cmbMetalList)
            {
                var selectedMetal = cmbMetalList.SelectedItem as IMetal;
                if (selectedMetal == null)
                    return;
                var metal = _instance.AvailableMetals.Where(o => o.MetalID == selectedMetal.MetalID). FirstOrDefault();
                if(metal != null)
                    DataContext = _metal = metal.Clone();
                if(selectedMetal is INewMetal)
                    chkNewMetal.IsChecked = true;
                else chkNewMetal.IsChecked = false;

            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cmbMetalList.SelectedIndex = -1;
            var isNewMetal = chkNewMetal.IsChecked.Value;
            DataContext = _metal = _instance.GetNewMetal(isNewMetal);          
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {        
            if(_instance.AddOrUpdateMetal(_metal))
            {
                MessageBox.Show("Updated Metal List", ProductInformation.Instance.OwnerName, MessageBoxButton.OK, MessageBoxImage.Information);
                Build();
            }
            else
                MessageBox.Show("Unable to update Metal List", ProductInformation.Instance.OwnerName, MessageBoxButton.OK, MessageBoxImage.Error);

        }
    }
}
