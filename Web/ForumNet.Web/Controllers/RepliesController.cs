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
        public async Task<IActionResult> Create(int id, string description)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Details", "Posts", new { id });
            }

            var authorId = await this.usersService.GetIdAsync(this.User);
            await this.repliesService.CreateAsync(description, id, authorId);

            return this.RedirectToAction("Details", "Posts", new { id });
        }
    }
}