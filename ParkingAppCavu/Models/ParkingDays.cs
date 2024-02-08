using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ParkingAppCavu.Models
{

    public class ParkingDays
    {
        [DataType(DataType.Currency)]
        public decimal PricePerDay { get; set; }

        public int SpacesAvailableOnDate { get; set; }

        public DateTime Date { get; set; }
    }
}
