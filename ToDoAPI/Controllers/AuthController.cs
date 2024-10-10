using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoAPI.Models;
using ToDoAPI.Models.ApplicationDbContext;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoAPI.Controllers
{
    [Route("")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly IConfiguration _configuration;
        public AuthController(AppDbContext context, UserManager<Users> userManager, SignInManager<Users> signInManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }



        // POST api/<AuthController>
        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Post([FromBody] UserLogin value)
        {
            try
            {
                if (String.IsNullOrEmpty(value.Email) || String.IsNullOrEmpty(value.Password))
                {
                    return BadRequest("Please provide valid details");
                }
                Users loginUser = await _userManager.FindByEmailAsync(value.Email);
                if (loginUser != null)
                {
                    bool isValidPassword = await _userManager.CheckPasswordAsync(loginUser, value.Password);
                    if (isValidPassword)
                    {
                        var authClaims = new List<Claim>
                                            {
                                                new Claim(ClaimTypes.Name, loginUser.Email),
                                                new Claim(ClaimTypes.Sid, loginUser.Id),
                                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                            };
                        var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:secret"]));

                        var token = new JwtSecurityToken(
                            expires: DateTime.Now.AddHours(3),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
                        //loginUser.token = token;
                        return Ok(new UserResponse()
                        {
                            Email = loginUser.Email,
                            Id = loginUser.Id,
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            UserName = loginUser.UserName
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return BadRequest("Please provide valid details");
        }

    }
}
