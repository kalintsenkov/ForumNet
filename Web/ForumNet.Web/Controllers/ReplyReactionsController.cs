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
    [Route("api/reply-reactions")]
    public class ReplyReactionsController : ControllerBase
    {
        private readonly IRepliesService repliesService;
        private readonly IReplyReactionsService replyReactionsService;

        public ReplyReactionsController(
            IRepliesService repliesService, 
            IReplyReactionsService replyReactionsService)
        {
            this.repliesService = repliesService;
            this.replyReactionsService = replyReactionsService;
        }

        [HttpPost]
        [Route("{replyId}/like")]
        public async Task<ActionResult<ReactionsCountViewModel>> Like(int replyId)
        {
            var isExisting = await this.repliesService.IsExisting(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.replyReactionsService.ReactAsync(ReactionType.Like, replyId, userId);
            var viewModel = await this.GetPostReactionsCountByIdAsync(replyId);

            return viewModel;
        }

        [HttpPost]
        [Route("{replyId}/love")]
        public async Task<ActionResult<ReactionsCountViewModel>> Love(int replyId)
        {
            var isExisting = await this.repliesService.IsExisting(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.replyReactionsService.ReactAsync(ReactionType.Love, replyId, userId);
            var viewModel = await this.GetPostReactionsCountByIdAsync(replyId);

            return viewModel;
        }

        [HttpPost]
        [Route("{replyId}/haha")]
        public async Task<ActionResult<ReactionsCountViewModel>> Haha(int replyId)
        {
            var isExisting = await this.repliesService.IsExisting(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.replyReactionsService.ReactAsync(ReactionType.Haha, replyId, userId);
            var viewModel = await this.GetPostReactionsCountByIdAsync(replyId);

            return viewModel;
        }

        [HttpPost]
        [Route("{replyId}/wow")]
        public async Task<ActionResult<ReactionsCountViewModel>> Wow(int replyId)
        {
            var isExisting = await this.repliesService.IsExisting(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.replyReactionsService.ReactAsync(ReactionType.Wow, replyId, userId);
            var viewModel = await this.GetPostReactionsCountByIdAsync(replyId);

            return viewModel;
        }

        [HttpPost]
        [Route("{replyId}/sad")]
        public async Task<ActionResult<ReactionsCountViewModel>> Sad(int replyId)
        {
            var isExisting = await this.repliesService.IsExisting(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.replyReactionsService.ReactAsync(ReactionType.Sad, replyId, userId);
            var viewModel = await this.GetPostReactionsCountByIdAsync(replyId);

            return viewModel;
        }

        [HttpPost]
        [Route("{replyId}/angry")]
        public async Task<ActionResult<ReactionsCountViewModel>> Angry(int replyId)
        {
            var isExisting = await this.repliesService.IsExisting(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.replyReactionsService.ReactAsync(ReactionType.Angry, replyId, userId);
            var viewModel = await this.GetPostReactionsCountByIdAsync(replyId);

            return viewModel;
        }

        private async Task<ReactionsCountViewModel> GetPostReactionsCountByIdAsync(int replyId)
        {
            var (likes, loves, haha, wow, sad, angry) = await this.replyReactionsService.GetCountByReplyIdAsync(replyId);
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