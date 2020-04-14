namespace ForumNet.Web.Components
{
    using Microsoft.AspNetCore.Mvc;

    using InputModels.Replies;

    [ViewComponent(Name = "CreateReply")]
    public class CreateReplyViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int? parentId, int postId)
        {
            var viewModel = new RepliesCreateInputModel
            {
                ParentId = parentId,
                PostId = postId
            };

            return this.View(viewModel);
        }
    }
}