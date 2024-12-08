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
    public interface IJewellery
    {
        Ornament Ornament { get; }
        float Weight { get; }
        WeightType WeightType { get; }
        float TotalAmount { get; }
    }
    [DataContract]
    public class Jewellery : CommonComponent, IJewellery
    {
        private Ornament _ornament;
        private float _totalAmount;
        private float _weight;
        private WeightType _weightType;
        [DataMember]
        public Ornament Ornament
        {
            get => _ornament;
            set
            {
                if (_ornament == value) return;
                _ornament = value;
                OnPropertyChanged(nameof(Ornament));
            }
        }
        [DataMember]
        public float Weight
        {
            get => _weight;
            set
            {
                if (_weight == value) return;
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }
        [DataMember]
        public float TotalAmount
        {
            get => _totalAmount;
            private set
            {
                if (_totalAmount == value) return;
                _totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
            }
        }
        [DataMember]
        public WeightType WeightType
        {
            get => _weightType;
            set
            {
                if (_weightType == value) return;
                _weightType = value;
                OnPropertyChanged(nameof(WeightType));
            }
        }
        public void CalculateTotalAmount()
        {
            if (Ornament != null)
            {
                var metalRateInGram = GetRatePerGram(Ornament.Metal.MetalRate, Ornament.Metal.WeightTypeForRate);
                var weightInGram = GetRatePerGram(Weight, WeightType);
                if (Ornament.MakingChargeType == WeightType.LumSumForMakingOnly)
                {
                    _totalAmount = (metalRateInGram * weightInGram) + Ornament.MakingCharges;
                }
                else
                {
                    var makingChagesInGram = GetRatePerGram(Ornament.MakingCharges, Ornament.MakingChargeType);
                    _totalAmount = (metalRateInGram + makingChagesInGram) * weightInGram;
                }
            }
        }
        private float GetRatePerGram(float rate, WeightType weightType)
        {
            switch (weightType)
            {
                case WeightType.InMiliGram:
                    return rate * 1000;
                case WeightType.InGram:
                    return rate;
                case WeightType.InTenGram:
                    return rate / 10;
                case WeightType.InKiloGram:
                    return rate / 1000;
                default:
                    return 0;
            }
        }

    }
}
