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
        [Route("like/{replyId}")]
        public async Task<ActionResult<ReactionsCountViewModel>> Like(int replyId)
        {
            var isExisting = await this.repliesService.IsExisting(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReactionsService.ReactAsync(ReactionType.Like, replyId, this.User.GetId());
            var viewModel = await this.GetPostReactionsCountByIdAsync(replyId);

            return viewModel;
        }

        [HttpPost]
        [Route("love/{replyId}")]
        public async Task<ActionResult<ReactionsCountViewModel>> Love(int replyId)
        {
            var isExisting = await this.repliesService.IsExisting(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReactionsService.ReactAsync(ReactionType.Love, replyId, this.User.GetId());
            var viewModel = await this.GetPostReactionsCountByIdAsync(replyId);

            return viewModel;
        }

        [HttpPost]
        [Route("haha/{replyId}")]
        public async Task<ActionResult<ReactionsCountViewModel>> Haha(int replyId)
        {
            var isExisting = await this.repliesService.IsExisting(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReactionsService.ReactAsync(ReactionType.Haha, replyId, this.User.GetId());
            var viewModel = await this.GetPostReactionsCountByIdAsync(replyId);

            return viewModel;
        }

        [HttpPost]
        [Route("wow/{replyId}")]
        public async Task<ActionResult<ReactionsCountViewModel>> Wow(int replyId)
        {
            var isExisting = await this.repliesService.IsExisting(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReactionsService.ReactAsync(ReactionType.Wow, replyId, this.User.GetId());
            var viewModel = await this.GetPostReactionsCountByIdAsync(replyId);

            return viewModel;
        }

        [HttpPost]
        [Route("sad/{replyId}")]
        public async Task<ActionResult<ReactionsCountViewModel>> Sad(int replyId)
        {
            var isExisting = await this.repliesService.IsExisting(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReactionsService.ReactAsync(ReactionType.Sad, replyId, this.User.GetId());
            var viewModel = await this.GetPostReactionsCountByIdAsync(replyId);

            return viewModel;
        }

        [HttpPost]
        [Route("angry/{replyId}")]
        public async Task<ActionResult<ReactionsCountViewModel>> Angry(int replyId)
        {
            var isExisting = await this.repliesService.IsExisting(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReactionsService.ReactAsync(ReactionType.Angry, replyId, this.User.GetId());
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