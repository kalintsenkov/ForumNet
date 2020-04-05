namespace ForumNet.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels;
    using ViewModels.Home;

    public class HomeController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IPostsService postsService;
        private readonly IPostReactionsService postReactionsService;
        private readonly IReplyReactionsService replyReactionsService;

        public HomeController(
            IUsersService usersService,
            IPostsService postsService,
            IPostReactionsService postReactionsService,
            IReplyReactionsService replyReactionsService)
        {
            this.usersService = usersService;
            this.postsService = postsService;
            this.postReactionsService = postReactionsService;
            this.replyReactionsService = replyReactionsService;
        }

        public IActionResult Index() => this.RedirectToAction("Trending", "Posts");

        public IActionResult Guidelines() => this.View();

        public IActionResult Privacy() => this.View();

        public IActionResult NotFound404() => this.View();

        public async Task<IActionResult> About()
        {
            var postsReactionsCount = await this.postReactionsService.GetTotalCountAsync();
            var repliesReactionsCount = await this.replyReactionsService.GetTotalCountAsync();

            var reactionsCount = postsReactionsCount + repliesReactionsCount;
            var postsCount = await this.postsService.GetCountAsync();
            var usersCount = await this.usersService.GetTotalCountAsync();
            var admins = await this.usersService.GetAdminsAsync<HomeAboutAdminViewModel>();

            var viewModel = new HomeAboutViewModel
            {
                ReactionsCount = reactionsCount,
                PostsCount = postsCount,
                UsersCount = usersCount,
                Admins = admins,
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
