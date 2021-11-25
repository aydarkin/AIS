using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Back.Models
{
    public class Person
    {
        [Key]
        public int UserId { get; set; }

        [BindNever]
        [JsonIgnore]
        public User? User { get; set; }

        public string Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public DateTime? BirthDate { get; set; }
        public Image? Avatar { get; set; }

        public int? GenderId { get; set; }
        public Gender? Gender { get; set; }

        public int? CityId { get; set; }
        public City? City { get; set; }

        [BindNever]
        public List<Interest> Interests { get; set; } = new List<Interest>();
    }
}
