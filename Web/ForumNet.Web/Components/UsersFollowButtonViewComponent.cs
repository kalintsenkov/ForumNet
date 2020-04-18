namespace ForumNet.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Users;
    using ViewModels.Users;

    [ViewComponent(Name = "UsersFollowButton")]
    public class UsersFollowButtonViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public UsersFollowButtonViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var followerId = this.UserClaimsPrincipal.GetId();
            var viewModel = new UsersDetailsViewModel
            {
                Id = userId,
                IsFollowed = await this.usersService.IsFollowedAlreadyAsync(userId, followerId)
            };

            return this.View(viewModel);
        }
    }
}
