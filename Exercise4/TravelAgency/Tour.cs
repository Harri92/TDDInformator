using System;

namespace TravelAgency
{
    public class Tour
    {
        public Tour(DateTime safariDate, int numberOfSeats, string name)
        {
            this.When = safariDate;
            this.NumberOfSeats = numberOfSeats;
            this.Name = name;
        }

        public DateTime When { get; set; }
        public int NumberOfSeats { get; set; }
        public string Name { get; set; }
    }
}