using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelAgency
{
    public class BookingSystem
    {
        private readonly ITourSchedule schedule;
        private readonly Dictionary<Passenger, List<Tour>> bookings;

        public BookingSystem(ITourSchedule schedule)
        {
            bookings = new Dictionary<Passenger, List<Tour>>();
            this.schedule = schedule;
        }

        public void CreateBooking(string tourName, DateTime date, Passenger passenger)
        {
            if (!bookings.ContainsKey(passenger))
                bookings[passenger] = new List<Tour>();

            var availableTours = schedule.GetToursFor(date);

            if (availableTours.All(tour => tour.Name != tourName))
                throw new NoSuchTourException();

            var tourToBook = availableTours.Single();

            if (!SeatsLeft(tourToBook))
                throw new NotEnoughSeatsException();

            bookings[passenger].Add(tourToBook);
        }

        public void CancelBooking(string tourName, DateTime date, Passenger passenger)
        {
            var toRemove = (from booking in bookings
                            let bookedPassenger = booking.Key
                            let tours = booking.Value
                            where bookedPassenger == passenger
                            from t in tours
                            where t.Name == tourName && t.When == date.Date
                            select t).SingleOrDefault();

            if (toRemove == null)
                throw new NoSuchBookingException();

            bookings[passenger].Remove(toRemove);
        }

        private bool SeatsLeft(Tour tour)
        {
            // Check how many bookings have been made for this tour
            var seatsTaken = (from booking in bookings
                              let allBookedTours = booking.Value
                              from singleTour in allBookedTours
                              where singleTour == tour
                              select singleTour).Count();

            return (tour.NumberOfSeats - seatsTaken) > 0;
        }

        public List<Booking> GetBookingsFor(Passenger passenger)
        {
            var result = from tour in bookings[passenger]
                         select new Booking
                             {
                                 BookedTour = tour,
                                 Passenger = passenger,
                             };

            return result.ToList();
        }
    }
}