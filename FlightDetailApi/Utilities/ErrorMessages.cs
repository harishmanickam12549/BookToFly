public static class ErrorMessages
{
    public const string NullOrEmptyFlightId = "Flight ID cannot be null or empty.";
    public const string InvalidFlightType = "Invalid flight type. Flight ID must start with 'IF' or 'DF'.";
    public static string FlightNotFound(string flightId) => $"Flight with ID {flightId} not found.";
    public const string NullFlightInput = "Flight input cannot be null.";
    public const string NullOrEmptyFlightType = "Flight type cannot be null or empty.";
    public const string NullOrEmptyFlightDetails = "Flight type, source, and destination must be provided.";
    public const string InvalidFlightId = "Invalid flight type. Flight ID must start with 'IF' or 'DF'.";

    public static string FlightNotFound(string flightId, string flightType) =>
        $"{flightType} flight with ID {flightId} not found.";
    public const string NullFlightObject = "Flight object cannot be null.";
    public const string InvalidFlightIdMissMatch = "Flight ID in route does not match Flight ID in body.";
    public const string InvalidFlightNumber = "Flight not found with the provided Flight Number.";
    public const string AllParametersRequired = "FlightType and FlightNumber both are required.";
    public const string FlightNotFoundOnSearch = "No flights found for the specified source, destination, and flight type.";
}
