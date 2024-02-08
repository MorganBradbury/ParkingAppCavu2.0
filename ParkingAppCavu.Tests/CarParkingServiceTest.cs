using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ParkingAppCavu.Data;
using ParkingAppCavu.Interfaces;
using ParkingAppCavu.Models;
using ParkingAppCavu.Services;
using System;

namespace ParkingAppCavu.Tests
{
    [TestClass]
    public class CarParkingServiceTests
    {
        private CarParkDbContext _dbContext;
        private Mock<IBookingUtils> _bookingUtilsMock;
        private CarParkingService _carParkingService;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<CarParkDbContext>()
                .UseInMemoryDatabase(databaseName: "LocalDb")
                .Options;

            _dbContext = new CarParkDbContext(options);
            _bookingUtilsMock = new Mock<IBookingUtils>();
            _carParkingService = new CarParkingService(_dbContext, _bookingUtilsMock.Object);
        }

        [TestMethod]
        // Create booking happy path.
        public void CreateBooking_WithAvailableSpaces_ReturnsBooking()
        {
            // Arrange
            _bookingUtilsMock.Setup(bu => bu.SpacesAvailable(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(10);

            var newBookingRequest = new Booking
            {
                BookingId = 1,
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(5),
            };

            // Act
            var result = _carParkingService.CreateBooking(newBookingRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newBookingRequest, result);
        }

        [TestMethod]
        // Create booking should fail as no spaces available.
        public void CreateBooking_WithNoAvailableSpaces_ReturnsNull()
        {
            // Arrange
            _bookingUtilsMock.Setup(bu => bu.SpacesAvailable(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(0);

            var newBookingRequest = new Booking
            {
                BookingId = 1,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(5),
            };

            // Act
            var result = _carParkingService.CreateBooking(newBookingRequest);

            // Assert
            Assert.IsNull(result);
        }
    }
}
