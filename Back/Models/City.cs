using System.ComponentModel.DataAnnotations;

namespace Back.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public Country Country { get; set; }

    }
}
