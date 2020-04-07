namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Contracts;
    using ViewModels.Messages;

    [Authorize]
    public class ChatController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IMessagesService messagesService;

        public ChatController(
            IUsersService usersService,
            IMessagesService messagesService)
        {
            this.usersService = usersService;
            this.messagesService = messagesService;
        }

        public async Task<IActionResult> All()
        {
            var userId = this.User.GetId();
            var viewModel = new ChatAllViewModel
            {
                Conversations = await this.messagesService.GetAllConversationsAsync<ChatConversationsViewModel>(userId),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> SendMessage()
        {
            var userId = this.User.GetId();
            var viewModel = new ChatSendMessageViewModel
            {
                Users = await this.usersService.GetAllAsync<ChatUserViewModel>(),
                Conversations = await this.messagesService.GetAllConversationsAsync<ChatConversationsViewModel>(userId),
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
                All = await this.messagesService.GetAllWithUserAsync<ChatMessagesWithUserViewModel>(userId, id),
                Conversations = await this.messagesService.GetAllConversationsAsync<ChatConversationsViewModel>(userId),
            };

            return this.View(viewModel);
        }
    }
}
