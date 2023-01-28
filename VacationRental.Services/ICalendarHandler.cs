using System;
using VacationRental.Services.Models;

namespace VacationRental.Services
{
    public interface ICalendarHandler
    {
        CalendarViewModel BuildCalendar(int rentalId, DateTime start, int nights);
    }
}
