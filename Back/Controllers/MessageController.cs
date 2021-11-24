using Back.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Message> Get()
        {
            List<Message> messages;
            using (var db = new AppDBContext())
            {
                messages = db.Messages.ToList();
            }
            return messages;
        }

        // GET api/message/5
        [HttpGet("{id}")]
        public ActionResult<Message> Get(int id)
        {
            Message message;
            using (var db = new AppDBContext())
            {
                message = db.Messages.Find(id);
            }
            if (message == null)
                return NotFound();

            return message;
        }

        // POST api/message
        [HttpPost]
        public ActionResult<Message> Post([FromBody] Message item)
        {
            Message message;
            using (var db = new AppDBContext())
            {
                message = db.Messages.Add(item).Entity;
                db.SaveChanges();
            }

            return message;
        }

        // DELETE api/message/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var db = new AppDBContext())
            {
                var forDelete = db.Messages.Find(id);

                if (forDelete == null)
                    return NotFound();

                db.Messages.Remove(forDelete);
                db.SaveChanges();
            }

            return Ok();
        }
    }
}