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

        public bool AddOrUpdateOrnament(Ornament ornament)
        {
            if(AvailableOrnaments == null)
                return false;
            var presentOrnament = GetOrnamentIfPresent(ornament);
            if (presentOrnament != null)
            {
                AvailableOrnaments.Remove(AvailableOrnaments.FirstOrDefault(o => o.OrnamentID == presentOrnament.OrnamentID));
            }
            if (ornament.IsMakingUpdateFromMetal)
                ornament.UpdatedRateAndMaking();
            AvailableOrnaments.Add(ornament);
            Save();  
            return true;
       }
        public Ornament GetNewOrnament()
        {
            return new Ornament();
        }
        private Ornament GetOrnamentIfPresent(Ornament ornament)
        {
            var retVal = _availableOrnaments.FirstOrDefault(o => o.OrnamentID == ornament.OrnamentID);
            return retVal;
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
                _availableOrnaments.Add(new Ornament { Name = "Gold Ring", Metal = MetalManager.Instance.AvailableMetals.Where(metal => metal.MetalName == "Gold 22 cr.").FirstOrDefault().Clone(), IsMakingUpdateFromMetal = true });
                _availableOrnaments.Add(new Ornament
                {
                    Name = "Gold Vedha",
                    Metal = MetalManager.Instance.AvailableMetals.Where(metal => metal.MetalName == "Gold 24 cr.").FirstOrDefault().Clone(),
                    IsMakingUpdateFromMetal = false,
                    MakingCharges = 0,
                    MakingChargeType
                 = WeightType.InGram
                });
                _availableOrnaments.Add(new Ornament { Name = "Silver Painjan", Metal = MetalManager.Instance.AvailableMetals.Where(metal => metal.MetalName.Trim() == "Silver").FirstOrDefault().Clone() , IsMakingUpdateFromMetal = true });
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
