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
    public interface IOrder
    {
        ICustomer Customer { get; }
        bool IsCompleted { get; set; }
        string OrderStatus { get;}
        string OrderID { get; }
        string CustomerID { get; }
        float TotalAmount { get; }
        float PaidAmount { get; }
        List<float> PaidAmountInstallments { get; }
        float RemainingAmount { get; }
        float DiscountGiven { get; }
        List<IJewellery> PurchasedJewelleries { get; }
        DateTime OrderPlacedDate { get; }
        DateTime OrderToBeCompleteDate { get; }
        DateTime OrderCompletedDate { get; }
        void AddJewellery(IJewellery jewellery);
        void RemoveJewellery(IJewellery jewellery);
        void UpdatePaidAmount();

    }
    [DataContract]
    public class Order : CommonComponent, IOrder
    {
        private bool _isCompleted;
        private string _orderID;
        private string _orderStatus;
        private float _totalAmount;
        private List<float> _paidAmountInstallments;
        private List<IJewellery> _purchasedJewelleries;
        private DateTime _orderPlacedDate = DateTime.Now;
        private DateTime _orderToBeCompleteDate = DateTime.Now;
        private DateTime _orderCompletedDate;
        private float _paidAmount = 0;
        private float _discountGiven = 0;
        public Order(ICustomer customer)
        {
            Customer = customer;
            _orderID = $"{Customer.OrderList.Count + 1:D2}";
            OrderStatus = "new";
            _purchasedJewelleries = new List<IJewellery>();
            _paidAmountInstallments = new List<float> { 0 };
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
                if (IsCompleted)
                {
                    OrderStatus = "completed";
                    OrderCompletedDate = DateTime.Now;
                }
                else
                    OrderStatus = "pending";
            }
        }
        [DataMember]
        public float PaidAmount
        {
            get => _paidAmount;
            set
            {
                if (_paidAmount == value) return;
                _paidAmount = value;
                OnPropertyChanged(nameof(PaidAmount));
                if (RemainingAmount == 0)
                    PaidAmount = PaidAmountInstallments.Sum();
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
        [IgnoreDataMember]
        public float TotalAmount
        {
            get => GetTotalAmount();
        }
        [DataMember]
        public List<float> PaidAmountInstallments
        {
            get => _paidAmountInstallments;
            private set
            {
                if (_paidAmountInstallments == value)
                    return;
                _paidAmountInstallments = value;
                OnPropertyChanged(nameof(PaidAmountInstallments));
            }
        }
        [DataMember]
        public float DiscountGiven
        {
            get => _discountGiven;
            set
            {
                if (value == _discountGiven) return;
                _discountGiven = value;
                OnPropertyChanged(nameof(DiscountGiven));
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(RemainingAmount));
            }
        }
        [IgnoreDataMember]
        public float RemainingAmount => TotalAmount - PaidAmountInstallments.Sum() - DiscountGiven;
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
        [IgnoreDataMember]
        public ObservableCollection<IJewellery> PurchasedJewelleriesObservable
        {
            get => new ObservableCollection<IJewellery>(PurchasedJewelleries);
        }
        [DataMember]
        public DateTime OrderPlacedDate
        {
            get => _orderPlacedDate;
            set
            {
                if (value == _orderPlacedDate) return;
                if (value.Date < DateTime.Today) return;
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
                if (value.Date < DateTime.Today) return;
                if (value.Date < OrderPlacedDate.Date) return;
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
        public void UpdatePaidAmount()
        {
            if (PaidAmount <= 0 || PaidAmount > RemainingAmount)
                return;
            else
                PaidAmountInstallments.Add(PaidAmount);
            OnPropertyChanged(nameof(PaidAmountInstallments));
            OnPropertyChanged(nameof(RemainingAmount));
            PaidAmount = 0;
        }
        public void AddJewellery(IJewellery jewellery)
        {
            PurchasedJewelleries.Add(jewellery);
            OnPropertyChanged(nameof(PurchasedJewelleries));
            OnPropertyChanged(nameof(PurchasedJewelleriesObservable));
            OnPropertyChanged(nameof(TotalAmount));
            OnPropertyChanged(nameof(RemainingAmount));

        }
        public void RemoveJewellery(IJewellery jewellery)
        {
            if (PurchasedJewelleries.Any(o => o.Ornament.Name == jewellery.Ornament.Name && o.Weight == jewellery.Weight && o.TotalAmount == o.TotalAmount))
            {
                if (IsCompleted)
                    return;
                var jewellertToRemove = PurchasedJewelleries.Where(o => o.Ornament.Name == jewellery.Ornament.Name && o.Weight == jewellery.Weight && o.TotalAmount == o.TotalAmount).FirstOrDefault();
                PurchasedJewelleries.Remove(jewellertToRemove);
                OnPropertyChanged(nameof(PurchasedJewelleries));
                OnPropertyChanged(nameof(PurchasedJewelleriesObservable));
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(RemainingAmount));              
            }
        }
        private float GetTotalAmount() => PurchasedJewelleries.Sum(o => o.TotalAmount);
        public override bool Equals(object? obj)
        {
            var otherOrder = obj as IOrder;
            if (otherOrder == null) return false;
            return this.OrderID == otherOrder.OrderID;
        }

    }
}
