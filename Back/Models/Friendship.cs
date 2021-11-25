using System.ComponentModel.DataAnnotations;

namespace Back.Models
{
    public enum FriendDirection
    {
        FirstToSecond = 0,
        SecondToFirst = 1,
        Both = 2
    }

    public class Friendship
    {
        public int FirstId { get; set; }
        public Person? First { get; set; }

        public int SecondId { get; set; }
        public Person? Second { get; set; }

        [Required]
        public FriendDirection Direction { get; set; }
    }
}
