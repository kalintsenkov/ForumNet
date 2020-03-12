namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.Replies;

    public class RepliesController : Controller
    {
        private readonly IRepliesService repliesService;
        private readonly IUsersService usersService;

        public RepliesController(IRepliesService repliesService, IUsersService usersService)
        {
            this.repliesService = repliesService;
            this.usersService = usersService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(RepliesCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Details", "Posts", new { id = input.PostId });
            }

            var authorId = await this.usersService.GetIdAsync(this.User);
            await this.repliesService.CreateAsync(input.Description, input.PostId, authorId);

            return this.RedirectToAction("Details", "Posts", new { id = input.PostId });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var reply = await this.repliesService.GetByIdAsync<RepliesEditInputModel>(id);
            if (reply == null)
            {
                return this.NotFound();
            }

            return this.View(reply);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(RepliesEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.repliesService.EditAsync(input.Id, input.Description);

            return this.RedirectToAction(nameof(Details), new { id = input.Id });
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var reply = await this.repliesService.GetByIdAsync<RepliesDetailsViewModel>(id);
            if (reply == null)
            {
                return this.NotFound();
            }

            return this.View(reply);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(int id)
        {
            var likes = await this.repliesService.LikeAsync(id);

            return this.Json(new { Likes = likes});
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Dislike(int id)
        {
            var likes = await this.repliesService.DislikeAsync(id);

            return this.Json(new { Likes = likes});
        }
    }
}