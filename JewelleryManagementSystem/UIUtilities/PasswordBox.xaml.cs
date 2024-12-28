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
using System.Windows.Shapes;

namespace JewelleryManagementSystem.UIUtilities
{
    /// <summary>
    /// Interaction logic for PasswordBoxWindow.xaml
    /// </summary>
    public partial class PasswordBoxWindow : Window
    {
        public PasswordBoxWindow()
        {
            InitializeComponent();
            Title = ProductInformation.Instance.ShopName;
            ProductInformation.Instance.OnShowMessageBox = ShowMessage;
        }
        public bool Proceed { get; private set; }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (GeneralSettings.Default.Password != null && GeneralSettings.Default.Password == txtPassword.Text)
            {
                DialogResult = Proceed = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect Password", ProductInformation.Instance.ShopName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ShowMessage(string message)
        {
            MessageBox.Show(message, ProductInformation.Instance.ShopName, MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
