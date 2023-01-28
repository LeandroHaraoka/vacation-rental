using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.Services;
using VacationRental.Services.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarHandler _calendarHandler;

        public CalendarController(
            ICalendarHandler calendarHandler)
        {
            _calendarHandler = calendarHandler;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            var calendar = _calendarHandler.BuildCalendar(rentalId, start, nights);

            return calendar;
        }
    }
}
