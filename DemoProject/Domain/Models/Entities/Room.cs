using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Domain.Models.Entities
{
    public class Room
    {
        public int Id { get; private set; }

        public int RoomNumber { get; private set; }

        public Owner Owner { get; private set; } = new Owner();

        public IEnumerable<Reservation> Reservations { get; set; }

        public IList<AMReservation> AMFreeCalendarDays { get; private set; } = new List<AMReservation>();

        public IList<PMReservation> PMFreeCalendarDays { get; private set; } = new List<PMReservation>();

        // EF constructor
        private Room() { }

        public Room(int roomNumber, Owner owner = null)
        {
            RoomNumber = roomNumber;
            Owner = owner;
        }

        public void ChangeOwner(string userIdentityId)
        {
            Owner.UserIdentityId = userIdentityId;
        }

        public void RemoveOwner()
        {
            Owner.UserIdentityId = "";
        }
    }
}
