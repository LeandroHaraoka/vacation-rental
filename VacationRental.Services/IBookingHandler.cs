using VacationRental.Domain.Entities;
using VacationRental.Services.Models;

namespace VacationRental.Services
{
    public interface IBookingHandler
    {
        ResourceIdViewModel CreateBooking(BookingBindingModel model);
    }
}
