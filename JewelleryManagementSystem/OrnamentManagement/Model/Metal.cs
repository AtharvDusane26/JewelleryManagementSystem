﻿using System.IO;
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
        public string MetalID { get;}
        public string MetalName { get; set; }
        public WeightType WeightTypeForRate { get; set; }
        public float MetalRate { get; set; }
        IMetal Clone();
    }
    public interface INewMetal : IMetal
    {
        public float MakingCharges { get; set; }
        public WeightType WeightTypeForMaking { get; set; }

    }
    public interface IOldMetal : IMetal
    {

    }
    [DataContract]
    public class Metal : CommonComponent, INewMetal
    {
        private string _metalName;
        private float _metalRate;
        private float _makingCharges;
        private WeightType _weightTypeForRate;
        public WeightType _weightTypeForMaking;
        public Metal()
        {
            MetalID = Guid.NewGuid().ToString();
        }
        [DataMember]
        public string MetalID
        {
            get;private set;
        }
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
                MetalID = this.MetalID,
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
    [DataContract]
    public class OldMetal : CommonComponent, IOldMetal
    {

        private string _metalName;
        private float _metalRate;
        private WeightType _weightTypeForRate;
        public OldMetal()
        {
            MetalID = Guid.NewGuid().ToString();
        }
        [DataMember]
        public string MetalID
        {
            get; private set;
        }
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
        public IMetal Clone()
        {
            return new OldMetal()
            {
                MetalID = this.MetalID,
                MetalName = this.MetalName,
                WeightTypeForRate = this.WeightTypeForRate,
                MetalRate = this.MetalRate,
            };
        }
        public override string ToString()
        {
            return MetalName;
        }
    }


}
