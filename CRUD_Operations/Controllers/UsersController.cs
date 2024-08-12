using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRUD_Operations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(JwtOptions jwtOptions) : ControllerBase
    {
        [HttpPost]
        [Route("auth")]
        public ActionResult<string> AuthenticateUser(AuthenticateRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Isuuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier,request.UserName),
                    new(ClaimTypes.Email,"a@b.com")
                })
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var accessToken = tokenHandler.WriteToken(securityToken);

            return Ok(accessToken);
        }
    }
}
