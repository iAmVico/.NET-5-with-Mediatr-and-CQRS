using DemoProject.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoProject.Application.ReservationCommands
{
    public class DeleteReservationCommand : IRequest
    {
        public int ReservationId { get; set; }
    }

    public class DeleteReservationHandler : IRequestHandler<DeleteReservationCommand>
    {
        private readonly DemoProjectDBContext _context;

        public DeleteReservationHandler(DemoProjectDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Unit> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations.Include(r => r.Room).Where(r => r.Id == request.ReservationId).FirstOrDefaultAsync();

            if (reservation == null)
            {
                throw new Exception("Reservation doesn't exist.");
            }

            var room = reservation.Room;

            if (room == null)
            {
                throw new Exception($"Room not found.");
            }

            var calendarDay = await _context.CalendarDays.Where(d => d.Date == reservation.Date.Date)
                .Include(d => d.AMFreeRooms)
                .ThenInclude(amr => amr.Room)
                .Include(d => d.PMFreeRooms)
                .ThenInclude(pmr => pmr.Room)
                .FirstOrDefaultAsync();

            if (calendarDay == null)
            {
                throw new Exception($"Calendar day not found.");
            }

            calendarDay.FreeRoom(room, reservation.Duration);

            _context.Remove(reservation);

            return Unit.Value;
        }
    }
}
