namespace ForumNet.Services
{
    using System;

    using Contracts;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now() => DateTime.UtcNow;
    }
}