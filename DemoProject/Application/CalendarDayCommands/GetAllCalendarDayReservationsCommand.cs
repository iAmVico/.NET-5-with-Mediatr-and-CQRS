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
    public class GetAllCalendarDayReservationsCommand : IRequest<IEnumerable<CalendarDay>>
    {
    }

    public class GetAllCalendarDaysCommandHandler : IRequestHandler<GetAllCalendarDayReservationsCommand, IEnumerable<CalendarDay>>
    {
        private readonly DemoProjectDBContext _context;

        public GetAllCalendarDaysCommandHandler(DemoProjectDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<CalendarDay>> Handle(GetAllCalendarDayReservationsCommand request, CancellationToken cancellationToken)
        {
            return await _context.CalendarDays
                            .Include(d => d.AMFreeRooms)
                            .Include(d => d.PMFreeRooms)
                            .OrderBy(d => d.Date)
                            .AsNoTracking()
                            .ToListAsync();
        }
    }
}
