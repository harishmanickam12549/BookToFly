using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using BookToFlyMVC.Models;
using BookToFlyMVC.DTO;
using BookToFlyMVC.Exceptions;
using AutoMapper;
using System.Text.Json;
using System.Text;

namespace BookToFlyMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        // Constructor: Initializes logger, HttpClient, and AutoMapper
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory client, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _client = client.CreateClient("FlightClient");
        }

        // Returns the login view
        public IActionResult Login()
        {
            return View();
        }

        // Handles the login post request
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginDetails)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDetails); // Return view with validation errors
            }

            try
            {
                var admin = _mapper.Map<LoginDTO>(loginDetails);
                var jsonContent = JsonSerializer.Serialize(admin);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(_client.BaseAddress + "Admin/Login", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonDoc = JsonDocument.Parse(jsonResponse);
                    var token = jsonDoc.RootElement.GetProperty("token").GetString();

                    // Store only the token in session
                    HttpContext.Session.SetString("JWT_TOKEN", token);

                    _logger.LogInformation("User {User} logged in successfully.", loginDetails.Username);
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    throw new LoginFailedException();
                }
            }
            catch (LoginFailedException ex)
            {
                _logger.LogError(ex, "Login failed for user {User}.", loginDetails.Username);
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during login for user {User}.", loginDetails.Username);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
            }

            return View(loginDetails);
        }

        // Handles logout functionality
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("JWT_TOKEN");

            _logger.LogInformation("User logged out successfully.");
            return RedirectToAction("Login");
        }

        // Returns the home page (index view)
        public IActionResult Index()
        {
            return View();
        }

        // Returns the privacy policy page view
        public IActionResult Privacy()
        {
            return View();
        }

        // This action method is for the search flight view
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult SearchFlight()
        {
            return View();
        }

        // Handles error pages and displays appropriate error messages
        public IActionResult Error(int? code = null, string errorMessage = null, string additionalDetails = null)
        {
            ViewData["ErrorCode"] = code ?? 500;
            ViewData["ErrorMessage"] = errorMessage ?? "Internal Server Error :(";
            ViewData["AdditionalDetails"] = additionalDetails ?? "Contact Aspire Systems Development Team.";

            _logger.LogError("Error encountered. Code: {Code}, Message: {Message}, Details: {Details}", 
                ViewData["ErrorCode"], ViewData["ErrorMessage"], ViewData["AdditionalDetails"]);

            return View("Error");
        }
    }
}
