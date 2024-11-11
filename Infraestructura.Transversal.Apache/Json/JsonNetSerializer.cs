using System;
using Newtonsoft.Json;

using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Json;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Json
{
    public class JsonNetSerializer : IJsonSerializer
    {
        public JsonSerializerSettings SerializerSettings { get; private set; }

        public Formatting Formatting { get; set; }

        public JsonNetSerializer()
        {
            SerializerSettings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting, SerializerSettings);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, SerializerSettings);
        }
    }
}
