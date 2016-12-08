using System;
using System.Collections.Generic;
using TravelAgency;

namespace TravelAgencyTests
{
    public class TourScheduleStub : ITourSchedule
    {
        public List<Tour> Tours;
        public List<DateTime> CallsToGetToursFor;

        public void CreateTour(string name, DateTime date, int numberOfSeats)
        {
            throw new NotImplementedException();
        }

        public List<Tour> GetToursFor(DateTime date)
        {
            if (CallsToGetToursFor == null)
                CallsToGetToursFor = new List<DateTime>();

            CallsToGetToursFor.Add(date);

            return Tours;
        }
    }
}