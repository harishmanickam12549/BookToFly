using FlightDetailApi.Constants;
using FlightDetailApi.DTO;
using FlightDetailApi.Models;
using FlightDetailApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDetailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;
        private readonly JWTTokenService _jwtTokenService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(AdminService adminService, JWTTokenService jwtTokenService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        /// <summary>
        /// Handles admin login and returns a JWT token if credentials are valid.
        /// </summary>
        /// <param name="login">Login details including username and password.</param>
        /// <returns>Returns JWT token if successful, otherwise an error response.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            _logger.LogInformation("Login attempt for user: {Username}", login?.Username);

            if (login == null || string.IsNullOrEmpty(login.Username))
            {
                _logger.LogWarning("Login failed: {Message}", ResponseMessages.UsernameOrPasswordEmpty);
                return BadRequest(new { success = false, message = ResponseMessages.UsernameOrPasswordEmpty });
            }

            try
            {
                var admin = await _adminService.LoginAsync(login.Username, login.Password);
                if (admin == null)
                {
                    _logger.LogWarning("Login failed: {Message}", ResponseMessages.InvalidCredentials);
                    return Unauthorized(new { success = false, message = ResponseMessages.InvalidCredentials });
                }

                var token = _jwtTokenService.GenerateJWTToken(admin.Username, "Admin");
                _logger.LogInformation("Login successful for user: {Username}. JWT token generated.", login.Username);
                return Ok(new { success = true, token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Username}", login.Username);
                return StatusCode(500, new { success = false, message = ResponseMessages.InternalServerError });
            }
        }

        /// <summary>
        /// Registers a new admin user.
        /// </summary>
        /// <param name="register">Admin user details.</param>
        /// <returns>Returns success response if registration is successful.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminModel register)
        {
            _logger.LogInformation("Registration attempt for user: {Username}", register?.Username);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Registration failed: Invalid model state. Errors: {Errors}", string.Join(", ", errors));
                return BadRequest(new { success = false, errors });
            }

            try
            {
                await _adminService.RegisterAsync(register);
                _logger.LogInformation("Registration successful for user: {Username}", register.Username);
                return Ok(new { success = true, message = ResponseMessages.RegistrationSuccess });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user: {Username}", register.Username);
                return StatusCode(500, new { success = false, message = ResponseMessages.InternalServerError });
            }
        }
    }
}
