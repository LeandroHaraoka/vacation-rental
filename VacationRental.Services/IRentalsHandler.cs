using VacationRental.Domain.Entities;
using VacationRental.Services.Models;

namespace VacationRental.Services
{
    public interface IRentalsHandler
    {
        RentalViewModel Update(int id, RentalBindingModel model);
    }
}