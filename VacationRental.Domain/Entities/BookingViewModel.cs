using System;
using System.Collections.Generic;

namespace VacationRental.Domain.Entities
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public int Unit { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }

        public DateTime GetEndDate() => Start.AddDays(Nights);

        public bool IsDateRangeAvailable(int preparationTime, DateTime startDate, int nights)
        {
            var endDate = startDate.AddDays(Nights + preparationTime).Date;
            var bookingStartDate = Start.Date;
            var bookingEndDate = Start.AddDays(Nights + preparationTime).Date;

            var bookingStartsAfter = bookingStartDate >= endDate;
            var bookingEndsBefore = bookingEndDate <= startDate;

            return bookingStartsAfter || bookingEndsBefore;
        }
    }
}
