using DemoProject.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Domain.Models.Entities
{
    public class CalendarDay
    {
        public int Id { get; private set; }

        public DateTime Date { get; private set; }

        public IList<AMReservation> AMFreeRooms { get; set; } = new List<AMReservation>();

        public IList<PMReservation> PMFreeRooms { get; set; } = new List<PMReservation>();

        public IList<Reservation> Reservations { get; set; } = new List<Reservation>();

        // EF consutrctor
        public CalendarDay() { }

        public CalendarDay(DateTime date)
        {
            Date = date;
        }


        public void FreeRoom(Room room, Duration duration)
        {
            switch (duration)
            {
                case Duration.FullDay:
                    if (!AMFreeRooms.Any(p => p.RoomId == room.Id) && !PMFreeRooms.Any(p => p.RoomId == room.Id))
                    {
                        AMFreeRooms.Add(new AMReservation
                        {
                            CalendarDay = this,
                            Room = room
                        });
                        PMFreeRooms.Add(new PMReservation
                        {
                            CalendarDay = this,
                            Room = room
                        });
                    }
                    break;
                case Duration.AM:
                    if (!AMFreeRooms.Any(p => p.RoomId == room.Id))
                    {
                        AMFreeRooms.Add(new AMReservation
                        {
                            CalendarDay = this,
                            Room = room
                        });
                    }
                    break;
                case Duration.PM:
                    if (!PMFreeRooms.Any(p => p.RoomId == room.Id))
                    {
                        PMFreeRooms.Add(new PMReservation
                        {
                            CalendarDay = this,
                            Room = room
                        });
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public void CancelFreeRoom(Room room)
        {
            var AMFreeRoom = AMFreeRooms.Where(p => p.RoomId == room.Id).FirstOrDefault();

            if (AMFreeRoom != null)
            {
                AMFreeRooms.Remove(AMFreeRoom);
            }

            var PMFreeRoom = PMFreeRooms.Where(p => p.RoomId == room.Id).FirstOrDefault();

            if (PMFreeRoom != null)
            {
                PMFreeRooms.Remove(PMFreeRoom);
            }
        }

        public void CancelReservation(Room room)
        {
            var AMFreeRoom = AMFreeRooms.Where(p => p.RoomId == room.Id).FirstOrDefault();

            if (AMFreeRoom != null)
            {
                AMFreeRooms.Remove(AMFreeRoom);
            }

            var PMFreeRoom = PMFreeRooms.Where(p => p.RoomId == room.Id).FirstOrDefault();

            if (PMFreeRoom != null)
            {
                PMFreeRooms.Remove(PMFreeRoom);
            }
        }

        public void AddReservation(Reservation reservation)
        {
            if (reservation.Duration == Duration.FullDay)
            {
                var Rooms = AMFreeRooms.Where(am => PMFreeRooms.Any(pm => am.RoomId == pm.RoomId));

                if (Rooms.Count() != 0)
                {
                    var Room = Rooms.First().Room;

                    var RoomAM = AMFreeRooms.Remove(AMFreeRooms.Where(p => p.RoomId == Room.Id).First());
                    var RoomPM = PMFreeRooms.Remove(PMFreeRooms.Where(p => p.RoomId == Room.Id).First());

                    reservation.SetRoom(Room);
                    Reservations.Add(reservation);
                }
            }

            if (reservation.Duration == Duration.AM)
            {
                if (AMFreeRooms.Count() != 0)
                {
                    var Room = AMFreeRooms.First().Room;

                    AMFreeRooms.Remove(AMFreeRooms.Where(p => p.RoomId == Room.Id).First());

                    reservation.SetRoom(Room);
                    Reservations.Add(reservation);
                }
            }

            if (reservation.Duration == Duration.PM)
            {
                if (PMFreeRooms.Count() != 0)
                {
                    var Room = PMFreeRooms.First().Room;

                    PMFreeRooms.Remove(PMFreeRooms.Where(p => p.RoomId == Room.Id).First());

                    reservation.SetRoom(Room);
                    Reservations.Add(reservation);
                }
            }
        }
    }
}
