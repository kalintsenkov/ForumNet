namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

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

        public async Task<IActionResult> All()
        {
            var categories = await this.categoriesService.GetAllAsync<CategoriesInfoViewModel>();
            var viewModel = new CategoriesAllViewModel
            {
                Categories = categories
            };

            foreach (var category in viewModel.Categories)
            {
                category.Threads = await this.categoriesService.GetThreadsCountByIdAsync(category.Id);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await this.categoriesService.GetByIdAsync<CategoriesInfoViewModel>(id);
            if (category == null)
            {
                return this.NotFound();
            }

            category.Threads = await this.categoriesService.GetThreadsCountByIdAsync(id);

            var posts = await this.postsService.GetAllByCategoryIdAsync<PostsListingViewModel>(id);
            foreach (var post in posts)
            {
                post.Tags = await this.tagsService.GetAllByPostIdAsync<TagsInfoViewModel>(post.Id);
            }

            var viewModel = new CategoriesDetailsViewModel
            {
                Category = category,
                Posts = posts
            };

            return View(viewModel);
        }
    }
}