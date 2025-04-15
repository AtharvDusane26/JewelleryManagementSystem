using JewelleryManagementSystem.ModelUtilities;
using JewelleryManagementSystem.OrnamentManagement.Model;
using JewelleryManagementSystem.RecieptManager;
using JewelleryManagementSystem.Settings.Model;
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
        bool? GSTOrderBillCreated { get; set; }
        IOrderCustomer Customer { get; }
        bool IsCompleted { get; set; }
        string OrderStatus { get; }
        string OrderID { get; }
        string CustomerID { get; }
        float TotalAmount { get; }
        float OldJewelleriesAmount { get; }
        float PaidAmount { get; }
        float GstAmount { get; }
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
        void UpdateStock();

    }
    [DataContract]
    public class Order : CommonComponent, IOrder
    {
        private bool _isCompleted;
        private string _orderID;
        private string _orderStatus;
        private List<float> _paidAmountInstallments;
        private List<IJewellery> _purchasedJewelleries;
        private DateTime _orderPlacedDate = DateTime.Now;
        private DateTime _orderToBeCompleteDate = DateTime.Now;
        private DateTime _orderCompletedDate;
        private float _paidAmount = 0;
        private float _discountGiven = 0;
        private string _customerId;
        private bool? _gSTBillCreated = null;
        public Order(ICustomer customer)
        {
            Customer = customer as IOrderCustomer;
            _customerId = customer.CustomerID;
            _orderID = $"{Customer.OrderList.Count + 1:D2}";
            OrderStatus = "new";
            _purchasedJewelleries = new List<IJewellery>();
            _paidAmountInstallments = new List<float> { 0 };
        }
        [IgnoreDataMember]
        public IOrderCustomer Customer { get; private set; }
        [DataMember]
        public bool? GSTOrderBillCreated
        {
            get => _gSTBillCreated;
            set
            {
                if (_gSTBillCreated == value) return;
                _gSTBillCreated = value;
                OnPropertyChanged(nameof(GSTOrderBillCreated));
                if (_gSTBillCreated == true)
                    PaidAmount += GstAmount;
            }
        }
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
                OnPropertyChanged(nameof(PaidAmount));
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
                if (RemainingAmount == 0 && PaidAmountInstallments != null)
                {
                    PaidAmount = PaidAmountInstallments.Sum();
                }
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
        [IgnoreDataMember]
        public float GstAmount
        {
            get => GetGstAmount();
        }
        [IgnoreDataMember]
        public float OldJewelleriesAmount
        {
            get => GetPurchasedJewelleriesAmount();
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
        public float RemainingAmount
        {
            get
            {
                if (PaidAmountInstallments != null)
                {
                    return TotalAmount - PaidAmountInstallments.Sum() - DiscountGiven - OldJewelleriesAmount;
                }
                return 0;
            }
        }
        [IgnoreDataMember]
        public string CustomerID => _customerId;

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
            OrderStatus = "pending";
            PurchasedJewelleries.Add(jewellery);
            OnPropertyChanged(nameof(PurchasedJewelleries));
            OnPropertyChanged(nameof(PurchasedJewelleriesObservable));
            OnPropertyChanged(nameof(TotalAmount));
            OnPropertyChanged(nameof(RemainingAmount));

        }
        public void RemoveJewellery(IJewellery newJewellery)
        {
            if (newJewellery is INewJewellery jewellery)
            {
                if (PurchasedJewelleries.Any(o => (o as INewJewellery).Ornament.Name == jewellery.Ornament.Name && o.NetWeight == jewellery.NetWeight && o.TotalAmount == o.TotalAmount))
                {
                    if (IsCompleted)
                        return;
                    var jewellertToRemove = PurchasedJewelleries.Where(o => (o as INewJewellery).Ornament.Name == jewellery.Ornament.Name && o.NetWeight == jewellery.NetWeight && o.TotalAmount == o.TotalAmount).FirstOrDefault();
                    PurchasedJewelleries.Remove(jewellertToRemove);
                    OnPropertyChanged(nameof(PurchasedJewelleries));
                    OnPropertyChanged(nameof(PurchasedJewelleriesObservable));
                    OnPropertyChanged(nameof(TotalAmount));
                    OnPropertyChanged(nameof(RemainingAmount));
                }
            }
        }
        private float GetGstAmount()
        {
            float gstAmount = 0f;
            foreach (var item in PurchasedJewelleries)
            {
                if (item is INewJewellery newJwl)
                {
                    gstAmount += ProductInformation.Instance.GSTInformation.GetGSTAmount(newJwl.NetAmount);
                }
            }
            return gstAmount;
        }
        public float GetPurchasedJewelleriesAmount()
        {
            float amount = 0;
            if (PurchasedJewelleries != null)
            {
                foreach (var item in PurchasedJewelleries)
                {
                    if (item is IOldJewellery)
                    {
                        amount += item.TotalAmount;
                    }
                }
            }
            return amount;
        }
        public float GetTotalAmount()
        {
            float totalAmount = 0;
            if (PurchasedJewelleries != null)
            {
                foreach (var item in PurchasedJewelleries)
                {
                    if (item is INewJewellery)
                    {
                        totalAmount += item.TotalAmount;
                    }
                }
            }
            return totalAmount;
        }
        public void UpdateStock()
        {
            if (IsCompleted)
            {
                foreach (var item in PurchasedJewelleries.Where(o => o is INewJewellery).ToList())
                {
                    var stock = StockManager.Instance.OrnamentStocks.Where(o => o.Ornament == (item as INewJewellery).Ornament.ToString()).FirstOrDefault();
                    if (stock != null)
                    {
                        stock.AvailableStock -= 1;
                        StockManager.Instance.AddOrUpdateStock(stock);
                    }
                }

            }
        }
        public override string ToString()
        {
            return this.OrderID.ToString();
        }
        public override bool Equals(object? obj)
        {
            var otherOrder = obj as IOrder;
            if (otherOrder == null) return false;
            return this.OrderID == otherOrder.OrderID;
        }

    }
}
