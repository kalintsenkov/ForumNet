namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Posts;
    using Services.Replies;
    using Services.Tags;
    using Services.Users;
    using ViewModels.Users;

    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IPostsService postsService;
        private readonly ITagsService tagsService;
        private readonly IRepliesService repliesService;

        public UsersController(
            IUsersService usersService,
            IPostsService postsService,
            ITagsService tagsService,
            IRepliesService repliesService)
        {
            this.usersService = usersService;
            this.postsService = postsService;
            this.tagsService = tagsService;
            this.repliesService = repliesService;
        }

        public async Task<IActionResult> Threads(string id)
        {
            var user = await this.usersService.GetByIdAsync<UsersDetailsViewModel>(id);
            if (user == null)
            {
                return this.NotFound();
            }

            user.Threads = await this.postsService.GetAllByUserIdAsync<UsersThreadsViewModel>(id);
            foreach (var thread in user.Threads)
            {
                thread.Activity = await this.postsService.GetLatestActivityByIdAsync(thread.Id);
                thread.Tags = await this.tagsService.GetAllByPostIdAsync<UsersThreadsTagsViewModel>(thread.Id);
            }

            return this.View(user);
        }

        public async Task<IActionResult> Replies(string id)
        {
            var user = await this.usersService.GetByIdAsync<UsersDetailsViewModel>(id);
            if (user == null)
            {
                return this.NotFound();
            }

            user.Replies = await this.repliesService.GetAllByUserIdAsync<UsersRepliesViewModel>(id);

            return this.View(user);
        }

        public async Task<IActionResult> Followers(string id)
        {
            var user = await this.usersService.GetByIdAsync<UsersDetailsViewModel>(id);
            if (user == null)
            {
                return this.NotFound();
            }

            user.Followers = await this.usersService.GetFollowersAsync<UsersFollowersViewModel>(id);

            return this.View(user);
        }

        public async Task<IActionResult> Following(string id)
        {
            var user = await this.usersService.GetByIdAsync<UsersDetailsViewModel>(id);
            if (user == null)
            {
                return this.NotFound();
            }

            user.Following = await this.usersService.GetFollowingAsync<UsersFollowingViewModel>(id);

            return this.View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string id)
        {
            var followerId = this.User.GetId();
            if (followerId == id)
            {
                return this.BadRequest();
            }

            var isFollowed = await this.usersService.FollowAsync(id, followerId);

            return this.Ok(isFollowed);
        }
    }
}