using System;
using System.ComponentModel.DataAnnotations;
using FlightDetailApi.Constants;

namespace FlightDetailApi.Models
{
    public class InternationalFlightDetails
    {
        [Key]
        [Required]
        [RegularExpression(RegexPatterns.FlightIdPatternIF, ErrorMessage = ModelConstants.FlightIdFormat)]
        public string FlightId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = ModelConstants.FlightNameLength)]
        public string AirlineName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = ModelConstants.SourceLength)]
        public string Source { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = ModelConstants.DestinationLength)]
        public string Destination { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan ArrivalTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan DepartureTime { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = ModelConstants.DurationPositive)]
        public double Duration { get; set; }

        [Required]
        [Range(1, 500, ErrorMessage = ModelConstants.AvailableSeatsRange)]
        public int AvailableSeats { get; set; }

        [Required]
        [Range(1000, 50000, ErrorMessage = ModelConstants.TicketPriceRange)]
        public decimal TicketPrice { get; set; }

        [Required(ErrorMessage = ModelConstants.AvailableDaysRequired)]
        [MinLength(1, ErrorMessage = ModelConstants.AtLeastOneAvailableDay)]
        public List<string> AvailableDays { get; set; }
    }
}
