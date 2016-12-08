using System;
using NUnit.Framework;
using TravelAgency;

namespace TravelAgencyTests
{
    [TestFixture]
    public class TourScheduleTests
    {
        private TourSchedule sut;

        [SetUp]
        public void SetUp()
        {
            sut = new TourSchedule();
        }

        [Test]
        public void CanCreateNewTour()
        {
            sut.CreateTour(
                "New years eve safari",
                new DateTime(2013, 1, 1),
                20);

            var tours = sut.GetToursFor(new DateTime(2013, 1, 1));

            Assert.AreEqual(1, tours.Count, "Have one booked tour");
            Assert.AreEqual(20, tours[0].NumberOfSeats, "Correct number of booked seats");
            Assert.AreEqual("New years eve safari", tours[0].Name, "Correct tour name");
        }

        [Test]
        public void ToursAreScheduledByDateOnly()
        {
            sut.CreateTour(
                "New years eve safari",
                new DateTime(2013, 1, 1, 10, 15, 0),
                20);

            var tours = sut.GetToursFor(new DateTime(2013, 1, 1));

            Assert.AreEqual(1, tours.Count);
            Assert.AreEqual(new DateTime(2013, 1, 1), tours[0].When);
        }

        [Test]
        public void GetToursForGivenDayOnly()
        {
            sut.CreateTour(
                "New years eve safari to Samburu",
                new DateTime(2013, 1, 1),
                20);
            sut.CreateTour(
                "New years eve safari to Lake Nakuru",
                new DateTime(2013, 1, 1),
                10);
            sut.CreateTour(
                "Christmas safari",
                new DateTime(2012, 12, 24),
                20);

            var tours = sut.GetToursFor(new DateTime(2013, 1, 1));

            var expectedDate = new DateTime(2013, 1, 1);
            Assert.AreEqual(2, tours.Count);
            Assert.AreEqual(expectedDate, tours[0].When);
            Assert.AreEqual(expectedDate, tours[1].When);
        }

        [Test]
        public void NoToursReturnsEmptyList()
        {
            CollectionAssert.IsEmpty(sut.GetToursFor(new DateTime(2013, 1, 1)));
        }

        [Test]
        public void ExceptionSuggestsNextDateWithoutTours()
        {
            MakeFullyScheduledDay(new DateTime(2013, 1, 1));

            try
            {
                sut.CreateTour(
                    "Safari to Amboseli",
                    new DateTime(2013, 1, 1),
                    10);
            }
            catch (TourAllocationException e)
            {
                // Should suggest the next day - No bookings made
                Assert.AreEqual(new DateTime(2013, 1, 2), e.NextAvailableTour);
            }
        }

        [Test]
        public void ExceptionSuggestsNextDateWhenSlotsLeft()
        {
            MakeFullyScheduledDay(new DateTime(2013, 1, 1));
        
            sut.CreateTour(
                "Christmas safari",
                new DateTime(2013, 1, 2),
                20);
        
            try
            {
                sut.CreateTour(
                    "Safari to Amboseli",
                    new DateTime(2013, 1, 1),
                    10);
            }
            catch (TourAllocationException e)
            {
                // Should suggest the next day - No bookings made
                Assert.AreEqual(new DateTime(2013, 1, 2), e.NextAvailableTour);
            }
        }


        [Test]
        public void ThreeToursOnSameDateThrowsException()
        {
            MakeFullyScheduledDay(new DateTime(2013, 1, 1));

            // Making the final booking should throw an exception
            Assert.Throws<TourAllocationException>(
                () => sut.CreateTour(
                    "Tour 4",
                    new DateTime(2013, 1, 1),
                    20));
        }

        [Test]
        public void BookingSameTourTwiceThrowsException()
        {
            sut.CreateTour(
                "Ordinary tour",
                new DateTime(2013, 2, 2),
                20);

            Assert.Throws<TourAlreadyBookedException>(
                () => sut.CreateTour(
                    "Ordinary tour",
                    new DateTime(2013, 2, 2),
                    10));
        }

        [Test]
        public void ReusingSameNameOnDifferentDateShouldNotThrowException()
        {
            sut.CreateTour(
                "Ordinary tour",
                new DateTime(2013, 2, 2),
                20);

            Assert.DoesNotThrow(
                () => sut.CreateTour(
                    "Ordinary tour",
                    new DateTime(2013, 2, 3),
                    20));
        }

        // Helper method
        private void MakeFullyScheduledDay(DateTime when)
        {
            sut.CreateTour(
                "Tour 1",
                when.Date,
                20);

            sut.CreateTour(
                "Tour 2",
                when.Date,
                20);

            sut.CreateTour(
                "Tour 3",
                when.Date,
                20);
        }
    }
}