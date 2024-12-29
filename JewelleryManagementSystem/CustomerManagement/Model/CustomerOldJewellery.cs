using JewelleryManagementSystem.ModelUtilities;
using JewelleryManagementSystem.OrnamentManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem.CustomerManagement.Model
{
    public interface IOldJewellery : IJewellery
    {
        IMetal Metal { get; }
        string OrnamentDescription { get; }
    }
    [DataContract]
    public class CustomerOldJewellery : CommonComponent, IOldJewellery
    {
        private float _netWeight;
        private WeightType _weightType = WeightType.InGram;
        private float _totalAmount = 0;
        private IMetal _metal;
        private string _ornamentDescription;
        [DataMember]
        public float NetWeight
        {
            get => _netWeight;
            set
            {
                if (_netWeight == value) return;
                _netWeight = value;
                OnPropertyChanged(nameof(NetWeight));
                OnPropertyChanged(nameof(TotalAmount));
            }
        }
        [IgnoreDataMember]
        public WeightType SelectedWeightType
        {
            get => WeightType.InGram;
        }
        [IgnoreDataMember]
        public float TotalAmount
        {
            get => UpdateTotalAmount();
        }
        [DataMember]
        public IMetal Metal
        {
            get => _metal;
            set
            {
                if (_metal == value) return;
                _metal = value;
                OnPropertyChanged(nameof(Metal));
            }
        }
        [DataMember]
        public string OrnamentDescription
        {
            get => _ornamentDescription;
            set
            {
                if (_ornamentDescription == value) return;
                _ornamentDescription = value;
                OnPropertyChanged(nameof(OrnamentDescription));
            }
        }
        private float UpdateTotalAmount()
        {
            _totalAmount = 0;
            if (Metal != null && NetWeight != 0)
            {
                var rate = Utilities.GetRatePerGram(Metal.MetalRate, Metal.WeightTypeForRate);
                _totalAmount = rate * NetWeight;
            }
            return _totalAmount;
        }
        public override string ToString()
        {
            return Metal?.MetalName;
        }
    }
}
