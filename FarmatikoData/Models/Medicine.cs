using FarmatikoData.Base;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FarmatikoData.Models
{
    public class Medicine : BaseEntity
    {
        public Medicine() { }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Strength { get; set; }
        public string Form { get; set; }
        [Required]
        public string WayOfIssuing { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        public float Price { get; set; }
        public string Packaging { get; set; }
        //[JsonPropertyName("PHMedicineList")]
        public ICollection<PharmacyHeadMedicine> Medicines { get; set; }
        public Medicine(string Name, string Strength, string Form, string WayOfIssuing, string Manufacturer, float Price, string Packaging)
        {
            this.Name = Name;
            this.Strength = Strength;
            this.Form = Form;
            this.WayOfIssuing = WayOfIssuing;
            this.Manufacturer = Manufacturer;
            this.Price = Price;
            this.Packaging = Packaging;
        }
    }
}
