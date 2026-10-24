using System;
using System.ComponentModel.DataAnnotations;
using FlightDetailApi.Constants;

namespace FlightDetailApi.Models
{
    public class BookingDetails
    {
        [Key]
        [Required]
        public string BookingId { get; set; }  // Unique ID for the booking

        [Required(ErrorMessage = ModelConstants.FlightIdRequired)]
        public string FlightId { get; set; }   // Associated Flight ID

        [Required(ErrorMessage = ModelConstants.PassengerNameRequired)]
        [StringLength(100, ErrorMessage = ModelConstants.PassengerNameLength)]
        public string PassengerName { get; set; }

        [Required(ErrorMessage = ModelConstants.EmailRequired)]
        [EmailAddress(ErrorMessage = ModelConstants.InvalidEmailFormat)]
        public string Email { get; set; }

        [Required(ErrorMessage = ModelConstants.PhoneNumberRequired)]
        [RegularExpression(RegexPatterns.PhoneNumberPattern, ErrorMessage = ModelConstants.InvalidPhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = ModelConstants.BookingDateRequired)]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = ModelConstants.TravelDateRequired)]
        [CustomValidation(typeof(BookingDetails), nameof(ValidateTravelDate))]
        public DateTime DateOfTravel { get; set; } // Date of travel

        public bool IsCanceled { get; set; }  // True if the ticket is canceled

        // Custom validation method for DateOfTravel
        public static ValidationResult ValidateTravelDate(DateTime date)
        {
            if (date < DateTime.Today)
            {
                return new ValidationResult(ModelConstants.TravelDateInPast);
            }
            return ValidationResult.Success;
        }   
    }
}
