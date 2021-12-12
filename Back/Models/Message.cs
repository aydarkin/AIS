using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Back.Models
{
    public class Message
    {
        public int? Id { get; set; }

        [Required]
        public int FromId { get; set; }

        [BindNever]
        [JsonIgnore]
        public User? From { get; set; }

        [Required]
        public int ToId { get; set; }

        [BindNever]
        [JsonIgnore]
        public User? To { get; set; }

        [Required]
        public string Text { get; set; }

        public Image? Image { get; set; }

        public DateTime? Date { get; set; }
    }
}
