namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.Posts;
    using ViewModels.Tags;

    [Authorize]
    public class TagsController : Controller
    {
        private readonly ITagsService tagsService;
        private readonly IPostsService postsService;

        public TagsController(
            ITagsService tagsService,
            IPostsService postsService)
        {
            this.tagsService = tagsService;
            this.postsService = postsService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var tag = await this.tagsService.GetById<TagsInfoViewModel>(id);
            if (tag == null)
            {
                return this.NotFound();
            }

            var posts = await this.postsService.GetAllByTagIdAsync<PostsListingViewModel>(id);
            foreach (var post in posts)
            {
                post.Activity = await this.postsService.GetLatestActivityById(post.Id);
                post.Tags = await this.tagsService.GetAllByPostIdAsync<PostsTagsDetailsViewModel>(post.Id);
            }

            var viewModel = new TagsDetailsViewModel
            {
                Tag = tag,
                Posts = posts
            };

            return this.View(viewModel);
        }
    }
}
