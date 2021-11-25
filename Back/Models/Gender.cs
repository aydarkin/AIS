using System.ComponentModel.DataAnnotations;

namespace Back.Models
{
    public class Gender
    {
        public int? Id { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
