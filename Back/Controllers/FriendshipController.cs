using Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                friendships = db.Friendships
                    // детализируем сущности Person
                    .Include(f => f.First)
                    .Include(f => f.Second)

                    // в Person детализируем Gender
                    .ThenInclude(p => p.Gender)
                    
                    .ToList();
            }
            return friendships;
        }

        [HttpGet("{id}")]
        public ActionResult<List<Friendship>> Get(int id)
        {
            List<Friendship> friendships;
            using (var db = new AppDBContext())
            {
                friendships = db.Friendships
                    // фильтр
                    .Where(f => f.FirstId == id || f.SecondId == id)

                    // детализируем сущности Person
                    .Include(f => f.First)
                    .Include(f => f.Second)

                    // в Person детализируем Gender
                    .ThenInclude(p => p.Gender)

                    .ToList();
            }

            return friendships;
        }

        [HttpPost]
        public ActionResult<Friendship> Post([FromBody] Friendship item)
        {
            // TODO перейти на механизм изменения направления дружбы

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
            // TODO перейти на механизм изменения направления дружбы

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
