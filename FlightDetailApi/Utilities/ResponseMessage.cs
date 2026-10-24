namespace FlightDetailApi.Constants
{
    public static class ResponseMessages
    {
        public const string UsernameOrPasswordEmpty = "Username or password should not be null.";
        public const string InvalidCredentials = "Invalid username or password.";
        public const string LoginSuccess = "Login successful.";
        public const string RegistrationSuccess = "Registration successful.";
        public const string InternalServerError = "An error occurred while processing your request.";

        //Booking Controller Response Messages
        public const string BookingNotFound = "Booking not found.";
        public const string NoSeatsAvailable = "No seats available for this flight or flight not found.";
        public const string TicketCancelled = "Ticket canceled successfully.";

        //Flight Controller Response Message
        public const string InvalidInput = "Invalid input.";
        public const string FlightAdded = "Flight added successfully.";
        public const string FlightAlreadyExists = "Flight with the given FlightId already exists.Flight Number =";
        public const string FlightUpdated = "Flight updated successfully.";
        public const string FlightDeleted = "Flight Deleted Successfully";
        public const string UnauthorizedAccess = "You do not have permission to perform this action.";

        //Ticked Controller Response Message
        public const string TicketNotFound = "No tickets found for this user.";
        public const string TicketAlreadyExists = "This booking ID is already associated with the user.";
        public const string TicketAdded = "Successfully added ticket.";
        public const string UsernameNotFound = "Error: Username not found.";
        public const string NoTicketsAvailable = "No ticket data available.";

        //User Controller Response Message
        public const string UsernameRequired = "Username is required.";
        public const string EmailExists = "Email is already registered.";
        public const string UsernameExists = "Username is already taken.";
        public const string RegistrationError = "Something went wrong during registration. Please try again.";
        public const string LoginFailed = "Invalid username or password.";
        public const string UserNotFound = "User not found.";

        //Logger Messages
        public const string SearchFlight = "GetFlightsBySourceAndDestination request received.";

        //Response message
        public const string FlightNotFoundOnFlightType = "No flights found for the specified Flight Type.";
    }
}
