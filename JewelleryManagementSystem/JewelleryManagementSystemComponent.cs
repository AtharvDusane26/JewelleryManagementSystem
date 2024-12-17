using JewelleryManagementSystem.CustomerManagement.Model;
using JewelleryManagementSystem.ModelUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JewelleryManagementSystem
{
    public class JewelleryManagementSystemComponent : CommonComponent
    {
        private ObservableCollection<ICustomer> _customerList;
        public ObservableCollection<ICustomer> CustomerList => _customerList;
        public CustomerManager CustomerManager { get; private set; }
        internal void Build()
        {
            CustomerManager = new CustomerManager();
            CustomerManager.Build();
            if (CustomerManager.CustomerList != null)
                _customerList = new ObservableCollection<ICustomer>(CustomerManager.CustomerList);
            else
                _customerList = new ObservableCollection<ICustomer>();
            OnPropertyChanged(nameof(CustomerList));
        }

    }
}
