using ParkingAppCavu.Models;
using System;

namespace ParkingAppCavu.Interfaces
{
    public interface IBookingUtils
    {
        decimal GetPricePerDay(DateTime selectedDate);
        int SpacesAvailable(DateTime dateFrom, DateTime dateTo);
        ParkingDays[] GetParkingDaysInfo(AvailabilityRequest availabilityRequest);

        bool DateValidate(DateTime dateFrom, DateTime dateTo);
    }
}