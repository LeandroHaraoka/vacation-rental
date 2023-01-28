using System;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using Xunit;

namespace VacationRental.Api.Tests.Domain
{
    public class BookingModelTests
    {
        public BookingModelTests()
        {
        }

        [InlineData(1, 5, 2023,01,12)]
        [InlineData(1, 8, 2023,01,01)]
        [Theory]
        public async Task GivenDateRange_ThenShouldReturnAvailable(int preparationTime, int nights, int year, int month, int day)
        {
            var startDate = new DateTime(year, month, day);
            var booking = new BookingViewModel()
            {
                Nights = 1,
                Start = new DateTime(2023,01,10)
            };

            Assert.True(booking.IsDateRangeAvailable(preparationTime, startDate, nights));
        }

        [InlineData(5, 1, 2023, 01, 12)]
        [InlineData(5, 1, 2023, 01, 07)]
        [InlineData(0, 1, 2023, 01, 10)]
        [Theory]
        public async Task GivenDateRange_ThenShouldReturnNotAvailable(int preparationTime, int nights, int year, int month, int day)
        {
            var startDate = new DateTime(year, month, day);
            var booking = new BookingViewModel()
            {
                Nights = 1,
                Start = new DateTime(2023, 01, 10)
            };

            Assert.False(booking.IsDateRangeAvailable(preparationTime, startDate, nights));
        }
    }
}
