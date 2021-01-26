using FarmatikoData.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmatikoData.DTOs
{
    public class PharmacyHeadDto
    {
        /*
         id?: string;
         PharmacyMedicines?: IMedicine[];
         Pharmacy?: IPharmacy[];
         Email: string;
         Name: string;
         Passwd: string;
         originalUserName?: string;
         Role?: string;
        */
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("PharmacyMedicines")]
        public List<Medicine> Medicines { get; set; }
        [JsonProperty("Pharmacy")]
        public List<Pharmacy> Pharmacies { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Passwd")]
        public string Password { get; set; }
    }
}
