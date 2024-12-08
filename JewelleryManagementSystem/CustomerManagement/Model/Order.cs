using JewelleryManagementSystem.ModelUtilities;
using JewelleryManagementSystem.OrnamentManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem.CustomerManagement.Model
{
    public interface IOrder
    {
        ICustomer Customer { get; }
        bool IsCompleted { get; }
        string OrderStatus { get; }
        string OrderID { get; }
        string CustomerID { get; }
        float TotalAmount { get; }
        float PaidAmount { get; }
        float RemainingAmount { get; }
        List<IJewellery> PurchasedJewelleries { get; }
        DateTime OrderPlacedDate { get; }
        DateTime OrderToBeCompleteDate { get; }
        DateTime OrderCompletedDate { get; }

    }
    [DataContract]
    public class Order : CommonComponent, IOrder
    {
        private bool _isCompleted;
        private string _orderID;
        private string _orderStatus;
        private float _totalAmount;
        private float _totalPaidAmount;
        private List<IJewellery> _purchasedJewelleries;
        private DateTime _orderPlacedDate;
        private DateTime _orderToBeCompleteDate;
        private DateTime _orderCompletedDate;
        public Order(ICustomer customer)
        {
            Customer = customer;
            _orderID = $"{Customer.OrderList.Count + 1:D2}";
            OrderStatus = "new";
        }
        [DataMember]
        public ICustomer Customer { get; private set; }
        [DataMember]
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted == value)
                    return;
                _isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));

            }
        }
        [DataMember]
        public string OrderID
        {
            get => _orderID;
            private set
            {
                if (_orderID == value)
                    return;
                _orderID = value;
                OnPropertyChanged(nameof(OrderID));
            }
        }
        [DataMember]
        public string OrderStatus
        {
            get => _orderStatus;
            set
            {
                if (value == _orderStatus) return;
                _orderStatus = value;
                OnPropertyChanged(nameof(OrderStatus));
            }
        }
        [DataMember]
        public float TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (value == _totalAmount)
                    return;
                _totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
            }
        }
        [DataMember]
        public float PaidAmount
        {
            get => _totalPaidAmount;
            set
            {
                if (_totalPaidAmount == value)
                    return;
                _totalPaidAmount = value;
                OnPropertyChanged(nameof(PaidAmount));
            }
        }
        [IgnoreDataMember]
        public float RemainingAmount => TotalAmount - PaidAmount;
        [IgnoreDataMember]
        public string CustomerID => Customer.CustomerID;

        [DataMember]
        public List<IJewellery> PurchasedJewelleries
        {
            get => _purchasedJewelleries;
            set
            {
                if (_purchasedJewelleries == value) return;
                _purchasedJewelleries = value;
                OnPropertyChanged(nameof(PurchasedJewelleries));
            }
        }
        [DataMember]
        public DateTime OrderPlacedDate
        {
            get => _orderPlacedDate;
            set
            {
                if (value == _orderPlacedDate) return;
                _orderPlacedDate = value;
                OnPropertyChanged(nameof(OrderPlacedDate));
            }
        }
        [DataMember]
        public DateTime OrderToBeCompleteDate
        {
            get => _orderToBeCompleteDate;
            set
            {
                if (_orderToBeCompleteDate == value) return;
                _orderToBeCompleteDate = value;
                OnPropertyChanged(nameof(OrderToBeCompleteDate));
            }
        }
        [DataMember]
        public DateTime OrderCompletedDate
        {
            get => _orderCompletedDate;
            set
            {
                if (value == _orderCompletedDate) return;
                _orderCompletedDate = value;
                OnPropertyChanged(nameof(OrderCompletedDate));

            }
        }
        public override bool Equals(object? obj)
        {
            var otherOrder = obj as IOrder;
            if (otherOrder == null) return false;
            return this.OrderID == otherOrder.OrderID;
        }

    }
}
