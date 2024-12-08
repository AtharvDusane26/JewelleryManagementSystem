using JewelleryManagementSystem.ModelUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem.CustomerManagement.Model
{
    public class CustomerManager : CommonComponent
    {
        private ICustomer _customer;
        private OrderManager _orderManager;
        private List<ICustomer> _customerList;

        public OrderManager OrderManager => _orderManager;
        public List<ICustomer> CustomerList => _customerList;
        public ICustomer Customer
        {
            get => _customer;
        }
        internal void Build()
        {
            var list = GetCustomers();
            if (list == null)
                _customerList = new List<ICustomer>();
            _customerList = list;
            _orderManager = new OrderManager();
        }
        public ICustomer GetNewCustomer()
        {
            var id = CreateUniqueCustomerID();
            return _customer = new Customer(id);
        }
        private string CreateUniqueCustomerID()
        {
            var lastCusId = GeneralSettings.Default.LastCustomerID;
            if (String.IsNullOrWhiteSpace(lastCusId))
                return $"{DateTime.Now.ToString("yyyyMM")}{"0001"}";
            var dateSplit = lastCusId.Substring(0, 6);
            var counter = Convert.ToInt32(lastCusId.Substring(6));
            if (dateSplit != DateTime.Now.ToString("yyyyMM"))
                counter = 1;
            return $"{DateTime.Now.ToString("yyyyMM")}{counter:D4}";

        }
        public void AddOrUpdateCustomer(ICustomer customer)
        {
            if (CustomerList == null)
                return;
            if (CustomerList.Any(o => o.CustomerID == customer.CustomerID))
                CustomerList.Remove(CustomerList.Where(o => o.CustomerID == customer.CustomerID).FirstOrDefault());
            CustomerList.Add(customer);
            OnPropertyChanged(nameof(CustomerList));
            DataManager.Save<List<ICustomer>>(CustomerList.ToList(), DirectoryPath.CustomerDirectory);
        }
        public ICustomer GetCustomer(int id)
        {
            var customer = CustomerList.Where(o => o.CustomerID.Equals(id)).FirstOrDefault();
            return customer;
        }
        private List<ICustomer> GetCustomers()
        {
            try
            {
                var list = DataManager.Read<List<ICustomer>>(DirectoryPath.CustomerDirectory);
                return list;
            }
            catch (Exception ex)
            {
                ShowMessageBox("Unable to Load Order List");
                return null;
            }
        }
        public void DeleteCustomer(int id)
        {
            var customer = CustomerList.Where(o => o.CustomerID.Equals(id)).FirstOrDefault();
            if (customer != null)
                CustomerList.Remove(customer);
        }

    }
}
