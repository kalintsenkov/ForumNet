namespace ForumNet.Services.Providers
{
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now() => DateTime.UtcNow;
    }
}