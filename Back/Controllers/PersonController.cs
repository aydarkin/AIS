using Back.Models;
using Microsoft.AspNetCore.Mvc;

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
                persons = db.Persons.ToList();
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

                if (item.Avatar != null)
                    editable.Avatar = item.Avatar;

                if (item.City != null)
                    editable.City = item.City;

                db.SaveChanges();
            }

            return Ok();
        }
    }
}
