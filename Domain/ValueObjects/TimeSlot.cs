using FluentResults;

namespace Domain.ValueObjects
{
    public record TimeSlot
    {
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        private TimeSlot(TimeSpan startTime, TimeSpan endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public static Result<TimeSlot> Create(TimeSpan startTime, TimeSpan endTime)
        {
            if (startTime >= endTime)
                return Result.Fail(new FluentResults.Error("End time must be greater than start time."));

            return Result.Ok(new TimeSlot(startTime, endTime));
        }

        public bool OverlapsWith(TimeSlot other)
        {
            return StartTime < other.EndTime && EndTime > other.StartTime;
        }
    }

}
