using Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Message> Get(int? from, int? to, int? forPerson)
        {
            IEnumerable<Message> messages;
            using (var db = new AppDBContext())
            {
                // конвеер пошел!
                messages = db.Messages;

                if (forPerson != null)
                {
                    messages = messages.Where(m => m.FromId == forPerson || m.ToId == forPerson);
                } else
                {
                    // добавляем фильтр по отправителю, если есть
                    if (from != null)
                        messages = messages.Where(m => m.FromId == from);

                    // добавляем фильтр по получателю, если есть
                    if (to != null)
                        messages = messages.Where(m => m.ToId == to);
                }
                

                //messages = messages.ToList();

                messages = messages.OrderByDescending((mes) => mes.Date).ToList();
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
                item.Date = DateTime.Now;
                message = db.Messages.Add(item).Entity;
                db.SaveChanges();

                db.Messages.Where(c => c.Id == message.FromId).Load();
                db.Messages.Where(c => c.Id == message.ToId).Load();
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