namespace ForumNet.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Data.Models;
    using ViewModels.Users;

    [ViewComponent(Name = "UserLoginStatus")]
    public class UserLoginStatusViewComponent : ViewComponent
    {
        private readonly UserManager<ForumUser> userManager;

        public UserLoginStatusViewComponent(UserManager<ForumUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync(this.UserClaimsPrincipal);

            var viewModel = new UserLoginStatusViewModel
            {
                UserName = user.UserName,
                ProfilePicture = user.ProfilePicture
            };

            return this.View(viewModel);
        }
    }
}