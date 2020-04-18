namespace ForumNet.Services
{
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now() => DateTime.UtcNow;
    }
}