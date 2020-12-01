using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FarmatikoData.Base
{
    public class BaseEntity
    {
        [JsonPropertyName("id")]
        public int Id {  get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
