namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Data.Models.Enums;
    using Infrastructure.Extensions;
    using Services.Reactions;

    [Route("api/post-reactions")]
    public class PostReactionsController : ApiController
    {
        private readonly IPostReactionsService postReactionsService;

        public PostReactionsController(IPostReactionsService postReactionsService) 
            => this.postReactionsService = postReactionsService;

        [HttpPost("like/{postId}")]
        public async Task<ActionResult<ReactionsCountServiceModel>> Like(int postId) 
            => await this.postReactionsService.ReactAsync(
                ReactionType.Like, 
                postId, 
                this.User.GetId());

        [HttpPost("love/{postId}")]
        public async Task<ActionResult<ReactionsCountServiceModel>> Love(int postId) 
            => await this.postReactionsService.ReactAsync(
                ReactionType.Love,
                postId, 
                this.User.GetId());

        [HttpPost("haha/{postId}")]
        public async Task<ActionResult<ReactionsCountServiceModel>> Haha(int postId) 
            => await this.postReactionsService.ReactAsync(
                ReactionType.Haha, 
                postId, 
                this.User.GetId());

        [HttpPost("wow/{postId}")]
        public async Task<ActionResult<ReactionsCountServiceModel>> Wow(int postId) 
            => await this.postReactionsService.ReactAsync(
                ReactionType.Wow, 
                postId, 
                this.User.GetId());

        [HttpPost("sad/{postId}")]
        public async Task<ActionResult<ReactionsCountServiceModel>> Sad(int postId) 
            => await this.postReactionsService.ReactAsync(
                ReactionType.Sad, 
                postId, 
                this.User.GetId());

        [HttpPost("angry/{postId}")]
        public async Task<ActionResult<ReactionsCountServiceModel>> Angry(int postId)
            => await this.postReactionsService.ReactAsync(
                ReactionType.Angry,
                postId, 
                this.User.GetId());
    }
}