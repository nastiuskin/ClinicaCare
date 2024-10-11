using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Appointments
{
    public class TimeSlot
    {
        public DateTime startTime {  get; set; }
        public DateTime endTime { get; set; }

        public TimeSlot(DateTime startTime, DateTime endTime)
        {
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public bool OverlapsWith(TimeSlot other)
        {
            return startTime < other.endTime && endTime > other.startTime; // to verify logic of intersection
        }
    }
   
}
