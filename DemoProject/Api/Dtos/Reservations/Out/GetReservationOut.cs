using DemoProject.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Api.Dtos.Reservations.Out
{
    public class GetReservationOut
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public Duration Duration { get; set; }

        public int RoomNumber { get; set; }
    }
}
