using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using JewelleryManagementSystem.ModelUtilities;

namespace JewelleryManagementSystem.OrnamentManagement.Model
{
    [DataContract]
    public class Ornament : CommonComponent
    {
        private IMetal _metal;
        private string _name;
        private WeightType _makingChargeType;
        private float _makingCharges;

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
                UpdatedRateAndMaking();
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
        public float MakingCharges
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
        private void UpdatedRateAndMaking()
        {
            MakingChargeType = Metal.WeightTypeForMaking;
            MakingCharges = Metal.MakingCharges;
        }
        public override string ToString()
        {
            return Metal.ToString() + " " + Name;
        }
    }
    public sealed class OrnamentManager
    {
        private List<Ornament> _availableOrnaments;
        private OrnamentManager _instance;
        private string _filePath = Path.Combine(DirectoryPath.OrnamentDirectory, "Ornaments.xml");
        private OrnamentManager()
        {
            Load();
        }
        public List<Ornament> AvailableOrnaments => _availableOrnaments;
        public OrnamentManager Instance
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
            DataManager.Save<List<Ornament>>(AvailableOrnaments, _filePath);
        }
        private void Load()
        {
            var list = DataManager.Read<List<Ornament>>(_filePath);
            _availableOrnaments = list != null ? list : new List<Ornament>();
        }
    }
}
