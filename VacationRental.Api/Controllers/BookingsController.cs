using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VacationRental.Domain.Entities;
using VacationRental.Services;
using VacationRental.Services.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly IBookingHandler _bookingHandler;

        public BookingsController(
            IDictionary<int, BookingViewModel> bookings,
            IBookingHandler bookingHandler)
        {

            _bookings = bookings;
            _bookingHandler = bookingHandler;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                throw new ApplicationException("Booking not found");

            return _bookings[bookingId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            var key = _bookingHandler.CreateBooking(model);
            return key;
        }
    }
}
