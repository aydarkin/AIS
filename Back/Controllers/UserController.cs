using Back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Back.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        // GET: api/user
        [HttpGet]
        public IEnumerable<User> Get()
        {
            List<User> users;
            using (var db = new AppDBContext())
            {
                users = db.Users.ToList();
            }
            return users;
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            User user;
            using (var db = new AppDBContext())
            {
                user = db.Users.Find(id);
            }
            if (user == null)
                return NotFound();

            return user;
        }

        // POST api/user
        [HttpPost]
        public ActionResult<User> Post([FromBody] User item)
        {
            User user;
            using (var db = new AppDBContext())
            {
                user = db.Users.Add(item).Entity;
                db.SaveChanges();
            }

            return user;
        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User item)
        {
            User user;
            using (var db = new AppDBContext())
            {
                var editable = db.Users.Find(id);

                if (editable == null)
                    return NotFound();

                if (item.Password != null)
                    editable.Password = item.Password;

                db.SaveChanges();
            }

            return Ok();
        }

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            User user;
            using (var db = new AppDBContext())
            {
                var forDelete = db.Users.Find(id);

                if (forDelete == null)
                    return NotFound();

                db.Users.Remove(forDelete);
                db.SaveChanges();
            }

            return Ok();
        }
    }
}
