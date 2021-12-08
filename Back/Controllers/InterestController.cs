using Back.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers
{
    [Route("api/interest")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Interest> Get()
        {
            List<Interest> interests;
            using (var db = new AppDBContext())
            {
                interests = db.Interests.ToList();
            }
            return interests;
        }

        [HttpGet("{id}")]
        public ActionResult<Interest> Get(int id)
        {
            Interest interest;
            using (var db = new AppDBContext())
            {
                interest = db.Interests.Find(id);
            }
            if (interest == null)
                return NotFound();

            return interest;
        }
    }
}
