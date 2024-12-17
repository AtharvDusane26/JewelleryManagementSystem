using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem.ModelUtilities
{
    public sealed class DirectoryPath
    {
        private static DirectoryPath _directoryPath;
        private static string _metalDirectory = Path.Combine(GeneralSettings.Default.BaseDirectory, GeneralSettings.Default.MetalDirectory);
        private static string _ornamentDirecory = Path.Combine(GeneralSettings.Default.BaseDirectory, GeneralSettings.Default.OrnamentDirectory);
        private static string _customerDirectory = Path.Combine(GeneralSettings.Default.BaseDirectory,GeneralSettings.Default.CustomerDirectory);
        private DirectoryPath()
        {
            CreateDirectoryPath();
        }
        public static DirectoryPath Instance
        {
            get
            {
                if (_directoryPath == null)
                    _directoryPath = new DirectoryPath();
                return _directoryPath;
            }
        }
        public static string MetalDirectory
        {
            get => _metalDirectory;
            private set => _metalDirectory = value;
        }
        public static string OrnamentDirectory
        {
            get => _ornamentDirecory;
            private set => _ornamentDirecory = value;
        }
        public static string CustomerDirectory
        {
            get => _customerDirectory;
            private set => _customerDirectory = value;
        }
        private static void CreateDirectoryPath()
        {
            if (!String.IsNullOrWhiteSpace(MetalDirectory) && !Directory.Exists(MetalDirectory))
            {
                var newDir = Path.Combine(GeneralSettings.Default.BaseDirectory, GeneralSettings.Default.MetalDirectory);
                Directory.CreateDirectory(newDir);
            }
            if (!String.IsNullOrWhiteSpace(OrnamentDirectory) && !Directory.Exists(OrnamentDirectory))
            {
                var newDir = Path.Combine(GeneralSettings.Default.BaseDirectory, GeneralSettings.Default.OrnamentDirectory);
                Directory.CreateDirectory(newDir);
            }
            if (!String.IsNullOrWhiteSpace(CustomerDirectory) && !Directory.Exists(CustomerDirectory))
            {
                var newDir = Path.Combine(GeneralSettings.Default.BaseDirectory, GeneralSettings.Default.CustomerDirectory);
                Directory.CreateDirectory(newDir);
            }
        }
    }
}
