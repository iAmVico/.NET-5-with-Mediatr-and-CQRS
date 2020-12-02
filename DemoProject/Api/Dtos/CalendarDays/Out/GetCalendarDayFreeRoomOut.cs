using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Api.Dtos.CalendarDays.Out
{
    public class GetCalendarDayFreeRoomOut
    {
        public DateTime Date { get; set; }

        public bool CanFreeAM { get; set; }

        public bool CanFreePM { get; set; }

        public int RoomId { get; set; }
    }
}
