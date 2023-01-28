using System;
using System.Collections.Generic;
using VacationRental.Domain.Entities;
using VacationRental.Services.Models;

namespace VacationRental.Services
{
    public class BookingHandler : IBookingHandler
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingHandler(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public ResourceIdViewModel CreateBooking(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");
            if (!_rentals.ContainsKey(model.RentalId))
                throw new ApplicationException("Rental not found");

            var rental = _rentals[model.RentalId];

            if (model.Unit <= 0 || model.Unit > rental.Units)
                throw new ApplicationException("Unit must be positive and less than or equal to rental units");

            for (var i = 0; i < model.Nights; i++)
            {
                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId == model.RentalId
                        && booking.Unit == model.Unit
                        && booking.IsDateRangeAvailable(rental.PreparationTimeInDays, model.Start.Date, model.Nights) is false)
                    {
                        throw new ApplicationException("Not available");
                    }
                }
            }

            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Unit = model.Unit,
                Start = model.Start.Date
            });

            return key;
        }
    }
}
