using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication6.Models;
using WebApplication6.Dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public UserManager<User> userManager { get; set; }
        public SignInManager<User> signInManager { get; set; }
        public IConfiguration config { get; }

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            if (ModelState.IsValid)
            {
                //mapping
                User appUser = new User();
                appUser.Email = register.Email;
                appUser.UserName = register.Email;
                appUser.PasswordHash = register.Password;
                appUser.Location = register.Location;
                appUser.Name = register.Name;
                
                //add to database
                IdentityResult result = await userManager.CreateAsync(appUser, register.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser,"Admin");
                    return Ok("Successully registered");
                }
                return BadRequest("Registration failed");
            }
            return BadRequest(ModelState);

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (ModelState.IsValid)
            {
                //check email
                User user = await userManager.FindByEmailAsync(login.Email);
                if (user != null)
                {
                    //check password
                    bool found = await userManager.CheckPasswordAsync(user, login.Password);
                    if (found)
                    {
                        //design token
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        var userRoles = await userManager.GetRolesAsync(user);
                        foreach (var role in userRoles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]));
                        var signingCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken token = new JwtSecurityToken(
                            audience: config["JWT:AudienceIP"],
                            issuer: config["JWT:IssuerIP"],
                            expires: DateTime.Now.AddDays(20),
                            signingCredentials: signingCred,
                            claims: claims
                            );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = DateTime.Now.AddDays(20)
                        });
                    }

                }
                ModelState.AddModelError("UserName", "userName or password wrong");
            }
            return BadRequest(ModelState);
        }
        [HttpPost("LogOut")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok("Successfully logged out.");
        }
    }
   
}
