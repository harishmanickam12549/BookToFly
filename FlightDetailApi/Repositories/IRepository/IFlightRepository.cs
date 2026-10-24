using FlightDetailApi.DTO;
using FlightDetailApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightDetailApi.Repositories.IRepository
{
    public interface IFlightRepository
    {
        // Adds a new flight asynchronously based on the provided flight details.
        Task AddFlightAsync(FlightInputDTO flightInput);
        
        // Updates an existing flight's details asynchronously.
        Task UpdateFlightAsync(FlightInputDTO flightInput);
        
        // Checks if a flight exists based on the flight ID.
        Task<bool> FlightExists(string flightId);
        
        // Retrieves a list of flight numbers based on the flight type.
        Task<List<string>> GetFlightNumbersByType(string flightType);
        
        // Fetches flights that match the specified source and destination.
        Task<List<FlightOutputDTO>> GetFlightsBySourceAndDestination(FlightSearchInput searchInput);
        
        // Retrieves flights asynchronously based on their type.
        Task<List<FlightOutputDTO>> GetFlightsByTypeAsync(string flightType);
        
        // Retrieves flight details by its unique ID.
        Task<object> GetFlightByIdAsync(string flightId);
        
        // Deletes a flight asynchronously based on the flight ID.
        Task DeleteFlightAsync(string flightId);
        
        // Decreases the available seats for a flight and returns success status.
        Task<bool> DecrementAvailableSeats(string flightId);
    }
}