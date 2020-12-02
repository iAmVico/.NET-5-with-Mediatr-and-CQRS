using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Domain.Models.Entities
{
    public class AMReservation
    {
        public int CalendarDayId { get; set; }
        public CalendarDay CalendarDay { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
