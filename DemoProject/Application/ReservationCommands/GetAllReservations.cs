using DemoProject.Domain.Infrastructure;
using DemoProject.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoProject.Application.ReservationCommands
{
    public class GetAllReservations : IRequest<IEnumerable<Reservation>>
    {
        public string UserIdentityId { get; set; }
    }

    public class GetAllReservationsHandler : IRequestHandler<GetAllReservations, IEnumerable<Reservation>>
    {
        private readonly DemoProjectDBContext _context;

        public GetAllReservationsHandler(DemoProjectDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Reservation>> Handle(GetAllReservations request, CancellationToken cancellationToken)
        {
            return await _context.Reservations.Include(r => r.Room).Where(r => r.UserIdentityId == request.UserIdentityId).AsNoTracking().ToListAsync();
        }
    }
}
