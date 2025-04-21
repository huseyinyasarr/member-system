using MemberSystem.Api.Modeller;
using MemberSystem.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MemberSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtTokenSettings _jwtTokenSettings;

        public AuthController(IUserService userService, IOptions<JwtTokenSettings> jwtTokenSettings)
        {
            _userService = userService;
            _jwtTokenSettings = jwtTokenSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var user = await _userService.AuthenticateUserAsync(model.PhoneNumber, model.Password);

            if (user == null)
            {
                return Unauthorized(); // 401 Yetkisiz
            }

            // JWT Token Oluşturma
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtTokenSettings.SecretKey ?? string.Empty); // UTF8 kullanın ve null kontrolü yapın
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.PhoneNumber)
                    // İstenirse diğer kullanıcı bilgilerini de claim olarak ekleyebilirsiniz
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtTokenSettings.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtTokenSettings.Issuer,
                Audience = _jwtTokenSettings.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }
    }

    public class LoginRequestModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}