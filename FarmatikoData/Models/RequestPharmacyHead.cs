using FarmatikoData.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace FarmatikoData.Models
{
    public class RequestPharmacyHead : BaseEntity
    {
        public RequestPharmacyHead()
        {
        }
        
        [JsonProperty("PharmacyHead")]
        public PharmacyHead Head { get; set; }
        
        [JsonProperty("Pharmacy")]
        public Pharmacy Pharmacy { get; set; }

    }
}
