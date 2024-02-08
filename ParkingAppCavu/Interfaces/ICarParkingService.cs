// ICarParkingService.cs
using ParkingAppCavu.Models;

namespace ParkingAppCavu.Interfaces
{
    public interface ICarParkingService
    {
        List<Booking> GetBookings();
        void ClearBookings();
        AvailabilityRequest? RequestAvailability(AvailabilityRequest availabilityRequest);
        Booking? CreateBooking(Booking newBookingRequest);
        bool CancelBookingById(int bookingId);
        bool AmendBookingById(Booking existingBookingRequest);
    }
}
