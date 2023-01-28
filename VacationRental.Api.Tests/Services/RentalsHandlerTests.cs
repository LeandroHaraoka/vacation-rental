using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Services;
using VacationRental.Services.Models;
using Xunit;

namespace VacationRental.Api.Tests.Services
{
    [Collection("Integration")]
    public class RentalsHandlerTests
    {
        private readonly HttpClient _client;

        public RentalsHandlerTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenRental_WhenPreparationTimeIncrease_ThenShouldUpdateRental()
        {
            var rentalId = 1;
            var newPreparationTime = 2;
            var newUnit = 2;
            var putRentalRequest = new RentalBindingModel() { Units = newUnit, PreparationTimeInDays = newPreparationTime };

            var handler = new RentalsHandler(RentalFeed(), BookingFeed());
            var result = handler.Update(rentalId, putRentalRequest);


            Assert.Equal(rentalId, result.Id);
            Assert.Equal(newUnit, result.Units);
            Assert.Equal(newPreparationTime, result.PreparationTimeInDays);
        }

        [Fact]
        public async Task GivenRental_WhenPreparationTimeIncrease_ThenShouldReturnOverlapErrorAndDoNotUpdate()
        {
            var newPreparationTime = 5;
            var newUnit = 2;
            var putRentalRequest = new RentalBindingModel() { Units = newUnit, PreparationTimeInDays = newPreparationTime };
            var rentalToUpdate = RentalFeed()[1];
            var handler = new RentalsHandler(RentalFeed(), BookingFeed());
            Assert.Throws<ApplicationException>(() => handler.Update(rentalToUpdate.Id, putRentalRequest));

            Assert.Equal(newUnit, rentalToUpdate.Units);
            Assert.NotEqual(newPreparationTime, rentalToUpdate.PreparationTimeInDays);
        }

        [Fact]
        public async Task GivenRental_WhenUnitDecrease_ThenShouldUpdateRental()
        {
            var newPreparationTime = 1;
            var newUnit = 1;
            var putRentalRequest = new RentalBindingModel() { Units = newUnit, PreparationTimeInDays = newPreparationTime };
            var rentalToUpdate = RentalFeed()[1];
            var handler = new RentalsHandler(RentalFeed(), BookingFeedForUnitDecreaseWithoutOverlap());
            var result = handler.Update(rentalToUpdate.Id, putRentalRequest);


            Assert.Equal(rentalToUpdate.Id, result.Id);
            Assert.Equal(newUnit, result.Units);
            Assert.Equal(newPreparationTime, result.PreparationTimeInDays);
        }

        [Fact]
        public async Task GivenRental_WhenUnitsDecrease_ThenShouldReturnOverlapErrorAndDoNotUpdate()
        {
            var newPreparationTime = 1;
            var newUnit = 1;
            var putRentalRequest = new RentalBindingModel() { Units = newUnit, PreparationTimeInDays = newPreparationTime };
            var rentalToUpdate = RentalFeed()[1];
            var handler = new RentalsHandler(RentalFeed(), BookingFeed());
            Assert.Throws<ApplicationException>(() => handler.Update(rentalToUpdate.Id, putRentalRequest));

            Assert.NotEqual(newUnit, rentalToUpdate.Units);
            Assert.Equal(newPreparationTime, rentalToUpdate.PreparationTimeInDays);
        }

        private Dictionary<int, RentalViewModel> RentalFeed()
        {
            var postRentalRequest = new RentalViewModel() { Id = 1, Units = 2, PreparationTimeInDays = 1 };
            var rentals = new Dictionary<int, RentalViewModel>();
            rentals.Add(1, postRentalRequest);
            return rentals;
        }

        private Dictionary<int, BookingViewModel> BookingFeed()
        {
            var bookings = new Dictionary<int, BookingViewModel>();
            var postBooking1Request = new BookingViewModel
            {
                RentalId = 1,
                Nights = 2,
                Unit = 1,
                Start = new DateTime(2000, 01, 01)
            };
            var postBooking2Request = new BookingViewModel
            {
                RentalId = 1,
                Nights = 1,
                Unit = 1,
                Start = new DateTime(2000, 01, 05)
            };
            var postBooking3Request = new BookingViewModel
            {
                RentalId = 1,
                Nights = 1,
                Unit = 2,
                Start = new DateTime(2000, 01, 05)
            };
            bookings.Add(1, postBooking1Request);
            bookings.Add(2, postBooking2Request);
            bookings.Add(3, postBooking3Request);

            return bookings;
        }

        private Dictionary<int, BookingViewModel> BookingFeedForUnitDecreaseWithoutOverlap()
        {
            var bookings = new Dictionary<int, BookingViewModel>();
            var postBooking1Request = new BookingViewModel
            {
                RentalId = 1,
                Nights = 2,
                Unit = 1,
                Start = new DateTime(2000, 01, 01)
            };
            var postBooking2Request = new BookingViewModel
            {
                RentalId = 1,
                Nights = 1,
                Unit = 2,
                Start = new DateTime(2000, 01, 05)
            };
            bookings.Add(1, postBooking1Request);
            bookings.Add(2, postBooking2Request);

            return bookings;
        }
    }
}
