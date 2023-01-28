using System.Collections.Generic;
using VacationRental.Domain.Entities;

namespace VacationRental.Services.Models
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }
    }
}
