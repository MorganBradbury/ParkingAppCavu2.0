using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParkingAppCavu.Data;
using ParkingAppCavu.Interfaces;
using ParkingAppCavu.Models;

namespace ParkingAppCavu.Utils
{
    // Utility class for handling booking-related operations.
    // Implements the IBookingUtils interface.
    public class BookingUtils : IBookingUtils
    {
        private readonly CarParkDbContext _dbContext;

        // Initializes a new instance of the BookingUtils class with the provided database context.
        // dbContext: The database context for accessing booking information.
        public BookingUtils(CarParkDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        private const int TotalSpacesPerDay = 10;
        private const decimal StandardDayPrice = 10;

        // Checks if the given date falls on a weekend (Saturday or Sunday).
        private static bool IsWeekend(DateTime date) =>
            date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

        // Calculates the price per day based on the selected date.
        public decimal GetPricePerDay(DateTime selectedDate) =>
            IsWeekend(selectedDate) ? StandardDayPrice / 2 : StandardDayPrice;

        // Calculates the available parking spaces for the specified time period.
        // Returns the number of available parking spaces.
        public int SpacesAvailable(DateTime dateFrom, DateTime dateTo) =>
            TotalSpacesPerDay - _dbContext.Bookings.Count(booking => dateFrom <= booking.ToDate && dateTo >= booking.FromDate);

        // Generates an array of ParkingDays objects containing information about available spaces and prices for each day in the specified time period.
        // Returns an array of ParkingDays objects.
        public ParkingDays[] GetParkingDaysInfo(AvailabilityRequest availabilityRequest) =>
            Enumerable.Range(0, (int)(availabilityRequest.ToDate - availabilityRequest.FromDate).TotalDays + 1)
                .Select(offset => availabilityRequest.FromDate.AddDays(offset))
                .Select(selectedDate => new ParkingDays
                {
                    SpacesAvailableOnDate = SpacesAvailable(selectedDate, selectedDate),
                    PricePerDay = GetPricePerDay(selectedDate),
                    Date = selectedDate
                })
                .ToArray();

        public bool DateValidate(DateTime dateFrom, DateTime dateTo) => (dateFrom > dateTo);


    }
}
