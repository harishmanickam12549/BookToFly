function updateFlightPrefix() {
    let flightType = document.getElementById("flightType");
    let prefixSpan = document.getElementById("prefixDisplay");
    let flightIdInput = document.getElementById("flightId");
    
    if (flightType.value === "") {  // Explicitly check for an empty string
        alert("Please select a flight type.");
        prefixSpan.textContent = "--";
        flightIdInput.value = "";
        return;
    }

    let prefix = flightType.value === "International" ? "IF" : "DF";
    prefixSpan.textContent = prefix;
    
    if (flightIdInput.value) {
        flightIdInput.value = flightIdInput.value.replace(/^(IF|DF)?/, prefix);
    }
}

   function calculateDuration() {
    let arrival = document.getElementById("ArrivalTime").value;
    let departure = document.getElementById("DepartureTime").value;
    let durationField = document.getElementById("Duration");

    if (arrival && departure) {
        let arrivalParts = arrival.split(":").map(Number);
        let departureParts = departure.split(":").map(Number);

        let arrivalMinutes = arrivalParts[0] * 60 + arrivalParts[1];
        let departureMinutes = departureParts[0] * 60 + departureParts[1];

        let duration;
        if (arrivalMinutes < departureMinutes) {
            // Flight reaches the destination on the next day
            duration = (1440 - departureMinutes) + arrivalMinutes;
        } else {
            duration = arrivalMinutes - departureMinutes;
        }

        durationField.value = (duration / 60).toFixed(1);
    }
}

function validateForm(event) {
    let flightType = document.getElementById("flightType").value;
            let flightId = document.getElementById("flightId").value;
            let isValid = true;

            document.getElementById("errorFlightType").textContent = "";
            document.getElementById("errorFlightId").textContent = "";

            if (flightType === "") {
                document.getElementById("errorFlightType").textContent = "Please select a flight type.";
                isValid = false;
            }

            if (flightId.trim() === "") {
                document.getElementById("errorFlightId").textContent = "Please enter a flight number.";
                isValid = false;
            }

            if (!isValid) {
                event.preventDefault();
            }
    console.log("Validating form...");
    let valid = true;
    let nameRegex = /^[a-zA-Z ]{3,}$/;
    let airline = document.getElementById("AirlineName").value;
    let source = document.getElementById("Source").value;
    let destination = document.getElementById("Destination").value;
    let arrival = document.getElementById("ArrivalTime").value;
    let departure = document.getElementById("DepartureTime").value;
    let availableSeats = document.getElementById("AvailableSeats").value;
    let ticketPrice = document.getElementById("TicketPrice").value;

    // Clear previous errors
    document.getElementById("errorAirline").innerText = "";
    document.getElementById("errorSource").innerText = "";
    document.getElementById("errorDestination").innerText = "";
    document.getElementById("errorTime").innerText = "";
    document.getElementById("errorSeats").innerText = "";
    document.getElementById("errorPrice").innerText = "";

    // Validate Airline Name
    if (!nameRegex.test(airline)) {
        document.getElementById("errorAirline").innerText = "Invalid airline name. Length should be at least 3 and contain only alphabets.";
        valid = false;
    }

    // Validate Source
    if (!nameRegex.test(source)) {
        document.getElementById("errorSource").innerText = "Invalid source. Length should be at least 3 and contain only alphabets.";
        valid = false;
    }

    // Validate Destination
    if (!nameRegex.test(destination)) {
        document.getElementById("errorDestination").innerText = "Invalid destination. Length should be at least 3 and contain only alphabets.";
        valid = false;
    }

    // Validate Arrival and Departure Time
    if (arrival === departure) {
        document.getElementById("errorTime").innerText = "Arrival and Departure time cannot be the same.";
        valid = false;
    }

    // Validate Available Seats
    if (availableSeats < 1 || availableSeats > 500) {
        document.getElementById("errorSeats").innerText = "Seats should be between 1 and 500.";
        valid = false;
    }

    // Validate Ticket Price
    if (ticketPrice < 1000 || ticketPrice > 50000) {
        document.getElementById("errorPrice").innerText = "Ticket price should be between 1000 and 50000.";
        valid = false;
    }

    // Validate Available Days
    const availableDays = document.querySelectorAll('input[name="AvailableDays"]:checked');
    const availableDaysError = document.getElementById('availableDaysError');
    if (availableDays.length === 0) {
        availableDaysError.textContent = 'Please select at least one available day.';
        valid = false;
    } else {
        availableDaysError.textContent = '';
    }

    // If validation fails, prevent form submission
    if (!valid) {
        console.log("Validation failed. Form submission prevented.");
        event.preventDefault(); // Prevent form submission
        return false; // Ensure the form does not submit
    }

    console.log("Validation passed. Form will be submitted.");
    return true; // Allow form submission
}

// // Re-enable the save button on input change
document.querySelectorAll('#flightForm input, #flightForm select, #flightForm textarea').forEach(element => {
    element.addEventListener('input', () => {
        validateForm(event); // Re-run validation
        // let saveButton = document.getElementById("saveButton");
        // saveButton.disabled = false; // Re-enable the button
    });
});
