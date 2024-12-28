using JewelleryManagementSystem.ModelUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem
{
    [DataContract]
    public sealed class ProductInformation : CommonComponent
    {
        private static ProductInformation _instance;
        private string _ownerName = "Test";
        private string _shopName = "Test";
        private string _address = "Test";
        private string _phoneNumber = "1234567890";
        private string _password = "Test";
        private readonly string _filePath = Path.Combine(DirectoryPath.ProductDirectory, "ProductInformation.xml");
        public ProductInformation()
        {
            if (!String.IsNullOrWhiteSpace(GeneralSettings.Default.Password))
                _password = GeneralSettings.Default.Password;
            else
            {
                GeneralSettings.Default.Password = _password;
                GeneralSettings.Default.Save();
            }
            ReadProductInformation();
        }
        [IgnoreDataMember]
        public static ProductInformation Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ProductInformation();
                return _instance;
            }
        }
        [IgnoreDataMember]
        public string ProductName => "Jewellery Management System";
        [IgnoreDataMember]
        public string ProductVersion => "1.0.0.0";
        [DataMember]
        public string OwnerName
        {
            get { return _ownerName; }
            set
            {
                if (_ownerName != value)
                {
                    _ownerName = value;
                    OnPropertyChanged(nameof(OwnerName));
                }
            }
        }
        [DataMember]
        public string ShopName
        {
            get => _shopName;
            set
            {
                if (_shopName != value)
                {
                    _shopName = value;
                    OnPropertyChanged(nameof(ShopName));
                }
            }
        }
        [IgnoreDataMember]
        public string OwenrSignature => $"{OwnerName}";
        [DataMember]
        public string Address
        {
            get => _address;
            set
            {
                if (value != _address)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }
        [DataMember]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (value != _phoneNumber)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }
        [IgnoreDataMember]
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    GeneralSettings.Default.Password = value;
                    GeneralSettings.Default.Save();
                }
            }
        }
        private void ReadProductInformation()
        {

            try
            {
                var info = DataManager.Read<ProductInformation>(_filePath);
                if (info != null)
                {
                    _ownerName = info.OwnerName;
                    _phoneNumber = info.PhoneNumber;
                    _shopName = info.ShopName;
                    _address = info.Address;
                }
            }
            catch
            {
                OnShowMessageBox?.Invoke("Unable To Read Product Information");
            }
        }
        public void UpdateProductInformation()
        {

            try
            {
                DataManager.Save<ProductInformation>(this, _filePath);
                OnShowMessageBox?.Invoke("Updated Product Information");
            }
            catch
            {
                OnShowMessageBox?.Invoke("Unable To Updated Product Information");
            }
        }

    }
}
