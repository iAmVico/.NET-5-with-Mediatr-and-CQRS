using DemoProject.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Domain.Models.Entities
{
    public class Reservation
    {
        public int Id { get; private set; }
        public DateTime Date { get; private set; }
        public string UserIdentityId { get; private set; }
        public Duration Duration { get; private set; }
        public Room Room { get; private set; }

        // EF constructor
        private Reservation() { }

        public Reservation(string userIdentityId, Duration duration, DateTime date)
        {
            UserIdentityId = userIdentityId;
            Duration = duration;
            Date = date;
        }

        public void SetRoom(Room room)
        {
            Room = room;
        }
    }
}
