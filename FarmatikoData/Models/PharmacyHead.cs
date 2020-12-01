using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FarmatikoData.Base;
using Newtonsoft.Json;

namespace FarmatikoData.Models
{
    public class PharmacyHead : BaseEntity
    {
        public PharmacyHead()
        {
        }
        [Required]
        [JsonProperty("Email")]
        public string Email { get; set; }
        [Required]
        [JsonProperty("Name")]
        public string Name { get; set; }
        [Required]
        [JsonProperty("Passwd")]
        public string Password { get; set; }

        [JsonProperty("PharmacyMedicines")]
        public List<Medicine> MedicineList { get; set; }
        [JsonProperty("Pharmacy")]
        public ICollection<Pharmacy> PharmaciesList { get; set; }
        //[JsonProperty("PHMedicineList")]
        public ICollection<PharmacyHeadMedicine> PHMedicineList { get; set; }

    }
}
