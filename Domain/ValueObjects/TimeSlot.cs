using FluentResults;

namespace Domain.ValueObjects
{
    public record TimeSlot
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        private TimeSlot(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public static Result<TimeSlot> Create(DateTime startTime, DateTime endTime)
        {
            if (startTime >= endTime)
            {
                return Result.Fail(new FluentResults.Error("End time must be greater than start time."));
            }

            return Result.Ok(new TimeSlot(startTime, endTime));
        }

        public bool OverlapsWith(TimeSlot other)
        {
            return StartTime < other.EndTime && EndTime > other.StartTime;
        }
    }

}
