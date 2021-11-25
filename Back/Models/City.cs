using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Back.Models
{
    public class City
    {
        public int? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int CountryId { get; set; }

        [BindNever]
        public Country? Country { get; set; }

    }
}
