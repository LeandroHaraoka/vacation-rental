using System;
using System.Collections.Generic;
using VacationRental.Domain.Entities;
using VacationRental.Services.Models;

namespace VacationRental.Services
{
    public class CalendarHandler : ICalendarHandler
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public CalendarHandler(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public CalendarViewModel BuildCalendar(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var rental = _rentals[rentalId];
            var preparationTimeInDays = rental.PreparationTimeInDays;

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationTimeViewModel>()
                };

                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId != rentalId) continue;

                    if (booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit });
                        continue;
                    }

                    if (preparationTimeInDays == 0) continue;

                    for (var j = 0; j < preparationTimeInDays; j++)
                    {
                        if (booking.Start.AddDays(booking.Nights + j) == date.Date)
                        {
                            date.PreparationTimes.Add(new PreparationTimeViewModel { Unit = booking.Unit });
                            continue;
                        }
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
