using System.ComponentModel.DataAnnotations;

namespace Back.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public Person? Person { get; set; }
    }
}
