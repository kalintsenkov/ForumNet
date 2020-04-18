namespace ForumNet.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.Users;
    using ViewModels.Users;

    [ViewComponent(Name = "UsersDetails")]
    public class UsersDetailsViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public UsersDetailsViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var viewModel = new UsersDetailsViewModel
            {
                Id = userId,
                FollowersCount = await this.usersService.GetFollowersCountAsync(userId),
                FollowingCount = await this.usersService.GetFollowingCountAsync(userId)
            };

            return this.View(viewModel);
        }
    }
}
