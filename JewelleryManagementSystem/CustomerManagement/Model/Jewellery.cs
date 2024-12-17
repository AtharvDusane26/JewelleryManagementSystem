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
        Ornament Ornament { get; set; }
        float Weight { get; }
        WeightType SelectedWeightType { get; }
        float TotalAmount { get; }
        string MakingChargesPerGram { get; }
    }
    [DataContract]
    public class Jewellery : CommonComponent, IJewellery
    {
        private Ornament _ornament;
        private float _totalAmount;
        private float _weight = 0f;
        [DataMember]
        public Ornament Ornament
        {
            get => _ornament;
            set
            {
                if (_ornament == value) return;
                _ornament = value;
                OnPropertyChanged(nameof(Ornament));
                CalculateTotalAmount();
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
                CalculateTotalAmount();
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
        [IgnoreDataMember]
        public WeightType SelectedWeightType => WeightType.InGram;
        [IgnoreDataMember]
        public string MakingChargesPerGram
        {
            get
            {
                if (Ornament != null && Ornament.MakingCharges != null)
                {
                    if (Ornament.MakingChargeType == WeightType.LumSumForMakingOnly)
                    {
                        return $"{Ornament.MakingCharges.Value} Rs. (lum sum)";
                    }
                    else
                    {
                        return $"{GetRatePerGram(Ornament.MakingCharges.Value, Ornament.MakingChargeType)} Rs. per gram";
                    }
                }
                return "0";
            }
        }
        public void CalculateTotalAmount()
        {
            if (Ornament != null && Weight != 0 && SelectedWeightType != null)
            {
                var metalRateInGram = GetRatePerGram(Ornament.Metal.MetalRate, Ornament.Metal.WeightTypeForRate);
                var weightInGram = GetRatePerGram(Weight, SelectedWeightType);
                if (Ornament.MakingChargeType == WeightType.LumSumForMakingOnly)
                {
                    TotalAmount = (metalRateInGram * weightInGram) + Ornament.MakingCharges.Value;
                }
                else
                {
                    var makingChagesInGram = GetRatePerGram(Ornament.MakingCharges.Value, Ornament.MakingChargeType);
                    TotalAmount = (metalRateInGram + makingChagesInGram) * weightInGram;
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
        public override string ToString()
        {
            return Ornament?.Name;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            var jewelleryObj = obj as IJewellery;
            if (jewelleryObj == null) return false;
            if(jewelleryObj.Ornament.ToString() == Ornament?.ToString() && jewelleryObj.Weight == Weight && jewelleryObj.TotalAmount == TotalAmount) return true;
            return false;
        }
    }
}
