namespace ForumNet.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Contracts;
    using ViewModels.Users;

    [ViewComponent(Name = "UserLoginStatus")]
    public class UserLoginStatusViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public UserLoginStatusViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id = this.UserClaimsPrincipal.GetId();
            var user = await this.usersService.GetByIdAsync<UserLoginStatusViewModel>(id);

            return this.View(user);
        }
    }
}