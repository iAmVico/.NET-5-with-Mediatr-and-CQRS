using AutoMapper;
using DemoProject.Api.Dtos.CalendarDays.Out;
using DemoProject.Api.Dtos.Reservations.Out;
using DemoProject.Api.Dtos.Rooms.Out;
using DemoProject.Domain.Models.Entities;
using DemoProject.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Api.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Reservation, GetReservationOut>()
                .ForMember(r => r.RoomNumber, ro => ro.MapFrom(r => r.Room.RoomNumber));
            CreateMap<CalendarDay, GetCalendarDayReservationOut>()
                .ForMember(d => d.IsFreeAM, cd => cd.MapFrom(d => d.AMFreeRooms.Count() != 0))
                .ForMember(d => d.IsFreePM, cd => cd.MapFrom(d => d.PMFreeRooms.Count() != 0));
            CreateMap<CalendarDay, GetCalendarDayFreeRoomOut>()
                .ForMember(d => d.CanFreeAM, cd => cd.MapFrom(
                    d => d.AMFreeRooms.Count() == 0 &&
                    d.Reservations.Where(r => r.Duration == Duration.FullDay || r.Duration == Duration.AM).Count() == 0
                    ))
                .ForMember(d => d.CanFreePM, cd => cd.MapFrom(
                    d => d.PMFreeRooms.Count() == 0 &&
                    d.Reservations.Where(r => r.Duration == Duration.FullDay || r.Duration == Duration.PM).Count() == 0
                    ));
            CreateMap<Room, GetRoomOut>()
                .ForMember(ro => ro.RoomNumber, r => r.MapFrom(ro => ro.RoomNumber))
                .ForMember(ro => ro.RoomId, r => r.MapFrom(ro => ro.Id))
                .ForMember(ro => ro.RoomOwner, r => r.MapFrom(ro => ro.Owner.UserIdentityId));
        }
    }
}
