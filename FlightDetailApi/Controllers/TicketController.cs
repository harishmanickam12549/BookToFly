using FlightDetailApi.Constants;
using FlightDetailApi.Data;
using FlightDetailApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightDetailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ApplicationDbContext context, ILogger<TicketController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Ticket/{username}
        // Retrieves a user's ticket booking IDs.
        [HttpGet("{username}")]
        public async Task<IActionResult> GetTicket(string username)
        {
            _logger.LogInformation("Fetching tickets for username: {Username}", username);

            if (string.IsNullOrEmpty(username))
            {
                _logger.LogWarning("Invalid username provided.");
                return BadRequest(ResponseMessages.InvalidInput);
            }

            var ticket = await _context.Tickets.FindAsync(username);
            if (ticket == null)
            {
                _logger.LogWarning("No tickets found for user: {Username}", username);
                return NotFound(ResponseMessages.TicketNotFound);
            }

            return Ok(new {ticketList=ticket.BookingId});
        }

        // POST: api/Ticket
        // Adds a new ticket booking for a user.
        [HttpPost]
        public async Task<IActionResult> AddTicket([FromQuery] string username, [FromQuery] int bookingId)
        {
            _logger.LogInformation("Received request to add a ticket for username: {Username}, BookingId: {BookingId}", username, bookingId);

            if (string.IsNullOrEmpty(username) || bookingId == 0)
            {
                _logger.LogWarning("Invalid input for adding ticket.");
                return BadRequest(new { success = false, message = ResponseMessages.InvalidInput });
            }

            // Check if the user exists
            var isUser = await _context.Users.AnyAsync(u => u.Username == username);
            if (!isUser)
            {
                _logger.LogWarning("User not found: {Username}", username);
                return BadRequest(new { success = false, message = ResponseMessages.UsernameNotFound });
            }

            // Find existing ticket entry for the user
            var ticketEntry = await _context.Tickets.FindAsync(username);

            if (ticketEntry != null)
            {
                // Add the booking ID to the user's existing list if not already added
                if (!ticketEntry.BookingId.Contains(bookingId))
                {
                    ticketEntry.BookingId.Add(bookingId);
                }
                else
                {
                    _logger.LogWarning("Booking ID {BookingId} already associated with user: {Username}", bookingId, username);
                    return BadRequest(new { success = false, message = ResponseMessages.TicketAlreadyExists });
                }
            }
            else
            {
                // Create a new entry if none exists
                var newTicket = new TicketDetails { Username = username, BookingId = new List<int> { bookingId } };
                await _context.Tickets.AddAsync(newTicket);
            }

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = ResponseMessages.TicketAdded });
        }

        // GET: api/Ticket/GetAllTickets
        // Retrieves all username-booking ID pairs.
        [HttpGet("GetAllTickets")]
        public async Task<IActionResult> GetAllTickets()
        {
            _logger.LogInformation("Fetching all ticket details.");

            var tickets = await _context.Tickets
                .Select(t => new { t.Username, BookingId = t.BookingId })
                .ToListAsync();

            if (tickets == null || !tickets.Any())
            {
                _logger.LogWarning("No ticket data available.");
                return NotFound(new { success = false, message = ResponseMessages.NoTicketsAvailable });
            }

            return Ok( tickets );
        }
    }
}
