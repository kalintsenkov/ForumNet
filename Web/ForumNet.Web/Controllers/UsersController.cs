namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
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
            var user = await this.usersService.GetByIdAsync<UsersThreadsViewModel>(id);
            if (user == null)
            {
                return this.NotFound();
            }

            user.Threads = await this.postsService.GetAllByUserIdAsync<UsersThreadsAllViewModel>(id);
            foreach (var thread in user.Threads)
            {
                thread.Activity = await this.postsService.GetLatestActivityById(thread.Id);
                thread.Tags = await this.tagsService.GetAllByPostIdAsync<UsersThreadsTagsViewModel>(thread.Id);
            }

            return this.View(user);
        }

        public async Task<IActionResult> Replies(string id)
        {
            var user = await this.usersService.GetByIdAsync<UsersRepliesViewModel>(id);
            if (user == null)
            {
                return this.NotFound();
            }

            user.Replies = await this.repliesService.GetAllByUserIdAsync<UsersRepliesAllViewModel>(id);
            foreach (var reply in user.Replies)
            {
                reply.Activity = await this.postsService.GetLatestActivityById(reply.PostId);
            }

            return this.View(user);
        }
    }
}