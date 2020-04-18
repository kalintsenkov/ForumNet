namespace ForumNet.Web.Hubs
{
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using Common;
    using Infrastructure.Extensions;
    using Services;
    using Services.Messages;
    using Services.Users;
    using ViewModels.Chat;

    public class ChatHub : Hub
    {
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

        public async Task WhoIsTyping(string name) 
            => await this.Clients.Others.SendAsync("SayWhoIsTyping", name);

        public async Task SendMessage(string message, string receiverId)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var authorId = this.Context.User.GetId();
            var currentTimeAsString = this.dateTimeProvider.Now();
            var user = await this.usersService.GetByIdAsync<ChatUserViewModel>(authorId);

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
                        .ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)
                });
        }
    }
}
