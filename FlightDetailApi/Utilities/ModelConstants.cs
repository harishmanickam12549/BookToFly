namespace FlightDetailApi.Constants
{
    public static class RegexPatterns
    {
        public const string PhoneNumberPattern = @"^[6-9]\d{9}$";
        public const string FlightIdPatternDF = @"^(DF)[0-9]{1,4}$";
        public const string FlightIdPatternIF = @"^(IF)[0-9]{1,4}$";
        public const string UsernameNotEmpty = @"^(?!\s*$).+"; // Ensure Username is not empty
        public const string PasswordComplexity = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{6,15}$"; 
        // Password must have uppercase, lowercase, special character
        public const string FullNamePattern = @"^[A-Za-z\s]+$"; // Only letters and spaces allowed
    }
    public static class ModelConstants
    {
        //Error Messages for Admin model (AdminModel)
        public const string UsernameRequired = "Username is required.";
        public const string UsernameLength = "Username cannot exceed 50 characters.";
        public const string UsernameExists = "User Name Already Exist";
        public const string PasswordRequired = "Password is required.";
        public const string PasswordLength = "Password must be between 6 and 100 characters.";

        //Error message for Booking Details (BookingDetails)
        public const string FlightIdRequired = "Flight ID is required.";
        public const string PassengerNameRequired = "Passenger name is required.";
        public const string PassengerNameLength = "Passenger name cannot exceed 100 characters.";
        public const string EmailRequired = "Email is required.";
        public const string InvalidEmailFormat = "Invalid email format.";
        public const string PhoneNumberRequired = "Phone number is required.";
        public const string InvalidPhoneNumber = "Phone number must start with 6, 7, 8, or 9 and be exactly 10 digits.";
        public const string BookingDateRequired = "Booking date is required.";
        public const string TravelDateRequired = "Date of travel is required.";
        public const string TravelDateInPast = "Date of travel cannot be in the past.";

        //Error message for Domestic and International Flight Details (DomesticFlightDetails)
        public const string FlightIdFormat = "Flight number must start with 'IF' (International Flight) followed by 1 to 4 digits.";
        public const string FlightNameLength = "Flight name cannot exceed 100 characters.";
        public const string SourceLength = "Source location cannot exceed 50 characters.";
        public const string DestinationLength = "Destination location cannot exceed 50 characters.";
        public const string DurationPositive = "Duration must be a positive number.";
        public const string AvailableSeatsRange = "Available seats should be between 1 and 500.";
        public const string TicketPriceRange = "Ticket price should be between 1000 and 50000.";
        public const string AvailableDaysRequired = "Available days are required.";
        public const string AtLeastOneAvailableDay = "There must be at least one available day.";

        //Error message for User Registration ()
        public const string UsernameNotEmpty = "Username must contain at least one character.";

        public const string EmailInvalid = "Invalid email address.";
        public const string PasswordComplexity = "Password must be between 6 and 15 characters and contain at least one uppercase letter, one lowercase letter, and one special character.";
        public const string FullNameLength = "Full Name cannot exceed 100 characters.";
        public const string FullNameFormat = "Full Name should only contain letters and spaces.";
        public const string PhoneNumberInvalid = "Invalid phone number.";
    }
}
