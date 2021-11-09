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
        public int Id { get; set; }

        [Required]
        public Person First { get; set; }

        [Required]
        public Person Second { get; set; }

        [Required]
        public FriendDirection Direction { get; set; }
    }
}
