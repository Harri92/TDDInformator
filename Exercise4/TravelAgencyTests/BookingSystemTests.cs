using System;
using System.Collections.Generic;
using NUnit.Framework;
using TravelAgency;

namespace TravelAgencyTests
{
    public class BookingSystemTests
    {
        private TourScheduleStub scheduleStub;
        private BookingSystem sut;
        private Passenger passenger;

        [SetUp]
        public void SetUp()
        {
            scheduleStub = new TourScheduleStub();
            sut = new BookingSystem(scheduleStub);

            passenger = new Passenger
                {
                    FirstName = "John",
                    LastName = "Doe",
                };
        }
        
        [Test]
        public void CanCreateBooking()
        {
            scheduleStub.Tours = new List<Tour>
                {
                    new Tour(new DateTime(2013, 1, 1), 2, "First tour"),
                };

            sut.CreateBooking("First tour", new DateTime(2013, 1, 1), passenger);
            var bookings = sut.GetBookingsFor(passenger);

            Assert.AreEqual(1, bookings.Count);
            var model = bookings[0];
            Assert.AreEqual(scheduleStub.Tours[0], model.BookedTour);
            Assert.AreEqual(passenger, model.Passenger);
            Assert.AreEqual(1, scheduleStub.CallsToGetToursFor.Count);
            Assert.AreEqual(new DateTime(2013, 1, 1), scheduleStub.CallsToGetToursFor[0]);
        }

        [Test]
        public void BookingOnInvalidTourThrowsException()
        {
            scheduleStub.Tours = new List<Tour>();

            Assert.Throws<NoSuchTourException>(
                () => sut.CreateBooking(
                    "Some non-existing tour", new DateTime(2013, 1, 1), passenger));
        }

        [Test]
        public void OverbookingThrowsException()
        {
            scheduleStub.Tours = new List<Tour>
                {
                    new Tour(new DateTime(2013, 1, 1), 0, "First tour"),
                };

            Assert.Throws<NotEnoughSeatsException>(
                () => sut.CreateBooking("First tour", new DateTime(2013, 1, 1), passenger));
        }

        [Test]
        public void CanCancelBooking()
        {
            scheduleStub.Tours = new List<Tour>
                {
                    new Tour(new DateTime(2013, 1, 1), 2, "First tour"),
                };

            sut.CreateBooking("First tour", new DateTime(2013, 1, 1), passenger);
            sut.CancelBooking("First tour", new DateTime(2013, 1, 1), passenger);

            var bookings = sut.GetBookingsFor(passenger);
            Assert.AreEqual(0, bookings.Count);
        }

        [Test]
        public void CancellingNonExistingBookingThrowsException()
        {
            Assert.Throws<NoSuchBookingException>(
                () => sut.CancelBooking("", new DateTime(2013, 1, 1), passenger));
        }
    }
}