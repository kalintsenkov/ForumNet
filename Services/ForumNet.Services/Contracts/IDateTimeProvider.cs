namespace ForumNet.Services.Contracts
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime Now();
    }
}