using DemoProject.Domain.Infrastructure;
using DemoProject.Domain.Models.Entities;
using DemoProject.Domain.Models.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoProject.Application.ReservationCommands
{
    public class CreateReservationCommand : IRequest<Reservation>
    {
        public DateTime ReservationDate { get; set; }

        public string UserIdentityId { get; set; }

        public Duration Duration { get; set; }
    }

    public class CreateNewReservationValidator : AbstractValidator<CreateReservationCommand>
    {
        public CreateNewReservationValidator()
        {
            RuleFor(r => r.UserIdentityId).NotEmpty().NotNull();
        }
    }

    public class CreateNewReservationHandler : IRequestHandler<CreateReservationCommand, Reservation>
    {
        private readonly DemoProjectDBContext _context;

        public CreateNewReservationHandler(DemoProjectDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Reservation> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var calendarDay = await _context.CalendarDays
                    .Where(d => d.Date.Date == request.ReservationDate.Date)
                    .Include(d => d.AMFreeRooms)
                    .ThenInclude(amr => amr.Room)
                    .Include(d => d.PMFreeRooms)
                    .ThenInclude(pmr => pmr.Room)
                    .FirstOrDefaultAsync();

            if (calendarDay == null)
            {
                throw new Exception($"Calendar day not found.");
            }

            var reservation = new Reservation(request.UserIdentityId, request.Duration, request.ReservationDate);

            calendarDay.AddReservation(reservation);

            return reservation;
        }
    }
}
