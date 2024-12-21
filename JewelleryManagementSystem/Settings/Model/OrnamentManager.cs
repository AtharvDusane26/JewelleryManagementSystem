using JewelleryManagementSystem.ModelUtilities;
using JewelleryManagementSystem.OrnamentManagement.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem.Settings.Model
{
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
