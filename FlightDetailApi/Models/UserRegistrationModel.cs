using System.ComponentModel.DataAnnotations;
using FlightDetailApi.Constants;

namespace FlightDetailApi.Models
{
    public class UserRegistrationModel
    {
        [Key]
        [Required(ErrorMessage = ModelConstants.UsernameRequired)]
        [StringLength(50, MinimumLength = 4, ErrorMessage = ModelConstants.UsernameLength)]
        [RegularExpression(RegexPatterns.UsernameNotEmpty, ErrorMessage = ModelConstants.UsernameNotEmpty)]
        public string Username { get; set; }

        [Required(ErrorMessage = ModelConstants.EmailRequired)]
        [EmailAddress(ErrorMessage = ModelConstants.EmailInvalid)]
        public string Email { get; set; }

        [Required(ErrorMessage = ModelConstants.PasswordRequired)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = ModelConstants.PasswordLength)]
        [DataType(DataType.Password)]
        [RegularExpression(RegexPatterns.PasswordComplexity, ErrorMessage = ModelConstants.PasswordComplexity)]
        public string Password { get; set; }

        [StringLength(100, ErrorMessage = ModelConstants.FullNameLength)]
        [RegularExpression(RegexPatterns.FullNamePattern, ErrorMessage = ModelConstants.FullNameFormat)]
        public string FullName { get; set; }

        [Phone(ErrorMessage = ModelConstants.PhoneNumberInvalid)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
