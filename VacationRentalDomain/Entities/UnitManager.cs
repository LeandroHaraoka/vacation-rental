using System;
using System.Collections.Generic;
using System.Linq;

namespace VacationRentalDomain.Entities
{
    public class UnitManager
    {
        public int UnitNumber { get; set; }
        public List<DateTime> OccupiedDates { get; set; } = new List<DateTime>();
        public List<BookingViewModel> Bookings { get; set; } = new List<BookingViewModel>();

        public bool TryAddBooking(BookingViewModel booking, int preparationTime)
        {
            var bookingDates = new List<DateTime>();

            for (var i = 0; i < booking.Nights + preparationTime; i++)
            {
                bookingDates.Add(booking.Start.AddDays(i).Date);
            }

            if (OccupiedDates.Intersect(bookingDates).Count() > 0) return false;

            OccupiedDates.AddRange(bookingDates);
            Bookings.Add(booking);

            return true;
        }

        public void UpdateBookingUnits()
        {
            foreach (var booking in Bookings)
            {
                booking.Unit = UnitNumber;
            }
        }
    }
}
