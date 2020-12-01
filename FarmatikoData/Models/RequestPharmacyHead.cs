using FarmatikoData.Base;
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
        [Required]
        [JsonPropertyName("PharmacyHead")]
        public PharmacyHead Head { get; set; }
        [Required]
        [JsonPropertyName("Pharmacy")]
        public Pharmacy Pharmacy { get; set; }

    }
}
