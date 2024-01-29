using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
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
                return StatusCode(StatusCodes.Status201Created, "User created successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Could Not Create The User.");
            }
        }

        //Updatera anv
        [HttpPatch("{userID}")]
        public IActionResult UpdateUser(int userID, [FromBody] User updateUser)
        {

            if (updateUser == null)
            {
                return BadRequest("Error: Invalid User Data.");
            }
            try
            {
                //User ID från token
                var tokenUserID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                //Uppdaterade anv har samma ID
                updateUser.UserID = tokenUserID;

                //Kolla att Anv ID som uppdateras är samma som den som uppdaterar
                if (_userRepo.GetUserThruID(userID).UserID != tokenUserID)
                {
                    return Forbid("Not authorized to update this rating.");
                }

                _userRepo.UpdateUser(tokenUserID, updateUser);
                return Ok("User Updated Successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Could Not Update The User");
            }
        }

        // Delete a user
        [HttpDelete("{userID}")]
        public IActionResult DeleteUser(int userID)
        {
            try
            {
                // User ID from token
                var tokenUserID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // check if the user ID of the recipe you're updating matches the person who updates it.
                if (_userRepo.GetUserThruID(userID).UserID != tokenUserID)
                {
                    return Forbid("Not authorized to delete this rating.");
                }

                _userRepo.DeleteUser(userID);
                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Could Not Delete User.");
            }
        }

        // Sign in
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid Username/Password.");
            }

            var authenticateUser = _userRepo.AuthenticateUser(user);

            if (authenticateUser != null)
            {
                // If the user exists in the database, generate and return a JWT token along with user ID
                var token = GenerateJwtToken(authenticateUser);
                return Ok(new { Token = token, UserID = authenticateUser.UserID });
            }
            else
            {
                return Unauthorized("Invalid Username/Password");
            }
        }

        // Generate a temporary 180 min token
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName), // Username claim
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()), // User ID claim
                new Claim(ClaimTypes.Email, user.Email), // Email claim
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Kaninburenärborta2001")); // Probably need to use a secure method to store and retrieve the key
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:5265",
                audience: "http://localhost:5265",
                claims: claims,
                expires: DateTime.Now.AddMinutes(180),
                signingCredentials: signinCredentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
