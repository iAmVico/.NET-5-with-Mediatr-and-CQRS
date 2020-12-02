using DemoProject.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Api.Dtos.Reservations.In
{
    public class CreateReservationIn
    {
        public DateTime Date { get; set; }

        public Duration Duration { get; set; }
    }
}
