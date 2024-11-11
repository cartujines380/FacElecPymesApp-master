using System;
using Newtonsoft.Json;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Models
{
    public class UserInfo
    {
        [JsonProperty("sub")]
        public string UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("unique_name")]
        public string UniqueName { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("middle_name")]
        public string MiddleName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }
    }
}
