using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FarmatikoData.Base;
using Microsoft.EntityFrameworkCore;

namespace FarmatikoData.Models
{
    public class Pharmacy : BaseEntity
    {
        public Pharmacy() { }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public bool WorkAllTime { get; set; }
        public Pharmacy(string Name, string Location, string Address, bool WorkAllTime)
        {
            this.Name = Name;
            this.Location = Location;
            this.Address = Address;
            this.WorkAllTime = WorkAllTime;
        }
        public int PheadId { get; set; }
        public PharmacyHead PHead { get; internal set; }
    }
}
