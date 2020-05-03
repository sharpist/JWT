using JWT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JWT.Controllers
{
    public class AccountController : Controller
    {
        // тестовые данные вместо использования базы данных
        private List<Person> accounts = new List<Person> {
            new Person { Login = "admin@mail.com",  Password = "12345", Role = "admin" },
            new Person { Login = "qwerty@mail.com", Password = "55555", Role = "user" }
        };

        [HttpPost]
        public IActionResult Token(string username, string password)
        {
            var identity = getIdentity(username, password);
            if (identity == null)
                return BadRequest(new { errorText = "Invalid username or password." });

            return generateJSONWebToken(identity);
        }

        private ClaimsIdentity getIdentity(string username, string password)
        {
            var account = accounts
                .FirstOrDefault(x => x.Login == username && x.Password == password);
            if (account != null) {
                var claims = new[] {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, account.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Role)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Token",
                    ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }
            return null;
        }

        private JsonResult generateJSONWebToken(ClaimsIdentity identity)
        {
            // ключ безопасности
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkey_texqtgwxknlho"));
            // алгоритм хеширования (симметричный, для вычисления нужен один секретный ключ)
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                // заявки
                issuer:    "MyAuthServer", // издатель токена
                audience:  "MyAuthClient", // потребитель токена
                notBefore: DateTime.UtcNow,
                claims:    identity?.Claims,
                expires:   DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)), // время жизни токена 30 мин.
                // подпись
                signingCredentials: credentials
                );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Json(response);
        }
    }
}
