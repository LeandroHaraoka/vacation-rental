using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VacationRental.Domain.Entities;
using VacationRental.Services;
using VacationRental.Services.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IRentalsHandler _rentalsHandler;

        public RentalsController(IDictionary<int, RentalViewModel> rentals, IRentalsHandler rentalsHandler)
        {
            _rentals = rentals;
            _rentalsHandler = rentalsHandler;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return _rentals[rentalId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            if (model.PreparationTimeInDays < 0)
                throw new ApplicationException("Preparation time should be greater than or equal to zero.");

            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return key;
        }

        [HttpPut("{id}")]
        public RentalViewModel Put([FromRoute] int id, [FromBody] RentalBindingModel model)
        {
            return _rentalsHandler.Update(id, model);
        }
    }
}
