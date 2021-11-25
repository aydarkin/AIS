using System.ComponentModel.DataAnnotations;

namespace Back.Models
{
    public class Interest
    {
        public int? Id { get; set; }

        [Required]
        public string Title { get; set; }

        public List<Person> Persons { get; set; } = new List<Person>();
    }
}
