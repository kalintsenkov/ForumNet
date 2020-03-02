namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.Categories;
    using ViewModels.Posts;
    using ViewModels.Tags;

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
                Categories = await this.categoriesService.GetAllAsync<CategoriesInfoViewModel>(),
                Tags = await this.tagsService.GetAllAsync<TagsInfoViewModel>()
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostsCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.categoriesService.GetAllAsync<CategoriesInfoViewModel>();
                input.Tags = await this.tagsService.GetAllAsync<TagsInfoViewModel>();

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

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var post = await this.postsService.GetByIdAsync<PostsDetailsViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            await this.postsService.ViewAsync(id);
            post.Tags = await this.tagsService.GetAllByPostIdAsync<TagsInfoViewModel>(id);

            return this.View(post);
        }

        [Authorize]
        public async Task<IActionResult> Like(int id)
        {
            await this.postsService.LikeAsync(id);

            return this.RedirectToAction("Details", new { id });
        }

        [Authorize]
        public async Task<IActionResult> Dislike(int id)
        {
            await this.postsService.DislikeAsync(id);

            return this.RedirectToAction("Details", new { id });
        }

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