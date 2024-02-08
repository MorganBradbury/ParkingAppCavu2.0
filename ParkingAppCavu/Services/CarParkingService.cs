// CarParkingService.cs
using ParkingAppCavu.Data;
using ParkingAppCavu.Interfaces;
using ParkingAppCavu.Models;
using ParkingAppCavu.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ParkingAppCavu.Services
{
    public class CarParkingService : ICarParkingService
    {
        // Database context for managing bookings
        private readonly CarParkDbContext _dbContext;

        // Utility class for booking-related operations
        private readonly IBookingUtils _bookingUtils;

        // Constructor to initialize the service with required dependencies
        public CarParkingService(CarParkDbContext dbContext, IBookingUtils bookingUtils)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _bookingUtils = bookingUtils ?? throw new ArgumentNullException(nameof(bookingUtils));
        }

        // Retrieve a list of all bookings
        public List<Booking> GetBookings() => _dbContext.Bookings.ToList();

        // Clear all existing bookings
        public void ClearBookings()
        {
            _dbContext.Bookings.RemoveRange(_dbContext.Bookings);
            _dbContext.SaveChanges();
        }

        // Request availability for a given time period
        public AvailabilityRequest RequestAvailability(AvailabilityRequest availabilityRequest)
        {
            // Check dates are correct and the dateFrom is before the dateTo
            bool areDatesInvalid = _bookingUtils.DateValidate(availabilityRequest.FromDate, availabilityRequest.ToDate);

            if (areDatesInvalid)
            {
                return null;
            }

            // Calculate parking days information using utility class
            ParkingDays[] parkingDaysPerQuery = _bookingUtils.GetParkingDaysInfo(availabilityRequest);

            // Update availability request with calculated values
            availabilityRequest.ParkingDays = parkingDaysPerQuery;
            availabilityRequest.TotalCost = parkingDaysPerQuery.Sum(p => p.PricePerDay);

            return availabilityRequest;
        }

        // Create a new booking
        public Booking CreateBooking(Booking newBookingRequest)
        {
            // Check dates are correct and the dateFrom is before the dateTo
            bool areDatesInvalid = _bookingUtils.DateValidate(newBookingRequest.FromDate, newBookingRequest.ToDate);

            if (areDatesInvalid)
            {
                return null;
            }

            // Check if there are available spaces for the requested time period
            int spacesAvailable = _bookingUtils.SpacesAvailable(newBookingRequest.FromDate, newBookingRequest.ToDate);
            if (spacesAvailable == 0) { return null; } // No available spaces; booking cannot be created

            // Add the new booking to the database
            _dbContext.Bookings.Add(newBookingRequest);
            _dbContext.SaveChanges();

            return newBookingRequest;
        }

        // Cancel a booking by its ID
        public bool CancelBookingById(int bookingId)
        {
            // Find the booking in the database by ID
            Booking bookingToRemove = _dbContext.Bookings.FirstOrDefault(b => b.BookingId == bookingId);

            if (bookingToRemove != null)
            {
                // Remove the booking and save changes
                _dbContext.Bookings.Remove(bookingToRemove);
                _dbContext.SaveChanges();
                return true;
            }

            return false; // Booking with the specified ID not found
        }

        // Amend an existing booking by updating its time period
        public bool AmendBookingById(Booking existingBookingRequest)
        {
            // Check dates are correct and the dateFrom is before the dateTo
            bool areDatesInvalid = _bookingUtils.DateValidate(existingBookingRequest.FromDate, existingBookingRequest.ToDate);

            if (areDatesInvalid)
            {
                return false;
            }

            // Check if there are available spaces for the updated time period
            int spacesAvailable = _bookingUtils.SpacesAvailable(existingBookingRequest.FromDate, existingBookingRequest.ToDate);
            if (spacesAvailable == 0)
            {
                return false; // No available spaces; booking cannot be amended
            }

            // Find the booking in the database by ID
            Booking bookingToAmend = _dbContext.Bookings.FirstOrDefault(b => b.BookingId == existingBookingRequest.BookingId);

            if (bookingToAmend != null)
            {
                // Update the booking's time period and save changes
                bookingToAmend.FromDate = existingBookingRequest.FromDate;
                bookingToAmend.ToDate = existingBookingRequest.ToDate;
                _dbContext.SaveChanges();
                return true;
            }

            return false; // Booking with the specified ID not found
        }
    }
}
