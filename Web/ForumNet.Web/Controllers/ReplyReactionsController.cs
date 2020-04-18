namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Data.Models.Enums;
    using Infrastructure.Extensions;
    using Services.Reactions;
    using Services.Replies;

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

        [HttpPost("like/{replyId}")]
        public async Task<ActionResult> Like(int replyId)
        {
            var isExisting = await this.repliesService.IsExistingAsync(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReactionsService.ReactAsync(ReactionType.Like, replyId, this.User.GetId());
            var result = await this.replyReactionsService.GetCountByReplyIdAsync(replyId);

            return this.Ok(result);
        }

        [HttpPost("love/{replyId}")]
        public async Task<ActionResult> Love(int replyId)
        {
            var isExisting = await this.repliesService.IsExistingAsync(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReactionsService.ReactAsync(ReactionType.Love, replyId, this.User.GetId());
            var result = await this.replyReactionsService.GetCountByReplyIdAsync(replyId);

            return this.Ok(result);
        }

        [HttpPost("haha/{replyId}")]
        public async Task<ActionResult> Haha(int replyId)
        {
            var isExisting = await this.repliesService.IsExistingAsync(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReactionsService.ReactAsync(ReactionType.Haha, replyId, this.User.GetId());
            var result = await this.replyReactionsService.GetCountByReplyIdAsync(replyId);

            return this.Ok(result);
        }

        [HttpPost("wow/{replyId}")]
        public async Task<ActionResult> Wow(int replyId)
        {
            var isExisting = await this.repliesService.IsExistingAsync(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReactionsService.ReactAsync(ReactionType.Wow, replyId, this.User.GetId());
            var result = await this.replyReactionsService.GetCountByReplyIdAsync(replyId);

            return this.Ok(result);
        }

        [HttpPost("sad/{replyId}")]
        public async Task<ActionResult> Sad(int replyId)
        {
            var isExisting = await this.repliesService.IsExistingAsync(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReactionsService.ReactAsync(ReactionType.Sad, replyId, this.User.GetId());
            var result = await this.replyReactionsService.GetCountByReplyIdAsync(replyId);

            return this.Ok(result);
        }

        [HttpPost("angry/{replyId}")]
        public async Task<ActionResult> Angry(int replyId)
        {
            var isExisting = await this.repliesService.IsExistingAsync(replyId);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReactionsService.ReactAsync(ReactionType.Angry, replyId, this.User.GetId());
            var result = await this.replyReactionsService.GetCountByReplyIdAsync(replyId);

            return this.Ok(result);
        }
    }
}