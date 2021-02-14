using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FarmatikoData.DTOs
{
    public class MedicineDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("strength")]
        public string Strength { get; set; }
        [JsonProperty("form")]
        public string Form { get; set; }
        [JsonProperty("wayofissuing")]
        public string WayOfIssuing { get; set; }
        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }
        [JsonProperty("price")]
        public float Price { get; set; }
        [JsonProperty("packaging")]
        public string Packaging { get; set; }
        [JsonPropertyName("headNames")]
        public ICollection<string> HeadNames { get; set; }
    }
}
