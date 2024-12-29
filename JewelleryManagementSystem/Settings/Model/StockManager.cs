using JewelleryManagementSystem.ModelUtilities;
using JewelleryManagementSystem.OrnamentManagement.Model;
using JewelleryManagementSystem.StockManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem.Settings.Model
{
    [DataContract]
    public sealed class StockManager : CommonComponent
    {
        private static StockManager _instance;
        private List<OrnamentStock> _ornamentStock;
        private string _filePath = Path.Combine(DirectoryPath.StockDirectory, "StockList.xml");
        private StockManager()
        {
            Load();
        }
        public static StockManager Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new StockManager();
                return _instance;
            }
        }
        [DataMember]
        public List<OrnamentStock> OrnamentStocks
        {
            get => _ornamentStock;
            private set
            {
                if(_ornamentStock ==  value) return;
                _ornamentStock = value;
            }
        }
        public bool AddOrUpdateStock(OrnamentStock ornamentStock)
        {
            if(ornamentStock == null || OrnamentStocks == null)
                return false;
            if(OrnamentStocks.Any(o => o.Ornament == ornamentStock.Ornament))
            {
                OrnamentStocks.Remove(OrnamentStocks.FirstOrDefault(o => o.Ornament == ornamentStock.Ornament));
            }
            OrnamentStocks.Add(ornamentStock);
            Save();
            return true;
        }
        public int CheckStockAvailability(Ornament ornament)
        {
            var ornamentStock = OrnamentStocks.Where(o => o.Ornament == ornament.ToString()).FirstOrDefault();
            if(ornamentStock == null)
                return 0;
            return ornamentStock.AvailableStock;
        }
        public void UpdateOrnamentWeight(Ornament ornament, float? totalWeight)
        {
            var ornamentStock = OrnamentStocks.Where(o => o.Ornament == ornament.ToString()).FirstOrDefault();
            if (ornamentStock == null)
                return;
            ornamentStock.TotalWeight = totalWeight;
            Save();
        }
        public OrnamentStock GetNewOrnamentStock(Ornament ornament)
        {
            return new OrnamentStock(ornament.ToString());
        }
        private void Save()
        {
            try
            {
                DataManager.Save<List<OrnamentStock>>(OrnamentStocks, _filePath);
            }
            catch (Exception ex)
            {
                OnShowMessageBox?.Invoke("Unable to save stock list");
            }
        }
        private void Load()
        {
            try
            {
              var list =   DataManager.Read<List<OrnamentStock>>(_filePath);
               OrnamentStocks = list != null ? list : new List<OrnamentStock>();
            }
            catch (Exception ex)
            {
                OnShowMessageBox?.Invoke("Unable to save stock list");
                OrnamentStocks = new List<OrnamentStock>();
            }
        }
        
    }
}
