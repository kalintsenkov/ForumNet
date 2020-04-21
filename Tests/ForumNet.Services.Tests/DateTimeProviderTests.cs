namespace ForumNet.Services.Tests
{
    using System;

    using FluentAssertions;
    using Xunit;

    using Providers.DateTime;

    public class DateTimeProviderTests
    {
        [Fact]
        public void NowMethodShouldReturnCurrentTime()
        {
            var dateTimeProvider = new DateTimeProvider();

            var time = dateTimeProvider.Now();

            time.Should().BeSameDateAs(DateTime.UtcNow);
        }
    }
}