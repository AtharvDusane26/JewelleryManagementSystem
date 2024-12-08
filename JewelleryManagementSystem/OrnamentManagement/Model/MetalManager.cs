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
        public override string ToString()
        {
            return MetalName;
        }
    }
    public sealed class MetalManager
    {
        private List<IMetal> _availableMetals;
        private string _filePath = Path.Combine(DirectoryPath.MetalDirectory, "Metals.xml");

        private static MetalManager _instance;
        private MetalManager()
        {
            GetAvailableMetals();
        }
        public static MetalManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MetalManager();
                return _instance;
            }
        }
        public List<IMetal> AvailableMetals => _availableMetals;
        public bool AddOrUpdateMetal(string metalName, WeightType weightType, float metalRate, WeightType weightTypeForMaking, float makingCharges)
        {
            IMetal metal = null;
            if (string.IsNullOrWhiteSpace(metalName))
                return false;
            if (_availableMetals != null && _availableMetals.Any(o => o.MetalName.ToLower().Trim() == metalName.ToLower().Trim()))
            {
                metal = _availableMetals.FirstOrDefault(o => o.MetalName.ToLower().Trim() == metalName.ToLower().Trim());
                metal.MetalName = metalName;
                metal.MetalRate = metalRate;
                metal.MakingCharges = makingCharges;
                metal.WeightTypeForRate = weightType;
                metal.WeightTypeForMaking = weightTypeForMaking;
            }
            else
            {
                metal = new Metal();
                metal.MetalName = metalName;
                metal.MetalRate = metalRate;
                metal.MakingCharges = makingCharges;
                metal.WeightTypeForRate = weightType;
                metal.WeightTypeForMaking = weightTypeForMaking;
                _availableMetals.Add(metal);
            }
            SaveMetalList();
            return true;
        }
        private void SaveMetalList()
        {
            if (_availableMetals != null)
                DataManager.Save(_availableMetals, _filePath);
        }
        private void GetAvailableMetals()
        {
            var list = DataManager.Read<List<IMetal>>(_filePath);
            if (list == null)
                _availableMetals = new List<IMetal>();
            _availableMetals = list.ToList();
        }

    }
}
