using DemoProject.Domain.Infrastructure;
using DemoProject.Domain.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoProject.Application.RoomCommands
{
    public class FreeRoomCommand : IRequest
    {
        public string UserIdentityId { get; set; }

        public DateTime DateTime { get; set; }

        public Duration Duration { get; set; }
    }

    public class FreeRoomCommandHandler : IRequestHandler<FreeRoomCommand>
    {
        private readonly DemoProjectDBContext _context;

        public FreeRoomCommandHandler(DemoProjectDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Unit> Handle(FreeRoomCommand request, CancellationToken cancellationToken)
        {
            var Room = await _context.Rooms.Where(p => p.Owner.UserIdentityId == request.UserIdentityId).FirstOrDefaultAsync();

            if (Room == null)
            {
                throw new Exception($"Room not found.");
            }

            var calendarDay = await _context.CalendarDays.Where(d => d.Date == request.DateTime.Date)
                .Include(d => d.AMFreeRooms)
                .ThenInclude(d => d.Room)
                .Include(d => d.PMFreeRooms)
                .ThenInclude(d => d.Room)
                .FirstOrDefaultAsync();

            if (calendarDay == null)
            {
                throw new Exception($"Calendar day not found.");
            }

            calendarDay.FreeRoom(Room, request.Duration);

            return Unit.Value;
        }
    }
}
