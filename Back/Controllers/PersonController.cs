using Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Back.Controllers
{
    [Route("api/person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Person> Get(string? fio, string? mode, int? id)
        {
            IEnumerable<Person> persons;
            using (var db = new AppDBContext())
            {
                db.Persons.Include(x => x.Interests).ToList();
                persons = db.Persons
                    .Include(x => x.Interests)
                    .Include(x => x.Avatar)
                    .Include(x => x.Gender)
                    .Include(x => x.City)
                    .Include("City.Country");

                // такой себе поиск
                if (fio != null)
                    persons = persons.Where(x => $"{x.Surname} {x.Name} {x.Patronymic}".ToLower().Contains(fio.ToLower()));

                if (mode == "recommended")
                {
                    var current = db.Persons.Find(id);
                    List<int> interests = current.Interests.Select(i => i.Id ?? -1).ToList();

                    persons = from x in persons
                              where x.UserId != current.UserId
                              orderby x.Interests.Select(i => i.Id ?? -1).Intersect(interests).ToList().Count descending
                              select x;
                    persons = persons.Take(5);
                }
                    
                persons = persons.ToList();
            }
            return persons;
        }

        [HttpGet("{id}")]
        public ActionResult<Person> Get(int id)
        {
            Person person;
            using (var db = new AppDBContext())
            {
                db.Persons.Include(x => x.Interests).ToList();
                person = db.Persons.Find(id);

                if (person.GenderId != null)
                    db.Genders.Where(c => c.Id == person.GenderId).Load();

                if (person.CityId != null)
                    db.Cities.Where(c => c.Id == person.CityId).Include("Country").Load();
                
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

                if (item.CityId != null || item.City != null)
                    editable.CityId = item.CityId ?? item.City?.Id;

                if (item.GenderId != null || item.Gender != null)
                    editable.GenderId = item.GenderId ?? item.Gender?.Id;

                if (item.Interests != null)
                {
                    db.Persons.Include(x => x.Interests).ToList();
                    editable.Interests.Clear();
                    db.SaveChanges();
                    foreach (var newInterest in item.Interests)
                    {
                        editable.Interests.Add(db.Interests.Find(newInterest.Id));
                    }
                }
                    

                db.SaveChanges();
            }

            return Ok();
        }
    }
}
