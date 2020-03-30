namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Contracts;
    using ViewModels.Posts;

    [Authorize]
    public class PostsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ITagsService tagsService;
        private readonly IPostsService postsService;
        private readonly IRepliesService repliesService;
        private readonly ICategoriesService categoriesService;

        public PostsController(
            IMapper mapper,
            ITagsService tagsService,
            IPostsService postsService,
            IRepliesService repliesService,
            ICategoriesService categoriesService)
        {
            this.mapper = mapper;
            this.tagsService = tagsService;
            this.postsService = postsService;
            this.repliesService = repliesService;
            this.categoriesService = categoriesService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All(int id, string search)
        {
            var viewModel = new PostsAllViewModel
            {
                Posts = await this.postsService.GetAllAsync<PostsListingViewModel>(search),
                PinnedPosts = await this.postsService.GetAllPinnedAsync<PostsListingViewModel>()
            };

            foreach (var post in viewModel.PinnedPosts)
            {
                post.Activity = await this.postsService.GetLatestActivityById(post.Id);
                post.Tags = await this.tagsService.GetAllByPostIdAsync<PostsTagsDetailsViewModel>(post.Id);
            }

            foreach (var post in viewModel.Posts)
            {
                post.Activity = await this.postsService.GetLatestActivityById(post.Id);
                post.Tags = await this.tagsService.GetAllByPostIdAsync<PostsTagsDetailsViewModel>(post.Id);
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Following(int id, string search)
        {
            var viewModel = new PostsAllViewModel
            {
                Posts = await this.postsService
                    .GetAllFollowingByUserIdAsync<PostsListingViewModel>(this.User.GetId(), search)
            };

            foreach (var post in viewModel.Posts)
            {
                post.Activity = await this.postsService.GetLatestActivityById(post.Id);
                post.Tags = await this.tagsService.GetAllByPostIdAsync<PostsTagsDetailsViewModel>(post.Id);
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new PostsCreateInputModel
            {
                Tags = await this.tagsService.GetAllAsync<PostsTagsDetailsViewModel>(),
                Categories = await this.categoriesService.GetAllAsync<PostsCategoryDetailsViewModel>()
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostsCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.categoriesService.GetAllAsync<PostsCategoryDetailsViewModel>();
                input.Tags = await this.tagsService.GetAllAsync<PostsTagsDetailsViewModel>();

                return this.View(input);
            }

            var postId = await this.postsService.CreateAsync(
                input.Title,
                input.PostType,
                input.Description,
                this.User.GetId(),
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

            post.Tags = await this.tagsService.GetAllByPostIdAsync<PostsTagsDetailsViewModel>(id);
            post.Replies = await this.repliesService.GetAllByPostIdAsync<PostsRepliesDetailsViewModel>(id, sort);

            return this.View(post);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await this.postsService.GetByIdAsync<PostsEditViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            if (post.AuthorId != this.User.GetId() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            post.Categories = await this.categoriesService.GetAllAsync<PostsCategoryDetailsViewModel>();
            post.Tags = await this.tagsService.GetAllAsync<PostsTagsDetailsViewModel>();

            return this.View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostsEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var viewModel = this.mapper.Map<PostsEditViewModel>(input);

                viewModel.Categories = await this.categoriesService.GetAllAsync<PostsCategoryDetailsViewModel>();
                viewModel.Tags = await this.tagsService.GetAllAsync<PostsTagsDetailsViewModel>();

                return this.View(viewModel);
            }

            var postAuthorId = await this.postsService.GetAuthorIdById(input.Id);
            if (postAuthorId != this.User.GetId() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            await this.postsService.EditAsync(input.Id, input.Title, input.Description, input.CategoryId, input.TagIds);

            return this.RedirectToAction(nameof(Details), new { id = input.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = await this.postsService.GetByIdAsync<PostsDeleteDetailsViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            if (post.Author.Id != this.User.GetId() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            post.Tags = await this.tagsService.GetAllByPostIdAsync<PostsTagsDetailsViewModel>(id);

            return this.View(post);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await this.postsService.GetByIdAsync<PostsDeleteConfirmedViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            if (post.AuthorId != this.User.GetId() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            await this.postsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(All));
        }
    }
}