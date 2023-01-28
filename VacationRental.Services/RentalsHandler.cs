using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Services.Models;

namespace VacationRental.Services
{
    public class RentalsHandler : IRentalsHandler
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public RentalsHandler(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public RentalViewModel Update(int id, RentalBindingModel model)
        {
            if (model.PreparationTimeInDays < 0)
                throw new ApplicationException("Preparation time should be greater than or equal to zero.");

            if (!_rentals.ContainsKey(id))
                throw new ApplicationException("Rental not found");

            var rental = _rentals[id];

            if (model.Units < rental.Units || model.PreparationTimeInDays > rental.PreparationTimeInDays)
            {
                RelocateBooking(model.Units, model.PreparationTimeInDays);
            }

            rental.Units = model.Units;
            rental.PreparationTimeInDays = model.PreparationTimeInDays;

            return rental;
        }

        private void RelocateBooking(int newUnits, int newPreparationTime)
        {
            var unitManagers = new List<UnitManager>();
            for (var i = 1; i <= newUnits; i++)
            {
                unitManagers.Add(new UnitManager { UnitNumber = i });
            }

            foreach (var booking in _bookings.Values.OrderBy(x => x.Start))
            {
                var bookingRelocated = TryRelocateBooking(unitManagers, booking, newPreparationTime);
                if (bookingRelocated is false)
                {
                    throw new Exception("Overlapping between existing bookings and/or preparation times occured.");
                }
            }

            Parallel.ForEach(unitManagers, x => x.UpdateBookingUnits());
        }

        private bool TryRelocateBooking(List<UnitManager> unitManagers, BookingViewModel booking, int newPreparationTime)
        {
            foreach (var unitManager in unitManagers)
            {
                if (unitManager.TryAddBooking(booking, newPreparationTime))
                    return true;
            }

            return false;
        }
    }
}
