namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Contracts;
    using ViewModels.Chat;

    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatService chatService;
        private readonly IUsersService usersService;
        private readonly IMessagesService messagesService;

        public ChatController(
            IChatService chatService,
            IUsersService usersService,
            IMessagesService messagesService)
        {
            this.chatService = chatService;
            this.usersService = usersService;
            this.messagesService = messagesService;
        }

        public async Task<IActionResult> All()
        {
            var userId = this.User.GetId();
            var viewModel = new ChatAllViewModel
            {
                Chats = await this.chatService.GetAllAsync<ChatUserViewModel>(userId),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> SendMessage()
        {
            var userId = this.User.GetId();
            var viewModel = new ChatSendMessageViewModel
            {
                Users = await this.usersService.GetAllAsync<ChatUserViewModel>(),
                Chats = await this.chatService.GetAllAsync<ChatUserViewModel>(userId),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ChatSendMessageInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.RedirectToAction(nameof(WithUser), new { id = input.ReceiverId });
            }

            await this.messagesService.CreateAsync(input.Content, this.User.GetId(), input.ReceiverId);

            return this.RedirectToAction(nameof(WithUser), new { id = input.ReceiverId });
        }

        public async Task<IActionResult> WithUser(string id)
        {
            var userId = this.User.GetId();
            var viewModel = new ChatWithUserViewModel
            {
                ReceiverId = id,
                Chats = await this.chatService.GetAllAsync<ChatUserViewModel>(userId),
                Messages = await this.messagesService.GetAllWithUserAsync<ChatMessagesWithUserViewModel>(userId, id),
            };

            return this.View(viewModel);
        }
    }
}
