using DemoProject.Domain.Infrastructure;
using DemoProject.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoProject.Application.RoomCommands
{
    public class GetRoomCommand : IRequest<Room>
    {
        public int RoomId { get; set; }
    }

    public class GetRoomHandler : IRequestHandler<GetRoomCommand, Room>
    {
        private readonly DemoProjectDBContext _context;

        public GetRoomHandler(DemoProjectDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Room> Handle(GetRoomCommand request, CancellationToken cancellationToken)
        {
            return await _context.Rooms.Where(p => p.Id == request.RoomId).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
