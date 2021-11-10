using System.ComponentModel.DataAnnotations;

namespace Back.Models
{
    public class Person
    {
        [Key]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "Не задано имя пользователя")]
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public DateTime? BirthDate { get; set; }
        public Image? Avatar { get; set; }
        public Gender? Gender { get; set; }
        public List<Interest> Interests { get; set; } = new List<Interest>();
    }
}
