using Back.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public ActionResult<User> Register([FromBody] User item)
        {
            User user;
            using (var db = new AppDBContext())
            {
                user = db.Users.Add(item).Entity;
                db.SaveChanges();
            }

            return user;
        }

        [HttpPost("auth")]
        public ActionResult<object> Token([FromBody] User item)
        {
            var identity = GetIdentity(item.Login, item.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Неправильный логин или пароль" });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            this.Response.Cookies.Append("token", "" + encodedJwt);
            this.Response.Cookies.Append("username", "" + identity.Name);

            return response;
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user;
            using (var db = new AppDBContext())
            {
                user = db.Users.FirstOrDefault(x => x.Login == username && x.Password == password);
            }


            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                };

                return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            }

            // если пользователь не найден
            return null;
        }

    }
}
