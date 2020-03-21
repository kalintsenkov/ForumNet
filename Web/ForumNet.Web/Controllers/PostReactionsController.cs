namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Data.Models.Enums;
    using Infrastructure.Extensions;
    using Services.Contracts;
    using ViewModels.Posts;

    [ApiController]
    [Route("api/[controller]")]
    public class PostReactionsController : ControllerBase
    {
        private readonly IPostsService postsService;
        private readonly IPostReactionsService postReactionsService;

        public PostReactionsController(
            IPostsService postsService,
            IPostReactionsService postReactionsService)
        {
            this.postsService = postsService;
            this.postReactionsService = postReactionsService;
        }

        [HttpPost]
        [Authorize]
        [Route("like/{postId}")]
        public async Task<IActionResult> LikePost(int postId)
        {
            var isExisting = await this.postsService.IsExisting(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.postReactionsService.ReactAsync(ReactionType.Like, postId, userId);
            var (likes, loves, haha, wow, sad, angry) = await this.postReactionsService.GetCountByPostIdAsync(postId);
            var viewModel = new PostsReactionsCountViewModel
            {
                Likes = likes,
                Loves = loves,
                Wow = wow,
                Haha = haha,
                Sad = sad,
                Angry = angry
            };

            return this.Ok(viewModel);
        }
    }
}