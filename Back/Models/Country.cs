using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Back.Models
{
    public class Country
    {
        public int? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [BindNever]
        [JsonIgnore]
        public List<City>? Cities { get; set; } = new List<City>();
    }
}
