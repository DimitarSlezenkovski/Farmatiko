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
        [JsonProperty("PharmacyList")]
        public virtual List<Pharmacy> Pharmacies { get; set; }
        public virtual List<PharmacyHeadMedicine> Medicines { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }


    }
}
