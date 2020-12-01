using FarmatikoData.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FarmatikoData.Models
{
    public class User : BaseEntity
    {
        public enum Role
        {
            Admin,
            PharmacyHead
        }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public Role UserRole { get; set; }
    }
}
