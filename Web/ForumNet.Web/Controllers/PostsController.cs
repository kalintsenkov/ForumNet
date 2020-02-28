namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.Posts;

    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly ICategoriesService categoriesService;
        private readonly IUsersService usersService;

        public PostsController(
            IPostsService postsService, 
            ICategoriesService categoriesService,
            IUsersService usersService)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
            this.usersService = usersService;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var viewModel = new PostsInputViewModel
            {
                Categories = await this.categoriesService.GetAllAsync<CategoriesListingViewModel>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostsInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.categoriesService.GetAllAsync<CategoriesListingViewModel>();

                return this.View(input);
            }

            var authorId = await this.usersService.GetIdAsync(this.User);
            var isCreated = await this.postsService.CreateAsync(
                input.Title, 
                input.PostType, 
                input.Description,
                authorId, 
                input.CategoryId, 
                input.ImageOrVideoUrl);

            if (!isCreated)
            {
                return this.View();
            }

            return RedirectToAction("Index", "Home");
        }

        //// GET: Posts/Details/5
        //public IActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Posts/Edit/5
        //public IActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// GET: Posts/Delete/5
        //public IActionResult Delete(int id)
        //{
        //    return View();
        //}
    }
}