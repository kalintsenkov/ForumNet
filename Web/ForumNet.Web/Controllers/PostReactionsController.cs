namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Data.Models.Enums;
    using Infrastructure.Extensions;
    using Services.Posts;
    using Services.Reactions;

    [Authorize]
    [ApiController]
    [Route("api/post-reactions")]
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

        [HttpPost("like/{postId}")]
        public async Task<ActionResult> Like(int postId)
        {
            var isExisting = await this.postsService.IsExistingAsync(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReactionsService.ReactAsync(ReactionType.Like, postId, this.User.GetId());
            var result = await this.postReactionsService.GetCountByPostIdAsync(postId);

            return this.Ok(result);
        }

        [HttpPost("love/{postId}")]
        public async Task<ActionResult> Love(int postId)
        {
            var isExisting = await this.postsService.IsExistingAsync(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReactionsService.ReactAsync(ReactionType.Love, postId, this.User.GetId());
            var result = await this.postReactionsService.GetCountByPostIdAsync(postId);

            return this.Ok(result);
        }

        [HttpPost("haha/{postId}")]
        public async Task<ActionResult> Haha(int postId)
        {
            var isExisting = await this.postsService.IsExistingAsync(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReactionsService.ReactAsync(ReactionType.Haha, postId, this.User.GetId());
            var result = await this.postReactionsService.GetCountByPostIdAsync(postId);

            return this.Ok(result);
        }

        [HttpPost("wow/{postId}")]
        public async Task<ActionResult> Wow(int postId)
        {
            var isExisting = await this.postsService.IsExistingAsync(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReactionsService.ReactAsync(ReactionType.Wow, postId, this.User.GetId());
            var result = await this.postReactionsService.GetCountByPostIdAsync(postId);

            return this.Ok(result);
        }

        [HttpPost("sad/{postId}")]
        public async Task<ActionResult> Sad(int postId)
        {
            var isExisting = await this.postsService.IsExistingAsync(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReactionsService.ReactAsync(ReactionType.Sad, postId, this.User.GetId());
            var result = await this.postReactionsService.GetCountByPostIdAsync(postId);

            return this.Ok(result);
        }

        [HttpPost("angry/{postId}")]
        public async Task<ActionResult> Angry(int postId)
        {
            var isExisting = await this.postsService.IsExistingAsync(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReactionsService.ReactAsync(ReactionType.Angry, postId, this.User.GetId());
            var result = await this.postReactionsService.GetCountByPostIdAsync(postId);

            return this.Ok(result);
        }
    }
}