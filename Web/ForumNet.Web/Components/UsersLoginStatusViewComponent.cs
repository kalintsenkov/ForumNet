namespace ForumNet.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Users;
    using ViewModels.Users;

    [ViewComponent(Name = "UsersLoginStatus")]
    public class UsersLoginStatusViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public UsersLoginStatusViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id = this.UserClaimsPrincipal.GetId();
            var user = await this.usersService.GetByIdAsync<UsersLoginStatusViewModel>(id);

            return this.View(user);
        }
    }
}