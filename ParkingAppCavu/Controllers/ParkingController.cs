using Microsoft.AspNetCore.Mvc;
using ParkingAppCavu.Interfaces;
using ParkingAppCavu.Models;
using System;

[Route("api/[controller]")]
[ApiController]
public class CarParkController : ControllerBase
{
    private readonly ICarParkingService _carParkingService;

    public CarParkController(ICarParkingService carParkService)
    {
        _carParkingService = carParkService ?? throw new ArgumentNullException(nameof(carParkService));
    }

    /// <summary>
    /// Request availability for parking based on the provided dates.
    /// </summary>
    [HttpGet("RequestAvailability")]
    public ActionResult<AvailabilityRequest> RequestAvailability([FromBody] AvailabilityRequest availabilityRequest)
    {
        if (ModelState.IsValid)
        {
            AvailabilityRequest bookingRequest = _carParkingService.RequestAvailability(availabilityRequest);

            if (bookingRequest != null)
            {
                return Ok(bookingRequest);
            }
        }

        return BadRequest("No availability for selected dates");
    }

    /// <summary>
    /// Create a new parking booking.
    /// </summary>
    [HttpPost("CreateBooking")]
    public ActionResult<Booking> CreateBooking([FromBody] Booking newBookingRequest)
    {
        if (ModelState.IsValid)
        {
            Booking generatedBooking = _carParkingService.CreateBooking(newBookingRequest);

            if (generatedBooking != null)
            {
                return Ok(generatedBooking);
            }
        }

        return BadRequest("Booking not created. Limit reached or invalid input.");
    }

    /// <summary>
    /// Cancel an existing parking booking by its ID.
    /// </summary>
    [HttpDelete("CancelBooking")]
    public ActionResult CancelBooking(int bookingId)
    {
        bool cancellationSuccess = _carParkingService.CancelBookingById(bookingId);

        if (cancellationSuccess)
        {
            return Ok("Booking canceled successfully.");
        }
        else
        {
            return NotFound("Booking not found or could not be canceled.");
        }
    }

    /// <summary>
    /// Amend an existing parking booking by updating its time period.
    /// </summary>
    [HttpPut("AmendBookingById")]
    public ActionResult AmendBookingById([FromBody] Booking existingBookingRequest)
    {
        bool amendmentSuccess = _carParkingService.AmendBookingById(existingBookingRequest);

        if (amendmentSuccess)
        {
            return Ok("Booking amended successfully.");
        }
        else
        {
            return NotFound("Booking not found or could not be amended.");
        }
    }

    /// <summary>
    /// Get a list of all parking bookings.
    /// </summary>
    [HttpGet("GetBookings")]
    public ActionResult<IEnumerable<Booking>> GetBookings()
    {
        return Ok(_carParkingService.GetBookings());
    }

    /// <summary>
    /// Clear all existing parking bookings.
    /// </summary>
    [HttpDelete("ClearBookings")]
    public ActionResult<bool> ClearBookings()
    {
        _carParkingService.ClearBookings();
        return Ok(true);
    }
}
