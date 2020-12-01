using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FarmatikoData.Base;
using Microsoft.EntityFrameworkCore;

namespace FarmatikoData.Models
{
    public class Pandemic : BaseEntity
    {
        public Pandemic() { }
        [Required]
        public string Name { get; set; }
        [Required]
        public int TotalMK { get; set; }
        [Required]
        public int ActiveMK { get; set; }
        [Required]
        public int DeathsMK { get; set; }
        [Required]
        public int NewMK { get; set; }
        [Required]
        public long TotalGlobal { get; set; }
        [Required]
        public long DeathsGlobal { get; set; }
        [Required]
        public long ActiveGlobal { get; set; }
        public Pandemic(string Name, int TotalMK, int ActiveMK,
            int DeathsMK, int NewMK, long TotalGlobal, long DeathsGlobal, long ActiveGlobal)
        {
            this.Name = Name;
            this.TotalMK = TotalMK;
            this.ActiveMK = ActiveMK;
            this.DeathsMK = DeathsMK;
            this.NewMK = NewMK;
            this.TotalGlobal = TotalGlobal;
            this.DeathsGlobal = DeathsGlobal;
            this.ActiveGlobal = ActiveGlobal;
        }
    }
}
