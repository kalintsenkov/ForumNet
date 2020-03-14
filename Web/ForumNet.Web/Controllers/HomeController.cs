namespace ForumNet.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels;
    using ViewModels.Home;
    using ViewModels.Posts;
    using ViewModels.Tags;

    public class HomeController : Controller
    {
        private readonly IPostsService postsService;
        private readonly ITagsService tagsService;

        public HomeController(IPostsService postsService, ITagsService tagsService)
        {
            this.postsService = postsService;
            this.tagsService = tagsService;
        }

        public async Task<IActionResult> Index(string sort)
        {
            var viewModel = new HomeViewModel
            {
                Posts = await this.postsService.GetAllAsync<PostsListingViewModel>(sort)
            };

            foreach (var post in viewModel.Posts)
            {
                post.Tags = await this.tagsService.GetAllByPostIdAsync<TagsInfoViewModel>(post.Id);
            }

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NotFound404()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
