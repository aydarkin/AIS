using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Back.Models
{
    public class Interest
    {
        public int? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [JsonIgnore]
        public List<Person> Persons { get; set; } = new List<Person>();
    }
}
