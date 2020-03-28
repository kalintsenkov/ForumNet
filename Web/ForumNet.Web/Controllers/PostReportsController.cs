namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Contracts;
    using ViewModels.PostReports;

    [Authorize]
    public class PostReportsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly IPostReportsService postReportsService;

        public PostReportsController(
            IPostsService postsService,
            IPostReportsService postReportsService)
        {
            this.postsService = postsService;
            this.postReportsService = postReportsService;
        }

        public async Task<IActionResult> Create(int id)
        {
            var post = await this.postsService.GetByIdAsync<PostReportsInputModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            return this.View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostReportsInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.postReportsService.CreateAsync(input.Description, input.Id, this.User.GetId());

            return this.RedirectToAction("Details", "Posts", new { id = input.Id });
        }
    }
}
