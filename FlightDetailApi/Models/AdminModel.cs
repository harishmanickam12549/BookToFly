using System.ComponentModel.DataAnnotations;
using FlightDetailApi.Validations;
using FlightDetailApi.Constants;

namespace FlightDetailApi.Models
{
    public class AdminModel
    {
        [Key]
        [Required(ErrorMessage = ModelConstants.UsernameRequired)]
        [StringLength(50, ErrorMessage = ModelConstants.UsernameLength)]
        [CheckUserName(ModelConstants.UsernameExists)] // Custom Attribute
        public string Username { get; set; }

        [Required(ErrorMessage = ModelConstants.PasswordRequired)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = ModelConstants.PasswordLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
