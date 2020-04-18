namespace ForumNet.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Posts;
    using Services.Tags;
    using ViewModels.Posts;
    using ViewModels.Tags;

    public class TagsController : Controller
    {
        private const int TagsPerPage = 9;

        private readonly ITagsService tagsService;
        private readonly IPostsService postsService;

        public TagsController(
            ITagsService tagsService,
            IPostsService postsService)
        {
            this.tagsService = tagsService;
            this.postsService = postsService;
        }

        public async Task<IActionResult> All(int page = 1, string search = null)
        {
            var skip = (page - 1) * TagsPerPage;
            var count = await this.tagsService.GetCountAsync();
            var tags = await this.tagsService.GetAllAsync<TagsInfoViewModel>(search, skip, TagsPerPage);
            var viewModel = new TagsAllViewModel
            {
                Tags = tags,
                Search = search,
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(count / (decimal)TagsPerPage)
            };

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id, string search = null)
        {
            var tag = await this.tagsService.GetByIdAsync<TagsInfoViewModel>(id);
            if (tag == null)
            {
                return this.NotFound();
            }

            var posts = await this.postsService.GetAllByTagIdAsync<PostsListingViewModel>(id, search);
            foreach (var post in posts)
            {
                post.Activity = await this.postsService.GetLatestActivityByIdAsync(post.Id);
                post.Tags = await this.tagsService.GetAllByPostIdAsync<PostsTagsDetailsViewModel>(post.Id);
            }

            var viewModel = new TagsDetailsViewModel
            {
                Search = search,
                Tag = tag,
                Posts = posts
            };

            return this.View(viewModel);
        }
    }
}
