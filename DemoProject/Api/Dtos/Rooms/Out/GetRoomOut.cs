using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Api.Dtos.Rooms.Out
{
    public class GetRoomOut
    {
        public int RoomNumber { get; set; }
        public int RoomId { get; set; }
        public string RoomOwner { get; set; }
    }
}
