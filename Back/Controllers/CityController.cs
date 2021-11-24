using Back.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/city")]
    public class CityController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<City> Get()
        {
            List<City> cities;
            using (var db = new AppDBContext())
            {
                cities = db.Cities.ToList();
            }
            return cities;
        }

        [HttpGet("{id}")]
        public ActionResult<City> Get(int id)
        {
            City city;
            using (var db = new AppDBContext())
            {
                city = db.Cities.Find(id);
            }
            if (city == null)
                return NotFound();

            return city;
        }

        [HttpPost]
        public ActionResult<City> Post([FromBody] City item)
        {
            City city;
            using (var db = new AppDBContext())
            {
                city = db.Cities.Add(item).Entity;
                db.SaveChanges();
            }

            return city;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] City item)
        {
            using (var db = new AppDBContext())
            {
                var editable = db.Cities.Find(id);

                if (editable == null)
                    return NotFound();

                if (item.Title != null)
                    editable.Title = item.Title;

                db.SaveChanges();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var db = new AppDBContext())
            {
                var forDelete = db.Cities.Find(id);

                if (forDelete == null)
                    return NotFound();

                db.Cities.Remove(forDelete);
                db.SaveChanges();
            }

            return Ok();
        }
    }
}
