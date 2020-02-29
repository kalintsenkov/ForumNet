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
        private readonly ITagsService tagsService;
        private readonly IUsersService usersService;

        public PostsController(
            IPostsService postsService, 
            ICategoriesService categoriesService,
            ITagsService tagsService,
            IUsersService usersService)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
            this.tagsService = tagsService;
            this.usersService = usersService;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var viewModel = new PostsCreateInputModel
            {
                Categories = await this.categoriesService.GetAllAsync<CategoriesListingViewModel>(),
                Tags = await this.tagsService.GetAllAsync<TagsListingViewModel>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostsCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.categoriesService.GetAllAsync<CategoriesListingViewModel>();
                input.Tags = await this.tagsService.GetAllAsync<TagsListingViewModel>();

                return this.View(input);
            }

            var authorId = await this.usersService.GetIdAsync(this.User);
            var postId = await this.postsService.CreateAsync(
                input.Title, 
                input.PostType, 
                input.Description, 
                authorId, 
                input.CategoryId, 
                input.ImageOrVideoUrl);

            await this.postsService.AddTagsAsync(postId, input.TagIds);

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