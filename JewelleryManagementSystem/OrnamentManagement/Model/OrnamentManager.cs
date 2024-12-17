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
    public sealed class OrnamentManager : CommonComponent
    {
        private List<Ornament> _availableOrnaments;
        private static OrnamentManager _instance;
        private string _filePath = Path.Combine(DirectoryPath.OrnamentDirectory, "Ornaments.xml");
        private OrnamentManager()
        {
            LoadDefaultOrnaments();
        }
        public List<Ornament> AvailableOrnaments => _availableOrnaments;
        public static OrnamentManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new OrnamentManager();
                return _instance;
            }
        }
        public void AddOrUpdateOrnament(string name, Metal metal)
        {
            var ornament = GetOrnamentIfPresent(name, metal);
            if (ornament != null)
            {
                ornament.Name = name;
                ornament.Metal = metal;
            }
            else
            {
                ornament = new Ornament()
                {
                    Name = name,
                    Metal = metal
                };
                _availableOrnaments.Add(ornament);
                Save();
            }
        }
        public void AddOrUpdateOrnament(string name, Metal metal, WeightType makingChargeType, float makingCharge)
        {
            var ornament = GetOrnamentIfPresent(name, metal);
            if (ornament != null)
            {
                ornament.Name = name;
                ornament.Metal = metal;
                ornament.MakingCharges = makingCharge;
                ornament.MakingChargeType = makingChargeType;
            }
            else
            {
                ornament = new Ornament()
                {
                    Name = name,
                    Metal = metal,
                    MakingCharges = makingCharge,
                    MakingChargeType = makingChargeType
                };
                _availableOrnaments.Add(ornament);
                Save();
            }
        }
        private Ornament GetOrnamentIfPresent(string name, Metal metal)
        {
            Ornament ornament = _availableOrnaments.FirstOrDefault(o => o.Name == name && o.Metal.MetalName == metal.MetalName);
            return ornament;
        }
        private void Save()
        {
            try
            {
                DataManager.Save<List<Ornament>>(AvailableOrnaments, _filePath);
            }
            catch (Exception ex)
            {
                OnShowMessageBox?.Invoke($"Unable to save ornaments,{ex.Message}");
            }
        }
        private void LoadDefaultOrnaments()
        {
            Load();
            if (_availableOrnaments.Count == 0)
            {
                _availableOrnaments.Add(new Ornament { Name = "Gold Ring", Metal = MetalManager.Instance.AvailableMetals.Where(metal => metal.MetalName == "Gold 22 cr.").FirstOrDefault().Clone() });
                _availableOrnaments.Add(new Ornament
                {
                    Name = "Gold Vedha",
                    Metal = MetalManager.Instance.AvailableMetals.Where(metal => metal.MetalName == "Gold 24 cr.").FirstOrDefault().Clone(),
                    MakingCharges = 0,
                    MakingChargeType
                 = WeightType.InGram
                });
                _availableOrnaments.Add(new Ornament { Name = "Silver Painjan", Metal = MetalManager.Instance.AvailableMetals.Where(metal => metal.MetalName.Trim() == "Silver").FirstOrDefault().Clone() });
                Save();
            }
        }
        private void Load()
        {
            try
            {
                var list = DataManager.Read<List<Ornament>>(_filePath);
                _availableOrnaments = list != null ? list : new List<Ornament>();
            }
            catch (Exception ex)
            {
                _availableOrnaments = new List<Ornament>();
                OnShowMessageBox?.Invoke($"Unable to load ornaments,{ex.Message}");
            }
        }

    }
}
