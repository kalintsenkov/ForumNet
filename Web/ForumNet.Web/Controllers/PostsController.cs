namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.Categories;
    using ViewModels.Posts;
    using ViewModels.Replies;
    using ViewModels.Tags;

    [Authorize]
    public class PostsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ITagsService tagsService;
        private readonly IPostsService postsService;
        private readonly IUsersService usersService;
        private readonly IRepliesService repliesService;
        private readonly ICategoriesService categoriesService;

        public PostsController(
            IMapper mapper,
            ITagsService tagsService,
            IPostsService postsService,
            IUsersService usersService,
            IRepliesService repliesService,
            ICategoriesService categoriesService)
        {
            this.mapper = mapper;
            this.tagsService = tagsService;
            this.postsService = postsService;
            this.usersService = usersService;
            this.repliesService = repliesService;
            this.categoriesService = categoriesService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All(int id, string sort)
        {
            var viewModel = new PostsAllViewModel
            {
                Posts = await this.postsService.GetAllAsync<PostsListingViewModel>(sort)
            };

            foreach (var post in viewModel.Posts)
            {
                post.Tags = await this.tagsService.GetAllByPostIdAsync<TagsInfoViewModel>(post.Id);
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await this.categoriesService.GetAllAsync<CategoriesInfoViewModel>();
            var tags = await this.tagsService.GetAllAsync<TagsInfoViewModel>();
            var viewModel = new PostsCreateInputModel
            {
                Categories = categories,
                Tags = tags
            };

            return this.View(viewModel);
        }

        [HttpPost]
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
                input.CategoryId);

            await this.postsService.AddTagsAsync(postId, input.TagIds);

            return this.RedirectToAction(nameof(Details), new { id = postId });
        }

        public async Task<IActionResult> Details(int id, string sort)
        {
            var post = await this.postsService.GetByIdAsync<PostsDetailsViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            await this.postsService.ViewAsync(id);

            this.ViewData["UserId"] = await this.usersService.GetIdAsync(this.User);
            post.Tags = await this.tagsService.GetAllByPostIdAsync<TagsInfoViewModel>(id);
            post.Replies = await this.repliesService.GetAllByPostIdAsync<RepliesDetailsViewModel>(id, sort);

            return this.View(post);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await this.postsService.GetByIdAsync<PostsEditViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            var currentUserId = await this.usersService.GetIdAsync(this.User);
            if (post.AuthorId != currentUserId)
            {
                return this.BadRequest();
            }

            post.Categories = await this.categoriesService.GetAllAsync<CategoriesInfoViewModel>();
            post.Tags = await this.tagsService.GetAllAsync<TagsInfoViewModel>();

            return this.View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostsEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var viewModel = this.mapper.Map<PostsEditViewModel>(input);

                viewModel.Categories = await this.categoriesService.GetAllAsync<CategoriesInfoViewModel>();
                viewModel.Tags = await this.tagsService.GetAllAsync<TagsInfoViewModel>();

                return this.View(viewModel);
            }

            var currentUserId = await this.usersService.GetIdAsync(this.User);
            if (input.AuthorId != currentUserId)
            {
                return this.BadRequest();
            }

            await this.postsService.EditAsync(input.Id, input.Title, input.Description, input.CategoryId, input.TagIds);

            return this.RedirectToAction(nameof(Details), new { id = input.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Like(int id)
        {
            var isExisting = await this.postsService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var likes = await this.postsService.LikeAsync(id);

            return this.Json(new { Likes = likes });
        }

        [HttpPost]
        public async Task<IActionResult> Dislike(int id)
        {
            var isExisting = await this.postsService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var likes = await this.postsService.DislikeAsync(id);

            return this.Json(new { Likes = likes });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await this.postsService.GetByIdAsync<PostsDeleteDetailsViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }
            
            var currentUserId = await this.usersService.GetIdAsync(this.User);
            if (post.AuthorId != currentUserId)
            {
                return this.BadRequest();
            }

            await this.postsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(All));
        }
    }
}