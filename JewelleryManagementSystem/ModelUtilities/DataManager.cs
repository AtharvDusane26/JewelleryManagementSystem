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
        public static void Save<T>(T serializableObject, string filePath, DataContractSerializerSettings settings = null)
        {
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T), settings);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                using (XmlWriter writer = XmlWriter.Create(fileStream))
                {
                    serializer.WriteObject(writer, serializableObject);
                }
            }
            catch (Exception ex)
            {
                CommonActions.OnError?.Invoke("Error in Save");
            }
        }
        public static T Read<T>(string filePath, DataContractSerializerSettings settings = null)
        {
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T), settings);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    T obj = (T)serializer.ReadObject(fileStream);
                    Console.WriteLine($"Object deserialized from {filePath}");
                    return obj;
                }
            }
            catch (Exception ex)
            {
                // Log error details
                CommonActions.OnError?.Invoke("Error in Read");
                return default;
            }
        }
    }
}
