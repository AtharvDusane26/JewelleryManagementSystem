using JewelleryManagementSystem.ModelUtilities;
using JewelleryManagementSystem.OrnamentManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem.CustomerManagement.Model
{
    public interface ICustomer
    {
        string CustomerID { get; }
        string CustomerName { get; }
        string CustomerAddress { get; }
        string CustomerEmail { get; }
        string CustomerPhoneNumber { get; }
        List<IOrder> OrderList { get; }
        // void UpdateCustomer();
    }
    [DataContract]
    public class Customer : CommonComponent, ICustomer
    {
        private string _customerID;
        private string _customerName;
        private string _customerAddress;
        private string _customerEmail;
        private string _customerPhoneNumber;
        private List<IOrder> _orders;
        public Customer(string customerID)
        {
            _customerID = customerID;
            _orders = new List<IOrder>();
        }
        [IgnoreDataMember]
        public ObservableCollection<IOrder> OrderListObservable => new ObservableCollection<IOrder>(_orders);
        [DataMember]
        public string CustomerID
        {
            get => _customerID;
            private set
            {
                if (_customerID == value)
                    return;
                _customerID = value;
                OnPropertyChanged(nameof(CustomerID));
            }
        }
        [DataMember]
        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (_customerName == value)
                    return;
                _customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }
        [DataMember]
        public string CustomerAddress
        {
            get => _customerAddress;
            set
            {
                if (value == _customerAddress)
                    return;
                _customerAddress = value;
                OnPropertyChanged(nameof(CustomerAddress));
            }
        }
        [DataMember]
        public string CustomerEmail
        {
            get => _customerEmail;
            set
            {
                if (value == _customerEmail)
                    return;
                _customerEmail = value;
                OnPropertyChanged(nameof(CustomerEmail));
            }
        }
        [DataMember]
        public string CustomerPhoneNumber
        {
            get => _customerPhoneNumber;
            set
            {
                if (_customerPhoneNumber == value)
                    return;
                _customerPhoneNumber = value;
                OnPropertyChanged(nameof(CustomerPhoneNumber));
            }
        }
        [DataMember]
        public List<IOrder> OrderList
        {
            get => _orders;
            set
            {
                if (value == _orders)
                    return;
                _orders = value;
                OnPropertyChanged(nameof(OrderList));
            }
        }
        public override bool Equals(object? obj)
        {
            var otherCustomer = obj as ICustomer;
            if (otherCustomer == null) return false;
            return this.CustomerID == otherCustomer.CustomerID;
        }

    }
}
