using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FarmatikoData.Base;
using Microsoft.EntityFrameworkCore;

namespace FarmatikoData.Models
{
    public class HealthcareWorker : BaseEntity
    {
        public HealthcareWorker() { }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Branch { get; set; }
        [Required]
        public HealthFacility Facility { get; set; }
        public string Title { get; set; }
        public HealthcareWorker(string Name, string Branch, HealthFacility Facility, string Title)
        {
            this.Name = Name;
            this.Branch = Branch;
            this.Facility = Facility;
            this.Title = Title;
        }
    }
}
