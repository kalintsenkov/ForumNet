namespace ForumNet.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Contracts;

    [ViewComponent(Name = "PostReply")]
    public class PostReplyViewComponent : ViewComponent
    {
        private readonly IRepliesService repliesService;

        public PostReplyViewComponent(IRepliesService repliesService)
        {
            this.repliesService = repliesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string description, int? parentId, int postId)
        {
            await this.repliesService.CreateAsync(description, parentId, postId, this.UserClaimsPrincipal.GetId());

            return this.View();
        }
    }
}