namespace ForumNet.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;

    public class PostsController : AdminController
    {
        private const string PinPostText = "Pin";
        private const string UnpinPostText = "Unpin";

        private readonly IPostsService postsService;

        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        [HttpPost]
        public async Task<IActionResult> Pin(int id)
        {
            var isExisting = await postsService.IsExistingAsync(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var isPinned = await this.postsService.PinAsync(id);
            var pinOrUnpinPostText = isPinned ? UnpinPostText : PinPostText;

            return this.Json(new { pinOrUnpin = pinOrUnpinPostText });
        }
    }
}