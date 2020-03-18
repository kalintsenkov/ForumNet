namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.Categories;
    using ViewModels.Posts;
    using ViewModels.Tags;

    public class CategoriesController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;
        private readonly ITagsService tagsService;

        public CategoriesController(
            ICategoriesService categoriesService,
            IPostsService postsService, 
            ITagsService tagsService)
        {
            this.categoriesService = categoriesService;
            this.postsService = postsService;
            this.tagsService = tagsService;
        }

        public async Task<IActionResult> All(string search)
        {
            var categories = await this.categoriesService.GetAllAsync<CategoriesInfoViewModel>(search);
            var viewModel = new CategoriesAllViewModel
            {
                Search = search,
                Categories = categories
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id, string search)
        {
            var category = await this.categoriesService.GetByIdAsync<CategoriesInfoViewModel>(id);
            if (category == null)
            {
                return this.NotFound();
            }

            var posts = await this.postsService.GetAllByCategoryIdAsync<PostsListingViewModel>(id, search);
            foreach (var post in posts)
            {
                post.Tags = await this.tagsService.GetAllByPostIdAsync<TagsInfoViewModel>(post.Id);
            }

            var viewModel = new CategoriesDetailsViewModel
            {
                Search = search,
                Category = category,
                Posts = posts
            };

            return View(viewModel);
        }
    }
}