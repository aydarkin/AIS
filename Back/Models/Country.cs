using System.ComponentModel.DataAnnotations;

namespace Back.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public List<City> Cities { get; set; } = new List<City>();
    }
}
