using Back.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers
{
    [Route("api/country")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Country> Get()
        {
            List<Country> countries;
            using (var db = new AppDBContext())
            {
                countries = db.Countries.ToList();
            }
            return countries;
        }

        [HttpGet("{id}")]
        public ActionResult<Country> Get(int id)
        {
            Country country;
            using (var db = new AppDBContext())
            {
                country = db.Countries.Find(id);
            }
            if (country == null)
                return NotFound();

            return country;
        }
    }
}
