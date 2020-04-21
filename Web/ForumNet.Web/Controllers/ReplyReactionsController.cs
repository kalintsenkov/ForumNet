namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Data.Models.Enums;
    using Infrastructure.Extensions;
    using Services.Reactions;

    [Route("api/reply-reactions")]
    public class ReplyReactionsController : ApiController
    {
        private readonly IReplyReactionsService replyReactionsService;

        public ReplyReactionsController(IReplyReactionsService replyReactionsService) 
            => this.replyReactionsService = replyReactionsService;

        [HttpPost("like/{replyId}")]
        public async Task<ActionResult<ReactionsCountServiceModel>> Like(int replyId)
            => await this.replyReactionsService.ReactAsync(
                ReactionType.Like, 
                replyId, 
                this.User.GetId());

        [HttpPost("love/{replyId}")]
        public async Task<ActionResult<ReactionsCountServiceModel>> Love(int replyId) 
            => await this.replyReactionsService.ReactAsync(
                ReactionType.Love, 
                replyId, 
                this.User.GetId());

        [HttpPost("haha/{replyId}")]
        public async Task<ActionResult<ReactionsCountServiceModel>> Haha(int replyId) 
            => await this.replyReactionsService.ReactAsync(
                ReactionType.Haha, 
                replyId, 
                this.User.GetId());

        [HttpPost("wow/{replyId}")]
        public async Task<ActionResult<ReactionsCountServiceModel>> Wow(int replyId) 
            => await this.replyReactionsService.ReactAsync(
                ReactionType.Wow,
                replyId,
                this.User.GetId());

        [HttpPost("sad/{replyId}")]
        public async Task<ActionResult<ReactionsCountServiceModel>> Sad(int replyId) 
            => await this.replyReactionsService.ReactAsync(
                ReactionType.Sad,
                replyId,
                this.User.GetId());

        [HttpPost("angry/{replyId}")]
        public async Task<ActionResult<ReactionsCountServiceModel>> Angry(int replyId)
            => await this.replyReactionsService.ReactAsync(
                ReactionType.Angry,
                replyId,
                this.User.GetId());
    }
}