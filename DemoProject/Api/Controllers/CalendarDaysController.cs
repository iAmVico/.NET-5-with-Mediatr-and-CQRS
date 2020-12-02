using AutoMapper;
using DemoProject.Api.Dtos.CalendarDays.Out;
using DemoProject.Application.CalendarDayCommands;
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
    public class CalendarDaysController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CalendarDaysController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Retrieves a list of calendar days. Per day is specified if a reservation is possible or not.
        /// </summary>
        /// <returns>Returns a list of calendar days.</returns>
        [HttpGet("get-possible-reservations")]
        public async Task<IActionResult> GetAllReservations()
        {
            var command = new GetAllCalendarDayReservationsCommand();

            try
            {
                var calendarDays = await _mediator.Send(command);

                var result = _mapper.Map<IEnumerable<GetCalendarDayReservationOut>>(calendarDays);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Retrieves a list of calendar days. Per day is specified if the current user can free his own room or not.
        /// </summary>
        /// <returns>Returns a list of calendar days.</returns>
        [HttpGet("get-available-free-spots")]
        public async Task<IActionResult> GetAllFreeSpots()
        {
            var command = new GetAllCalendarDayFreeRoomsCommand
            {
                UserIdentityId = User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier")
            };

            try
            {
                var calendarDays = await _mediator.Send(command);

                var result = _mapper.Map<IEnumerable<GetCalendarDayFreeRoomOut>>(calendarDays);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
