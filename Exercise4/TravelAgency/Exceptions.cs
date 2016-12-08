using System;

namespace TravelAgency
{
    public class TourAllocationException : Exception
    {
        public TourAllocationException(DateTime nextAvailableTour)
        {
            NextAvailableTour = nextAvailableTour;
        }

        public DateTime NextAvailableTour { get; set; }
    }

    public class NoSuchTourException : Exception
    {
    }

    public class TourAlreadyBookedException : Exception
    {
    }

    public class NotEnoughSeatsException : Exception
    {
    }

    public class NoSuchBookingException : Exception
    {
    }
}