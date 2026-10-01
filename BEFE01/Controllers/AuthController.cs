using BEFE01.Dtos;
using BEFE01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BEFE01.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;

        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
        }

        [HttpPost("register")]
        public async Task Register(UserCreateDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true,
                FamilyName = dto.FamilyName,
                GivenName = dto.GivenName,
                RefreshToken = ""
            };
            await _userManager.CreateAsync(user, dto.Password);

            if (_userManager.Users.Count() == 1)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, dto.Password);
                if (result)
                {
                    //van ilyen user és jó a jelszava
                    //todo: generate token
                    var claim = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName!),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    foreach (var role in await _userManager.GetRolesAsync(user))
                    {
                        claim.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var accessToken = GenerateAccessToken(claim);
                    var refreshToken = await GenerateRefreshToken(user);

                    return Ok(new LoginResultDto()
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                        AccessTokenExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["jwt:access_expiry_minutes"])),
                        RefreshToken = refreshToken,
                        RefreshTokenExpiration = DateTime.Now.AddMinutes(24 * 60 * Convert.ToInt32(_configuration["jwt:refresh_expiry_days"]))
                    });
                }
                else
                {
                    throw new ArgumentException("Nem jó a jelszó");
                    //return BadRequest("Nem jó a jelszó");
                }
            }
            else
            {
                throw new ArgumentException("Nincs ilyen user");
                //return BadRequest("Nincs ilyen user");
            }

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid client request");
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);
            var username = principal?.Identity?.Name;

            var user = await _userManager.GetUserAsync(principal!);
            if (user is null || user.RefreshToken != refreshToken)
                return BadRequest("Invalid client request");

            var newAccessToken = GenerateAccessToken(principal?.Claims);
            var newRefreshToken = await GenerateRefreshToken(user);
            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken,
                AccessTokenExpiration = newAccessToken.ValidTo,
                RefreshTokenExpiration = DateTime.Now.AddMinutes(24 * 60 * Convert.ToInt32(_configuration["jwt:refresh_expiry_days"]))
            });
        }

        [Authorize]
        [HttpPost("upload-profile-picture")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            return Ok(stream.ToArray());

            /*await using var stream = new FileStream(
            filePath,
            FileMode.Create);

            await file.CopyToAsync(stream);*/
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["jwt:key"] ?? "")),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }



        private JwtSecurityToken GenerateAccessToken(IEnumerable<Claim>? claims)
        {
            var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["jwt:key"] ?? throw new Exception("jwt:key not found in appsettings.json")));

            return new JwtSecurityToken(
                  issuer: _configuration["jwt:issuer"],
                  audience: _configuration["jwt:audience"],
                  claims: claims?.ToArray(),
                  expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["jwt:access_expiry_minutes"])),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );
        }

        private async Task<string> GenerateRefreshToken(AppUser user)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                string result = Convert.ToBase64String(randomNumber);
                user.RefreshToken = result;
                await _userManager.UpdateAsync(user);
                return result;
            }
        }



        [HttpPost("login-test")]
        public IActionResult LoginTest(LoginDto dto)
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

                var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("NagyonhosszútitkosítókulcsNagyonhosszútitkosítókulcsNagyonhosszútitkosítókulcsNagyonhosszútitkosítókulcsNagyonhosszútitkosítókulcsNagyonhosszútitkosítókulcs"));

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
