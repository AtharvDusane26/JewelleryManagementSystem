using JewelleryManagementSystem.ModelUtilities;
using JewelleryManagementSystem.OrnamentManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem.StockManagement
{
    [DataContract]
    public class OrnamentStock : CommonComponent
    {
        private string _ornament;
        private int _availbleStock;
        private float? _totalWeight;
        public OrnamentStock(string ornament)
        {
            _ornament = ornament;
        }
        [DataMember]
        public string Ornament
        {
            get => _ornament;
            private set => _ornament = value;
        }
        [DataMember]
        public int AvailableStock
        {
            get => _availbleStock;
            set
            {
                if(_availbleStock == value) return;
                _availbleStock = value;
                OnPropertyChanged(nameof(AvailableStock));
            }
        }
        [DataMember]
        public float? TotalWeight
        {
            get => _totalWeight;
            set
            {
                if(_totalWeight == value) return;
                _totalWeight = value;
                OnPropertyChanged(nameof(TotalWeight));
            }
        }

    }
}
