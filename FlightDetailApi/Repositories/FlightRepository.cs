using FlightDetailApi.DTO;
using FlightDetailApi.Models;
using FlightDetailApi.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FlightDetailApi.Repositories.IRepository;
using BookToFlyMVC.Validations;


namespace FlightDetailApi.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FlightRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Adds a new international flight to the database
        public async Task AddInternationalFlightAsync(InternationalFlightDetails flight)
        {
            if (flight == null)
                throw new ArgumentNullException(ErrorMessages.NullFlightObject);

            _context.InternationalFlightDetails.Add(flight);
            await _context.SaveChangesAsync();
        }

        // Adds a new domestic flight to the database
        public async Task AddDomesticFlightAsync(DomesticFlightDetails flight)
        {
            if (flight == null)
                throw new ArgumentNullException(ErrorMessages.NullFlightObject);

            _context.DomesticFlightDetails.Add(flight);
            await _context.SaveChangesAsync();
        }

        // Checks if a flight exists based on its FlightId
        public async Task<bool> FlightExists(string flightId)
        {
            if (string.IsNullOrEmpty(flightId))
                throw new ArgumentException(ErrorMessages.NullOrEmptyFlightId);

            return await _context.InternationalFlightDetails.AnyAsync(f => f.FlightId == flightId) ||
                   await _context.DomesticFlightDetails.AnyAsync(f => f.FlightId == flightId);
        }


        // Adds a flight based on the given FlightInputDTO, determining if it's international or domestic
        public async Task AddFlightAsync(FlightInputDTO flightInput)
        {
            if (flightInput == null)
                throw new ArgumentNullException(nameof(flightInput));

            if (string.IsNullOrEmpty(flightInput.FlightId))
                throw new ArgumentException(ErrorMessages.NullOrEmptyFlightId);

            if (flightInput.FlightId.StartsWith("IF")) // International Flight
            {
                flightInput.FlightType = "International";
                var flight = _mapper.Map<InternationalFlightDetails>(flightInput);
                _context.InternationalFlightDetails.Add(flight);
            }
            else if (flightInput.FlightId.StartsWith("DF")) // Domestic Flight
            {
                flightInput.FlightType = "Domestic";
                var flight = _mapper.Map<DomesticFlightDetails>(flightInput);
                _context.DomesticFlightDetails.Add(flight);
            }
            else
            {
                throw new ArgumentException(ErrorMessages.InvalidFlightId);
            }

            await _context.SaveChangesAsync();
        }

        // Retrieves a flight by its ID, determining if it's international or domestic
        public async Task<object> GetFlightByIdAsync(string flightId)
        {
            if (string.IsNullOrEmpty(flightId))
                throw new ArgumentException(ErrorMessages.NullOrEmptyFlightId);

            if (flightId.StartsWith("IF"))
            {
                return await _context.InternationalFlightDetails.FirstOrDefaultAsync(f => f.FlightId == flightId)
                       ?? throw new FlightNotFoundException(ErrorMessages.FlightNotFound(flightId, "International"));
            }
            else if (flightId.StartsWith("DF"))
            {
                return await _context.DomesticFlightDetails.FirstOrDefaultAsync(f => f.FlightId == flightId)
                       ?? throw new FlightNotFoundException(ErrorMessages.FlightNotFound(flightId, "Domestic"));
            }

            throw new ArgumentException(ErrorMessages.InvalidFlightId);
        }

        // Gets a list of flight numbers by type (International or Domestic)
        public async Task<List<string>> GetFlightNumbersByType(string flightType)
        {
            if (string.IsNullOrEmpty(flightType))
                throw new ArgumentException(ErrorMessages.NullOrEmptyFlightType);

            if (flightType.Equals("International", StringComparison.OrdinalIgnoreCase))
            {
                return await _context.InternationalFlightDetails.Select(f => f.FlightId).ToListAsync();
            }
            else if (flightType.Equals("Domestic", StringComparison.OrdinalIgnoreCase))
            {
                return await _context.DomesticFlightDetails.Select(f => f.FlightId).ToListAsync();
            }

            throw new ArgumentException(ErrorMessages.InvalidFlightType);
        }

        // Retrieves flights based on source and destination with a specified type
        public async Task<List<FlightOutputDTO>> GetFlightsBySourceAndDestination(FlightSearchInput searchInput)
        {
            if (searchInput == null)
                throw new ArgumentNullException(nameof(searchInput));

            if (string.IsNullOrEmpty(searchInput.FlightType) ||
                string.IsNullOrEmpty(searchInput.Source) ||
                string.IsNullOrEmpty(searchInput.Destination))
                throw new ArgumentException(ErrorMessages.NullOrEmptyFlightDetails);

            var allFlights = new List<FlightOutputDTO>();

            if (searchInput.FlightType.Equals("International", StringComparison.OrdinalIgnoreCase))
            {
                var flights = await _context.InternationalFlightDetails
                    .Where(f => f.Source.ToLower() == searchInput.Source.ToLower() &&
                                f.Destination.ToLower() == searchInput.Destination.ToLower())
                    .ToListAsync();
                allFlights.AddRange(_mapper.Map<List<FlightOutputDTO>>(flights));
            }
            else if (searchInput.FlightType.Equals("Domestic", StringComparison.OrdinalIgnoreCase))
            {
                var flights = await _context.DomesticFlightDetails
                    .Where(f => f.Source.ToLower() == searchInput.Source.ToLower() &&
                                f.Destination.ToLower() == searchInput.Destination.ToLower())
                    .ToListAsync();
                allFlights.AddRange(_mapper.Map<List<FlightOutputDTO>>(flights));
            }
            else
            {
                throw new ArgumentException(ErrorMessages.InvalidFlightType);
            }

            return allFlights;
        }
        // Retrieves all flights based on the specified type (International or Domestic)
        public async Task<List<FlightOutputDTO>> GetFlightsByTypeAsync(string flightType)
        {
            if (string.IsNullOrEmpty(flightType))
                throw new ArgumentException(ErrorMessages.NullOrEmptyFlightType);

            if (flightType.Equals("International", StringComparison.OrdinalIgnoreCase))
            {
                var flights = await _context.InternationalFlightDetails.ToListAsync();
                return _mapper.Map<List<FlightOutputDTO>>(flights);
            }
            else if (flightType.Equals("Domestic", StringComparison.OrdinalIgnoreCase))
            {
                var flights = await _context.DomesticFlightDetails.ToListAsync();
                return _mapper.Map<List<FlightOutputDTO>>(flights);
            }

            throw new ArgumentException(ErrorMessages.InvalidFlightType);
        }

        // Deletes a flight based on its ID
        public async Task DeleteFlightAsync(string flightId)
        {
            if (string.IsNullOrEmpty(flightId))
                throw new ArgumentException(ErrorMessages.NullOrEmptyFlightId);

            if (flightId.StartsWith("IF"))
            {
                var flight = await _context.InternationalFlightDetails.FirstOrDefaultAsync(f => f.FlightId == flightId);
                if (flight == null)
                    throw new FlightNotFoundException(ErrorMessages.FlightNotFound(flightId));

                _context.InternationalFlightDetails.Remove(flight);
            }
            else if (flightId.StartsWith("DF"))
            {
                var flight = await _context.DomesticFlightDetails.FirstOrDefaultAsync(f => f.FlightId == flightId);
                if (flight == null)
                    throw new FlightNotFoundException(ErrorMessages.FlightNotFound(flightId));

                _context.DomesticFlightDetails.Remove(flight);
            }
            else
            {
                throw new ArgumentException(ErrorMessages.InvalidFlightType);
            }

            await _context.SaveChangesAsync();
        }
        // Updates an existing flight's details
        public async Task UpdateFlightAsync(FlightInputDTO flightInput)
        {
            if (flightInput == null)
                throw new ArgumentNullException(nameof(flightInput), ErrorMessages.NullFlightInput);

            var flight = await GetFlightByIdAsync(flightInput.FlightId);
            if (flight == null)
                throw new FlightNotFoundException(ErrorMessages.FlightNotFound(flightInput.FlightId));

            if (flight is InternationalFlightDetails internationalFlight)
            {
                _mapper.Map(flightInput, internationalFlight);
                _context.InternationalFlightDetails.Update(internationalFlight);
            }
            else if (flight is DomesticFlightDetails domesticFlight)
            {
                _mapper.Map(flightInput, domesticFlight);
                _context.DomesticFlightDetails.Update(domesticFlight);
            }
            else
            {
                throw new FlightApiException(ErrorMessages.InvalidFlightType);
            }

            await _context.SaveChangesAsync();
        }
        // Decrements available seats for a flight
        public async Task<bool> DecrementAvailableSeats(string flightId)
        {
            if (string.IsNullOrEmpty(flightId))
                throw new ArgumentException(ErrorMessages.NullOrEmptyFlightId);

            if (flightId.StartsWith("IF")) // International Flight
            {
                var flight = await _context.InternationalFlightDetails
                                           .FirstOrDefaultAsync(f => f.FlightId == flightId);
                if (flight == null)
                    throw new FlightNotFoundException(ErrorMessages.FlightNotFound(flightId));

                if (flight.AvailableSeats > 0)
                {
                    flight.AvailableSeats--;
                    _context.InternationalFlightDetails.Update(flight);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false; // No seats available
            }
            else if (flightId.StartsWith("DF")) // Domestic Flight
            {
                var flight = await _context.DomesticFlightDetails
                                           .FirstOrDefaultAsync(f => f.FlightId == flightId);
                if (flight == null)
                    throw new FlightNotFoundException(ErrorMessages.FlightNotFound(flightId));

                if (flight.AvailableSeats > 0)
                {
                    flight.AvailableSeats--;
                    _context.DomesticFlightDetails.Update(flight);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false; // No seats available
            }
            throw new ArgumentException(ErrorMessages.InvalidFlightType);
        }

    }
}