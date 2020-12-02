using DemoProject.Bootstrapper.Helpers;
using DemoProject.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoProject.Application.RoomCommands
{

    public class CancelFreeRoomCommand : IRequest<IEnumerable<User>>
    {
        public string UserId { get; set; }

        public DateTime Date { get; set; }
    }

    public class CancelFreeRoomCommandHandler : IRequestHandler<CancelFreeRoomCommand, IEnumerable<User>>
    {
        private readonly DemoProjectDBContext _context;
        private readonly IAzureAuthProvider _azureAuthProvider;
        private readonly IConfiguration _configuration;

        public CancelFreeRoomCommandHandler(DemoProjectDBContext context, IAzureAuthProvider azureAuthProvider, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _azureAuthProvider = azureAuthProvider ?? throw new ArgumentNullException(nameof(azureAuthProvider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<User>> Handle(CancelFreeRoomCommand request, CancellationToken cancellationToken)
        {
            var day = await _context.CalendarDays
                .Include(c => c.AMFreeRooms)
                .Include(c => c.PMFreeRooms)
                .Include(c => c.Reservations)
                .Where(d => d.Date.Date == request.Date.Date)
                .FirstOrDefaultAsync();

            if (day == null)
            {
                throw new Exception("Calendar day not found.");
            }

            var Room = await _context.Rooms.Where(u => u.Owner.UserIdentityId == request.UserId).FirstOrDefaultAsync();

            if (Room == null)
            {
                throw new Exception("Room not found.");
            }

            var reservations = day.Reservations.Where(r => r.Room.Id == Room.Id);

            var userList = new List<User>();

            if (reservations.Count() == 0)
            {
                day.CancelFreeRoom(Room);
            }
            else
            {
                foreach (var reservation in reservations)
                {
                    var employeeGroupId = _configuration["AzureAd:EmployeeGroupId"];

                    GraphServiceClient graphClient = new GraphServiceClient(_azureAuthProvider.GetProvider());

                    var result = await graphClient.Groups[employeeGroupId].Members
                        .Request()
                        .GetAsync();

                    foreach (User user in result.CurrentPage)
                    {
                        if (user.Id == reservation.UserIdentityId)
                        {
                            userList.Add(user);
                        }
                    }

                    while (result.NextPageRequest != null)
                    {
                        result = await result.NextPageRequest.GetAsync();
                        foreach (User user in result.CurrentPage)
                        {
                            if (user.Id == reservation.UserIdentityId)
                            {
                                userList.Add(user);
                            }
                        }
                    }
                }
            }

            return userList;
        }
    }
}
