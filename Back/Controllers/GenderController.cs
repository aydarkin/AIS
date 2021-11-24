using Back.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers
{
    [Route("api/gender")]
    [ApiController]
    public class GenderController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Gender> Get()
        {
            List<Gender> genders;
            using (var db = new AppDBContext())
            {
                genders = db.Genders.ToList();
            }
            return genders;
        }

        [HttpGet("{id}")]
        public ActionResult<Gender> Get(int id)
        {
            Gender gender;
            using (var db = new AppDBContext())
            {
                gender = db.Genders.Find(id);
            }
            if (gender == null)
                return NotFound();

            return gender;
        }
    }
}
