using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JewelleryManagementSystem.ModelUtilities
{
    public class DataManager
    {
        private static DataContractSerializerSettings Settings
        {
            get
            {
                return new DataContractSerializerSettings()
                {
                    KnownTypes = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => type.IsClass && type.GetCustomAttributes(typeof(DataContractAttribute), true).Any())
                    .ToList()
                };
            }
        }
        public static void Save<T>(T serializableObject, string filePath)
        {
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T), Settings);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                using (XmlWriter writer = XmlWriter.Create(fileStream))
                {
                    serializer.WriteObject(writer, serializableObject);
                }
            }
            catch
            {
                throw;
            }
        }
        public static T Read<T>(string filePath)
        {
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T), Settings);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    T obj = (T)serializer.ReadObject(fileStream);
                    Console.WriteLine($"Object deserialized from {filePath}");
                    return obj;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
