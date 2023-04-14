using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;

namespace MSD.Shared.Utility
{
    public static class Serializer
    {
        public static string Serialize<T>(T source, string dateTimeFormat = "yyyy-MM-dd'T'HH:mm:ssZ")
        {
            var settings = new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true,
                DateTimeFormat = new DateTimeFormat(dateTimeFormat),
                EmitTypeInformation = EmitTypeInformation.Never
                
            };

            var serializer = new DataContractJsonSerializer(typeof(T), settings);

            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, source);
                byte[] jsonBytes = memoryStream.ToArray();
                memoryStream.Close();
                return Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
            }
        }

        public static T Deserialize<T>(string json, string dateTimeFormat = "yyyy-MM-dd'T'HH:mm:ssZ") where T : class
        {
            var settings = new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true,
                DateTimeFormat = new DateTimeFormat(dateTimeFormat)
            };

            var serializer = new DataContractJsonSerializer(typeof(T), settings);

            T outObject;

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                outObject = serializer.ReadObject(memoryStream) as T;
            }

            return outObject;
        }
    }
}
