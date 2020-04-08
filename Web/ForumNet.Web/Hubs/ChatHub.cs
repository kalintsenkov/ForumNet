namespace ForumNet.Web.Hubs
{
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using Infrastructure.Extensions;
    using Services.Contracts;
    using ViewModels.Messages;

    public class ChatHub : Hub
    {
        private const string DateTimeDefaultFormat = "dd MMM yyyy HH:mm";

        private readonly IUsersService usersService;
        private readonly IMessagesService messagesService;
        private readonly IDateTimeProvider dateTimeProvider;

        public ChatHub(
            IUsersService usersService,
            IMessagesService messagesService,
            IDateTimeProvider dateTimeProvider)
        {
            this.usersService = usersService;
            this.messagesService = messagesService;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task SendMessage(string message, string receiverId)
        {
            var authorId = this.Context.User.GetId();
            var currentTimeAsString = this.dateTimeProvider.Now();
            var user = await this.usersService.GetByIdAsync<ChatUserViewModel>(authorId);

            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            await this.messagesService.CreateAsync(message, authorId, receiverId);
            await this.Clients.All.SendAsync(
                "ReceiveMessage",
                new ChatMessagesWithUserViewModel
                {
                    AuthorId = authorId,
                    AuthorUserName = user.UserName,
                    AuthorProfilePicture = user.ProfilePicture,
                    Content = message,
                    CreatedOn = currentTimeAsString
                        .ToString(DateTimeDefaultFormat, CultureInfo.InvariantCulture)
                });
        }
    }
}
