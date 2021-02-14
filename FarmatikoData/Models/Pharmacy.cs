using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FarmatikoData.Base;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FarmatikoData.Models
{
    public class Pharmacy : BaseEntity
    {
        public Pharmacy() { }
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }
        [Required]
        [JsonProperty("location")]
        public string Location { get; set; }
        [Required]
        [JsonProperty("address")]
        public string Address { get; set; }
        [Required]
        [JsonProperty("workAllTime")]
        public bool WorkAllTime { get; set; }
        public Pharmacy(string Name, string Location, string Address, bool WorkAllTime)
        {
            this.Name = Name;
            this.Location = Location;
            this.Address = Address;
            this.WorkAllTime = WorkAllTime;
        }
        public int? PheadId { get; set; }
        public PharmacyHead PharmacyHead { get; set; }
    }
}
