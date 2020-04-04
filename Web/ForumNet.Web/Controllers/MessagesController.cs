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

        public async Task<IActionResult> Create()
        {
            var userId = this.User.GetId();
            var viewModel = new MessagesCreateViewModel
            {
                Users = await this.usersService.GetAllAsync<MessagesCreateUserViewModel>(),
                All = await this.messagesService.GetAllConversationsAsync<MessagesAllViewModel>(userId),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Send(MessagesSendInputModel input)
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
                All = await this.messagesService.GetAllConversationsAsync<MessagesAllViewModel>(userId),
                AllWithUser = await this.messagesService.GetAllWithUserAsync<MessagesDetailsAllViewModel>(userId, id)
            };

            return this.View(viewModel);
        }
    }
}
