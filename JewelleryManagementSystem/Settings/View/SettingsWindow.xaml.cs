﻿using JewelleryManagementSystem.UIUtilities;
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

namespace JewelleryManagementSystem.Settings.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Title = ProductInformation.Instance.ShopName;
            Loaded += (o, e) => Build();
        }
        private void Build()
        {
        label1:
            var box = new PasswordBoxWindow();
            box.Owner = this;
            if (box.ShowDialog() == true)
            {
                if (box.Proceed)
                {
                    metalTab.Content = new MetalSettingsControl();
                    ornamentTab.Content = new OrnamentSettingsControl();
                    stockTab.Content = new OrnamentStockSetting();
                    generalSettingTab.Content = new GeneralSettingsControl();
                }
                else
                    goto label1;
            }

        }
    }
}
