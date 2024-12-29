using JewelleryManagementSystem.ModelUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem
{
    public interface IGST
    {
        string GSTNumber { get; }
        float GSTPercent { get;}
        float GetGSTAmount(float netAmount);
    }
    [DataContract]
    public class GST : CommonComponent, IGST
    {
        private string _gSTNumber = "1234567890";
        private float _gSTPercent = 3f;
        [DataMember]
        public string GSTNumber
        {
            get => _gSTNumber;
            set
            {
                if(_gSTNumber == value) return;
                _gSTNumber = value;
                OnPropertyChanged(nameof(GSTNumber));
            }
        }
        [DataMember]
        public float GSTPercent
        {
            get => _gSTPercent;
            set
            {
                if(_gSTPercent == value) return;
                _gSTPercent = value;
                OnPropertyChanged(nameof(GSTPercent));
            }
        }
        public float GetGSTAmount(float netAmount)
        {
            return (netAmount * _gSTPercent) / 100;
        }

    }
}
