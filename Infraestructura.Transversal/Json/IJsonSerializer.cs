using System;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Json
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);

        T Deserialize<T>(string json);
    }
}
