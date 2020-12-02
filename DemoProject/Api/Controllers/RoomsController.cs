using DemoProject.Api.Dtos.Rooms.In;
using DemoProject.Application.RoomCommands;
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
    public class RoomsController : Controller
    {
        private readonly IMediator _mediator;

        public RoomsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Retrieves the room with the specified id.
        /// </summary>
        /// <param name="id">Room id.</param>
        /// <returns>Return a room object.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var command = new GetRoomCommand
            {
                RoomId = id
            };

            try
            {
                var room = await _mediator.Send(command);

                if (room == null)
                {
                    return NotFound($"Room not found.");
                }

                return Ok(room);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Marks a room free on a specified date, for a specified duration, for the current user.
        /// </summary>
        /// <param name="dto">An object that containts a date and a duration (enum).</param>
        /// <returns>Returns status code 200 if succeeded.</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Free([FromBody] FreeRoomIn dto)
        {
            var command = new FreeRoomCommand
            {
                UserIdentityId = User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier"),
                DateTime = dto.Date,
                Duration = dto.Duration
            };

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

        /// <summary>
        /// Cancel a user's free spot marking.
        /// </summary>
        /// <param name="dto">An object that contains a room id and a date.</param>
        /// <returns>Returns empty or a list if other user's reservation and contant ifno.</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Cancel([FromBody] CancelFreeRoomIn dto)
        {
            var command = new CancelFreeRoomCommand()
            {
                UserId = User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier"),
                Date = dto.Date
            };

            try
            {
                var result = await _mediator.Send(command);

                if (result.Count() != 0)
                {
                    return Ok(result.Select(u => new { u.DisplayName, u.MobilePhone, u.BusinessPhones, u.Mail }));
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
