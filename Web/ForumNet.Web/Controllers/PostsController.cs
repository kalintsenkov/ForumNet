namespace ForumNet.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using InputModels.Posts;
    using Services.Contracts;
    using ViewModels.Posts;

    [Authorize]
    public class PostsController : Controller
    {
        private const int PostsPerPage = 6;

        private readonly ITagsService tagsService;
        private readonly IUsersService usersService;
        private readonly IPostsService postsService;
        private readonly IRepliesService repliesService;
        private readonly ICategoriesService categoriesService;

        public PostsController(
            ITagsService tagsService,
            IUsersService usersService,
            IPostsService postsService,
            IRepliesService repliesService,
            ICategoriesService categoriesService)
        {
            this.tagsService = tagsService;
            this.usersService = usersService;
            this.postsService = postsService;
            this.repliesService = repliesService;
            this.categoriesService = categoriesService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Trending(int page = 1, string search = null)
        {
            var skip = (page - 1) * PostsPerPage;
            var count = await this.postsService.GetCountAsync();
            var posts = await this.postsService.GetAllAsync<PostsListingViewModel>(skip, PostsPerPage, search);
            foreach (var post in posts)
            {
                post.Activity = await this.postsService.GetLatestActivityByIdAsync(post.Id);
                post.Tags = await this.tagsService.GetAllByPostIdAsync<PostsTagsDetailsViewModel>(post.Id);
            }

            var viewModel = new PostsAllViewModel
            {
                Posts = posts,
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(count / (decimal)PostsPerPage)
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Following(int page = 1, string search = null)
        {
            var userId = this.User.GetId();
            var skip = (page - 1) * PostsPerPage;
            var count = await this.postsService.GetFollowingCountAsync(userId);
            var posts = await this.postsService.GetAllFollowingByUserIdAsync<PostsListingViewModel>(
                userId,
                skip,
                PostsPerPage,
                search);

            foreach (var post in posts)
            {
                post.Activity = await this.postsService.GetLatestActivityByIdAsync(post.Id);
                post.Tags = await this.tagsService.GetAllByPostIdAsync<PostsTagsDetailsViewModel>(post.Id);
            }

            var viewModel = new PostsAllViewModel
            {
                Posts = posts,
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(count / (decimal)PostsPerPage),
                FollowingCount = await this.usersService.GetFollowingCountAsync(userId)
            };

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

        public async Task<IActionResult> Details(int id, string sort = null)
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
            var post = await this.postsService.GetByIdAsync<PostsEditInputModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            if (post.AuthorId != this.User.GetId() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            post.Tags = await this.tagsService.GetAllAsync<PostsTagsDetailsViewModel>();
            post.Categories = await this.categoriesService.GetAllAsync<PostsCategoryDetailsViewModel>();

            return this.View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostsEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Tags = await this.tagsService.GetAllAsync<PostsTagsDetailsViewModel>();
                input.Categories = await this.categoriesService.GetAllAsync<PostsCategoryDetailsViewModel>();

                return this.View(input);
            }

            var postAuthorId = await this.postsService.GetAuthorIdByIdAsync(input.Id);
            if (postAuthorId != this.User.GetId() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            await this.postsService.EditAsync(
                input.Id,
                input.Title,
                input.Description,
                input.CategoryId,
                input.TagIds);

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

            return this.RedirectToAction(nameof(Trending));
        }
    }
}