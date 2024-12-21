using System.IO;
using System.Runtime.Serialization;
using JewelleryManagementSystem.ModelUtilities;

namespace JewelleryManagementSystem.OrnamentManagement.Model
{
    public enum WeightType
    {
        InMiliGram,
        InGram,
        InTenGram,
        InKiloGram,
        LumSumForMakingOnly
    }
    public interface IMetal
    {
        public string MetalName { get; set; }
        public float MakingCharges { get; set; }
        public WeightType WeightTypeForRate { get; set; }
        public WeightType WeightTypeForMaking { get; set; }
        public float MetalRate { get; set; }
        IMetal Clone();
    }
    [DataContract]
    public class Metal : CommonComponent, IMetal
    {
        private string _metalName;
        private float _metalRate;
        private float _makingCharges;
        private WeightType _weightTypeForRate;
        public WeightType _weightTypeForMaking;
        [DataMember]
        public string MetalName
        {
            get => _metalName;
            set
            {
                if (_metalName == value)
                    return;
                _metalName = value;
                OnPropertyChanged(nameof(MetalName));
            }
        }
        [DataMember]
        public float MetalRate
        {
            get => _metalRate;
            set
            {
                if (value == _metalRate)
                    return;
                _metalRate = value;
                OnPropertyChanged(nameof(MetalRate));
            }
        }
        [DataMember]
        public float MakingCharges
        {
            get => _makingCharges;
            set
            {
                if (value == _makingCharges)
                    return;
                _makingCharges = value;
                OnPropertyChanged(nameof(MakingCharges));
            }
        }
        [DataMember]
        public WeightType WeightTypeForRate
        {
            get => _weightTypeForRate;
            set
            {
                if (value == _weightTypeForRate)
                    return;
                _weightTypeForRate = value;
                OnPropertyChanged(nameof(WeightTypeForRate));
            }
        }
        [DataMember]
        public WeightType WeightTypeForMaking
        {
            get => _weightTypeForMaking;
            set
            {
                if (value == _weightTypeForMaking)
                    return;
                _weightTypeForMaking = value;
                OnPropertyChanged(nameof(WeightTypeForMaking));
            }
        }
        public IMetal Clone()
        {
            return new Metal()
            {
                MetalName = this.MetalName,
                MakingCharges = this.MakingCharges,
                WeightTypeForRate = this.WeightTypeForRate,
                MetalRate = this.MetalRate,
                WeightTypeForMaking = this.WeightTypeForMaking
            };
        }
        public override string ToString()
        {
            return MetalName;
        }
    }
   
}
