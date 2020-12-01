using FarmatikoData.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FarmatikoData.Models
{
    public class PharmacyHeadMedicine : BaseEntity
    {
        //[JsonPropertyName("PheadId")]
        public int PheadId { get; set; }
        //[JsonPropertyName("Head")]
        public PharmacyHead Head { get; set; }
        //[JsonPropertyName("MedicineId")]
        public int MedicineId { get; set; }
        //[JsonPropertyName("Medicine")]
        public Medicine Medicine { get; set; }
    }
}
