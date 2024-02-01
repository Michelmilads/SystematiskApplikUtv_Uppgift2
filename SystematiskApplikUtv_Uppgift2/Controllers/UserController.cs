using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SystematiskApplikUtv_Uppgift2.Entities;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;

namespace SystematiskApplikUtv_Uppgift2.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserRepo userRepo, ILogger<UserController> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }

        //Skapa anv
        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Error: Invalid User Data.");
            }
            try
            {
                _userRepo.CreateUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        //Updatera lösenord på användare
        [HttpPatch]
        public IActionResult UpdateUser([FromQuery] string passWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                if (_userRepo.GetUserThruID(GetCurrentUser()) == null)
                    return NotFound();


                _userRepo.UpdateUser(GetCurrentUser(), passWord);
                return Ok("Successfully Updated User.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Could Not Update The User.");
            }
        }

        // Delete a user
        [HttpDelete]
        public IActionResult DeleteUser()
        {
            try
            {
                if (GetCurrentUser() == 0)
                    return BadRequest();

                _userRepo.DeleteUser(GetCurrentUser());
                return Ok("Successfully Deleted User.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // Sign in
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromQuery] string userName, string passWord)
        {
            try
            {
                var user = _userRepo.AuthenticateUser(userName, passWord);

                if (user != null)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(token);
                }

                return Unauthorized("Invalid Username/Password");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserID", user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Kaninburenärborta2001Michelälskarattspela1999")); // Probably need to use a secure method to store and retrieve the key
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:1999",
                audience: "http://localhost:1999",
                claims: claims,
                expires: DateTime.Now.AddMinutes(180),
                signingCredentials: signinCredentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private int GetCurrentUser()
        {
            var idClaim = User.FindFirst("UserID");
            if (idClaim == null)
                return 0;

            var parsed = int.TryParse(idClaim.Value, out int id);
            if (parsed)
                return id;

            return 0;
        }
    }
}
