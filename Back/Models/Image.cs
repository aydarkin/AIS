using System.ComponentModel.DataAnnotations;

namespace Back.Models
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public byte[] Data { get; set; }
    }
}
