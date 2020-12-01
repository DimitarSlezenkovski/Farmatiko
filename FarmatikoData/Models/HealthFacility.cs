using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FarmatikoData.Base;
using Microsoft.EntityFrameworkCore;

namespace FarmatikoData.Models
{
    public class HealthFacility : BaseEntity
    {
        public HealthFacility() { }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Municipality { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Type { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public HealthFacility(string Name, string Municipality, string Address, string Type, string Email, string Phone)
        {
            this.Name = Name;
            this.Municipality = Municipality;
            this.Address = Address;
            this.Type = Type;
            this.Email = Email;
            this.Phone = Phone;
        }
    }
}