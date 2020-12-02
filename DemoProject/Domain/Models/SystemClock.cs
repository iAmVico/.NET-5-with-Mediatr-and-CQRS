using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Domain.Models
{
    public static class SystemClock
    {
        public static Func<DateTime> GetUtcNow = () => DateTime.UtcNow;

        public static void Reset()
        {
            GetUtcNow = () => DateTime.UtcNow;
        }
    }
}
