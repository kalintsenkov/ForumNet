namespace ForumNet.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Contracts;
    using ViewModels.Chat;

    [ViewComponent(Name = "ChatConversations")]
    public class ChatConversationsViewComponent : ViewComponent
    {
        private readonly IChatService chatService;

        public ChatConversationsViewComponent(IChatService chatService)
        {
            this.chatService = chatService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = this.UserClaimsPrincipal.GetId();
            var chats = await this.chatService.GetAllAsync<ChatUserViewModel>(userId);

            return this.View(chats);
        }
    }
}
