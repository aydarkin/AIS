using Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back.Controllers
{
    [Route("api/person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            List<Person> persons;
            using (var db = new AppDBContext())
            {
                persons = db.Persons
                    .Include(x => x.Avatar)
                    .Include(x => x.Gender)
                    .Include(x => x.City)
                    .Include("City.Country")
                    .Include(x => x.Interests)
                    .ToList();
            }
            return persons;
        }

        [HttpGet("{id}")]
        public ActionResult<Person> Get(int id)
        {
            Person person;
            using (var db = new AppDBContext())
            {
                person = db.Persons.Find(id);

                db.Genders.Where(c => c.Id == person.GenderId).Load();
                db.Cities.Where(c => c.Id == person.CityId).Load();
            }
            if (person == null)
                return NotFound();

            return person;
        }

        [HttpPost]
        public ActionResult<Person> Post([FromBody] Person item)
        {
            Person person;
            using (var db = new AppDBContext())
            {
                person = db.Persons.Add(item).Entity;
                db.SaveChanges();

                db.Genders.Where(c => c.Id == person.GenderId).Load();
                db.Cities.Where(c => c.Id == person.CityId).Load();
            }

            return person;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Person item)
        {
            using (var db = new AppDBContext())
            {
                var editable = db.Persons.Find(id);

                if (editable == null)
                    return NotFound();

                if (item.Name != null)
                    editable.Name = item.Name;

                if (item.Surname != null)
                    editable.Surname = item.Surname;

                if (item.Patronymic != null)
                    editable.Patronymic = item.Patronymic;

                if (item.BirthDate != null)
                    editable.BirthDate = item.BirthDate;

                if (item.CityId != null)
                    editable.CityId = item.CityId;

                db.SaveChanges();
            }

            return Ok();
        }
    }
}
