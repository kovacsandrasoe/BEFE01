using BEFE01.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BEFE01.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(LoginDto dto)
        {
            if (dto.UserName == "test"
                && dto.Password == "Almafa123!!!")
            {
                //jwt token generálás
                //claim = olyan adat, ami a tokenbe beletitkosítódik
                var claim = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "test"),
                    new Claim(ClaimTypes.NameIdentifier, "111"),
                    new Claim(ClaimTypes.Role, "admin")
                };

                var signinKey = new SymmetricSecurityKey(new byte[] {12,32,54,76,43,23, 12, 32, 54, 76, 43, 23 , 12, 32, 54, 76, 43, 23, 12, 32, 54, 76, 43, 23, 12, 32, 54, 76, 43, 23, 12, 32, 54, 76, 43, 23 });

                var jwt = new JwtSecurityToken(
                    issuer: "localhost",
                    audience: "localhost",
                    claims: claim.ToArray(),
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: new 
                    SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
            }
            return Unauthorized();
        }
    }
}
