namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ForumNet.Common;
    using Services.Contracts;
    using ViewModels.Replies;

    [Authorize]
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

        public async Task<IActionResult> Edit(int id)
        {
            var reply = await this.repliesService.GetByIdAsync<RepliesEditInputModel>(id);
            if (reply == null)
            {
                return this.NotFound();
            }

            var currentUserId = await this.usersService.GetIdAsync(this.User);
            if (reply.AuthorId != currentUserId && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.Unauthorized();
            }

            return this.View(reply);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RepliesEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUserId = await this.usersService.GetIdAsync(this.User);
            var replyAuthorId = await this.repliesService.GetAuthorIdById(input.Id);
            if (replyAuthorId != currentUserId && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.Unauthorized();
            }

            await this.repliesService.EditAsync(input.Id, input.Description);

            return this.RedirectToAction(nameof(Details), new { id = input.Id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var reply = await this.repliesService.GetByIdAsync<RepliesDetailsViewModel>(id);
            if (reply == null)
            {
                return this.NotFound();
            }

            this.ViewData["UserId"] = await this.usersService.GetIdAsync(this.User);

            return this.View(reply);
        }

        [HttpPost]
        public async Task<IActionResult> Like(int id)
        {
            var isExisting = await this.repliesService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var likes = await this.repliesService.LikeAsync(id);

            return this.Json(new { Likes = likes });
        }

        [HttpPost]
        public async Task<IActionResult> Dislike(int id)
        {
            var isExisting = await this.repliesService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var likes = await this.repliesService.DislikeAsync(id);

            return this.Json(new { Likes = likes });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var reply = await this.repliesService.GetByIdAsync<RepliesDeleteDetailsViewModel>(id);
            if (reply == null)
            {
                return this.NotFound();
            }

            var currentUserId = await this.usersService.GetIdAsync(this.User);
            if (reply.AuthorId != currentUserId && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.Unauthorized();
            }

            await this.repliesService.DeleteAsync(id);

            return this.RedirectToAction("Details", "Posts", new { id = reply.PostId });
        }
    }
}