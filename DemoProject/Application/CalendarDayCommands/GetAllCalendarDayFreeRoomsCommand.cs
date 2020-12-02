using DemoProject.Domain.Infrastructure;
using DemoProject.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoProject.Application.CalendarDayCommands
{
    public class GetAllCalendarDayFreeRoomsCommand : IRequest<IEnumerable<CalendarDay>>
    {
        public string UserIdentityId { get; set; }
    }

    public class GetAllCalendarDayFreeRoomsCommandHandler : IRequestHandler<GetAllCalendarDayFreeRoomsCommand, IEnumerable<CalendarDay>>
    {
        private readonly DemoProjectDBContext _context;

        public GetAllCalendarDayFreeRoomsCommandHandler(DemoProjectDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<CalendarDay>> Handle(GetAllCalendarDayFreeRoomsCommand request, CancellationToken cancellationToken)
        {
            var room = await _context.Rooms
                .Include(r => r.Reservations)
                .Where(r => r.Owner.UserIdentityId == request.UserIdentityId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (room == null)
            {
                throw new Exception($"Room not found");
            }

            var calendarDays = await _context.CalendarDays
                            .Include(d => d.AMFreeRooms)
                            .Include(d => d.PMFreeRooms)
                            .Include(d => d.Reservations)
                            .ThenInclude(r => r.Room)
                            .OrderBy(d => d.Date)
                            .AsNoTracking()
                            .ToListAsync();

            foreach (var day in calendarDays)
            {
                day.AMFreeRooms = day.AMFreeRooms.Where(p => p.RoomId == room.Id).ToList();
                day.PMFreeRooms = day.PMFreeRooms.Where(p => p.RoomId == room.Id).ToList();
                day.Reservations = day.Reservations.Where(p => p.Room.Id == room.Id).ToList();
            }

            return calendarDays;
        }
    }
}