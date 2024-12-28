using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JewelleryManagementSystem.ModelUtilities
{
    [DataContract]
    public class CommonActions
    {
        public Action<String> OnShowMessageBox;
    }
    public interface ICommonComponent : INotifyPropertyChanged { }
    [DataContract]
    public class CommonComponent : CommonActions, ICommonComponent
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void ShowMessageBox(String message)
        {
            OnShowMessageBox?.Invoke(message);
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public void OnAllPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
