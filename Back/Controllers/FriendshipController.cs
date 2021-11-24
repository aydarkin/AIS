using Back.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers
{
    [Route("api/friendship")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Friendship> Get()
        {
            List<Friendship> friendships;
            using (var db = new AppDBContext())
            {
                friendships = db.Friendships.ToList();
            }
            return friendships;
        }

        [HttpGet("{id}")]
        public ActionResult<Friendship> Get(int id)
        {
            Friendship friendship;
            using (var db = new AppDBContext())
            {
                friendship = db.Friendships.Find(id);
            }
            if (friendship == null)
                return NotFound();

            return friendship;
        }

        [HttpPost]
        public ActionResult<Friendship> Post([FromBody] Friendship item)
        {
            Friendship friendship;
            using (var db = new AppDBContext())
            {
                friendship = db.Friendships.Add(item).Entity;
                db.SaveChanges();
            }

            return friendship;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var db = new AppDBContext())
            {
                var forDelete = db.Friendships.Find(id);

                if (forDelete == null)
                    return NotFound();

                db.Friendships.Remove(forDelete);
                db.SaveChanges();
            }

            return Ok();
        }
    }
}
