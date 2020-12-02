using AutoMapper;
using DemoProject.Api.Dtos.Reservations.In;
using DemoProject.Api.Dtos.Reservations.Out;
using DemoProject.Application.ReservationCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DemoProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ReservationsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        /// <summary>
        /// Retrieves all user's reservations.
        /// </summary>
        /// <returns>Returns all reservations for the current user.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var command = new GetAllReservations
            {
                UserIdentityId = User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier")
            };

            try
            {
                var response = await _mediator.Send(command);

                var result = _mapper.Map<IEnumerable<GetReservationOut>>(response);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Creates a new reservation on a specified date, for a specified duration, for the current user.
        /// </summary>
        /// <param name="dto">An object that containts a date and a duration (enum).</param>
        /// <returns>Returns the reservation id.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationIn dto)
        {
            var command = new CreateReservationCommand
            {
                ReservationDate = dto.Date,
                Duration = dto.Duration,
                UserIdentityId = User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier")
            };

            try
            {
                var reservation = await _mediator.Send(command);

                return Ok(reservation.Id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Cancels a reservation for the current user.
        /// </summary>
        /// <param name="id">Reservation id.</param>
        /// <returns>Returns status code 200 if succeeded.</returns>
        [HttpDelete]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var command = new DeleteReservationCommand { ReservationId = id };

            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}