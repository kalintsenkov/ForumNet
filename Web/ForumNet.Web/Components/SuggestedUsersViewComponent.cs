namespace ForumNet.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.Users;
    using ViewModels.Users;

    [ViewComponent(Name = "SuggestedUsers")]
    public class SuggestedUsersViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public SuggestedUsersViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var suggestedUsers = await this.usersService.GetFollowersAsync<UsersFollowersAllViewModel>(userId);

            return this.View(suggestedUsers);
        }
    }
}
