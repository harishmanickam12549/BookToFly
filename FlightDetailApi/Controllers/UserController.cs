using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightDetailApi.Constants;
using FlightDetailApi.Data;
using FlightDetailApi.Models;
using FlightDetailApi.DTO;

namespace FlightDetailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UserController> _logger;

        public UserController(ApplicationDbContext dbContext, ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegistrationModel userDetail)
        {
            if (userDetail == null || string.IsNullOrWhiteSpace(userDetail.Username))
            {
                _logger.LogWarning("Invalid registration attempt: Username missing.");
                return BadRequest(new { success = false, message = ResponseMessages.UsernameRequired });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                // Check if Email already exists
                if (await _dbContext.Users.AnyAsync(u => u.Email == userDetail.Email))
                {
                    _logger.LogWarning("Registration attempt failed: Email {Email} already exists.", userDetail.Email);
                    return Conflict(new { success = false, message = ResponseMessages.EmailExists });
                }

                // Check if Username already exists
                if (await _dbContext.Users.AnyAsync(u => u.Username == userDetail.Username))
                {
                    _logger.LogWarning("Registration attempt failed: Username {Username} already taken.", userDetail.Username);
                    return Conflict(new { success = false, message = ResponseMessages.UsernameExists });
                }

                // Hash password using BCrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDetail.Password);

                // Map UserRegistrationModel to User entity
                var newUser = new UserRegistrationModel
                {
                    Username = userDetail.Username,
                    Email = userDetail.Email,
                    Password = hashedPassword, // Store hashed password
                    FullName = userDetail.FullName,
                    PhoneNumber = userDetail.PhoneNumber
                };

                // Save the new user
                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("User {Username} registered successfully.", userDetail.Username);
                return Ok(new { success = true, message = ResponseMessages.RegistrationSuccess });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, new { success = false, message = ResponseMessages.RegistrationError });
            }
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDTO loginDetails)
        {
            if (loginDetails == null || string.IsNullOrEmpty(loginDetails.Username) || string.IsNullOrEmpty(loginDetails.Password))
            {
                _logger.LogWarning("Login attempt failed: Missing username or password.");
                return BadRequest(new { success = false, message = ResponseMessages.LoginFailed });
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == loginDetails.Username);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed: Username {Username} not found.", loginDetails.Username);
                return Unauthorized(new { success = false, message = ResponseMessages.LoginFailed });
            }

            // Verify the password using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDetails.Password, user.Password);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Login attempt failed: Incorrect password for user {Username}.", loginDetails.Username);
                return Unauthorized(new { success = false, message = ResponseMessages.LoginFailed });
            }

            _logger.LogInformation("User {Username} logged in successfully.", loginDetails.Username);
            return Ok(new { success = true, message = ResponseMessages.LoginSuccess });
        }

        // GET: api/user/{username}
        [HttpGet("{username}")]
        public async Task<ActionResult> GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                _logger.LogWarning("User fetch failed: Username required.");
                return BadRequest(new { success = false, message = ResponseMessages.UsernameRequired });
            }

            var user = await _dbContext.Users
                .Where(u => u.Username == username)
                .Select(u => new
                {
                    u.Username,
                    u.Email,
                    u.FullName,
                    u.PhoneNumber
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogWarning("User fetch failed: Username {Username} not found.", username);
                return NotFound(new { success = false, message = ResponseMessages.UserNotFound });
            }

            return Ok( user );
        }
    }
}