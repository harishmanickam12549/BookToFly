using BookToFlyMVC.Validations;
using FlightDetailApi.Constants;
using FlightDetailApi.DTO;
using FlightDetailApi.Models;
using FlightDetailApi.Repositories;
using FlightDetailApi.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDetailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;
        private readonly ILogger<FlightController> _logger;

        public FlightController(IFlightRepository flightRepository, ILogger<FlightController> logger)
        {
            _flightRepository = flightRepository;
            _logger = logger;
        }
        // POST: api/Flight/AddFlight
        // Adds a single flight (Admin-only access)
        [HttpPost("AddFlight")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddFlight([FromBody] FlightInputDTO flightInput)
        {
            _logger.LogInformation("AddFlight request received for FlightId: {FlightId}", flightInput?.FlightId);

            if (flightInput == null)
            {
                _logger.LogWarning("Flight input is null.");
                return BadRequest(new { message = "Flight input is required." });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid flight input model for FlightId: {FlightId}", flightInput.FlightId);
                return BadRequest(ModelState);
            }

            // Check if flight already exists
            if (await _flightRepository.FlightExists(flightInput.FlightId))
            {
                _logger.LogWarning("Duplicate FlightId detected: {FlightId}", flightInput.FlightId);
                return BadRequest(ResponseMessages.FlightAlreadyExists+flightInput.FlightId);
            }

            try
            {
                await _flightRepository.AddFlightAsync(flightInput);
                return Ok(ResponseMessages.FlightAdded);
            }
            catch (FlightApiException flightApiException)
            {
                _logger.LogWarning("Invalid flight type for FlightId: {FlightId} {flightApiException}", flightInput.FlightId, flightApiException.Message);
                return BadRequest(flightApiException.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding flight: {FlightId}", flightInput.FlightId);
                return StatusCode(500, $"An error occurred while saving the flight: {ex.Message}");
            }
        }

        // GET: api/Flight/DisplayAllFlightNumbersByType?FlightType=Domestic
        // Retrieves flight numbers based on flight type.
        [HttpGet("DisplayAllFlightNumbersByType")]
        public async Task<IActionResult> DisplayAllFlightNumbersByType([FromQuery] string flightType)
        {
            _logger.LogInformation("DisplayAllFlightNumbersByType request received for FlightType: {FlightType}", flightType);

            if (string.IsNullOrEmpty(flightType))
            {
                return BadRequest("FlightType is required.");
            }

            var flightNumbers = await _flightRepository.GetFlightNumbersByType(flightType);

            if (!flightNumbers.Any())
            {
                return Ok(new { message = ResponseMessages.FlightNotFoundOnFlightType });
            }

            return Ok(flightNumbers);
        }

        // POST: api/Flight/DisplayFlightBySourceAndDestination
        // Fetches flights based on source and destination.
        [HttpPost("DisplayFlightBySourceAndDestination")]
        public async Task<IActionResult> GetFlightsBySourceAndDestination(FlightSearchInput searchInput)
        {
            _logger.LogInformation(ResponseMessages.SearchFlight);

            if (string.IsNullOrWhiteSpace(searchInput.Source) || string.IsNullOrWhiteSpace(searchInput.Destination) || string.IsNullOrWhiteSpace(searchInput.FlightType))
            {
                return BadRequest(ErrorMessages.AllParametersRequired);
            }

            var flights = await _flightRepository.GetFlightsBySourceAndDestination(searchInput);

            if (!flights.Any())
            {
                return NotFound(ErrorMessages.FlightNotFoundOnSearch);
            }

            return Ok(flights);
        }

        // GET: api/Flight/DisplayFlightByType
        // Fetches flights based on flight type.
        [HttpGet("DisplayFlightByType")]
        public async Task<List<FlightOutputDTO>> GetFlightsByType(string flightType)
        {
            return await _flightRepository.GetFlightsByTypeAsync(flightType);
        }

        // GET: api/Flight/MatchFlightByNumberAndType
        // Matches a flight based on its type and number.
        [HttpGet("MatchFlightByNumberAndType")]
        public async Task<IActionResult> MatchFlightByNumberAndType([FromQuery] string flightType, [FromQuery] string flightNumber)
        {
            _logger.LogInformation("MatchFlightByNumberAndType invoked with FlightType: {FlightType}, FlightNumber: {FlightNumber}", flightType, flightNumber);

            if (string.IsNullOrEmpty(flightType) || string.IsNullOrEmpty(flightNumber))
            {
                return BadRequest(ErrorMessages.AllParametersRequired);
            }

            try
            {
                var flightLists = await _flightRepository.GetFlightsByTypeAsync(flightType);
                var flight = flightLists.FirstOrDefault(f => f.FlightId.Equals(flightNumber, StringComparison.OrdinalIgnoreCase));

                if (flight == null)
                {
                    return NotFound(ErrorMessages.InvalidFlightNumber);
                }

                return Ok(flight);
            }
            catch (FlightNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (FlightApiException ex)
            {
                _logger.LogError(ex, "An error occurred while matching the flight.");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Flight/UpdateFlight/{flightId}
        // Updates flight details (Admin-only access).
        [HttpPut("UpdateFlight/{flightId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFlight(string flightId, [FromBody] FlightInputDTO flightInput)
        {
            _logger.LogInformation("UpdateFlight request received for FlightId: {FlightId}", flightId);

            if (flightInput == null)
            {
                return BadRequest("Flight input is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (flightId != flightInput.FlightId)
            {
                return BadRequest(ErrorMessages.InvalidFlightIdMissMatch);
            }

            try
            {
                await _flightRepository.UpdateFlightAsync(flightInput);
                return Ok(ResponseMessages.FlightUpdated);
            }
            catch (FlightNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (UnauthorizedFlightAccessException ex)
            {
                _logger.LogWarning(ex.Message);
                return Forbid(ResponseMessages.UnauthorizedAccess);
            }
            catch (FlightApiException ex)
            {
                _logger.LogError(ex, "Error updating flight {FlightId}", flightId);
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Flight/DeleteFlight/{flightId}
        // Deletes a flight.
        [HttpDelete("DeleteFlight/{flightId}")]
        public async Task<IActionResult> DeleteFlight(string flightId)
        {
            try
            {
                await _flightRepository.DeleteFlightAsync(flightId);
                return Ok(new { success = true, message = ResponseMessages.FlightDeleted });
            }
            catch (FlightNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
