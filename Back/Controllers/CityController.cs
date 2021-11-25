using Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                cities = db.Cities
                    .Include(c => c.Country) // подгружаем связанные данные
                    .ToList();
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

                // грузим связанный объект, он автоматически привяжется к city.Country
                db.Countries.Where(c => c.Id == city.CountryId).Load();
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
                // добавляем и получаем отслеживаемую сущность
                city = db.Cities.Add(item).Entity;

                // выполняем запрос
                db.SaveChanges();

                // грузим связанный объект, он автоматически привяжется к city.Country
                db.Countries.Where(c => c.Id == city.CountryId).Load();
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
