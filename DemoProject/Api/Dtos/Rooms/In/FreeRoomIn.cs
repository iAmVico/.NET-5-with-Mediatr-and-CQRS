using DemoProject.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Api.Dtos.Rooms.In
{
    public class FreeRoomIn
    {
        public DateTime Date { get; set; }

        public Duration Duration { get; set; }
    }
}
