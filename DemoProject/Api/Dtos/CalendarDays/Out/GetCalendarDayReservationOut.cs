using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Api.Dtos.CalendarDays.Out
{
    public class GetCalendarDayReservationOut
    {
        public DateTime Date { get; set; }

        public bool IsFreeAM { get; set; }

        public bool IsFreePM { get; set; }
    }
}
