using System;
using System.Linq;
using System.Collections.Generic;

namespace TravelAgency
{
    public class TourSchedule : ITourSchedule
    {
        private List<Tour> scheduledTours = new List<Tour>();
        private const int MaxTours = 3;

        public void CreateTour(string name, DateTime date, int numberOfSeats)
        {
            if (TourAlreadyExists(name, date))
                throw new TourAlreadyBookedException();

            if (!HasTourSlotsLeft(date))
                throw new TourAllocationException(SuggestDateFor(date));

            var newTour = new Tour(date.Date, numberOfSeats, name);
            scheduledTours.Add(newTour);
        }

        private DateTime SuggestDateFor(DateTime tried)
        {
            var availableDate = scheduledTours
                .Where(tour => tour.When > tried.Date && HasTourSlotsLeft(tour.When))
                .Select(tour => tour.When)
                .OrderBy(tour => tour);

            return availableDate.Any() 
                ? availableDate.First()
                : scheduledTours.Max(tour => tour.When).AddDays(1);
        }

        public List<Tour> GetToursFor(DateTime date)
        {
            return scheduledTours
                .Where(tour => tour.When.Date == date.Date)
                .ToList();
        }

        private bool HasTourSlotsLeft(DateTime date)
        {
            return scheduledTours.Count(
                tour => tour.When == date.Date) < MaxTours;
        }

        private bool TourAlreadyExists(string name, DateTime date)
        {
            return scheduledTours
                .Where(tour => tour.When == date)
                .Any(tour => tour.Name == name);
        }
    }
}