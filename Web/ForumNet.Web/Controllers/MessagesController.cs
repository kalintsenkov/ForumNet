namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Contracts;
    using ViewModels.Messages;

    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IMessagesService messagesService;

        public MessagesController(
            IUsersService usersService,
            IMessagesService messagesService)
        {
            this.usersService = usersService;
            this.messagesService = messagesService;
        }

        public async Task<IActionResult> All()
        {
            var userId = this.User.GetId();
            var viewModel = new MessagesAllViewModel
            {
                Conversations = await this.messagesService.GetAllConversationsAsync<MessagesConversationsViewModel>(userId),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var userId = this.User.GetId();
            var viewModel = new MessagesCreateViewModel
            {
                Users = await this.usersService.GetAllAsync<MessagesCreateUserViewModel>(),
                Conversations = await this.messagesService.GetAllConversationsAsync<MessagesConversationsViewModel>(userId),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MessagesCreateInputModel input)
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
            var viewModel = new MessagesWithUserViewModel
            {
                ReceiverId = id,
                AllWithUser = await this.messagesService.GetAllWithUserAsync<MessagesAllWithUserViewModel>(userId, id),
                Conversations = await this.messagesService.GetAllConversationsAsync<MessagesConversationsViewModel>(userId),
            };

            return this.View(viewModel);
        }
    }
}
