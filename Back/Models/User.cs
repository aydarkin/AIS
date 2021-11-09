using System.ComponentModel.DataAnnotations;

namespace Back.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public int Login { get; set; }

        [Required]
        public int Password { get; set; }

        public Person? Person { get; set; }
    }
}
