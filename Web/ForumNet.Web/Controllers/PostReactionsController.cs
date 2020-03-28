namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Data.Models.Enums;
    using Infrastructure.Extensions;
    using Services.Contracts;
    using ViewModels.Reactions;

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

        [HttpPost]
        [Route("like/{postId}")]
        public async Task<ActionResult<ReactionsCountViewModel>> Like(int postId)
        {
            var isExisting = await this.postsService.IsExisting(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReactionsService.ReactAsync(ReactionType.Like, postId, this.User.GetId());
            var viewModel = await this.GetPostReactionsCountByIdAsync(postId);

            return viewModel;
        }

        [HttpPost]
        [Route("love/{postId}")]
        public async Task<ActionResult<ReactionsCountViewModel>> Love(int postId)
        {
            var isExisting = await this.postsService.IsExisting(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReactionsService.ReactAsync(ReactionType.Love, postId, this.User.GetId());
            var viewModel = await this.GetPostReactionsCountByIdAsync(postId);

            return viewModel;
        }

        [HttpPost]
        [Route("haha/{postId}")]
        public async Task<ActionResult<ReactionsCountViewModel>> Haha(int postId)
        {
            var isExisting = await this.postsService.IsExisting(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReactionsService.ReactAsync(ReactionType.Haha, postId, this.User.GetId());
            var viewModel = await this.GetPostReactionsCountByIdAsync(postId);

            return viewModel;
        }

        [HttpPost]
        [Route("wow/{postId}")]
        public async Task<ActionResult<ReactionsCountViewModel>> Wow(int postId)
        {
            var isExisting = await this.postsService.IsExisting(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReactionsService.ReactAsync(ReactionType.Wow, postId, this.User.GetId());
            var viewModel = await this.GetPostReactionsCountByIdAsync(postId);

            return viewModel;
        }

        [HttpPost]
        [Route("sad/{postId}")]
        public async Task<ActionResult<ReactionsCountViewModel>> Sad(int postId)
        {
            var isExisting = await this.postsService.IsExisting(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReactionsService.ReactAsync(ReactionType.Sad, postId, this.User.GetId());
            var viewModel = await this.GetPostReactionsCountByIdAsync(postId);

            return viewModel;
        }

        [HttpPost]
        [Route("angry/{postId}")]
        public async Task<ActionResult<ReactionsCountViewModel>> Angry(int postId)
        {
            var isExisting = await this.postsService.IsExisting(postId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReactionsService.ReactAsync(ReactionType.Angry, postId, this.User.GetId());
            var viewModel = await this.GetPostReactionsCountByIdAsync(postId);

            return viewModel;
        }

        private async Task<ReactionsCountViewModel> GetPostReactionsCountByIdAsync(int postId)
        {
            var (likes, loves, haha, wow, sad, angry) = await this.postReactionsService.GetCountByPostIdAsync(postId);
            var viewModel = new ReactionsCountViewModel
            {
                Likes = likes,
                Loves = loves,
                Wow = wow,
                Haha = haha,
                Sad = sad,
                Angry = angry
            };

            return viewModel;
        }
    }
}