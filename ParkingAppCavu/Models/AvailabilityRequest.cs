using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ParkingAppCavu.Models
{
    public class AvailabilityRequest
    {

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public ParkingDays[]? ParkingDays { get; set; }

        public decimal TotalCost { get; set; }
    }
}
