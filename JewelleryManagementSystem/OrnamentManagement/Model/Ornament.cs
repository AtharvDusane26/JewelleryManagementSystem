using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using JewelleryManagementSystem.CustomerManagement.Model;
using System.Windows.Media.Media3D;
using JewelleryManagementSystem.ModelUtilities;

namespace JewelleryManagementSystem.OrnamentManagement.Model
{
    [DataContract]
    public class Ornament : CommonComponent
    {
        private IMetal _metal;
        private string _name;
        private WeightType _makingChargeType;
        private float? _makingCharges = null;

        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        [DataMember]
        public IMetal Metal
        {
            get => _metal;
            set
            {
                if (value == null)
                    return;
                _metal = value;
                OnPropertyChanged(nameof(Metal));
            }
        }
        [DataMember]
        public WeightType MakingChargeType
        {
            get => _makingChargeType;
            set
            {
                if (_makingChargeType == value)
                    return;
                _makingChargeType = value;
                OnPropertyChanged(nameof(MakingChargeType));
            }
        }
        [DataMember]
        public float? MakingCharges
        {
            get => _makingCharges;
            set
            {
                if (_makingCharges == value)
                    return;
                _makingCharges = value;
                OnPropertyChanged(nameof(MakingCharges));
            }
        }
        public void UpdatedRateAndMaking()
        {
            MakingChargeType = Metal.WeightTypeForMaking;
            MakingCharges = Metal.MakingCharges;
        }
        public Ornament Clone()
        {
            return new Ornament()
            {
                Name = this.Name,
                Metal = this.Metal.Clone(),
                MakingCharges = this.MakingCharges,
                MakingChargeType = this.MakingChargeType,
            };
        }
        public override string ToString()
        {
            return Name + $" ({Metal.ToString()})";
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            var ornamentObj = obj as Ornament;
            if (ornamentObj == null) return false;
            if (ornamentObj.ToString() == this.ToString() && ornamentObj.MakingCharges == MakingCharges) return true;
            return false;
        }
    }
 
}
