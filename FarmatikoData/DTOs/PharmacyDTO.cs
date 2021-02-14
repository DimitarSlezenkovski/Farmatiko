using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmatikoData.DTOs
{
    public class PharmacyDTO
    {
        public PharmacyDTO() { }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("workAllTime")]
        public bool WorkAllTime { get; set; }
        [JsonProperty("headName")]
        public string HeadName { get; set; }
    }
}
