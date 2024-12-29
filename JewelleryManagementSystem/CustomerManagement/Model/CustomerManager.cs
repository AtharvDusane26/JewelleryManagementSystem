using JewelleryManagementSystem.ModelUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem.CustomerManagement.Model
{
    public class CustomerManager : CommonComponent
    {
        private ICustomer _customer;
        private OrderManager _orderManager;
        private string _filePath = Path.Combine(DirectoryPath.CustomerDirectory, "Customer.xml");
        private List<ICustomer> _customerList;
        public OrderManager OrderManager => _orderManager;
        public List<ICustomer> CustomerList => _customerList;
        public ICustomer Customer
        {
            get => _customer;
            set => _customer = value;
        }
        internal void Build()
        {
            var list = GetCustomers();
            if (list == null)
                _customerList = new List<ICustomer>();
            else
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
            return $"{DateTime.Now.ToString("yyyyMM")}{++counter:D4}";

        }
        public void AddOrUpdateCustomer(out string errorMessage)
        {
            errorMessage = string.Empty;
            if (CustomerList == null)
            {
                errorMessage = "Invalid Customer List";
                return;
            }
            if (!ValidateCustomer())
            {
                errorMessage = "Invalid Customer Data";
                return;
            }
            if (CustomerList.Any(o => o.CustomerID == Customer.CustomerID))
                CustomerList.Remove(CustomerList.Where(o => o.CustomerID == Customer.CustomerID).FirstOrDefault());
            CustomerList.Add(Customer);
            OnPropertyChanged(nameof(CustomerList));
            DataManager.Save<List<ICustomer>>(CustomerList.ToList(), _filePath);
            errorMessage = "Updated Customer List";
            GeneralSettings.Default.LastCustomerID = Customer.CustomerID;
            GeneralSettings.Default.Save();
        }
        private bool ValidateCustomer()
        {
            if (Customer == null) return false;
            if (String.IsNullOrWhiteSpace(Customer.CustomerName)) return false;
            return true;
        }
        public ICustomer GetCustomer(string id)
        {
            var customer = CustomerList.Where(o => o.CustomerID.Equals(id)).FirstOrDefault();
            return customer;
        }
        private List<ICustomer> GetCustomers()
        {
            try
            {
                var list = DataManager.Read<List<ICustomer>>(_filePath); ;
                return list;
            }
            catch (Exception ex)
            {
                OnShowMessageBox?.Invoke("Unable to Load Order List");
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
