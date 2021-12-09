using System.ComponentModel.DataAnnotations;

namespace Back.Models
{
    public enum FriendDirection
    {
        /// <summary>
        /// Запрос от первого ко второму
        /// </summary>
        FirstToSecond = 0,
        /// <summary>
        /// Запрос от второго к первому
        /// </summary>
        SecondToFirst = 1,
        /// <summary>
        /// Заявка в друзья принята
        /// </summary>
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
