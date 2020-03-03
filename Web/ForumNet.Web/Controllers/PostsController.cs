namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;
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
        private readonly IMapper mapper;

        public PostsController(
            IPostsService postsService,
            ICategoriesService categoriesService,
            ITagsService tagsService,
            IUsersService usersService, 
            IMapper mapper)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
            this.tagsService = tagsService;
            this.usersService = usersService;
            this.mapper = mapper;
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

            return this.RedirectToAction(nameof(Details), new { id = postId });
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

            this.ViewData["UserId"] = await this.usersService.GetIdAsync(this.User);
            post.Tags = await this.tagsService.GetAllByPostIdAsync<TagsInfoViewModel>(id);

            return this.View(post);
        }

        [Authorize]
        public async Task<IActionResult> Like(int id)
        {
            await this.postsService.LikeAsync(id);

            return this.RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        public async Task<IActionResult> Dislike(int id)
        {
            await this.postsService.DislikeAsync(id);

            return this.RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await this.postsService.GetByIdAsync<PostsEditViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            post.Categories = await this.categoriesService.GetAllAsync<CategoriesInfoViewModel>();
            post.Tags = await this.tagsService.GetAllAsync<TagsInfoViewModel>();

            return this.View(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(PostsEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var viewModel = this.mapper.Map<PostsEditViewModel>(input);
                
                viewModel.Categories = await this.categoriesService.GetAllAsync<CategoriesInfoViewModel>();
                viewModel.Tags = await this.tagsService.GetAllAsync<TagsInfoViewModel>();

                return this.View(viewModel);
            }

            await this.postsService.EditAsync(input.Id, input.Title, input.Description, input.CategoryId, input.TagIds);

            return this.RedirectToAction(nameof(Details), new { id = input.Id });
        }

        //// GET: Posts/Delete/5
        //public IActionResult Delete(int id)
        //{
        //    return View();
        //}
    }
}