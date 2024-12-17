using JewelleryManagementSystem.ModelUtilities;
using JewelleryManagementSystem.OrnamentManagement.Model;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Xaml;

namespace JewelleryManagementSystem.CustomerManagement.Model
{
    public class OrderManager : CommonComponent
    {
        private IOrder _order;
        private List<IOrder> _orders;
        public List<IOrder> OrderList => _orders;

        public IOrder Order
        {
            get => _order;
        }
        public OrnamentManager OrnamentManager => OrnamentManager.Instance;
        public MetalManager MetalManager => MetalManager.Instance;
        public ICustomer Customer { get; private set; }
        public void Build(ICustomer customer)
        {
            if (customer == null)
                return;
            Customer = customer;
            _orders = customer.OrderList;
            _orders = Customer.OrderList;
        }
        public IOrder GetNewOrder()
        {
            return _order = new Order(Customer);
        }
        public bool AddOrUpdateOrder()
        {
            if (OrderList == null || Order == null)
                return false;
            if (!ValidateOrder(Order))
            {
                return false;
            }
            if (OrderList.Any(o => o.OrderID == Order.OrderID))
                OrderList.Remove(OrderList.Where(o => o.OrderID == Order.OrderID).FirstOrDefault());
            OrderList.Add(Order);
            if(Customer is CommonComponent component)
                component.OnAllPropertyChanged();
            Order.IsCompleted = true;
            return true;
        }
        private bool ValidateOrder(IOrder order)
        {
            if (order == null) return false;
            if (order.PurchasedJewelleries == null || order.PurchasedJewelleries.Count == 0) return false;
            return true;

        }
        public IOrder GetOrder(int id)
        {
            var order = OrderList.Where(o => o.OrderID.Equals(id)).FirstOrDefault();
            return order;
        }
        public void DeleteOrder(int id)
        {
            var customer = OrderList.Where(o => o.OrderID.Equals(id)).FirstOrDefault();
            if (customer != null)
                OrderList.Remove(customer);
        }
    }
}
