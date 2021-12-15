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

                    // в Person детализируем City
                    .ThenInclude(p => p.City)
                    .ThenInclude(p => p.Country)

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

                    // в Person детализируем City
                    .ThenInclude(p => p.City)
                    .ThenInclude(p => p.Country)

                    .ToList();
            }

            return friendships;
        }

        [HttpPost]
        public ActionResult<Friendship> Post([FromBody] Friendship item)
        {
            Friendship friendship = null;
            using (var db = new AppDBContext())
            {
                // считаем firstId инициатором
                var query = db.Friendships.Where(f =>
                    (f.FirstId == item.FirstId && f.SecondId == item.SecondId)
                    || (f.SecondId == item.FirstId && f.FirstId == item.SecondId)
                );
                if (query.Any())
                    friendship = query.First();

                if (friendship != null)
                {
                    friendship.Direction = FriendDirection.Both;
                } else
                {
                    item.Direction = FriendDirection.FirstToSecond;
                    friendship = db.Friendships.Add(item).Entity;
                }
                db.SaveChanges();
            }

            return friendship;
        }

        [HttpDelete()]
        public IActionResult Delete(int from, int to)
        {
            using (var db = new AppDBContext())
            {
                Friendship friendship = null;

                var query = db.Friendships.Where(f =>
                    (f.FirstId == from && f.SecondId == to)
                    || (f.SecondId == from && f.FirstId == to)
                );
                if (query.Any())
                    friendship = query.First();

                if (friendship != null)
                {
                    if (friendship.Direction == FriendDirection.Both)
                        // удаление из друзей, подписчик
                        friendship.Direction = friendship.FirstId == from ? FriendDirection.SecondToFirst : FriendDirection.FirstToSecond;
                    else
                        // отмена подписки
                        db.Friendships.Remove(friendship);
                }
                else
                    return NotFound();
                
                db.SaveChanges();
            }

            return Ok();
        }
    }
}
