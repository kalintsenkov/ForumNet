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

        [Authorize]
        public async Task<IActionResult> Reply(RepliesReplyInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.ModelState.AddModelError(nameof(input.Description), "Description should be less than 1000 symbols");
                return this.RedirectToAction("Details", "Posts", new { id = input.Id });
            }

            var authorId = await this.usersService.GetIdAsync(this.User);
            await this.repliesService.CreateAsync(input.Description, input.Id, authorId);

            return this.RedirectToAction("Details", "Posts", new { id = input.Id });
        }
    }
}