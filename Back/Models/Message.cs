using System.ComponentModel.DataAnnotations;

namespace Back.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public User From { get; set; }

        [Required]
        public User To { get; set; }

        [Required]
        public string Text { get; set; }

        public Image? Image { get; set; }

        public DateTime? Date { get; set; }
    }
}
